using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyGoatAIController : AIController
{
    public float moveSpeed = 5f;
    public float burstSpeed = 10f;
    public float burstDuration = 0.5f;
    public float moveDelay = 0.5f;
    public float detectionRadius = 5f;
    public float collisionForce = 100f;
    public int collisionDamage = 10;
    public float stunDuration = 2f;

    private Transform[] waypoints;
    private int currentWaypoint = 0;
    private float moveDelayEndTime = 0f;
    private PlayerController playerController;
    private float nextDamageTime = 0f;
    private float burstEndTime = 3f;
    private int hopCount = 1;

    private enum GoatState { Patrol, BurstMode, Dead };
    private GoatState currentState = GoatState.Patrol;

    private void Start()
    {
        waypoints = GameObject.FindObjectsOfType<EnemyPatrolPoints>()[0].GetUnusedPatrolPoints(EnemyPatrolPoints.EnemyType.IcyGoat);
        playerController = PlayerController.Instance;
        currentHealth = maxHealth;
        nextDamageTime = Time.time;
    }

    private void Update()
    {
        Debug.Log("IcyGoatAIController.Update" + currentState);
        switch (currentState)
        {
            case GoatState.Patrol:
                UpdatePatrolState();
                break;
            case GoatState.BurstMode:
                UpdateBurstModeState();
                break;
            case GoatState.Dead:
                UpdateDeadState();
                break;
            default:
                break;
        }
    }

    private void UpdatePatrolState()
    {
        animator.SetBool("isPatrolling", true);

        if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) <= 0.1f)
        {
            if (moveDelayEndTime <= Time.time)
            {
                currentState = GoatState.Patrol;
                moveDelayEndTime = Time.time + moveDelay;
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            }
        }
        else
        {
            Vector2 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, playerController.transform.position) <= detectionRadius)
        {
            currentState = GoatState.BurstMode;
            animator.SetBool("isPatrolling", false);
        }
    }

  
private void UpdateBurstModeState()
{
    animator.SetBool("isBursting", true);

    if (Time.time > burstEndTime)
    {
        currentState = GoatState.Patrol;
        animator.SetBool("isBursting", false);
    }
    else
    {
        Vector2 direction = (playerController.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, playerController.transform.position);

        // Two hops towards player
        if (distance > 0.5f)
        {
            transform.Translate(direction * burstSpeed * Time.deltaTime * 2);
        }
        else
        {
            transform.Translate(direction * burstSpeed * Time.deltaTime * distance * 2);
        }

        // Check for impact damage
        if (distance < 1.0f)
        {
            if (Time.time > nextDamageTime)
            {
                nextDamageTime = Time.time + 1.0f;
                playerController.TakeDamage(collisionDamage);

                // Push the player away
                Vector3 collisionDirection = (playerController.transform.position - transform.position).normalized;
                playerController.PushAndStun(collisionDirection, collisionForce);
            }
        }

        // Wait for one second before making next two hops
        if (Time.time > burstEndTime - burstDuration / 2f)
        {
            transform.Translate(-direction * burstSpeed * Time.deltaTime * 2);
        }
    }
}


    IEnumerator BurstDelay()
    {
        yield return new WaitForSeconds(1f);
    }

    private void UpdateDeadState()
    {
        // Do nothing
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         currentState = GoatState.BurstMode;
    //     }
    // }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         if (Time.time >= nextDamageTime)
    //         {
    //             nextDamageTime = Time.time + 1.0f;
    //             playerController.TakeDamage(collisionDamage);
    //             Vector3 collisionDirection = (playerController.transform.position - transform.position).normalized;
    //             //playerController.Stun(stunDuration, collisionDirection * collisionForce);
    //         }
    //     }
    // }

    public void Die()
    {
        currentState = GoatState.Dead;

    }
}
