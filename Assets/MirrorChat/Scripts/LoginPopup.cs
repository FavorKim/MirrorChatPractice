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

    // m_ << �̰� �� ����? -> ���� �������� �Դϴٸ� �˷��ֱ� ����.
    // ���������� ���������� �ٸ��� Ŭ���� �� ��𼭵� �����ϰ� ������ �����ϹǷ�

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
    Ŭ�� - ȣ��Ʈ�� ����
    Ŭ�� - Ŭ��� ����
    Input - ���� �ּ� �Է� �Ϸ�
    Input - ���� �ּ� �Է� ��
    
    Input - ���� �г��� �Է� �Ϸ� (OnSubmit) << �̺�Ʈ���� �� On�� �ٴ°�? -> �������� �༮ ��, �������� ���ؼ��� ȣ��Ǵ� �Լ�
    Input - ���� �г��� �Է� �� (OnValueChanged)
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
