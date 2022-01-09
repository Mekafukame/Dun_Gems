using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject Player;

    private void Update()
    {
        if (Player)
        {            
            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
            {
                PauseGame(PauseMenu);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
            {
                ResumeGame(PauseMenu, SettingsMenu);
            }
        }
    }
    public void PauseGame(GameObject pauseMenu)
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame(GameObject pauseMenu, GameObject settingsMenu)
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void ResumeGame(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);        
        Time.timeScale = 1;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
