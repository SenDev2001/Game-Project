using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class M_GameManager : MonoBehaviour
{
    public GameObject restartUI;
    public GameObject pauseButton;
    
    private int score = 0;
    private string playerName;
    

    void Start()
    {
        if (restartUI != null)
        {
            restartUI.SetActive(false);
            
        }

        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
        }    
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");

    }

    public void ShowRestartUI()
    {

        if (restartUI != null)
        {
            restartUI.SetActive(true);
            Time.timeScale = 0;
        }
        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        M_ScoreManager scoreManager = FindObjectOfType<M_ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.SubmitScore();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        M_ScoreManager scoreManager = FindObjectOfType<M_ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }


    public void GoToMenu()
    {
        Time.timeScale = 1;

        M_ScoreManager scoreManager = FindObjectOfType<M_ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }
        SceneManager.LoadScene("Menu");
    }

    public void quitGame()
    {

        {
            Debug.Log("Quitting the game...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }
    }
}
