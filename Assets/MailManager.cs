using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailManager : MonoBehaviour
{
    [Header("������ �� ����")]
    public GameObject mailItemPrefab;       
    public Transform mailListParent;       
    public Inventory inventory;             

    void Start()
    {
        // �׽�Ʈ�� ���� ���� �߰�
        AddMail("�⼮ ����", "���� 100��");
        AddMail("���� ����", "�� 10��");
    }

    public void AddMail(string title, string reward)
    {
        GameObject item = Instantiate(mailItemPrefab, mailListParent);

        // �ؽ�Ʈ ���� (RewardText�� ã�ų� ��� TextMeshProUGUI �� ù ��° ���)
        var text = item.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = $"{title} - {reward}";
        else
            Debug.LogWarning("RewardText�� ã�� �� �����ϴ�.");

        // ��ư ���� (GetButton ã�Ƽ� Ŭ�� �� ������ ����)
        var button = item.GetComponentInChildren<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                inventory.AddItem(reward);    
                Destroy(item);               
            });
        }
        else
        {
            Debug.LogWarning("GetButton�� ã�� �� �����ϴ�.");
        }
    }
}
