using Mirror;
using UnityEngine;

public class ChatUser : NetworkBehaviour
{
    // SyncVar : 서버 변수를 모든 클라에 자동으로 동기화
    // 클라가 직접 접근하지 않고 서버에서 변경해야하는 녀석에게 붙이는 Attribute
    [SyncVar]
    public string PlayerName;

    // 호스트 혹은 서버에서만 호출됨. Like Start(), but only called on server and host
    public override void OnStartServer()
    {
        PlayerName = (string)this.connectionToClient.authenticationData;
    }

    // 많은 클라이언트 중, 로컬 플레이어만 호출 (나한테만)
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
