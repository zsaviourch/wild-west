using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnyxSnipperBullet : MonoBehaviour
{
    // References
    [SerializeField] int energyConsumedPerBullet;
    [SerializeField] int bulletDamage;
    [SerializeField] int bulletSpeedScaler;
    [SerializeField] bool bulletFollowEnemy;
    [SerializeField] bool aoeDamage;
    [SerializeField] int aoeDamageAmount;
    [SerializeField] float aoeRadius;
    [SerializeField] float bulletTravelTime;
    public float currentBulletTravelTime;
    public bool hitStuff;
    public Vector3 hitPosition;
    public TrailRenderer trailRenderer;
    public bool bulletDirectionDecided;
    public Vector3 decidedDirection;
    public Transform shootingPosTransform;
    public Rigidbody2D rb;
    [SerializeField] GameObject explosionPrefab;

    // constructor
    public OnyxSnipperBullet(int energyConsumedPerBullet, int bulletDamage, float aoeRadius, int bulletSpeedScaler, bool bulletFollowEnemy,
         bool aoeDamage, int aoeDamageAmount, float bulletTravelTime, GameObject explosionPrefab)
    {
        this.energyConsumedPerBullet = energyConsumedPerBullet;
        this.bulletDamage = bulletDamage;
        this.bulletSpeedScaler = bulletSpeedScaler;
        this.bulletFollowEnemy = bulletFollowEnemy;
        this.aoeDamage = aoeDamage;
        this.aoeDamageAmount = aoeDamageAmount;
        this.aoeRadius = aoeRadius;
        this.bulletTravelTime = bulletTravelTime;
        this.explosionPrefab = explosionPrefab;
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
        hitStuff = false;
        trailRenderer = GetComponent<TrailRenderer>();
        bulletDirectionDecided = false;
        shootingPosTransform = GameObject.FindWithTag("shootingPos").transform;
        rb = GetComponent<Rigidbody2D>();
    }


    // hit enermy or obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {


        // stop when hitting enemy, obstacle, or ground
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("obstacle") || collision.gameObject.CompareTag("ground"))
        {

            GetComponent<Rigidbody2D>().isKinematic = true;
            Destroy(GetComponent<TrailRenderer>());
            hitStuff = true;
        }
            //collision.gameObject.GetComponent<Enemy_Test>().health -= bulletDamage;
            //Destroy(gameObject);
        // hit the obstacle
        //if (collision.gameObject.CompareTag("obstacle"))
        //{
        //    Destroy(gameObject);
        //}
    }

    // destroy the bullet if it exceeds the travel length
    private void Update()
    {
        currentBulletTravelTime += Time.deltaTime;
        DestroyBulletWhenExistingTooLong();
        hitPosition = hitStop();
        if (hitPosition != Vector3.zero)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            Destroy(GetComponent<TrailRenderer>());
            hitStuff = true;
            hitPosition = findHitPoint();
            if (hitPosition != Vector3.zero)
            {
                transform.position = hitPosition;
            }
            
            //transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }

        // Moving before hitting anything
        if (!hitStuff)
        {
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
                rb.AddForce(decidedDirection * bulletSpeedScaler, ForceMode2D.Force);
            }
        }
            
    }

    public void DestroyBulletWhenExistingTooLong()
    {
        if (currentBulletTravelTime >= bulletTravelTime)
        {
            if (hitStuff && hitPosition != Vector3.zero)
            {
                Explode(hitPosition);
                AkSoundEngine.PostEvent("sniperExplode", gameObject);
            }
            else
            {
                Explode(transform.position);
                AkSoundEngine.PostEvent("sniperExplode", gameObject);
            }

        }
    }

    public Vector3 hitStop()
    {
        Vector3 hitPos = Vector3.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 0.01f);
        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("enemy") || hit.collider.gameObject.CompareTag("obstacle") || hit.collider.gameObject.CompareTag("ground"))
            {
                hitPos = hit.point;
            }
        }
        return hitPos;
    }

    // shoot a ray backwards to reset the position
    public Vector3 findHitPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.forward, 0.01f);
        Vector3 hitPos = Vector3.zero;
        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("enemy") || hit.collider.gameObject.CompareTag("obstacle") || hit.collider.gameObject.CompareTag("ground"))
            {
                hitPos = hit.point;
                // Deduct the first contact medium damage
                if (hit.rigidbody.gameObject.CompareTag("enemy"))
                {
                    hit.rigidbody.gameObject.GetComponent<Enemy_Test>().health -= bulletDamage;
                }
                
            }
        }
        return hitPos;
    }

    public void Explode(Vector3 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, aoeRadius);
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.CompareTag("enemy"))
            {
                c.gameObject.GetComponent<Enemy_Test>().health -= aoeDamageAmount;
            }
        }
        StartCoroutine(ExplodeAnimation());
        Destroy(gameObject);
    }

    private IEnumerator ExplodeAnimation()
    {
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(explosion);
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("explosion");
        foreach (GameObject e in explosions)
        {
            Destroy(e);
        }
        
    }

    //private void OnDestroy()
    //{
    //    trailRenderer.transform.parent = null;
    //    trailRenderer.autodestruct = true;
    //    trailRenderer = null;
    //}
}
