using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;  // Reference to Pause UI
    [SerializeField] GameObject volumePanel; // Reference to Volume Panel UI

    private bool isPaused = false;

    private void Update()
    {
        // Check for Space key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle Pause
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true); // Show Pause Menu
        Time.timeScale = 0;       // Pause game
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false); // Hide Pause Menu
        Time.timeScale = 1;         // Resume game
        isPaused = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1; // Resume game before loading Menu
    }

    public void Volume()
    {
        pauseMenu.SetActive(false);  // Hide Pause Menu
        volumePanel.SetActive(true); // Show Volume Panel
        Time.timeScale = 0;          // Keep game paused
    }

    public void CloseVolumePanel()
    {
        volumePanel.SetActive(false); // Hide Volume Panel
        pauseMenu.SetActive(true);    // Show Pause Menu
    }
}

