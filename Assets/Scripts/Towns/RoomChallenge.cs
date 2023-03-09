using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChallenge : MonoBehaviour
{
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
        foreach(SpawnPoint point in spawnPoints)
        {
            GameObject spawnedObject = point.Spawn();
            if (spawnedObject != null)
            {
                if (spawnedObject.tag == "enemy")
                {
                    AIController enemy = spawnedObject.GetComponent<AIController>();
                    enemy.OnDied += cleanUpEnemyList;
                    enemiesToBeat.Add(enemy);
                }
                else if (spawnedObject.tag == "Player")
                {
                    spawnedObject.gameObject.SetActive(true);
                    playerHealth = spawnedObject.GetComponent<HealthAndEnergy>();
                    playerHealth.OnDied += reactToPlayerDeath;
                }
            }
        }
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
    private List<AIController> enemiesToBeat = new();
    private HealthAndEnergy playerHealth;
    private SpawnPoint[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    private void Start()//IEnumerator
    {
        //yield return new WaitForSeconds(3);
        StartChallenge();
    }

    private void cleanUpEnemyList(AIController deadEnemy)
    {
        deadEnemy.OnDied -= cleanUpEnemyList;
        enemiesToBeat.Remove(deadEnemy);
        if (enemiesToBeat.Count <= 0)
        {
            EndChallenge(true);
        }
    }
    private void reactToPlayerDeath(HealthAndEnergy deadPlayer)
    {
        deadPlayer.OnDied -= reactToPlayerDeath;
        EndChallenge(false);
        AkSoundEngine.PostEvent("playerDie", null);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied -= reactToPlayerDeath;
        }
        foreach(AIController controller in enemiesToBeat)
        {
            if (controller != null)
            {
                controller.OnDied -= cleanUpEnemyList;
            }
        }
    }
}
