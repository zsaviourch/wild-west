using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JasperRifle))]
[RequireComponent(typeof(BerylShotgun))]
[RequireComponent(typeof(OnyxSnipper))]
[RequireComponent(typeof(TopazGun))]
public class GunManager : MonoBehaviour
{
    private static GunManager instance;
    public bool isInitialized = false;
    public GunType gunName;
    public bool randomGun;

    // Singleton Instance
    public static GunManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GunManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GunManager).Name;
                    instance = obj.AddComponent<GunManager>();
                }
            }
            return instance;
        }
    }

    public GunType GunName
    {
        get
        {
            return gunName;
        }
        set
        {
            gunName = value;
        }
    }
    public bool RandomGun
    {
        get
        {
            return randomGun;
        }
        set
        {
            randomGun = value;
        }
    }

    public GunManager(GunType gun, bool random)
    {
        gunName = gun;
        isInitialized = true;

        SetUpGunScript(gun);
    }

    // References
    public GameObject player;
    private JasperRifle JasperRifleScript;
    private BerylShotgun BerylShotgunScript;
    private OnyxSnipper OnyxSniperScript;
    private TopazGun TopazGunScript;

    public enum GunType
    {
        JasperRifle,
        BerylShotgun,
        OnyxSniper,
        TopazGun,
        NoGun

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        JasperRifleScript = GetComponent<JasperRifle>();
        BerylShotgunScript = GetComponent<BerylShotgun>();
        OnyxSniperScript = GetComponent<OnyxSnipper>();
        TopazGunScript = GetComponent<TopazGun>();
        gunName = GunType.NoGun;

        JasperRifleScript.enabled = false;
        BerylShotgunScript.enabled = false;
        OnyxSniperScript.enabled = false;
        TopazGunScript.enabled = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(randomGun);
        if (isInitialized == false)
        {
            if (gunName != GunType.NoGun)
            {
                SetUpGunScript(gunName);
                isInitialized = true;
            }
            else if (randomGun)
            {
                SetUpGunScript(true);
                isInitialized = true;
            }
            else
            {
                Debug.Log("Either enter a gun type or set the randomization variable to true!");
            }
        }
    }

    void SetUpGunScript(GunType name)
    {
        switch (name)
        {
            case GunType.JasperRifle:
                ToggleGunScript(GunType.JasperRifle, true);
                ToggleGunScript(GunType.BerylShotgun, false);
                ToggleGunScript(GunType.OnyxSniper, false);
                ToggleGunScript(GunType.TopazGun, false);
                break;
            case GunType.BerylShotgun:
                ToggleGunScript(GunType.JasperRifle, false);
                ToggleGunScript(GunType.BerylShotgun, true);
                ToggleGunScript(GunType.OnyxSniper, false);
                ToggleGunScript(GunType.TopazGun, false);
                break;
            case GunType.OnyxSniper:
                ToggleGunScript(GunType.JasperRifle, false);
                ToggleGunScript(GunType.BerylShotgun, false);
                ToggleGunScript(GunType.OnyxSniper, true);
                ToggleGunScript(GunType.TopazGun, false);
                break;
            case GunType.TopazGun:
                ToggleGunScript(GunType.JasperRifle, false);
                ToggleGunScript(GunType.BerylShotgun, false);
                ToggleGunScript(GunType.OnyxSniper, false);
                ToggleGunScript(GunType.TopazGun, true);
                break;
        }
    }

    void SetUpGunScript(bool rand)
    {
        if (rand)
        {
            int randNum = Random.Range(1, 4);
            switch(randNum) 
            {
                case 1:
                    SetUpGunScript(GunType.JasperRifle);
                    break;
                case 2:
                    SetUpGunScript(GunType.BerylShotgun);
                    break;
                case 3:
                    SetUpGunScript(GunType.OnyxSniper);
                    break;
                case 4:
                    SetUpGunScript(GunType.TopazGun);
                    break;
            }
        }
        else
        {
            Debug.Log("Please enter the gun type or set the randomization variable to true to initialize the character!");
        }
    }

    void ToggleGunScript(GunType name, bool flag)
    {
        switch (name)
        {
            case GunType.JasperRifle:
                JasperRifleScript.enabled = flag;
                break;
            case GunType.BerylShotgun:
                BerylShotgunScript.enabled = flag;
                break;
            case GunType.OnyxSniper:
                OnyxSniperScript.enabled = flag;
                break;
            case GunType.TopazGun:
                TopazGunScript.enabled = flag;
                break;
        }
    }
}
