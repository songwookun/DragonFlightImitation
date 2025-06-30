using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject chatOverlay;                      // ��ü ä�� �г�
    public Button toggleOverlayButton;                  // ��ǳ�� ���� ��ư
    public Button closeButton;                          // �ݱ� ��ư

    public TMP_InputField inputField;                   // �Է�â
    public Button sendButton;                           // "�Է�" ��ư
    public TextMeshProUGUI previewText;                 // ��� �ֱ� �޽��� �̸�����

    [Header("ä�� �α�")]
    public Transform chatContent;                       // ScrollView > Content
    public GameObject chatTextPrefab;                   // ������ ä�� Text ������

    private List<string> chatHistory = new List<string>();

    void Start()
    {
        if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);

        if (toggleOverlayButton != null)
            toggleOverlayButton.onClick.AddListener(() => chatOverlay.SetActive(true));

        if (closeButton != null)
            closeButton.onClick.AddListener(() => chatOverlay.SetActive(false));
    }

    public void SendMessage()
    {
        string msg = inputField.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;

        AddChatLine("��", msg);
        inputField.text = "";

        // TODO: ���߿� ������ �޽��� ����
        // SendToServer(msg);
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

    // ���߿� �������� �޽����� �޾��� �� ȣ��
    public void OnReceiveFromServer(string sender, string message)
    {
        AddChatLine(sender, message);
    }
}
