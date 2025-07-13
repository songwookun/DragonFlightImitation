using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class StartGameButton : MonoBehaviour
{
    public TMP_Dropdown modeDropdown; 

    public void OnStartButtonClicked()
    {
        int selectedIndex = modeDropdown.value;

        switch (selectedIndex)
        {
            case 0: 
                SceneManager.LoadScene("GameScene");
                break;
            case 1: 
                SceneManager.LoadScene("HardScene");
                break;
            default:
                Debug.LogWarning("알 수 없는 모드 선택됨");
                break;
        }
    }
}
