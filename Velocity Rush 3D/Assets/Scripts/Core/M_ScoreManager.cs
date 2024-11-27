using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

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

            
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score added successfully!");
                
                StartCoroutine(GetLeaderboard());
            }
            else
            {
               
                Debug.LogError("Failed to add score: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);

                
                if (leaderboardText != null)
                {
                    leaderboardText.text = "Error submitting score: " + request.error;
                }
            }
        }
    }

    IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;

                // Here we manually wrap the JSON array in a root object if needed
                if (jsonResponse.StartsWith("["))
                {
                    jsonResponse = "{\"leaderboardEntries\":" + jsonResponse + "}";
                }

                // Deserialize the JSON response into a LeaderboardResponse object
                LeaderboardResponse leaderboardResponse = JsonUtility.FromJson<LeaderboardResponse>(jsonResponse);

                // Build a formatted string for the leaderboard
                string leaderboardDisplay = "Leaderboard:\n";
                foreach (LeaderboardEntry entry in leaderboardResponse.leaderboardEntries)
                {
                    leaderboardDisplay += entry.name + ": " + entry.score + "\n";
                }

                // Update the leaderboard text
                if (leaderboardText != null)
                {
                    leaderboardText.text = leaderboardDisplay;
                }
            }
            else
            {
                // If an error occurs fetching the leaderboard
                if (leaderboardText != null)
                {
                    leaderboardText.text = "Error fetching leaderboard: " + request.error;
                }
            }
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

