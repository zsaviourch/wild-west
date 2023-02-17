using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAIController : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float tackleSpeed = 4.0f;
    public float collisionDamage = 10.0f;
    public float defensiveModeDamageReduction = 0.5f;
    public float maxHealth = 100.0f;

    private float currentHealth;
    private bool inDefensiveMode = false;
    private bool playerInLOS = false;

    private Transform player;
    private Grid grid;
    private Node currentNode;
    private List<Node> path;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("plaher is "+player);
        grid = FindObjectOfType<Grid>();
        currentNode = grid.NodeFromWorldPoint(transform.position);
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            inDefensiveMode = true;
            // TODO: Set up defensive mode behavior
        }
        else
        {
            inDefensiveMode = false;
            path = Pathfinding.FindPath(grid, transform.position, player.position);
            if (path != null && path.Count > 0)
            {
                currentNode = path[0];
            }
        }

        if (inDefensiveMode)
        {
            // TODO: Set up defensive mode behavior
        }
        else
        {
            // Move towards player if not in defensive mode and player is visible
            if (playerInLOS)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
            }
            // Follow path if it exists and player is not visible
            else if (path != null && path.Count > 0)
            {
                if (Vector3.Distance(transform.position, currentNode.worldPosition) < 0.1f)
                {
                    path.RemoveAt(0);
                    if (path.Count > 0)
                    {
                        currentNode = path[0];
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentNode.worldPosition, movementSpeed * Time.deltaTime);
                }
            }
        }
    }

    private bool CanSeePlayer()
    {
        // TODO: Improvise the hit for CanSeePlayer
        Vector3 viewDirection = (transform.position + transform.right * 2.0f) - transform.position;
        float maxRaycastDistance = 5.0f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, viewDirection, maxRaycastDistance, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, viewDirection, Color.black);

        if (hit.collider != null && hit.collider.tag == "Player")
        {
            playerInLOS = true;
            return true;
        }
        else
        {
            playerInLOS = false;
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Deal collision damage to player
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(collisionDamage);
            }

            // Tackle player
            Vector3 playerDirection = collision.transform.position - transform.position;
            collision.rigidbody.AddForce(playerDirection.normalized * tackleSpeed, ForceMode2D.Impulse);

            // Take damage when tackling player
            currentHealth -= collisionDamage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // TODO: Implement death behavior
        Destroy(gameObject);
    }
}
