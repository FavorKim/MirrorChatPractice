using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField Input_NetworkAddress;
    [SerializeField] private TMP_InputField Input_UserName;

    [SerializeField] private Button Btn_StartAsHostServer;
    [SerializeField] private Button Btn_StartAsClient;

    [SerializeField] private Text Text_Error;

    
    private string m_originNetworkAddress;

    // m_ << 이거 왜 붙임? -> 나는 전역변수 입니다를 알려주기 위해.
    // 전역변수는 지역변수와 다르게 클래스 내 어디서든 접근하고 수정이 가능하므로

    private void Awake()
    {
        
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
        
    }

    /*
    클릭 - 호스트로 시작
    클릭 - 클라로 시작
    Input - 서버 주소 입력 완료
    Input - 서버 주소 입력 중
    
    Input - 유저 닉네임 입력 완료 (OnSubmit) << 이벤트에는 왜 On이 붙는가? -> 수동적인 녀석 즉, 누군가에 의해서만 호출되는 함수
    Input - 유저 닉네임 입력 중 (OnValueChanged)
    */

    public void OnClick_StartAsHostServer()
    {

    }

    public void OnClick_StartAsClient()
    {

    }

    public void OnValueChanged_ToggleBtn(string userName)
    {

    }

    
}
