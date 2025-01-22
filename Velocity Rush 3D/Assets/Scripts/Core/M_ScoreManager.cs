using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class M_ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public TMP_Text scoreText;
    public TMP_Text leaderboardText;

    private string addScoreUrl = "https://sendev2001.pythonanywhere.com/addscore";
    private string leaderboardUrl = "https://sendev2001.pythonanywhere.com/leaderboard";

    private string playerName;

    private void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
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
    }

    public void SubmitScore()
    {
        StartCoroutine(AddScoreToAPI(playerName, Score));
    }

    private IEnumerator AddScoreToAPI(string name, int score)
    {
        string json = BuildScoreJson(name, score);
        UnityWebRequest request = CreateWebRequest(json);
        yield return request.SendWebRequest();

        HandleAddScoreResponse(request);
    }

    private string BuildScoreJson(string name, int score)
    {
        return "{\"name\": \"" + name + "\", \"score\": " + score + "}";
    }

    private UnityWebRequest CreateWebRequest(string json)
    {
        UnityWebRequest request = new UnityWebRequest(addScoreUrl + "?timestamp=" + Time.time, "POST"); // Added timestamp for cache busting
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private void HandleAddScoreResponse(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.Success)
        {
            LogError(request);
            return;
        }

        Debug.Log("Score added successfully!");
        StartCoroutine(GetLeaderboard());
    }

    private void LogError(UnityWebRequest request)
    {
        Debug.LogError("Failed to add score: " + request.error);
        Debug.LogError("Response: " + request.downloadHandler.text);

        if (leaderboardText != null)
        {
            leaderboardText.text = "\nTry Again!\nNo Coins Collected";
        }
    }

    private IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl + "?timestamp=" + Time.time);  // Added timestamp for cache busting
        yield return request.SendWebRequest();

        HandleGetLeaderboardResponse(request);
    }

    private void HandleGetLeaderboardResponse(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.Success)
        {
            HandleLeaderboardError(request);
            return;
        }

        DisplayLeaderboard(request.downloadHandler.text);
    }

    private void HandleLeaderboardError(UnityWebRequest request)
    {
        if (leaderboardText != null)
        {
            leaderboardText.text = "Error fetching leaderboard: " + request.error;
        }
    }

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

    private string FormatLeaderboardResponse(string jsonResponse)
    {
        if (jsonResponse.StartsWith("["))
        {
            jsonResponse = "{\"leaderboardEntries\":" + jsonResponse + "}";
        }

        return jsonResponse;
    }

    private string BuildLeaderboardDisplay(LeaderboardResponse leaderboardResponse)
    {
        string leaderboardDisplay = "\n";
        foreach (LeaderboardEntry entry in leaderboardResponse.leaderboardEntries)
        {
            leaderboardDisplay += entry.name + ": " + entry.score + "\n";
        }

        return leaderboardDisplay;
    }

    public void RestartGame()
    {
        SubmitScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class LeaderboardResponse
{
    public LeaderboardEntry[] leaderboardEntries;
}
