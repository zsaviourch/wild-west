using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// To Do: Let the Level loader remember what city it got to previously to then choose the next one it hasn't been to

public class LevelLoader : MonoBehaviour
{
    public GameObject mapCanvas; // This is essentially the loading screen
    public Transform horseIcon;
    public float horseIconSpeed = 100f;
    public Animator crossfade;

    public List<Transform> cities;
    [SerializeField] private Transform currentLocation;
    [SerializeField] private Transform destination;

    [SerializeField] private float transitionTime = 1f; // Should be the same amount of time it takes for the crossfade animation to complete
    public GameObject readyButton;
    private bool _readyButtonPressed;
    private bool canHorseMove;

    public bool HorseArrivedAtDestination()
    {
        if (horseIcon.position == destination.position)
        {
            Debug.Log("Horse Icon has Arrived at destination!");
            return true;
        }
        else
            return false;
    }
    
    private bool ReadyToContinue() // When play clicks the button that appears on the screen when prompted, they will load into the next level
    {
        if (_readyButtonPressed)
            return true;
        else
            return false;
    }
    
    private void Start()
    {
        readyButton.SetActive(false);
        horseIcon.position = currentLocation.position; // I'm going to use this line when we load into the next scene so the horse icon knows where to start for the next map transition
        mapCanvas.SetActive(false);
        canHorseMove = false;
    }

    private void Update()
    {
        if (canHorseMove)
            MoveHorseIcon();
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadLevel(sceneId));
    }

    public void PlayerReadyToggle()
    {
        _readyButtonPressed = true;
    }

    private void MoveHorseIcon()
    {
        horseIcon.position = Vector3.MoveTowards(horseIcon.position, destination.position, Time.deltaTime * horseIconSpeed);
    }
    
    IEnumerator LoadLevel(int sceneId) // sceneID is the Level's build order index
    {
        // Play Transition Animation
        crossfade.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);
        
        mapCanvas.SetActive(true);
        crossfade.SetTrigger("End");
        
        yield return new WaitForSeconds(transitionTime);

        canHorseMove = true;

        // Wait until the horse has arrived at the next city
        yield return new WaitUntil(HorseArrivedAtDestination);

        // Enable continue button & wait for player to press continue button
        readyButton.SetActive(true);
        yield return new WaitUntil(ReadyToContinue);
        
        crossfade.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);

        currentLocation = destination; // Our new current location is now what was our destination
        
        // Load Scene
        SceneManager.LoadScene(sceneId);
    }
}
