using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class GameController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text playerNameText;
    public int score = 0;

    private string playerName;
    private const string API_URL = "http://localhost:5003";

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        score = 0;
        UpdateScoreText();
        StartCoroutine(GameSimulation());
    }

    private IEnumerator GameSimulation()
    {
        yield return new WaitForSeconds(5);
        score = 1500;
        UpdateScoreText();
        ShowGameOverScreen();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
        playerNameText.text = "Name: " + playerName;
    }

    private void ShowGameOverScreen()
    {
        PlayerPrefs.SetInt("PlayerScore", score);
        StartCoroutine(SendScoreToAPI(playerName, score));
        SceneManager.LoadScene("RestartUI");
    }

    private IEnumerator SendScoreToAPI(string name, int score)
    {
        string url = API_URL + "/addscore";
        string json = JsonUtility.ToJson(new PlayerScore(name, score));

        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score successfully added to the leaderboard!");
        }
        else
        {
            Debug.LogError("Error adding score: " + request.error);
        }
    }
}

[System.Serializable]
public class PlayerScore
{
    public string name;
    public int score;

    public PlayerScore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
