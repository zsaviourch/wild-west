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

    // constructor
    public OnyxSnipperBullet(int energyConsumedPerBullet, int bulletDamage, float aoeRadius, int bulletSpeedScaler, bool bulletFollowEnemy,
         bool aoeDamage, int aoeDamageAmount, float bulletTravelTime)
    {
        this.energyConsumedPerBullet = energyConsumedPerBullet;
        this.bulletDamage = bulletDamage;
        this.bulletSpeedScaler = bulletSpeedScaler;
        this.bulletFollowEnemy = bulletFollowEnemy;
        this.aoeDamage = aoeDamage;
        this.aoeDamageAmount = aoeDamageAmount;
        this.aoeRadius = aoeRadius;
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
        hitStuff = false;
    }


    // hit enermy or obstacles
    private void OnCollisionEnter(Collision collision)
    {


        // stop when hitting enemy, obstacle, or ground
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("obstacle") || collision.gameObject.CompareTag("ground"))
        {

            GetComponent<Rigidbody>().isKinematic = true;
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
            GetComponent<Rigidbody>().isKinematic = true;
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
            transform.position += transform.forward * bulletSpeedScaler * Time.deltaTime;
        }
            
    }

    public void DestroyBulletWhenExistingTooLong()
    {
        if (currentBulletTravelTime >= bulletTravelTime)
        {
            if (hitStuff && hitPosition != Vector3.zero)
            {
                Explode(hitPosition);
            }
            else
            {
                Explode(transform.position);
            }
            
        }
    }

    public Vector3 hitStop()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.01f))
        {
            if (hit.rigidbody.gameObject.CompareTag("enemy") || hit.rigidbody.gameObject.CompareTag("obstacle") || hit.rigidbody.gameObject.CompareTag("ground"))
            {
                hitPos = hit.point;
            }
        }
        return hitPos;
    }

    // shoot a ray backwards to reset the position
    public Vector3 findHitPoint()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 1f))
        {
            if (hit.rigidbody.gameObject.CompareTag("enemy") || hit.rigidbody.gameObject.CompareTag("obstacle") || hit.rigidbody.gameObject.CompareTag("ground"))
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
        Collider[] colliders = Physics.OverlapSphere(pos, aoeRadius);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("enemy"))
            {
                c.gameObject.GetComponent<Enemy_Test>().health -= aoeDamageAmount;
            }
        }
        Destroy(gameObject);
    }
}
