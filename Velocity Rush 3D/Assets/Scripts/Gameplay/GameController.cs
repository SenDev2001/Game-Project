using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class GameController : MonoBehaviour
{
    public TMP_Text scoreText;       // Text to display score
    public TMP_Text playerNameText;  // Text to display player name
    public int score = 0;            // Player score

    private string playerName;
    private const string API_URL = "http://localhost:5003";  // Flask API URL (change if hosted elsewhere)

    void Start()
    {
        // Retrieve player name from PlayerPrefs (set in the Menu Scene)
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");

        // Initialize score and update UI
        score = 0;  // Set initial score
        UpdateScoreText();

        // Simulate gameplay (replace with actual game logic)
        StartCoroutine(GameSimulation());
    }

    private IEnumerator GameSimulation()
    {
        yield return new WaitForSeconds(5);  // Simulate 5 seconds of gameplay
        score = 1500;  // Example score after the player dies

        // Update score UI
        UpdateScoreText();

        // After death, show the player's name and score, and send the data to the backend
        ShowGameOverScreen();
    }

    // Update the score display on the screen
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
        playerNameText.text = "Name: " + playerName;
    }

    // After death, show the game over screen and send the score to the backend
    private void ShowGameOverScreen()
    {
        // Save the player's score to PlayerPrefs
        PlayerPrefs.SetInt("PlayerScore", score);

        // Send the player score to the Flask API
        StartCoroutine(SendScoreToAPI(playerName, score));

        // Load the Restart UI or Game Over screen
        SceneManager.LoadScene("RestartUI");  // Load Restart UI Scene
    }

    // Send the player’s score to the Flask backend API
    private IEnumerator SendScoreToAPI(string name, int score)
    {
        string url = API_URL + "/addscore";  // Flask endpoint

        // Create a JSON object
        string json = JsonUtility.ToJson(new PlayerScore(name, score));

        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
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

// Helper class to format player data
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