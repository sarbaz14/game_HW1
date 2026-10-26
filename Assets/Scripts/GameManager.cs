using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UIManager uiManager;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (uiManager == null)
        {
            uiManager = Object.FindAnyObjectByType<UIManager>();
        }
    }

    public void UpdateScore(int score)
    {
        if (uiManager != null) uiManager.SetScore(score);
    }

    public void UpdateLives(int lives)
    {
        if (uiManager != null) uiManager.SetLives(lives);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (uiManager != null) uiManager.ShowGameOver(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause()
    {
        if (isGameOver) return;
        bool isPaused = Time.timeScale == 0f;
        Time.timeScale = isPaused ? 1f : 0f;
        if (uiManager != null) uiManager.SetPause(!isPaused);
    }
}
