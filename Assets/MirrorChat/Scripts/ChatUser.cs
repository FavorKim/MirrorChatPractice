using Mirror;
using UnityEngine;

public class ChatUser : NetworkBehaviour
{
    // SyncVar : ���� ������ ��� Ŭ�� �ڵ����� ����ȭ
    // Ŭ�� ���� �������� �ʰ� �������� �����ؾ��ϴ� �༮���� ���̴� Attribute
    [SyncVar]
    public string PlayerName;

    // ȣ��Ʈ Ȥ�� ���������� ȣ���. Like Start(), but only called on server and host
    public override void OnStartServer()
    {
        PlayerName = (string)this.connectionToClient.authenticationData;
    }

    // ���� Ŭ���̾�Ʈ ��, ���� �÷��̾ ȣ�� (�����׸�)
    public override void OnStartLocalPlayer()
    {
        var objChatUI = GameObject.Find("ChattingUI");
        if (objChatUI != null)
        {
            var chattingUI = objChatUI.GetComponent<ChattingUI>();
            if(chattingUI != null)
            {
                
            }

        }
    }
}
