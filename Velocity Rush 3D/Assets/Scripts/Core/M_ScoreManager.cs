using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class M_ScoreManager : MonoBehaviour
{
    // Score variables and UI
    public int Score { get; private set; }
    public TMP_Text scoreText;             // Display current score
    public TMP_Text leaderboardText;       // Display leaderboard

    // Flask API URLs
    private string addScoreUrl = "http://sendev2001.pythonanywhere.com/addscore";
    private string leaderboardUrl = "http://sendev2001.pythonanywhere.com/leaderboard";

    private string playerName;

    private void Start()
    {
        // Get the player name from PlayerPrefs
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");

        ResetScore();
    }

    // Reset the score to 0
    public void ResetScore()
    {
        Score = 0;
        UpdateScoreText();
    }

    // Add score to the current score
    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScoreText();
    }

    // Update the score display
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score.ToString();
        }
    }

    // Method to submit the player's score and name to the Flask API
    public void SubmitScore()
    {
        // Send the player's name and score to the Flask API
        StartCoroutine(AddScoreToAPI(playerName, Score));
    }

    // Coroutine to send the player's score and name to the Flask backend
    IEnumerator AddScoreToAPI(string name, int score)
    {
        // Create JSON data with player name and score
        string json = "{\"name\": \"" + name + "\", \"score\": " + score + "}";

        // Create UnityWebRequest for POST
        using (UnityWebRequest request = new UnityWebRequest(addScoreUrl, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set content-type to application/json
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score added successfully!");
                StartCoroutine(GetLeaderboard());
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    // Coroutine to fetch the leaderboard from the Flask API
    IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse the response and display leaderboard
                string jsonResponse = request.downloadHandler.text;
                leaderboardText.text = "Leaderboard:\n" + jsonResponse;
            }
            else
            {
                leaderboardText.text = "Error: " + request.error;
            }
        }
    }

    // Restart the game when the player dies or chooses to restart
    public void RestartGame()
    {
        // Submit the score before restarting
        SubmitScore();

        // Reload the Game Scene (or a new one)
        SceneManager.LoadScene("GameScene"); // Or you could use "MainMenu" to go back to the menu
    }
}