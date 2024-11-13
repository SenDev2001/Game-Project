using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMP_InputField playerNameInputField;   // Reference to the Input Field for name
    public Button saveButton;                      // Reference to the Save Button
    public Button playButton;                      // Reference to the Play Button

    private string playerName;

    void Start()
    {
        // Initially, disable the Play and Save buttons
        saveButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        playerNameInputField.gameObject.SetActive(true);

        // Listen for changes in the input field
        playerNameInputField.onValueChanged.AddListener(OnNameEntered);

        // Add onClick listeners for the buttons
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    // Called when the player enters their name in the input field
    private void OnNameEntered(string name)
    {
        playerName = name;

        // If the player has entered a valid name, enable the Save button
        if (!string.IsNullOrEmpty(playerName))
        {
            saveButton.gameObject.SetActive(true);
        }
        else
        {
            saveButton.gameObject.SetActive(false);
        }
    }

    // Method to be called when the Save button is clicked
    public void OnSaveButtonClicked()
    {
        // Save the player's name using PlayerPrefs
        PlayerPrefs.SetString("PlayerName", playerName);

        // Disable the Save button and enable the Play button
        saveButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        playerNameInputField.gameObject.SetActive(false);
    }

    // Method to be called when the Play button is clicked
    public void OnPlayButtonClicked()
    {
        // Load the Game Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
