using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAIController : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float agressiveRange = 3f;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int damagePerHit = 10;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float proximityDamage = 5f;
    [SerializeField] private float proximityCheckInterval = 1f;
    [SerializeField] private float proximityRange = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] public Grid navigationMesh;

    private int currentPatrolPointIndex = 0;
    private Vector3 currentDestination;
    private int currentHealth;
    private bool isDefensive = false;
    private bool isCoolingDown = false;
    private bool isProximityAttacking = false;
    private bool isBurstAttacking = false;
    private float lastProximityCheckTime = 0f;
    private float lastAttackTime = 0f;
    private float shellEndTime = 0f;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentDestination = patrolPoints[currentPatrolPointIndex].position;
        animator.SetBool("isPatrolling", true);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isDefensive)
        {
            if (Time.time > shellEndTime)
            {
                isDefensive = false;
                animator.SetBool("isShell", false);
            }
        }

        if (isBurstAttacking)
        {
            return;
        }

        if (isProximityAttacking)
        {
            if (Vector3.Distance(transform.position, playerController.transform.position) > proximityRange)
            {
                isProximityAttacking = false;
            }
            else if (Time.time - lastProximityCheckTime > proximityCheckInterval)
            {
                lastProximityCheckTime = Time.time;
                playerController.TakeDamage(proximityDamage);
            }
        }

        if (Vector3.Distance(transform.position, playerController.transform.position) < detectionRange)
        {
            isDefensive = true;
            isProximityAttacking = false;
            animator.SetBool("isShell", true);

            if (!isCoolingDown && Time.time - lastAttackTime > attackCooldown && Vector3.Distance(transform.position, playerController.transform.position) < agressiveRange)
            {
                isCoolingDown = true;
                lastAttackTime = Time.time;
                animator.SetBool("isBurst", true);
                StartCoroutine(DoBurstAttack());
            }
        }

        if (!isDefensive && !isBurstAttacking)
        {
            if (Vector3.Distance(transform.position, currentDestination) < 0.1f)
            {
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
                currentDestination = patrolPoints[currentPatrolPointIndex].position;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
            animator.SetFloat("moveX", currentDestination.x - transform.position.x);
            animator.SetFloat("moveY", currentDestination.y - transform.position.y);
        }
    }

    
private IEnumerator DoBurstAttack()
{
    isBurstAttacking = true;

    animator.SetBool("isChasing", true);

    Vector2 targetPosition = playerController.transform.position;
    List<Node> path = Pathfinding.FindPath(navigationMesh, transform.position, targetPosition);

    // Move towards player using pathfinding
    int pathIndex = 0;
    while (pathIndex < path.Count)
    {
        Vector2 direction = ((Vector2)path[pathIndex].worldPosition - (Vector2)transform.position).normalized;
        transform.position = transform.position + (Vector3)direction * moveSpeed * Time.deltaTime;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        yield return null;

        if (Vector2.Distance(transform.position, path[pathIndex].worldPosition) < 0.1f)
        {
            pathIndex++;
        }
    }

    animator.SetBool("isChasing", false);
    animator.SetBool("isBurst", true);
    yield return new WaitForSeconds(0.5f);

    // Impact hit
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));
    foreach (Collider2D enemy in hitEnemies)
    {
        enemy.GetComponent<PlayerController>().TakeDamage(damagePerHit);
    }

    animator.SetBool("isBurst", false);
    isCoolingDown = false;
    isBurstAttacking = false;
    lastAttackTime = Time.time;
}




private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, detectionRange);

    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, agressiveRange);

    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, proximityRange);

    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(transform.position, attackRange);
}
}