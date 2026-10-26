using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Score UI")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    [Header("Lives UI")]
    public Image[] hearts; // 3 heart icons

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverHighScoreText;

    private int lastScore = 0;

    private void Start()
    {
        SetScore(0);
        UpdateHighScore();
        SetPause(false);
        ShowGameOver(false);
    }

    public void SetScore(int score)
    {
        lastScore = score;
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        int high = PlayerPrefs.GetInt("HighScore", 0);
        if (score > high)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            UpdateHighScore();
        }
    }

    public void UpdateHighScore()
    {
        int high = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
            highScoreText.text = "High Score: " + high;
    }

    public void SetLives(int lives)
    {
        if (hearts != null)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] != null)
                    hearts[i].enabled = i < lives;
            }
        }
    }

    public void SetPause(bool paused)
    {
        if (pausePanel != null)
            pausePanel.SetActive(paused);
    }

    public void ShowGameOver(bool show = true)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
            if (show)
            {
                int high = PlayerPrefs.GetInt("HighScore", 0);
                if (gameOverScoreText != null)
                    gameOverScoreText.text = "Your Score: " + lastScore;
                if (gameOverHighScoreText != null)
                    gameOverHighScoreText.text = "Best: " + high;
            }
        }
    }

    public void OnResumeClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TogglePause();
        else
        {
            Time.timeScale = 1f;
            SetPause(false);
        }
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPauseClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TogglePause();
    }
}
