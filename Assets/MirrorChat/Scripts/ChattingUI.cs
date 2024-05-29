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

    //���� �¸� - ����� �÷��̾�� �̸�
    // ��� Dictionary�� ���� ��� �ν��Ͻ� -> �� ���ܸ� ���ϱ� ����
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDict = new();




    public void SetLocalPlayerName(string userName)
    {
        _localPlayerName = userName;
    }

    public override void OnStartServer()
    {
        this.gameObject.SetActive(true);

        _connectedNameDict.Clear();

        // �� üũ ��� �̷� ������ Ȯ��
        //if(_connectedNameDict.Count > 0)
    }

    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
        // ���ڿ� �ʱ�ȭ
        Text_ChatHistory.text = string.Empty;
    }

    [Command(requiresAuthority = false)] // Command : Ŭ���̾�Ʈ�� ������ ����� ȣ��
    // ������. �޽��� ������
    void Command_SendMsg(string msg, NetworkConnectionToClient sender = null)
    {
        if (!_connectedNameDict.ContainsKey(sender))    // ������ �ְڴ�
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
    // ������ Rpc�� ȣ���ϸ� �츮�� �޽����� ������.
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
    // ���� ���� ���� : OnClick�̶�� ����Ģ�� �����ϰ� �Ǳ� ������
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
            // ���� ���� ���� : OnClick�̶�� ����Ģ�� �����ϰ� �Ǳ� ������
            SendMsg();
        }
    }
}
