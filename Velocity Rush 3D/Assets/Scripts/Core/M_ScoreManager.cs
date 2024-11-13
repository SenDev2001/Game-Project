using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class M_ScoreManager : MonoBehaviour
{
    
    public int Score { get; private set; }
    public TMP_Text scoreText;            
    public TMP_InputField playerNameInput;
    public TMP_Text leaderboardText;      

    
    private string addScoreUrl = "http://sendev2001.pythonanywhere.com/addscore";
    private string leaderboardUrl = "http://sendev2001.pythonanywhere.com/leaderboard";

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

    
    public void SubmitScore()
    {
        string playerName = playerNameInput.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player name is required!");
            return;
        }

        
        StartCoroutine(AddScoreToAPI(playerName, Score));
    }

    
    IEnumerator AddScoreToAPI(string name, int score)
    {
       
        string json = "{\"name\": \"" + name + "\", \"score\": " + score + "}";

        
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
                
                Debug.LogError("Error: " + request.error);
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
                leaderboardText.text = "Leaderboard:\n" + jsonResponse;
            }
            else
            {
                leaderboardText.text = "Error: " + request.error;
            }
        }
    }
}
