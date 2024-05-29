using Mirror;
using UnityEngine;

public partial class NetworkAuth
{
    [SerializeField] LoginPopup _loginPopup;
    
    public void OnInputValueChanged_SetPlayerName(string userName)
    {
        _playerName = userName;
        _loginPopup.SetUIOnAuthValueChanged();
    }

    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthRespMsg>(OnAuthRespMsg, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<AuthRespMsg>();
    }

    // 클라에서 인증 요청 시 호출 됨
    public override void OnClientAuthenticate()
    {
        NetworkClient.Send(new AuthReqMsg { authUserName = _playerName });
    }

    // 서버에서 인증 응답(Response) 시 호출 됨
    public void OnAuthRespMsg(AuthRespMsg msg)
    {
        if (msg.code == 100)
        {
            Debug.Log($"Auth Response : {msg.code} : {msg.msg}");
            this.ClientAccept();
        }
        else
        {
            Debug.Log($"Auth Response : {msg.code} {msg.msg}");
            NetworkManager.singleton.StopHost();
            _loginPopup.SetUIOnAuthError(msg.msg);
        }
    }
}
