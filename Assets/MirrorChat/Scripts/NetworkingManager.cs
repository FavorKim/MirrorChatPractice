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
            // [TODO] chattingUI ���� � ��� �θ� �� _chattingUI.
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        if(_loginPopup != null)
        {
            // �ٸ� Ŭ�������� ���� �ٸ� Ŭ������ �޼��带 ȣ���ϴ� ������ ���ȿ� ���.
            _loginPopup.SetUIOnClientDisconnected();
            // [TODO] loginPopup ���� � ��� �θ� �� _chattingUI.
        }
    }
}