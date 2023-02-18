using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopazGun : MonoBehaviour
{
    // References
    public string gunName;
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
    public bool mouseButtonDown;
    public bool shootingPrepared;
    [SerializeField] float shootingPreparationTime;
    public float currentShootingPreparationTime;

    // Constructor
    public TopazGun(int energyInitialAmount, float firingFrequencyInterval, int energyRegeneratePerSecond, bool rangeWeapon,
        float minimumReloadingTime, float fullyReloadedTime, GameObject bulletPrefab, BulletType bulletType, float shootingPreparationTime)
    {
        this.energyInitialAmount = energyInitialAmount;
        this.firingFrequencyInterval = firingFrequencyInterval;
        this.energyRegeneratePerSecond = energyRegeneratePerSecond;
        this.rangeWeapon = rangeWeapon;
        this.minimumReloadingTime = minimumReloadingTime;
        this.fullyReloadedTime = fullyReloadedTime;
        this.bulletType = bulletType;
        this.bulletPrefab = bulletPrefab;
        this.shootingPreparationTime = shootingPreparationTime;
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
        gunName = "TopazGun";
        currentReloadTime = 0f;
        currentEnergyAmount = energyInitialAmount;
        reloadInitiated = false;
        energyConsumePerBullet = FindEnergyConsumePerBullet(this.bulletType);
        currentShootingTime = 0f;
        shootInitiated = true;
        shootingPos = GameObject.FindWithTag("shootingPos").transform;
        mouseButtonDown = false;
        shootingPrepared = false;
        currentShootingPreparationTime = 0f;

    }

    private void Update()
    {
        // Reload
        if (Input.GetMouseButtonDown(1) && currentEnergyAmount < energyInitialAmount)
        {
            reloadInitiated = true;
            currentReloadTime += Time.deltaTime;
            Reload();
        }

        // Check whether the mouse button has been held down constantly
        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseButtonDown = false;
        }

        // Shoot
        if (shootingPrepared && mouseButtonDown && shootInitiated && currentEnergyAmount > energyConsumePerBullet)
        {
            Shoot();
        }

        // Shooting flag adjustment
        if (currentShootingTime <= firingFrequencyInterval)
        {
            currentShootingTime += Time.deltaTime;
            shootInitiated = false;
        }
        else
        {
            shootInitiated = true;
            currentShootingTime = 0f;
        }

        // Shooting 2s preparation time update
        if (mouseButtonDown)
        {
            if (currentShootingPreparationTime < shootingPreparationTime)
            {
                currentShootingPreparationTime += Time.deltaTime;
                shootingPrepared = false;
            }
            else
            {
                shootingPrepared = true;
            }
        }
        else
        {
            currentShootingPreparationTime = 0f;
            shootingPrepared = false;
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
            currentReloadTime = 0f;
            shootInitiated = false;

        }
    }
}
