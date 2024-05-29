using Mirror;
using Org.BouncyCastle.Asn1.Crmf;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    // ������ ���� ����ȭ �� ������ �ʿ��� ��Ҵ� ��з��� �̸��� ��Ȯ�ϰ�
    [SerializeField] private TMP_InputField Input_NetworkAddress;
    [SerializeField] private TMP_InputField Input_UserName;

    [SerializeField] private Button Btn_StartAsHostServer;
    [SerializeField] private Button Btn_StartAsClient;

    [SerializeField] private Text Text_Error;

    // ����ȭ �� ������ �ʿ������� Ŭ���� ������ ��������μ� ����ϴ� �����̶�� ���ó��
    [SerializeField] private NetworkingManager _netManager;
    
    private string m_originNetworkAddress;

    // m_ << �̰� �� ����? -> ���� �������� �Դϴٸ� �˷��ֱ� ����.
    // ���������� ���������� �ٸ��� Ŭ���� �� ��𼭵� �����ϰ� ������ �����ϹǷ�

    /*
    ��� - ������ �����ϱ� ���� �ʿ��� ��� (Init, Register �� �߿䵵 ��)
    �ߴ� - �߿� ���� (�߿䵵 ��)
    �ϴ� - UI���� ��ȣ�ۿ�
    */

    private void Awake()
    {
        SetDefaultNetworkAddress();
        Text_Error.gameObject.SetActive(false);
        Btn_StartAsHostServer.interactable = false;
        Btn_StartAsClient.interactable = false;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        // �Ʒ��� ���� Update������ ���������� ���ڿ��� �˻��ϴ� ���� �δ�ȴ�.
        CheckNetworkAddressValidUpdate();
    }


    public string GetUserName() { return  Input_UserName.text; }

    private void SetDefaultNetworkAddress()
    {
        // IsNullOrWhiteSpace / IsNullOrEmpty�� ���ڿ��� ��üũ
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "localhost";
        }
        m_originNetworkAddress = NetworkManager.singleton.networkAddress;
    }

    private void CheckNetworkAddressValidUpdate()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = m_originNetworkAddress;
        }

        if(Input_NetworkAddress.text != NetworkManager.singleton.networkAddress)
        {
            Input_NetworkAddress.text = NetworkManager.singleton.networkAddress;
        }
    }

    private void LoginPopupSelfOpenClose(bool activeself)
    {
        // ��������� ������ �ʿ��� ���� ������ ���� �����ϱ� ���ٴ� ĸ��ȭ. (�������� ����)
        this.gameObject.SetActive(activeself);
    }

    /*
    Ŭ�� - ȣ��Ʈ�� ����
    Ŭ�� - Ŭ��� ����

    Input - ���� �ּ� �Է� �Ϸ�
    Input - ���� �ּ� �Է� ��
    
    Input - ���� �г��� �Է� �Ϸ� (OnSubmit) << �̺�Ʈ���� �� On�� �ٴ°�? -> �������� �༮ ��, �������� ���ؼ��� ȣ��Ǵ� �Լ�
    Input - ���� �г��� �Է� �� (OnValueChanged)
    */

    public void SetUIOnClientDisconnected()
    {
        LoginPopupSelfOpenClose(true);
        Input_UserName.text = string.Empty;
        Input_UserName.ActivateInputField();
    }

    public void SetUIOnAuthValueChanged()
    {
        Text_Error.text = string.Empty;
        Text_Error.gameObject.SetActive(false);
    }

    public void OnClick_StartAsHostServer()
    {
        if(_netManager == null)
        {
            return;
        }

        _netManager.StartHost();

        LoginPopupSelfOpenClose(false);

    }

    public void OnClick_StartAsClient()
    {
        if (_netManager == null) return;

        _netManager.StartClient();

        LoginPopupSelfOpenClose(false);
    }

    public void OnValueChanged_ToggleBtn(string userName)
    {
        bool isUserNameValid = !string.IsNullOrEmpty(Input_UserName.text);
        Btn_StartAsHostServer.interactable = isUserNameValid;
        Btn_StartAsClient.interactable = isUserNameValid;
    }

    public void SetUIOnAuthError(string msg)
    {
        Text_Error.text = msg;
        Text_Error.gameObject.SetActive(true);
    }
}
