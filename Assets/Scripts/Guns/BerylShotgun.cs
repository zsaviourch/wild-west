using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerylShotgun : MonoBehaviour
{
    // References
    public string gunName;
    public int energyInitialAmount;
    public int currentEnergyAmount;
    [SerializeField] float firingFrequencyInterval;
    public float currentShootingTime;
    public bool shootInitiated;
    [SerializeField] int energyRegeneratePerSecond;
    [SerializeField] bool rangeWeapon;
    [SerializeField] float minimumReloadingTime;
    [SerializeField] float fullyReloadedTime;

    public BulletType bulletType;
    [SerializeField] GameObject bulletPrefab;
    public Transform playerTransform;
    public Transform leftHand;
    public Transform rightHand;
    public GameObject player;

    public float currentReloadTime;
    public bool reloadInitiated;
    public int energyConsumePerBullet;

    public Transform shootingPos;
    public float currentReloadPreparationTime;

    // Constructor
    public BerylShotgun(int energyInitialAmount, float firingFrequencyInterval, int energyRegeneratePerSecond, bool rangeWeapon,
        float minimumReloadingTime, float fullyReloadedTime, GameObject bulletPrefab, BulletType bulletType)
    {
        this.energyInitialAmount = energyInitialAmount;
        this.firingFrequencyInterval = firingFrequencyInterval;
        this.energyRegeneratePerSecond = energyRegeneratePerSecond;
        this.rangeWeapon = rangeWeapon;
        this.minimumReloadingTime = minimumReloadingTime;
        this.fullyReloadedTime = fullyReloadedTime;
        this.bulletType = bulletType;
        this.bulletPrefab = bulletPrefab;
    }

    // property access
    public int EnergyIntialAmount
    {
        get
        {
            return energyInitialAmount;
        }
        set
        {
            if (value > 0)
            {
                energyInitialAmount = value;
            }
            else
            {
                Debug.LogError("energyIntialAmount should be larger than zero!");
            }
        }
    }
    public float FiringFrequencyInterval
    {
        get
        {
            return firingFrequencyInterval;
        }
        set
        {
            firingFrequencyInterval = value;
        }
    }
    public int EnergyRegeneratePerSecond
    {
        get
        {
            return energyRegeneratePerSecond;
        }
        set
        {
            if (value > 0)
            {
                energyRegeneratePerSecond = value;
            }
            else
            {
                Debug.LogError("energyRegeneratePerSecond should be larger than zero!");
            }
        }
    }
    public bool RangeWeapon
    {
        get
        {
            return rangeWeapon;
        }
        set
        {
            rangeWeapon = value;
        }
    }
    public float MinimumReloadingTime
    {
        get
        {
            return minimumReloadingTime;
        }
        set
        {
            if (value > 0)
            {
                minimumReloadingTime = value;
            }
            else
            {
                Debug.LogError("reloadingTime should be larger than zero!");
            }
        }
    }
    public float FullyReloadedTime
    {
        get
        {
            return fullyReloadedTime;
        }
        set
        {
            if (value > 0 && value > minimumReloadingTime)
            {
                fullyReloadedTime = value;
            }
            else
            {
                Debug.LogError("fullyReloadedTime should be larger than zero and the reloadingTime");
            }
        }
    }

    // enum of bullets
    public enum BulletType
    {
        JasperRifleBullet,
        BerylliumShotgunBullet,
        OnyxSniperBullet,
        TopazGunBullet

    }

    private void Awake()
    {
        gunName = "BerylShotgun";
        player = GameObject.FindWithTag("Player");
        currentReloadTime = 0f;
        energyInitialAmount = player.GetComponent<HealthAndEnergy>().energyInitialAmount;
        currentEnergyAmount = energyInitialAmount;
        reloadInitiated = false;
        energyConsumePerBullet = FindEnergyConsumePerBullet(this.bulletType);
        currentShootingTime = 0f;
        shootInitiated = false;
        shootingPos = GameObject.FindWithTag("shootingPos").transform;
        currentReloadPreparationTime = 0f;


    }

    private void Update()
    {
        currentEnergyAmount = player.GetComponent<HealthAndEnergy>().currentEnergyAmount;

        // Reload
        if (Input.GetMouseButtonDown(1) && currentEnergyAmount < energyInitialAmount)
        {
            reloadInitiated = true;
            currentReloadTime += Time.deltaTime;
            Reload();
            // AkSoundEngine.PostEvent("gunReload", gameObject);
        }

        // Shoot
        if (Input.GetMouseButtonDown(0) && currentEnergyAmount > energyConsumePerBullet)
        {
            Shoot();
        }

        // Shooting flag adjustment
        if (currentShootingTime <= firingFrequencyInterval)
        {
            currentShootingTime += Time.deltaTime;
        }
        else
        {
            shootInitiated = true;
            currentShootingTime = 0f;
        }

        // Add energy when idle
        if (!Input.GetMouseButtonUp(1) && !Input.GetMouseButtonUp(0) && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            currentReloadPreparationTime += Time.deltaTime;
        }
        else
        {
            currentReloadPreparationTime = 0f;
        }

        if (currentReloadPreparationTime >= minimumReloadingTime)
        {
            if (currentEnergyAmount <= EnergyIntialAmount)
            {
                currentEnergyAmount += (int)System.Math.Round(energyRegeneratePerSecond * Time.deltaTime);
                player.GetComponent<HealthAndEnergy>().currentEnergyAmount = currentEnergyAmount;
            }
        }
    }

    // methods
    public void Reload()
    {
        // Play reloading animation here

        // Replenish one unit energy
        currentEnergyAmount = energyInitialAmount;
        player.GetComponent<HealthAndEnergy>().currentEnergyAmount = currentEnergyAmount;
        AudioManager.Instance.Play("shotgunReload");
    }

    public int FindEnergyConsumePerBullet(BulletType type)
    {
        int cost = -1;
        switch (type)
        {
            case BulletType.JasperRifleBullet:
                cost = 8;
                break;
            case BulletType.BerylliumShotgunBullet:
                cost = 20;
                break;
            case BulletType.OnyxSniperBullet:
                cost = 30;
                break;
            case BulletType.TopazGunBullet:
                cost = 1;
                break;
        }
        if (cost == -1)
        {
            Debug.LogError("cost not found!");
        }
        return cost;
    }

    public void Shoot()
    {
        if (shootInitiated == true)
        {

            StartCoroutine(InstantiateBullets());


            currentEnergyAmount -= energyConsumePerBullet;
            player.GetComponent<HealthAndEnergy>().currentEnergyAmount = currentEnergyAmount;
            currentReloadTime = 0f;
            shootInitiated = false;
            // AkSoundEngine.PostEvent("gunShoot", gameObject);
            AudioManager.Instance.Play("shotgunShot");
        }
    }

    private IEnumerator InstantiateBullets()
    {
        if (gameObject.transform.localScale.x == 1)
        {
            GameObject Bullet1 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet1.transform.localScale = new Vector3(-1, 1, 1);
            Bullet1.transform.Rotate(new Vector3(0, 0, 22.5f));
            GameObject Bullet2 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet2.transform.localScale = new Vector3(-1, 1, 1);
            Bullet2.transform.Rotate(new Vector3(0, 0, 11.25f));
            GameObject Bullet3 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet3.transform.localScale = new Vector3(-1, 1, 1);
            Bullet2.transform.Rotate(new Vector3(0, 0, 0));
            GameObject Bullet4 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet4.transform.localScale = new Vector3(-1, 1, 1);
            Bullet4.transform.Rotate(new Vector3(0, 0, -11.25f));
            GameObject Bullet5 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet5.transform.localScale = new Vector3(-1, 1, 1);
            Bullet5.transform.Rotate(new Vector3(0, 0, -22.5f));
        }
        else
        {
            GameObject Bullet6 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet6.transform.right = shootingPos.right;
            Bullet6.transform.Rotate(new Vector3(0, 0, 22.5f));
            GameObject Bullet7 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet7.transform.right = shootingPos.right;
            Bullet7.transform.Rotate(new Vector3(0, 0, 11.25f));
            GameObject Bullet8 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet8.transform.right = shootingPos.right;
            GameObject Bullet9 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet9.transform.right = shootingPos.right;
            Bullet9.transform.Rotate(new Vector3(0, 0, -11.25f));
            GameObject Bullet10 = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            Bullet10.transform.right = shootingPos.right;
            Bullet10.transform.Rotate(new Vector3(0, 0, -22.5f));
        }

        yield return new WaitForSeconds(0f);
    }
}
