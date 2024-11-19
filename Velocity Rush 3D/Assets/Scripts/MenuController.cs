using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_InputField playerNameInputField;   
    public Button saveButton;                      
    public Button playButton;                     

    private string playerName;

    void Start()
    {
       
        saveButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);

       
        playerNameInputField.onValueChanged.AddListener(OnNameEntered);

      
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

   
    private void OnNameEntered(string name)
    {
        playerName = name;

      
        saveButton.gameObject.SetActive(!string.IsNullOrEmpty(playerName));
    }


    public void OnSaveButtonClicked()
    {
        PlayerPrefs.SetString("PlayerName", playerName); 

      
        saveButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        playerNameInputField.gameObject.SetActive(false);
    }

   
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

}
