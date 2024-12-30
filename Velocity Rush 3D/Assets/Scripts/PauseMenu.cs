using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject volumePanel;
    
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Volume()
    {
        pauseMenu.SetActive(false);
        volumePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseVolumePanel()
    {
        volumePanel.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
