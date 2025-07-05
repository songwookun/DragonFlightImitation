using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject chatOverlay;
    public Button toggleOverlayButton;
    public Button closeButton;

    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI previewText;

    [Header("ä�� �α�")]
    public Transform chatContent;
    public GameObject chatTextPrefab;

    private List<string> chatHistory = new List<string>();

    // TCP ���� ���� ����
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    void Start()
    {

        if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);

        if (toggleOverlayButton != null)
            toggleOverlayButton.onClick.AddListener(() => chatOverlay.SetActive(true));

        if (closeButton != null)
            closeButton.onClick.AddListener(() => chatOverlay.SetActive(false));

        ConnectToServer(); // ���� ����
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
            client = new TcpClient("127.0.0.1", 7777); // ���� IP, ��Ʈ
            stream = client.GetStream();
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        catch
        {
            Debug.LogWarning("������ ������ �� �����ϴ�.");
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
                string[] split = msg.Split(new[] { ':' }, 2);
                if (split.Length == 2)
                {
                    string sender = split[0].Trim();
                    string message = split[1].Trim();
                    UnityMainThreadDispatcher.Instance().Enqueue(() => OnReceiveFromServer(sender, message));
                }
            }
        }
    }

    public void SendMessage()
    {
        string msg = inputField.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;

        AddChatLine("��", msg);
        inputField.text = "";

        if (stream != null && stream.CanWrite)
        {
            string full = "��:" + msg;
            byte[] data = Encoding.UTF8.GetBytes(full);
            stream.Write(data, 0, data.Length);
        }
    }

    public void AddChatLine(string sender, string message)
    {
        string full = $"{sender}: {message}";
        chatHistory.Add(full);

        GameObject chatItem = Instantiate(chatTextPrefab, chatContent);
        TextMeshProUGUI textComp = chatItem.GetComponent<TextMeshProUGUI>();
        if (textComp != null)
            textComp.text = full;

        if (previewText != null)
            previewText.text = full;
    }

    public void OnReceiveFromServer(string sender, string message)
    {
        AddChatLine(sender, message);
    }
}
