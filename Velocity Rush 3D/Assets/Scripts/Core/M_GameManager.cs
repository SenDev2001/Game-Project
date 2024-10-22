using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class M_GameManager : MonoBehaviour
{
    public static M_GameManager Instance { get; private set; }

    public GameObject restartUI; // Assign your Restart UI GameObject here
    public TMP_Text scoreText; // Assign a Text UI element to display the score

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (restartUI != null)
        {
            restartUI.SetActive(false); // Ensure the UI is hidden at start
        }
    }

    private void Start()
    {
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
}
