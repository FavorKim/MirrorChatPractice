using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkAuth : NetworkAuthenticator
{
    // 서버
    readonly HashSet<NetworkConnection> _connectionPendingDisconnect = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _playerNames = new HashSet<string>();


    [Header("ClientUserName")]
    public string _playerName;

    // 패킷이 이런 방식이더라
    public struct AuthReqMsg : NetworkMessage
    {
        // 인증을 위해 사용
        // 구글 OAuth같은 걸 사용시, 이 부분에서 액세스 토큰 등을 추가
        public string authUserName;
    }

    public struct AuthRespMsg : NetworkMessage
    {
        public byte code;
        public string msg;
    }

    #region ServerSide
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {

    }

    public override void OnStartServer()
    {
        // 클라로부터 인증 요청 처리를 위한 핸들러 연결
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMsg, false);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<AuthRespMsg>();
    }

    public void OnAuthRequestMsg(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
        // 클라 인증 요청 메시지 도착 시의 처리
        Debug.Log($"인증 요청 : {msg.authUserName}");

        // 중복방지
        if (_connectionPendingDisconnect.Contains(conn)) return;

        // 웹 서버, DB, Playfab API 등을 호출해 인증을 확인하기.
        if(!_playerNames.Contains(msg.authUserName))
        {
            _playerNames.Add(msg.authUserName);

            // 대입한 인증값은 Player.OnStartServer 시점에서 읽음

            conn.authenticationData = msg.authUserName;

            // 패킷 짜고
            AuthRespMsg authRespMsg = new AuthRespMsg
            {
                code = 100,
                msg = "Auth Success"
            };
            // 전송
            conn.Send(authRespMsg);

            // 받은 사람 수령
            ServerAccept(conn);
        }
        else    // 인증 실패 시
        {
            _connectionPendingDisconnect.Add(conn);

            AuthRespMsg authRespMsg = new AuthRespMsg
            {
                code = 200,
                msg = "UserName Already in Use. Try Again"
            };

            conn.Send(authRespMsg);
            conn.isAuthenticated = false;   // 인증 실패.
            StartCoroutine(DelayedDisconnect(conn, 1.0f));  // 딜레이 후, 서버 연결 해제
        }
    }


    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {

    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.ServerReject(conn);

        yield return null;
        _connectionPendingDisconnect.Remove(conn);
    }

    #endregion
    

}
