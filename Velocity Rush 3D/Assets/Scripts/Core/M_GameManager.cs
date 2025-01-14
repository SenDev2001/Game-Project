using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class M_GameManager : MonoBehaviour
{
    public GameObject restartUI;
    public GameObject pauseButton;
    
    private int score = 0;
    private string playerName;
    

    void Start()
    {
        // Hide restart UI when the game starts
        if (restartUI != null)
        {
            restartUI.SetActive(false);
            
        }
        // Show the pause button when the game starts
        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
        }
        //Get player's name from saved data
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
    }

   // restart UI and pauses the game
    public void ShowRestartUI()
    {

        if (restartUI != null)
        {
            restartUI.SetActive(true);
            Time.timeScale = 0;
        }
        // Hide pause button when the game is paused
        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }
        // Submit the score if the score manager exists
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

    // This adds points to the score
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
