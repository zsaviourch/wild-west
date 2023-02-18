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
    private GameObject townManagerInstance; 

    private void Start()
    {
        // Set the initial state of the game to START
        Debug.Log("Entering Gamestate START");
        currentState = GameState.START;
    }

    private void Update()
    {
        // Check the current state of the game
        switch (currentState)
        {
            case GameState.START:
                // Do any necessary initialization for the game
                // and transition to the GAME_LOOP state
                Debug.Log("Entering Gamestate GAME_LOOP");
                currentState = GameState.GAME_LOOP;
                break;
            case GameState.GAME_LOOP:
                // If the TownManager instance hasn't been created yet, create it
                if (townManagerInstance == null && townManagerPrefab != null)
                {
                    townManagerInstance = Instantiate(townManagerPrefab);
                }

                if (false)
                {
                    // If the game is over, transition to the END state
                    Debug.Log("Entering Gamestate END");
                    currentState = GameState.END;
                }
                break;
            case GameState.END:
                // Do any necessary clean up for the game
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
