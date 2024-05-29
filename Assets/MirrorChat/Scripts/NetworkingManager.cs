using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingManager : NetworkManager
{
    [SerializeField] LoginPopup _loginPopup;
    [SerializeField] ChattingUI _chattingUI;
    
    public void OnInputValueChanged_SetHostName(string HostName)
    {
        this.networkAddress = HostName;
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // if (_chattingUI == null) { return; } Early return

        if(_chattingUI != null)
        {
            // [TODO] chattingUI 에서 어떤 기능 부를 것 _chattingUI.
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        if(_loginPopup != null)
        {
            // 다른 클래스에서 직접 다른 클래스의 메서드를 호출하는 형식은 보안에 취약.
            _loginPopup.SetUIOnClientDisconnected();
            // [TODO] loginPopup 에서 어떤 기능 부를 것 _chattingUI.
        }
    }
}