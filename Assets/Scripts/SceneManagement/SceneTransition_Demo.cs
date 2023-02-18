using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition_Demo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int sceneNumber = SceneManager.GetActiveScene().buildIndex;
            LoadScene(sceneNumber);
        }
    }

    void LoadScene(int currentSceneNum)
    {
        switch (currentSceneNum)
        {
            case 0:
                SceneManager.LoadScene(1);
                break;
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                SceneManager.LoadScene(0);
                break;

        }
    }
}
