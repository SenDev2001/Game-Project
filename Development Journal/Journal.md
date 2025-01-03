# GAME PROJECT (VELOCITY RUSH 3D)
 #### GAME ENGINE PROGRAMING AND DEVELOPMENT 
 #### KURANAGE SENITH MADUSHAN PEREREA 
 #### 2412587

## INTRODUCTION
Velocity Rush is a 3D endless runner game designed for android and webgl and it’s made for the winter season environment and player have run through road while snow falling and avoid obstacles and collecting coins also velocity rush has leaderboard system so players can see names and score what they got and whose got high score.
## RESEARCH 
- I have done research subway surface case study to better understand its gaming mechanics and design components, which may inspire and lead the development of my own game. I looked at key elements including endless runner gameplay the necessity of user input, and the usage dynamic challenges. This case study helped me grasp the fundamental features of a successful mobile game, such as level design, coins, player movements and obstacles which I want to include to my project.
  - References - Subway Surfers case study (s.d.) At: https://unity.com/case-study/subway-surfers (Accessed 03/12/2024).
    
- Also, I have done research Temple Run Documentation to get ideas for my velocity rush game, The documentation game details on gaming mechanics, design aspects, and user engagement methods. This study helped me in better understanding basic aspects such as obstacle design, powerups and level design same as subway surfers.
  - References - Template documentation (2024) At: https://templerun.fandom.com/wiki/Category:Template_documentation (Accessed 03/12/2024).

- I looked at documentation in unity input systems for mobile. The unity input system literature version 1.11.2 provided a detailed description of how to configure and implement touch controllers, multi touch gestures and swipe recognition on mobile devices.
  -  References - Input System | Input System | 1.11.2 (s.d.) At: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.11/manual/index.html (Accessed 03/12/2024).

- I followed YouTube videos to understand swipe detection for my first mobile game velocity rush. These tutorials demonstrated how to create swipe detection for touch and mouse inputs in unity’s new input system. I learnt how to detect directional swipes, analyse input data and combine swipe actions into game mechanism. In addition, I learnt visual upgrades such as trail effects to improve the user experience. This understanding allowed me to develop responsive and accurate swipe controls, resulting in intuitive gameplay. These elements are essential for mobile games, since they increase play interest and correspond with the main dynamics of and endless runner game.
  - References - detect Swipe NEW Input system (touch / mouse) | how to detect swipe in unity with New Input system (2022) At: https://www.youtube.com/watch?v=Xm9_rcmv3UU (Accessed 03/12/2024). Swipe Detection + Trail Effect w/ New Input System - Unity Tutorial (2021) At: https://www.youtube.com/watch?v=XUx_QlJpd0M (Accessed 03/12/2024).Swipe Detection + Trail Effect w/ New Input System - Unity Tutorial - YouTube (s.d.) At: https://www.youtube.com/watch?v=XUx_QlJpd0M (Accessed  12/12/2024).


- I have done research flask documentation to learn how to build a leaderboard system with Flask. I learnt Flask’s lightweight and flexible characteristics, as well as how to configure routes for receiving request and dynamically handle data with templates. I also looked at flask SQL Alchemy for database interaction and learnt how to create Restful APIs for leaderboard functions, Additionally I learnt how to deploy flask apps and secure routs, this study game me the groundwork to create a practical and user-friendly leaderboard system. 
  - References - Welcome to Flask — Flask Documentation (3.1.x) (s.d.) At: https://flask.palletsprojects.com/en/stable/ (Accessed  12/12/2024).


- I studied the Flask-CORS guide and it helped me to allow cross-origin requests in my Flask backend. If the Unity game front-end and Flask backend are maintained in separate domains, cross-origin requests are naturally blocked in browsers. Using CORS(app) helped me get better performance between my game and the backend server.
  - References - Flask-CORS — Flask-Cors 3.0.10 documentation (s.d.) At: https://flask-cors.readthedocs.io/en/latest/ (Accessed 12/12/2024).

- I have done research on how to deploy a flask in pythonanywhere. This guide give detailed information on how to setting up with a flask backend server. I've been taught how to configure WSGI, define environments, and manage database connections. That's all I need to deploy a Flask app, as well as instructions for running the app with ease Deployed the leaderboard server and provided the technical principles necessary to facilitate easy access and interaction with my game. 
  - References - PythonAnywere (2015) Setting up Flask applications on PythonAnywhere. At: https://help.pythonanywhere.com/pages/Flask/ (Accessed  12/12/2024).


