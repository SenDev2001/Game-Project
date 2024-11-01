using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public TMP_Text scoreText;

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
            if (scoreText != null)
            {
                scoreText.text = "Score: " + Score.ToString();
            }
        }
    }
}
