using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAIController : AIController
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float burstSpeed = 4f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float agressiveRange = 3f;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private int proximityDamage = 5;
    [SerializeField] private float proximityCheckInterval = 1f;
    [SerializeField] private float proximityRange = 2f;

    public Transform[] patrolPoints;

    private Grid navigationMesh;
    private int currentPatrolPointIndex = 0;
    private bool isDefensive = false;
    private bool isCoolingDown = false;
    private bool isProximityAttacking = false;
    private bool isBurstAttacking = false;
    private float lastProximityCheckTime = 0f;
    private float lastAttackTime = 0f;
    private float shellEndTime = 0f;

    private void Start()
    {
        patrolPoints = GameObject.FindObjectsOfType<EnemyPatrolPoints>()[0].GetUnusedPatrolPoints(EnemyPatrolPoints.EnemyType.Golem);
        //Debug.Log("golem patrol points are " + patrolPoints.length);
        navigationMesh = Grid.Instance;
        animator.SetBool("isPatrolling", true);
        currentPatrolPointIndex = 0;
    }

    private void Update()
    {
        if (IsDead)
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
            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > proximityRange)
            {
                isProximityAttacking = false;
            }
            else if (Time.time - lastProximityCheckTime > proximityCheckInterval)
            {
                lastProximityCheckTime = Time.time;
                PlayerController.Instance.TakeDamage(proximityDamage);
            }
        }

if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < detectionRange)
{
    isDefensive = true;
    isProximityAttacking = false;
    animator.SetBool("isShell", true);

    if (!isCoolingDown && Time.time - lastAttackTime > attackCooldown && Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < agressiveRange)
    {
        isCoolingDown = true;
        lastAttackTime = Time.time;
        animator.SetBool("isBurst", true);
        StartCoroutine(DoBurstAttack());
    }
}
else if (!isDefensive && !isBurstAttacking)
{
    // Move towards the next patrol point
    Vector3 targetPosition = patrolPoints[currentPatrolPointIndex].position;
    if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
    {
        // Calculate the direction towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the enemy towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Set the animation parameters for the movement direction
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }
    else
    {
        // Move to the next patrol point
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
    }
}

    }

    public override void TakeDamage(int damage)
    {
        if (isDefensive)
        {
            //AudioManager.Instance.Play("golemShieldHit");
            AkSoundEngine.PostEvent("golemShieldHit", this.gameObject);
            return;
        }
        //return;

        base.TakeDamage(damage);
        //AudioManager.Instance.Play("golemHurt");
        AkSoundEngine.PostEvent("golemHurt", this.gameObject);
        if (isDead)
        {
            //AudioManager.Instance.Play("golemDie");
            AkSoundEngine.PostEvent("golemDie", this.gameObject);
        }
    }

    private IEnumerator DoBurstAttack()
{
    isBurstAttacking = true;

    animator.SetBool("isChasing", true);

    Vector2 targetPosition = PlayerController.Instance.transform.position;

    // Determine the direction to the player
    Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

    // Move towards player in a straight line
    while (Vector2.Distance(transform.position, targetPosition) > attackRange)
    {
        transform.position = transform.position + (Vector3)direction * burstSpeed * Time.deltaTime;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        yield return null;
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
        //AudioManager.Instance.Play("golemAttack");
        AkSoundEngine.PostEvent("golemAttack", this.gameObject);
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