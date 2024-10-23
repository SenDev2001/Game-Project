using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class M_ScoreManager : MonoBehaviour
{
    public static M_ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    public TMP_Text scoreText; 

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
    }

    private void Start()
    {
        ResetScore(); 
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score.ToString();
        }
        else
        {
            scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
            scoreText.text = "Score: " + Score.ToString();

        }
    }
}
