using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    public GameObject gameReadyPanel;
    public TMP_Dropdown modeDropdown;

    public void OpenGameReadyPanel()
    {
        gameReadyPanel.SetActive(true);
    }

    public void StartGame()
    {
        int selectedIndex = modeDropdown.value;
        if (selectedIndex == 0)
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (selectedIndex == 1)
        {
            SceneManager.LoadScene("HardScene");
        }
    }
}
