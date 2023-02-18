using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopazGunBullet : MonoBehaviour
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

    public bool bulletDirectionDecided;
    public Vector3 decidedDirection;
    public Transform shootingPosTransform;
    public Rigidbody2D rb;

    // constructor
    public TopazGunBullet(int energyConsumedPerBullet, int bulletDamage, int bulletSpeedScaler, bool bulletFollowEnemy,
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
        bulletDirectionDecided = false;
        shootingPosTransform = GameObject.FindWithTag("shootingPos").transform;
        rb = GetComponent<Rigidbody2D>();
    }


    // hit enermy or obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // hit the enemy
        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<Enemy_Test>().health -= bulletDamage;
            Destroy(gameObject);
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
        transform.position += transform.forward * bulletSpeedScaler * Time.deltaTime;

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

    public void DestroyBulletWhenExistingTooLong()
    {
        if (currentBulletTravelTime >= bulletTravelTime)
        {
            Destroy(gameObject);
        }
    }
}
