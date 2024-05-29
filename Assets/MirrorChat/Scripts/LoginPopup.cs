using Mirror;
using Org.BouncyCastle.Asn1.Crmf;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    // 구동을 위해 직렬화 및 연동이 필요한 요소는 대분류와 이름을 명확하게
    [SerializeField] private TMP_InputField Input_NetworkAddress;
    [SerializeField] private TMP_InputField Input_UserName;

    [SerializeField] private Button Btn_StartAsHostServer;
    [SerializeField] private Button Btn_StartAsClient;

    [SerializeField] private Text Text_Error;

    // 직렬화 및 연동이 필요하지만 클래스 내에서 멤버변수로서 사용하는 성향이라면 멤버처럼
    [SerializeField] private NetworkingManager _netManager;
    
    private string m_originNetworkAddress;

    // m_ << 이거 왜 붙임? -> 나는 전역변수 입니다를 알려주기 위해.
    // 전역변수는 지역변수와 다르게 클래스 내 어디서든 접근하고 수정이 가능하므로

    /*
    상단 - 로직을 구동하기 위해 필요한 요소 (Init, Register 등 중요도 순)
    중단 - 중요 로직 (중요도 순)
    하단 - UI관련 상호작용
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
        // 아래와 같이 Update문에서 지속적으로 문자열을 검사하는 것은 부담된다.
        CheckNetworkAddressValidUpdate();
    }


    public string GetUserName() { return  Input_UserName.text; }

    private void SetDefaultNetworkAddress()
    {
        // IsNullOrWhiteSpace / IsNullOrEmpty는 문자열의 널체크
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
        // 멤버변수의 수정이 필요한 것은 변수에 직접 접근하기 보다는 캡슐화. (조류독감 옮음)
        this.gameObject.SetActive(activeself);
    }

    /*
    클릭 - 호스트로 시작
    클릭 - 클라로 시작

    Input - 서버 주소 입력 완료
    Input - 서버 주소 입력 중
    
    Input - 유저 닉네임 입력 완료 (OnSubmit) << 이벤트에는 왜 On이 붙는가? -> 수동적인 녀석 즉, 누군가에 의해서만 호출되는 함수
    Input - 유저 닉네임 입력 중 (OnValueChanged)
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
