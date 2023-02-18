using UnityEngine;

public enum GameState
{
    START,
    GAME_LOOP,
    END
}

public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public GameObject townManagerPrefab; 
    public GameObject eventSystemPrefab;
    public GameObject dialogueSystemPrefab;
    public GameObject audioManagerPrefab;
    private GameObject townManagerInstance; 
    private GameObject eventSystemInstance;
    private GameObject dialogueSystemInstance;
    private GameObject audioManagerInstance;

    private void Start()
    {
        Debug.Log("Entering Gamestate START");
        currentState = GameState.START;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.START:
                Debug.Log("Entering Gamestate GAME_LOOP");
                currentState = GameState.GAME_LOOP;
                break;
            case GameState.GAME_LOOP:
                // If the TownManager instance hasn't been created yet, create it
                if (townManagerInstance == null && townManagerPrefab != null)
                {
                    townManagerInstance = Instantiate(townManagerPrefab);
                }

                // Spawn the Event System prefab if it hasn't been spawned yet
                if (eventSystemInstance == null && eventSystemPrefab != null)
                {
                    eventSystemInstance = Instantiate(eventSystemPrefab);
                }

                // Spawn the Dialogue System prefab if it hasn't been spawned yet
                if (dialogueSystemInstance == null && dialogueSystemPrefab != null)
                {
                    dialogueSystemInstance = Instantiate(dialogueSystemPrefab);
                }

                // Spawn the Dialogue System prefab if it hasn't been spawned yet
                if (audioManagerInstance == null && audioManagerPrefab != null)
                {
                    audioManagerInstance = Instantiate(audioManagerPrefab);
                }

                if (false)
                {
                    // If the game is over, transition to the END state
                    Debug.Log("Entering Gamestate END");
                    currentState = GameState.END;
                }
                break;
            case GameState.END:
                if(false)
                {
                    Debug.Log("Entering Gamestate START");
                    currentState = GameState.START;
                }
                break;
            default:
                break;
        }
    }
}
