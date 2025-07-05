using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailManager : MonoBehaviour
{
    [Header("프리팹 및 참조")]
    public GameObject mailItemPrefab;       
    public Transform mailListParent;       
    public Inventory inventory;             

    void Start()
    {
        // 테스트용 더미 우편 추가
        AddMail("출석 보상", "코인 100개");
        AddMail("점검 보상", "젬 10개");
    }

    public void AddMail(string title, string reward)
    {
        GameObject item = Instantiate(mailItemPrefab, mailListParent);

        // 텍스트 설정 (RewardText를 찾거나 모든 TextMeshProUGUI 중 첫 번째 사용)
        var text = item.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = $"{title} - {reward}";
        else
            Debug.LogWarning("RewardText를 찾을 수 없습니다.");

        // 버튼 설정 (GetButton 찾아서 클릭 시 아이템 수령)
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
            Debug.LogWarning("GetButton을 찾을 수 없습니다.");
        }
    }
}
