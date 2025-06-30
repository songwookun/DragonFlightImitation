using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    public GameObject gameReadyPanel;

    public void OpenGameReadyPanel()
    {
        gameReadyPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
