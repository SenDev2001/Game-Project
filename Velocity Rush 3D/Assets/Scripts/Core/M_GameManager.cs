using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class M_GameManager : MonoBehaviour
{

    public GameObject restartUI; 
    public TMP_Text scoreText;

    private int score = 0;

   private void Start()
    {
        if (restartUI != null)
        {
            restartUI.SetActive(false); 
        }
        UpdateScoreText();
    }
    public void ShowRestartUI()
    {
        if (restartUI != null)
        {
            restartUI.SetActive(true);
            Time.timeScale = 0; // Pause the game
        }
         

    }

        public void RestartGame()
        {
            Time.timeScale = 1; // Resume the game
            M_ScoreManager.Instance.ResetScore(); // Reset the score
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        public void QuitGame()
        {
            Application.Quit();
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
    private void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}

