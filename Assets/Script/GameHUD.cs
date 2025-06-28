
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    public TextMeshProUGUI killScoreText;
    public TextMeshProUGUI timeScoreText;

    private int killScore = 0;
    private int timeScore = 0;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            int secondsPassed = Mathf.FloorToInt(timer);
            AddTimeScore(1000 * secondsPassed);
            timer -= secondsPassed;
        }
    }

    public void AddKillScore(int amount = 100)
    {
        killScore += amount;
        UpdateKillScoreDisplay();
    }

    void AddTimeScore(int amount)
    {
        timeScore += amount;
        UpdateTimeScoreDisplay();
    }

    void UpdateKillScoreDisplay()
    {
        if (killScoreText != null)
            killScoreText.text = killScore.ToString("N0");
    }

    void UpdateTimeScoreDisplay()
    {
        if (timeScoreText != null)
            timeScoreText.text = timeScore.ToString("N0");
    }
}
