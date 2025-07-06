using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public GameObject mailItemPrefab;
    public Transform mailListParent;
    public Inventory inventory;

    private NetworkStream stream;

    void Start()
    {
        if (ChatManager.client != null)
        {
            stream = ChatManager.client.GetStream();
            RequestMailList();
        }
    }

    public void RequestMailList()
    {
        SendToServer("MAIL_REQ");
    }

    void SendToServer(string msg)
    {
        byte[] data = Encoding.UTF8.GetBytes(msg);
        stream.Write(data, 0, data.Length);
    }

    public void ReceiveMailList(string data)
    {
        // 출석 보상|코인 100개;점검 보상|젬 10개
        string[] items = data.Split(';');
        foreach (string item in items)
        {
            string[] split = item.Split('|');
            if (split.Length != 2) continue;

            string title = split[0];
            string reward = split[1];
            AddMail(title, reward);
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
            Debug.Log($"{title} 수령");
            inventory.AddItem(reward);
            SendToServer($"MAIL_RECV:{reward}");
            Destroy(obj);
        });
    }
}
