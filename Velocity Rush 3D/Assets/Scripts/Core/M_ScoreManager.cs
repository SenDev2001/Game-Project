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

    IEnumerator AddScoreToAPI(string name, int score)
    {
        string json = "{\"name\": \"" + name + "\", \"score\": " + score + "}";
        Debug.Log("Sending JSON: " + json);

        using (UnityWebRequest request = new UnityWebRequest(addScoreUrl, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleScoreSubmissionFailure(request.error);
                yield break;
            }

            Debug.Log("Score added successfully!");
            StartCoroutine(GetLeaderboard());
        }
    }

    private void HandleScoreSubmissionFailure(string error)
    {
        Debug.LogError("Failed to add score: " + error);
        if (leaderboardText != null)
        {
            leaderboardText.text = "ohh no You fail Restart the game ";
        }
    }

    IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                HandleLeaderboardFailure(request.error);
                yield break;
            }

            ProcessLeaderboardResponse(request.downloadHandler.text);
        }
    }

    private void ProcessLeaderboardResponse(string jsonResponse)
    {
        if (jsonResponse.StartsWith("["))
        {
            jsonResponse = "{\"leaderboardEntries\":" + jsonResponse + "}";
        }

        LeaderboardResponse leaderboardResponse = JsonUtility.FromJson<LeaderboardResponse>(jsonResponse);

        string leaderboardDisplay = "\n";
        foreach (LeaderboardEntry entry in leaderboardResponse.leaderboardEntries)
        {
            leaderboardDisplay += entry.name + ": " + entry.score + "\n";
        }

        if (leaderboardText != null)
        {
            leaderboardText.text = leaderboardDisplay;
        }
    }

    private void HandleLeaderboardFailure(string error)
    {
        Debug.LogError("Error fetching leaderboard: " + error);
        if (leaderboardText != null)
        {
            leaderboardText.text = "Error fetching leaderboard: " + error;
        }
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
