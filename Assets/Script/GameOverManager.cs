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


        gameOverScoreText.text = $"총 점수 : {totalScore}";
    }

    public void OnClickRankingRegister()
    {
        GameHUD hud = Object.FindFirstObjectByType<GameHUD>();
        if (hud == null)
        {
            Debug.LogError("GameHUD를 찾을 수 없습니다.");
            return;
        }

        int score = hud.GetTotalScore();
        TcpRankingClient.SendScore("플레이어1", score);
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
