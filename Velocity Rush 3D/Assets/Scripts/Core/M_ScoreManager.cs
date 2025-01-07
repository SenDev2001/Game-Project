using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class M_ScoreManager : MonoBehaviour
{
    // The current player's score
    public int Score { get; private set; }

    // UI elements for displaying the score and leaderboard
    public TMP_Text scoreText;
    public TMP_Text leaderboardText;

    // URLs for interacting with the backend API
    private string addScoreUrl = "https://sendev2001.pythonanywhere.com/addscore";
    private string leaderboardUrl = "https://sendev2001.pythonanywhere.com/leaderboard";

    // Player's name (loaded from PlayerPrefs)
    private string playerName;

    // Initialize the manager and set up player data
    private void Start()
    {
        // Load player name or default to "Guest"
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        // Reset the score to start the game fresh
        ResetScore();
    }

    // Reset the score to zero and update the display
    public void ResetScore()
    {
        Score = 0;
        UpdateScoreText();
    }

    // Add a specified amount to the score and update the display
    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScoreText();
    }

    // Update the score displayed in the UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score.ToString();
        }
    }

    // Submit the player's score to the backend API
    public void SubmitScore()
    {
        // Start the coroutine to submit the score to the server
        StartCoroutine(AddScoreToAPI(playerName, Score));
    }

    // Coroutine to send the score to the backend API
    private IEnumerator AddScoreToAPI(string name, int score)
    {
        string json = BuildScoreJson(name, score);
        Debug.Log("Sending JSON: " + json);

        UnityWebRequest request = CreateWebRequest(json);
        yield return request.SendWebRequest();

        // Handle the response from the server after submitting the score
        HandleAddScoreResponse(request);
    }

    // Create a JSON string for the score submission
    private string BuildScoreJson(string name, int score)
    {
        return "{\"name\": \"" + name + "\", \"score\": " + score + "}";
    }

    // Create the UnityWebRequest for submitting the score
    private UnityWebRequest CreateWebRequest(string json)
    {
        UnityWebRequest request = new UnityWebRequest(addScoreUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    // Handle the response after submitting the score
    private void HandleAddScoreResponse(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.Success)
        {
            LogError(request);
            return;
        }

        Debug.Log("Score added successfully!");
        // After adding the score, fetch the leaderboard
        StartCoroutine(GetLeaderboard());
    }

    // Log errors if the score submission fails
    private void LogError(UnityWebRequest request)
    {
        Debug.LogError("Failed to add score: " + request.error);
        Debug.LogError("Response: " + request.downloadHandler.text);

        if (leaderboardText != null)
        {
            leaderboardText.text = "Error submitting score: " + request.error;
        }
    }

    // Coroutine to fetch the leaderboard data
    private IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
        yield return request.SendWebRequest();

        // Handle the response for the leaderboard data
        HandleGetLeaderboardResponse(request);
    }

    // Handle the response after fetching the leaderboard data
    private void HandleGetLeaderboardResponse(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.Success)
        {
            HandleLeaderboardError(request);
            return;
        }

        // Display the leaderboard if the data was successfully fetched
        DisplayLeaderboard(request.downloadHandler.text);
    }

    // Handle any errors that occur while fetching the leaderboard
    private void HandleLeaderboardError(UnityWebRequest request)
    {
        if (leaderboardText != null)
        {
            leaderboardText.text = "Error fetching leaderboard: " + request.error;
        }
    }

    // Parse and display the leaderboard from the JSON response
    private void DisplayLeaderboard(string jsonResponse)
    {
        string formattedResponse = FormatLeaderboardResponse(jsonResponse);
        LeaderboardResponse leaderboardResponse = JsonUtility.FromJson<LeaderboardResponse>(formattedResponse);
        string leaderboardDisplay = BuildLeaderboardDisplay(leaderboardResponse);

        if (leaderboardText != null)
        {
            leaderboardText.text = leaderboardDisplay;
        }
    }

    // Ensure the JSON response is in the correct format
    private string FormatLeaderboardResponse(string jsonResponse)
    {
        if (jsonResponse.StartsWith("["))
        {
            jsonResponse = "{\"leaderboardEntries\":" + jsonResponse + "}";
        }

        return jsonResponse;
    }

    // Build a string to display the leaderboard
    private string BuildLeaderboardDisplay(LeaderboardResponse leaderboardResponse)
    {
        string leaderboardDisplay = "\n";
        foreach (LeaderboardEntry entry in leaderboardResponse.leaderboardEntries)
        {
            leaderboardDisplay += entry.name + ": " + entry.score + "\n";
        }

        return leaderboardDisplay;
    }

    // Restart the game and submit the score
    public void RestartGame()
    {
        SubmitScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}

// Class representing a leaderboard entry (name and score)
[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;
}

// Class for the leaderboard response (array of entries)
[System.Serializable]
public class LeaderboardResponse
{
    public LeaderboardEntry[] leaderboardEntries;
}
