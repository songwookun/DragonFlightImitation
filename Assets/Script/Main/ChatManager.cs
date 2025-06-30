using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject chatOverlay;                      // 전체 채팅 패널
    public Button toggleOverlayButton;                  // 말풍선 열기 버튼
    public Button closeButton;                          // 닫기 버튼

    public TMP_InputField inputField;                   // 입력창
    public Button sendButton;                           // "입력" 버튼
    public TextMeshProUGUI previewText;                 // 상단 최근 메시지 미리보기

    [Header("채팅 로그")]
    public Transform chatContent;                       // ScrollView > Content
    public GameObject chatTextPrefab;                   // 생성할 채팅 Text 프리팹

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

        AddChatLine("나", msg);
        inputField.text = "";

        // TODO: 나중에 서버로 메시지 전송
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

    // 나중에 서버에서 메시지를 받았을 때 호출
    public void OnReceiveFromServer(string sender, string message)
    {
        AddChatLine(sender, message);
    }
}
