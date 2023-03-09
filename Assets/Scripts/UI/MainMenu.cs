using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    public SettingsMenu settingsMenu;
    public GameObject creditsPage;
    [SerializeField] private Animator creditsAnim;
    [SerializeField] private Animator crossfade;
    
    [Header("Variables")]
    [SerializeField] private float transitionTime = 1f;
    /*[Tooltip("Name of the first level to load when the player presses the start button")]
    public string sceneToLoad;*/

    private void Awake()
    {
        if (creditsPage.activeSelf)
            creditsPage.SetActive(false); // Disabled by default, in case left active prior to playmode
    }

    public void StartGame()
    {
        // SceneManager.LoadScene(sceneToLoad);
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

    public void ToggleCreditsPage()
    {
        if (!creditsPage.activeSelf)
        {
            StartCoroutine(StartCredits());
        }
        else
        {
            StartCoroutine(EndCredits());
        }
    }

    IEnumerator StartCredits()
    {
        crossfade.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        creditsPage.SetActive(true);
        creditsAnim.SetTrigger("StartCredits");
        AkSoundEngine.PostEvent("creditsStart", this.gameObject);
        //AudioManager.Instance.Play("creditsTheme");
        //AudioManager.Instance.Stop("menuTheme");
        crossfade.SetTrigger("End");
        
        yield return new WaitForSeconds(65f); // If the entire credits finishes playing, stop looping credits music
        //AudioManager.Instance.Stop("creditsTheme");
    }
    
    IEnumerator EndCredits()
    {
        crossfade.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        creditsAnim.SetTrigger("EndCredits");
        //AudioManager.Instance.Stop("creditsTheme");
        //AudioManager.Instance.Play("menuTheme");
        creditsPage.SetActive(false);
        AkSoundEngine.PostEvent("menuStart", this.gameObject);
        crossfade.SetTrigger("End");
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
