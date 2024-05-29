using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkAuth : NetworkAuthenticator
{
    // ����
    readonly HashSet<NetworkConnection> _connectionPendingDisconnect = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _playerNames = new HashSet<string>();


    [Header("ClientUserName")]
    public string _playerName;

    // ��Ŷ�� �̷� ����̴���
    public struct AuthReqMsg : NetworkMessage
    {
        // ������ ���� ���
        // ���� OAuth���� �� ����, �� �κп��� �׼��� ��ū ���� �߰�
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
        // Ŭ��κ��� ���� ��û ó���� ���� �ڵ鷯 ����
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMsg, false);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<AuthRespMsg>();
    }

    public void OnAuthRequestMsg(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
        // Ŭ�� ���� ��û �޽��� ���� ���� ó��
        Debug.Log($"���� ��û : {msg.authUserName}");

        // �ߺ�����
        if (_connectionPendingDisconnect.Contains(conn)) return;

        // �� ����, DB, Playfab API ���� ȣ���� ������ Ȯ���ϱ�.
        if(!_playerNames.Contains(msg.authUserName))
        {
            _playerNames.Add(msg.authUserName);

            // ������ �������� Player.OnStartServer �������� ����

            conn.authenticationData = msg.authUserName;

            // ��Ŷ ¥��
            AuthRespMsg authRespMsg = new AuthRespMsg
            {
                code = 100,
                msg = "Auth Success"
            };
            // ����
            conn.Send(authRespMsg);

            // ���� ��� ����
            ServerAccept(conn);
        }
        else    // ���� ���� ��
        {
            _connectionPendingDisconnect.Add(conn);

            AuthRespMsg authRespMsg = new AuthRespMsg
            {
                code = 200,
                msg = "UserName Already in Use. Try Again"
            };

            conn.Send(authRespMsg);
            conn.isAuthenticated = false;   // ���� ����.
            StartCoroutine(DelayedDisconnect(conn, 1.0f));  // ������ ��, ���� ���� ����
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