- I did research about how to integrate UnityWebRequest with Flask server by watching YouTube video tutorials. The purpose of this study is to understand the methods required to create connections between a game and a Flask-backend server, specifically the requirements for sending and receiving data efficiently. 
  - References - High Score Table with Saving and Loading (Unity Tutorial for Beginners) - YouTube (s.d.) At: https://www.youtube.com/watch?v=iAbaqGYdnyI&t=150s (Accessed 03/12/2024). How to send a request from a Unity game to a Flask application server - YouTube (s.d.) At: https://www.youtube.com/watch?v=6JXYiGWGG-Y (Accessed 03/12/2024). Unity3d - UnityWebRequest Get async tutorial - YouTube (s.d.) At: https://www.youtube.com/watch?v=Yp8uPxEn6Vg&t=136s (Accessed 03/12/2024).

## Game Development Process
### Process Of Completing The task 
- #### Gameplay Design: 
  - I started by researching current endless runner games to learn about effective game mechanism. I concentrated on creating important gameplay elements such as player movement, swipe gestures, obstacle avoidance (add road barriers and busses), and snow partials.
  #### [Game Scene Screenshots] 
- #### Mobile Controls: 
  - I used unity’s new input System to build touch and swipe detection for seamless player interactions. 
#### [Mobile Game Screenshots]
- #### Testing and Feedbacks:
   - I did user tests to identify areas for upgrades, such as Swipe detection difficulties and leaderboard access.
#### [User Testing Videos]
  
### Code Snippets 
- Spawning Roads
```csharp
 private void SpawnRoad()
 {
     int prefabIndex = prefabOrder[currentPrefabIndex];
     GameObject road = Instantiate(roadPrefabs[prefabIndex]);

     road.transform.SetParent(transform);
     road.transform.position = new Vector3(0, 0, spawnZ);

     SpawnSnowAboveRoad(road);

     activeRoads.Enqueue(road);

     spawnZ += roadLength;
     currentPrefabIndex = (currentPrefabIndex + 1) % prefabOrder.Length;
```
The roads and snow are spawned in an Endless Runner game plays a very important role in keeping the player on a continuous journey and creating a lively environment. Spawning roads helps to keep the player on a continuous journey, and adding elements like snow makes the environment look lively and attractive. Together, these two processes help to maintain the great experience in my velocity rush game. Accordingly, the way roads are spawned is a key component of the Endless Runner game, making the game seem like a long and continuous journey.

- Score and Leaderboard manager
  ```csharp
  
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
            else
            {
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
  ```
  This code snippet the method of managing player scores, sending them to an API over the Internet, and retrieving leaderboard information for an advanced game played on Unity. The process of preserving the player's score information at all times, adding new scores, and then sending them to the API is successfully implemented. This method allows the player to add scores using the "AddScore" method, send them to the API using the "SubmitScore" method, and retrieve leaderboard information from the Internet using the "GetLeaderboard" method. Also, the "RestartGame" method provides a process for updating the player's score and restarting the game. Successfully, this code uses the Unity WebRequest environment to interface with the API, allowing for online scoring and leaderboard display during gameplay, providing a better experience for the player. This approach allows players to save their scores online and get the latest competitive information in game formats like Endless Runner.

## Critical Reflection 
### Challenges and Area for Improvement 

### Future Improvements

## Bibliography 
- Subway Surfers Case Study (s.d.) At: Unity Case Study - Subway Surfers (Accessed 03/12/2024).
- Temple Run Template Documentation (2024) At: [Temple Run Wiki] (https://templerun.fandom.com/wiki/Category:Template_documentation) (Accessed 03/12/2024).
- Unity Input System 1.11.2 (s.d.) At: Unity Input System Documentation (Accessed 03/12/2024).
- Detect Swipe NEW Input System (Touch/Mouse) At: YouTube - Swipe Detection Tutorial (Accessed 03/12/2024).
- Flask Documentation (3.1.x) (s.d.) At: Flask Official Documentation (Accessed 03/12/2024).
- High Score Table with Saving and Loading (Unity Tutorial for Beginners) At: YouTube - Unity High Score Tutorial (Accessed 03/12/2024).
- How to Send a Request from a Unity Game to a Flask Application Server At: YouTube Tutorial (Accessed 03/12/2024).











 
  

