using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void ItchPageBtn()
    {
        // We will link this to our game jam download page. For now, it will just link to the jam itself.
        Application.OpenURL("https://itch.io/jam/brackeys-9");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
