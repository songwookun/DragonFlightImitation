using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Text;

public class LoginManager : MonoBehaviour
{
    [Header("�г�")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("�α���")]
    public TMP_InputField loginIdInput;
    public TMP_InputField loginPwInput;
    public Button loginButton;

    [Header("ȸ������")]
    public TMP_InputField registerIdInput;
    public TMP_InputField registerPwInput;
    public TMP_InputField registerNickInput;
    public Button registerButton;

    [Header("�г��� ���")]
    public TextMeshProUGUI nicknameText;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void OnLoginClicked()
    {
        string id = loginIdInput.text.Trim();
        string pw = loginPwInput.text.Trim();

        if (id == "" || pw == "") return;

        string msg = $"login:{id},{pw}";
        SendToServer(msg, OnLoginResponse);
        SceneManager.LoadScene("MainScene");
    }

    public void OnRegisterClicked()
    {
        string id = registerIdInput.text.Trim();
        string pw = registerPwInput.text.Trim();
        string nick = registerNickInput.text.Trim();

        if (id == "" || pw == "" || nick == "") return;

        string msg = $"register:{id},{pw},{nick}";
        SendToServer(msg, OnRegisterResponse);
    }

    void SendToServer(string message, System.Action<string> callback)
    {
        TcpClient client = new TcpClient();
        client.BeginConnect("127.0.0.1", 6000, result =>
        {
            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string res = Encoding.UTF8.GetString(buffer, 0, bytes);

                stream.Close();
                client.Close();

                UnityMainThreadDispatcher.Instance().Enqueue(() => callback(res));
            }
        }, null);
    }

    public void OnLoginResponse(string res)
    {
        if (res.StartsWith("LOGIN_SUCCESS:"))
        {
            string nickname = res.Substring("LOGIN_SUCCESS:".Length);
            nicknameText.text = $"ȯ���մϴ�, {nickname}��!";
            loginPanel.SetActive(false);
        }
        else
        {
            nicknameText.text = "�α��� ����!";
        }
    }

    public void OnRegisterResponse(string res)
    {
        if (res == "REGISTER_SUCCESS")
        {
            nicknameText.text = "ȸ������ ����! �α������ּ���.";
            ShowLoginPanel();
        }
        else
        {
            nicknameText.text = "�̹� �����ϴ� ���̵��Դϴ�.";
        }
    }
}
