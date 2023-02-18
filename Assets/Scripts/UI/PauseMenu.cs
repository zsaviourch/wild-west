using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenuContainer;

    public bool gameIsPaused; // Any other scripts that need to know if the game is paused can reference this bool from this script
    
    void Start()
    {
        pauseMenuContainer.SetActive(false);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Main_Menu")
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenuContainer.SetActive(true);
        Time.timeScale = 0f; // Time is now paused
        gameIsPaused = true;
        Debug.Log("Game is Paused");
    }

    public void ResumeGame()
    {
        pauseMenuContainer.SetActive(false);
        Time.timeScale = 1f; // Time is now back at normal scale
        gameIsPaused = false;
        Debug.Log("Game is Unpaused");
    }
    
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

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}
