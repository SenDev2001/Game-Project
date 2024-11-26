using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_InputField playerNameInputField;   
    public Button saveButton;                      
    public Button playButton;
    public Button quitButton;

    private string playerName;

    void Start()
    {
       
        saveButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

       
        playerNameInputField.onValueChanged.AddListener(OnNameEntered);

      
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

   
    private void OnNameEntered(string name)
    {
        playerName = name;

      
        saveButton.gameObject.SetActive(!string.IsNullOrEmpty(playerName));
    }


    public void OnSaveButtonClicked()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        Debug.Log("Player Name Saved: " + playerName);


        saveButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        playerNameInputField.gameObject.SetActive(false);

        playButton.onClick.RemoveAllListeners(); 
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

   
    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene("Game");
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
} 

