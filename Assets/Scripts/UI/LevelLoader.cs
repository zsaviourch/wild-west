using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    [SerializeField] private float transitionTime = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // Play Transition Animation
        transition.SetTrigger("Start");
        
        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load Scene
        SceneManager.LoadScene(levelIndex);
        // transition.SetTrigger("End");
    }
}