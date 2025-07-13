using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        int killScore = FindAnyObjectByType<GameHUD>().KillScore;
        int timeScore = FindAnyObjectByType<GameHUD>().TimeScore;
        int totalScore = killScore + timeScore;


        gameOverScoreText.text = $"�� ���� : {totalScore}";
    }

    public void OnClickRankingRegister()
    {
        GameHUD hud = Object.FindFirstObjectByType<GameHUD>();
        if (hud == null)
        {
            Debug.LogError("GameHUD�� ã�� �� �����ϴ�.");
            return;
        }

        int score = hud.GetTotalScore();
        TcpRankingClient.SendScore("�÷��̾�1", score);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene("MainScene");
    }
}
