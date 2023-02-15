using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public GameObject creditsPage;
    [Tooltip("Name of the first level to load when the player presses the start button")]
    public string sceneToLoad;

    private void Awake()
    {
        if (creditsPage.activeSelf)
            creditsPage.SetActive(false); // Disabled by default, in case left active prior to playmode
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    // Toggles the visibility of the settings menu
    public void ToggleSettingsMenu()
    {
        if (!settingsMenu.gameObject.activeSelf)
        {
            settingsMenu.gameObject.SetActive(true);
        }
        else
        {
            settingsMenu.gameObject.SetActive(false);
        }
    }
    
    public void CloseCreditsPage()
    {
        creditsPage.SetActive(false);
    }
    
    public void ToggleCreditsPage()
    {
        if (!creditsPage.activeSelf)
        {
            creditsPage.SetActive(true);
        }
        else
        {
            creditsPage.SetActive(false);
        }
    }
    
    // We will link this to our game jam download page. For now, it will just link to the jam itself.
    public void ItchPageBtn()
    {
        Application.OpenURL("https://itch.io/jam/brackeys-9");
    }
    
    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
