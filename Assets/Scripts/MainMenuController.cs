using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text highScoreText;

    private void Start()
    {
        UpdateHighScore();
    }

    private void OnEnable()
    {
        UpdateHighScore();
    }

    public void UpdateHighScore()
    {
        int high = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + high;
        }
        else
        {
            // Fallback: search for HighScore object in scene if not assigned
            var go = GameObject.Find("HighScore");
            if (go != null)
            {
                var tmp = go.GetComponent<TMP_Text>();
                if (tmp != null)
                {
                    tmp.text = "High Score: " + high;
                }
            }
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
