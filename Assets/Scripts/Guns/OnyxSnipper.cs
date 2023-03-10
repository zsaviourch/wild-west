using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnyxSnipper : MonoBehaviour
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
    public OnyxSnipper(int energyInitialAmount, float firingFrequencyInterval, int energyRegeneratePerSecond, bool rangeWeapon,
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
        gunName = "OnyxSnipper";
        currentReloadTime = 0f;
        player = GameObject.FindWithTag("Player");
        energyInitialAmount = player.GetComponent<HealthAndEnergy>().energyInitialAmount;
        currentEnergyAmount = energyInitialAmount;
        reloadInitiated = false;
        energyConsumePerBullet = FindEnergyConsumePerBullet(this.bulletType);
        currentShootingTime = 0f;
        shootInitiated = true;
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
            // AkSoundEngine.PostEvent("gunShoot", gameObject);
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
        AudioManager.Instance.Play("sniperReload");
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
            GameObject bullet = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            bullet.transform.right = shootingPos.right;
            currentEnergyAmount -= energyConsumePerBullet;
            player.GetComponent<HealthAndEnergy>().currentEnergyAmount = currentEnergyAmount;
            currentReloadTime = 0f;
            shootInitiated = false;
            AudioManager.Instance.Play("sniperShot");
        }
    }
}
