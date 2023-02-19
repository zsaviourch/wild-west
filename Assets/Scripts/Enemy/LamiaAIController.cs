using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamiaAIController : AIController
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;

    public Transform[] patrolPoints;

    private int currentPatrolPointIndex = 0;
    private Vector2 currentTarget;
    private bool isChasing = false;
    private bool isShooting = false;
    private float lastAttackTime = -10f;

    private void Start()
    {
        patrolPoints = EnemyPatrolPoints.Instance.GetUnusedPatrolPoints(EnemyPatrolPoints.EnemyType.Lamia);
        currentTarget = transform.position;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer <= shootingRange)
        {
            isShooting = true;
            isChasing = false;
            Shoot();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            isShooting = false;
            currentTarget = PlayerController.Instance.transform.position;
        }
        else
        {
            isChasing = false;
            isShooting = false;
            Patrol();
        }

        if (isChasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        }

        animator.SetFloat("moveX", currentTarget.x - transform.position.x);
        animator.SetFloat("moveY", currentTarget.y - transform.position.y);
        animator.SetBool("isChasing", isChasing);
    }

    private void Patrol()
    {
        currentTarget = patrolPoints[currentPatrolPointIndex].position;
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        if (Vector2.Distance(transform.position, currentTarget) <= 0.1f)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }
    }

    private void Shoot()
    {
        if (Time.time > lastAttackTime + 1f)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("attack");
            AudioManager.Instance.Play("lamiaAttack");

            Vector2 direction = (PlayerController.Instance.transform.position - shootPoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!isDead)
        {
            base.TakeDamage(damage);
            AudioManager.Instance.Play("lamiaHurt");
            if (isDead)
            {
                StopAllCoroutines();
                AudioManager.Instance.Play("lamiaDie");
                animator.SetBool("isDead", true);
                // disable all colliders and the Rigidbody2D
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }
                Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
                rb2d.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isShooting && other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(damagePerHit);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the chase range as a red circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Visualize the shooting range as a yellow circle
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

}
