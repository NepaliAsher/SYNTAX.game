using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    private int score = 0;
    private bool gameOver = false;
    private UIManager uiManager;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        Time.timeScale = 1f;
    }

    public void AddScore(int points)
    {
        if (!gameOver)
        {
            score += points;
            uiManager.UpdateScore(score);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        Time.timeScale = 0f;
        uiManager.ShowGameOver(score);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public int GetScore() => score;
}