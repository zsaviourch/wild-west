using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAIController : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int damagePerHit = 10;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float proximityDamage = 5f;
    [SerializeField] private float proximityCheckInterval = 1f;
    [SerializeField] private float proximityRange = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

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

            if (!isCoolingDown && Time.time - lastAttackTime > attackCooldown)
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
    yield return new WaitForSeconds(0.5f);

    if (Vector3.Distance(transform.position, playerController.transform.position) < attackRange)
    {
        playerController.TakeDamage(damagePerHit);
    }

    isProximityAttacking = true;
    animator.SetBool("isBurst", false);
    yield return new WaitForSeconds(0.5f);
    isBurstAttacking = false;
}

private void OnCollisionStay2D(Collision2D collision)
{
    if (isProximityAttacking && collision.gameObject.CompareTag("Player"))
    {
        if (Time.time - lastProximityCheckTime > proximityCheckInterval)
        {
            lastProximityCheckTime = Time.time;
            playerController.TakeDamage(proximityDamage);
        }
    }
}

public void TakeDamage(int damage)
{
    if (isDefensive)
    {
        currentHealth -= Mathf.FloorToInt(damage * 0.5f);
    }
    else
    {
        currentHealth -= damage;
    }

    if (currentHealth <= 0)
    {
        isDead = true;
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}

private IEnumerator ResetCooldown()
{
    yield return new WaitForSeconds(attackCooldown);
    isCoolingDown = false;
}

private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, detectionRange);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, proximityRange);
}
}