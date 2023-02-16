using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JasperRifle : MonoBehaviour
{
    // References
    [SerializeField] int energyInitialAmount;
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

    // Constructor
    public JasperRifle(int energyInitialAmount, float firingFrequencyInterval, int energyRegeneratePerSecond, bool rangeWeapon,
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
        currentReloadTime = 0f;
        currentEnergyAmount = energyInitialAmount;
        reloadInitiated = false;
        energyConsumePerBullet = FindEnergyConsumePerBullet(this.bulletType);
        currentShootingTime = 0f;
        shootInitiated = true;
        shootingPos = GameObject.FindWithTag("shootingPos").transform;

    }

    private void Update()
    {
        // Reload
        if (Input.GetKeyDown(KeyCode.R) && currentEnergyAmount < energyInitialAmount)
        {
            reloadInitiated = true;
            currentReloadTime += Time.deltaTime;
            Reload();
        }

        // Shoot
        if (Input.GetKeyUp(KeyCode.Mouse0) && currentEnergyAmount > energyConsumePerBullet)
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
    }

    // methods
    public void Reload()
    {
        // Play reloading animation here

        // Replenish one unit energy
        currentEnergyAmount += energyRegeneratePerSecond;
        
    }

    public int FindEnergyConsumePerBullet(BulletType type)
    {
        int cost = -1;
        switch (type)
        {
            case BulletType.JasperRifleBullet:
                cost = 5;
                break;
            case BulletType.BerylliumShotgunBullet:
                cost = 10;
                break;
            case BulletType.OnyxSniperBullet:
                cost = 15;
                break;
            case BulletType.TopazGunBullet:
                cost = 20;
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
            Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            currentEnergyAmount -= energyConsumePerBullet;
            currentReloadTime = 0f;
            shootInitiated = false;
        }
    }
}
