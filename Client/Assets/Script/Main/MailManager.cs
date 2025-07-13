using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public static MailManager Instance;

    public GameObject mailItemPrefab;
    public Transform mailListParent;
    public Inventory inventory;

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
        ConnectToMailServer();
    }

    void OnDestroy()
    {
        stream?.Close();
        client?.Close();
        receiveThread?.Abort();
    }

    void ConnectToMailServer()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 7778);
            stream = client.GetStream();
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            RequestMailList(); 
        }
        catch
        {
            Debug.LogWarning("우편 서버 연결 실패");
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
                Debug.Log("[우편 응답 수신] " + msg);

                if (msg.StartsWith("MAIL:"))
                {
                    string data = msg.Substring("MAIL:".Length);
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        DisplayMailList(data);
                    });
                }
            }
        }
    }

    void RequestMailList()
    {
        Send("MAIL_REQ");
    }

    public void Send(string msg)
    {
        if (stream != null && stream.CanWrite)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }
    }

    void DisplayMailList(string data)
    {
        string[] items = data.Split(';');
        foreach (string item in items)
        {
            string[] split = item.Split('|');
            if (split.Length == 2)
                AddMail(split[0], split[1]);
        }
    }

    public void AddMail(string title, string reward)
    {
        GameObject obj = Instantiate(mailItemPrefab, mailListParent);
        TMP_Text[] texts = obj.GetComponentsInChildren<TMP_Text>();
        Button getButton = obj.GetComponentInChildren<Button>();

        texts[0].text = title;
        texts[1].text = reward;

        getButton.onClick.AddListener(() =>
        {
            inventory.AddItem(reward);
            Send($"MAIL_RECV:{reward}");
            Destroy(obj);
        });
    }
}
