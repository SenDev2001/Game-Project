using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class M_GameManager : MonoBehaviour
{
    public GameObject restartUI;     
    public TMP_Text scoreText;        
    public TMP_Text playerNameText;   
    private int score = 0;

    private string playerName;

    void Start()
    {
        if (restartUI != null)
        {
            restartUI.SetActive(false);
        }

        
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");

        
        UpdateScoreText();
    }

    public void ShowRestartUI()
    {
        
        if (restartUI != null)
        {
            restartUI.SetActive(true);
            Time.timeScale = 0;
        }

       
        if (playerNameText != null)
        {
            playerNameText.text = "Player: " + playerName; 
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; 
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
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
