using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    public GameObject chatOverlay;
    public Button toggleOverlayButton;
    public Button closeButton;

    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI previewText;

    public Transform chatContent;
    public GameObject chatTextPrefab;

    public static TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    private MailManager mailManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        var dispatcher = UnityMainThreadDispatcher.Instance();

        if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);

        if (toggleOverlayButton != null)
            toggleOverlayButton.onClick.AddListener(() => chatOverlay.SetActive(true));

        if (closeButton != null)
            closeButton.onClick.AddListener(() => chatOverlay.SetActive(false));

        ConnectToServer();
    }

    void OnDestroy()
    {
        stream?.Close();
        client?.Close();
        receiveThread?.Abort();
    }

    void ConnectToServer()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 7777);
            stream = client.GetStream();
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        catch
        {
            Debug.LogWarning("서버에 연결할 수 없습니다.");
        }
    }

    void ReceiveLoop()
    {
        byte[] buffer = new byte[1024];
        while (client.Connected)
        {
            int bytesRead = 0;

            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
            }
            catch
            {
                break;
            }

            if (bytesRead > 0)
            {
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("[서버 수신] " + msg);

                if (msg.StartsWith("CHAT:"))
                {
                    string[] split = msg.Substring("CHAT:".Length).Split(new[] { ':' }, 2);
                    if (split.Length == 2)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AddChatLine(split[0], split[1]));
                }
                else if (msg.StartsWith("MAIL:"))
                {
                    string data = msg.Substring("MAIL:".Length);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        if (mailManager == null)
                            mailManager = FindObjectOfType<MailManager>();

                        mailManager.ReceiveMailList(data);
                    });
                }
            }
        }
    }

    public void SendMessage()
    {
        string msg = inputField.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;

        AddChatLine("나", msg);
        inputField.text = "";

        if (stream != null && stream.CanWrite)
        {
            string full = "CHAT:나:" + msg;
            byte[] data = Encoding.UTF8.GetBytes(full);
            stream.Write(data, 0, data.Length);
        }
    }

    public void AddChatLine(string sender, string message)
    {
        string full = $"{sender}: {message}";

        GameObject chatItem = Instantiate(chatTextPrefab, chatContent);
        TextMeshProUGUI textComp = chatItem.GetComponent<TextMeshProUGUI>();
        if (textComp != null)
            textComp.text = full;

        if (previewText != null)
            previewText.text = full;
    }
}
