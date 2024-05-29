using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChattingUI : NetworkBehaviour
{
    [SerializeField] private TMP_InputField Input_Message;
    [SerializeField] private Text Text_ChatHistory;

    [SerializeField] private Scrollbar ScrollBar_Chat;

    [SerializeField] private Button Btn_SendMessage;
    [SerializeField] private Button Btn_Exit;

    internal static string _localPlayerName;

    //서버 온리 - 연결된 플레이어들 이름
    // 멤버 Dictionary는 선언 즉시 인스턴스 -> 널 예외를 피하기 위함
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDict = new();




    public void SetLocalPlayerName(string userName)
    {
        _localPlayerName = userName;
    }

    public override void OnStartServer()
    {
        this.gameObject.SetActive(true);

        _connectedNameDict.Clear();

        // 널 체크 대신 이런 식으로 확인
        //if(_connectedNameDict.Count > 0)
    }

    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
        // 문자열 초기화
        Text_ChatHistory.text = string.Empty;
    }

    [Command(requiresAuthority = false)] // Command : 클라이언트가 서버의 기능을 호출
    // 서버야. 메시지 보내줘
    void Command_SendMsg(string msg, NetworkConnectionToClient sender = null)
    {
        if (!_connectedNameDict.ContainsKey(sender))    // 없으면 넣겠다
        {
            var player = sender.identity.GetComponent<ChatUser>();
            var playerName = player.PlayerName;
            _connectedNameDict.Add(sender, playerName);

        }

        if (!string.IsNullOrWhiteSpace(msg))
        {
            string senderName = _connectedNameDict[sender];
            // [TODO] OnRecvMessage(senderName, msg.Trim());
        }
    }

    [ClientRpc]
    // 서버가 Rpc를 호출하면 우리는 메시지를 받을게.
    void OnRecvMessage(string senderName, string msg)
    {
        string formatedMsg = (senderName == _localPlayerName) ?
            $"<color=red>{senderName}:</color>{msg}" :
            $"<color=blue>{senderName}:</color>{msg}";

        AppendMsg(formatedMsg);
    }


    public void RemoveNameOnServerDisconnected(NetworkConnectionToClient conn)
    {
        _connectedNameDict.Remove(conn);
    }

    //========================UI==========================
    void AppendMsg(string msg)
    {
        StartCoroutine(CorAppendAndScroll(msg));
    }

    IEnumerator CorAppendAndScroll(string msg)
    {
        Text_ChatHistory.text += msg + '\n';
        
        yield return null;
        yield return null;

        ScrollBar_Chat.value = 0;
    }
    //==================================================
    private void SendMsg()
    {
        string currentChatMsg = Input_Message.text;
        if (!string.IsNullOrWhiteSpace(currentChatMsg))
        {
            Command_SendMsg(currentChatMsg.Trim());
        }
    }
    // 굳이 빼는 이유 : OnClick이라는 명명규칙을 위반하게 되기 때문에
    public void OnClick_SendMessage()
    {
        SendMsg();
    }

    public void OnClick_Exit()
    {
        NetworkManager.singleton.StopHost();
        this.gameObject.SetActive(false);
    }

    public void OnValueChanged_ToggleBtn(string val)
    {
        Btn_SendMessage.interactable = !string.IsNullOrEmpty(val);
    }

    public void OnEndEdit_Sending()
    {
        if (Input.GetKeyDown(KeyCode.Return) ||
           Input.GetKeyDown(KeyCode.KeypadEnter) ||
           Input.GetButtonDown("Submit"))
        {
            // 굳이 빼는 이유 : OnClick이라는 명명규칙을 위반하게 되기 때문에
            SendMsg();
        }
    }
}
