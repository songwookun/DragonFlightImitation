using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    [Header("UI")]
    public GameObject chatOverlay;
    public Button toggleOverlayButton;
    public Button closeButton;

    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI previewText;

    public Transform chatContent;
    public GameObject chatTextPrefab;

    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UnityMainThreadDispatcher.Instance();

        ConnectToChatServer();

        if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);

        if (toggleOverlayButton != null)
            toggleOverlayButton.onClick.AddListener(() => chatOverlay.SetActive(true));

        if (closeButton != null)
            closeButton.onClick.AddListener(() => chatOverlay.SetActive(false));
    }

    void OnDestroy()
    {
        stream?.Close();
        client?.Close();
        receiveThread?.Abort();
    }

    void ConnectToChatServer()
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
            Debug.LogWarning("채팅 서버 연결 실패");
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
            catch { break; }

            if (bytesRead > 0)
            {
                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (msg.StartsWith("CHAT:"))
                {
                    string content = msg.Substring("CHAT:".Length);
                    string[] split = content.Split(new[] { ':' }, 2);
                    if (split.Length == 2)
                    {
                        string sender = split[0];
                        string message = split[1];

                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            AddChatLine(sender, message);
                        });
                    }
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
            string full = $"CHAT:나:{msg}";
            byte[] data = Encoding.UTF8.GetBytes(full);
            stream.Write(data, 0, data.Length);
        }
    }

    public void AddChatLine(string sender, string message)
    {
        string full = $"{sender}: {message}";
        GameObject chatItem = Instantiate(chatTextPrefab, chatContent);
        TextMeshProUGUI text = chatItem.GetComponent<TextMeshProUGUI>();
        if (text != null)
            text.text = full;

        if (previewText != null)
            previewText.text = full;
    }
}
