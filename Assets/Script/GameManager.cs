
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameOver { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void EndGame()
    {
        if (!IsGameOver)
        {
            IsGameOver = true;
            Debug.Log("게임 종료!");
            Time.timeScale = 0f;
        }
    }
}
