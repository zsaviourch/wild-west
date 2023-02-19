using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomQuest : MonoBehaviour
{
    public List<AIController> enemiesToBeat;

    public enum ChallengeState
    {
        PENDING,
        ACTIVE,
        SUCCESS,
        FAILED
    }

    public ChallengeState State => State;
    private ChallengeState state = ChallengeState.PENDING;

    public void StartChallenge()
    {
        if (state == ChallengeState.PENDING)
        {
            state = ChallengeState.ACTIVE;
        }
    }

    public void EndChallenge(bool playerWin)
    {
        if (playerWin)
        {
            state = ChallengeState.SUCCESS;
            Debug.Log("Area Clear!");
        }
        else
        {
            state = ChallengeState.FAILED;
            Debug.Log("Try Again...");
        }
        StartCoroutine(GoToNext());

        IEnumerator GoToNext()
        {
            yield return new WaitForSeconds(5);
            LevelAtlus.Instance.ProgressTown(playerWin);
        }
    }

    private void Start()//IEnumerator
    {
        //yield return new WaitForSeconds(3);
        StartChallenge();
    }
}
