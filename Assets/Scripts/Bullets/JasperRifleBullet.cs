using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasperRifleBullet : MonoBehaviour
{
    // References
    [SerializeField] int energyConsumedPerBullet;
    [SerializeField] int bulletDamage;
    [SerializeField] int bulletSpeedScaler;
    [SerializeField] bool bulletFollowEnemy;
    [SerializeField] bool aoeDamage;
    [SerializeField] int aoeDamageAmount;
    [SerializeField] float bulletTravelTime;
    public float currentBulletTravelTime;
    public Rigidbody2D rb;
    public Transform shootingPosTransform;
    public bool closestEnemyFound;
    public bool seekingEnemyInitiated;
    public Vector3 closestEnemyPos;
    public TrailRenderer trail;
    public float distanceToClosestEnemy;
    public bool distanceCalculated;
    public bool bulletDirectionDecided;
    public Vector3 decidedDirection;

    // constructor
    public JasperRifleBullet(int energyConsumedPerBullet, int bulletDamage, int bulletSpeedScaler, bool bulletFollowEnemy,
         bool aoeDamage, int aoeDamageAmount, float bulletTravelTime)
    {
        this.energyConsumedPerBullet = energyConsumedPerBullet;
        this.bulletDamage = bulletDamage;
        this.bulletSpeedScaler = bulletSpeedScaler;
        this.bulletFollowEnemy = bulletFollowEnemy;
        this.aoeDamage = aoeDamage;
        this.aoeDamageAmount = aoeDamageAmount;
        this.bulletTravelTime = bulletTravelTime;
    }

    // property access
    public int EnergyConsumedPerBullet
    {
        get
        {
            return energyConsumedPerBullet;
        }
        set
        {
            if (value > 0)
            {
                energyConsumedPerBullet = value;
            }
            else
            {
                Debug.LogError("energyIntialAmount should be larger than zero!");
            }
        }
    }
    public int BulletDamage
    {
        get
        {
            return bulletDamage;
        }
        set
        {
            if (value > 0)
            {
                bulletDamage = value;
            }
            else
            {
                Debug.LogError("bulletDamage should be larger than zero!");
            }
        }

    }
    public int BulletSpeedScaler
    {
        get
        {
            return bulletSpeedScaler;
        }
        set
        {
            if (value > 0)
            {
                bulletSpeedScaler = value;
            }
            else
            {
                Debug.LogError("bulletSpeedScaler should be larger than zero!");
            }
        }
    }
    public bool BulletFollowEnemy
    {
        get
        {
            return bulletFollowEnemy;
        }
        set
        {
            bulletFollowEnemy = value;
        }
    }
    public bool AOEDamage
    {
        get
        {
            return aoeDamage;
        }
        set
        {
            aoeDamage = value;
        }
    }
    public int AOEDamageAmount
    {
        get
        {
            return aoeDamageAmount;
        }
        set
        {
            if (value > 0)
            {
                aoeDamageAmount = value;
            }
            else
            {
                Debug.LogError("aoeDamageAmount should be larger than zero!");
            }
        }
    }
    public float BulletTravelTime
    {
        get
        {
            return bulletTravelTime;
        }
        set
        {
            if (bulletTravelTime > 0)
            {
                bulletTravelTime = value;
            }
            else
            {
                Debug.LogError("bulletTravelTime should be larger than zero!");
            }
        }
    }

    private void Awake()
    {
        currentBulletTravelTime = 0f;
        rb = GetComponent<Rigidbody2D>();
        shootingPosTransform = GameObject.FindWithTag("shootingPos").transform;
        closestEnemyFound = false;
        seekingEnemyInitiated = false;
        trail = GetComponent<TrailRenderer>();
        distanceCalculated = false;
        bulletDirectionDecided = false;
    }


    // hit enermy or obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // hit the enemy
        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<AIController>().TakeDamage(bulletDamage);
            rb.isKinematic = true;
            //Destroy(gameObject);
        }

        // hit the obstacle
        if (collision.gameObject.CompareTag("obstacle"))
        {
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }

    // destroy the bullet if it exceeds the travel length
    private void Update()
    {
        currentBulletTravelTime += Time.deltaTime;
        DestroyBulletWhenExistingTooLong();

        if (closestEnemyFound)
        {
            if (distanceCalculated == false)
            {
                distanceToClosestEnemy = Vector3.Distance(transform.position, closestEnemyPos);
                distanceCalculated = true;
            }
            if (Vector3.Distance(transform.position, closestEnemyPos) > distanceToClosestEnemy * 0.3)
            {
                rb.AddForce(shootingPosTransform.forward * bulletSpeedScaler, ForceMode2D.Force);
                rb.AddForce(((closestEnemyPos - transform.position).normalized) * 2f * bulletSpeedScaler, ForceMode2D.Force);
            }
            else
            {
                Vector3 followingTargetDirection = Vector3.MoveTowards(transform.position, closestEnemyPos, bulletSpeedScaler * Time.deltaTime);
                Vector3 upDirection = Vector3.MoveTowards(transform.position, shootingPosTransform.forward, bulletSpeedScaler * Time.deltaTime);
                transform.position = followingTargetDirection;
            }
            
        }
        else
        {
            if (seekingEnemyInitiated == false)
            {
                closestEnemyPos = ReturnClosestEnemyPosition();
                seekingEnemyInitiated = true;
            }


            
            if (bulletDirectionDecided == false)
            {
                if (GameObject.FindWithTag("Player").GetComponent<Transform>().localScale.x == 1)
                {
                    decidedDirection = shootingPosTransform.right;
                    bulletDirectionDecided = true;
                }
                else
                {
                    decidedDirection = -shootingPosTransform.right;
                    bulletDirectionDecided = true;
                }
            }
            rb.AddForce(decidedDirection * bulletSpeedScaler, ForceMode2D.Force);


        }
        
        
    }

    public void DestroyBulletWhenExistingTooLong()
    {
        if (currentBulletTravelTime >= bulletTravelTime)
        {
            Destroy(gameObject);
        }
    }

    // Return the closest enemy position, it returns Vector3.zero if no enemy found
    public Vector3 ReturnClosestEnemyPosition ()
    {
        float minimumDistance = Mathf.Infinity;
        Vector3 closestEnemyPos = Vector3.zero;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {

                Vector3 enemyPosition = enemy.transform.position;
                float distance = Vector3.Distance(enemyPosition, transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    closestEnemyPos = enemyPosition;
                }
            }
        }
        else
        {
            Debug.Log("There is no enemy!");
        }

        if (closestEnemyPos != Vector3.zero)
        {
            closestEnemyFound = true;
        }
        else
        {
            closestEnemyFound = false;
        }
   
        return closestEnemyPos;
    }

    private void OnDestroy()
    {
        trail.transform.parent = null;
        trail.autodestruct = true;
        trail = null;
    }
}
