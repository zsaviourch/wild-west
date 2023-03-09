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
    public GunType? gunName;
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

    public GunType? GunName
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

        TakeGun(null, true);
    }

    public void TakeGun(GunType? name = null, bool isRand = false)
    {
        Reset();
        if(!isRand)
        {
            SetUpGunScript(name);
        }
        else
        {
            int randomIndex = Random.Range(0, GunType.GetValues(typeof(GunType)).Length - 1);
            GunType randomGunType = (GunType)randomIndex;
            SetUpGunScript(randomGunType);
        }
    }

    void Reset()
    {
        JasperRifleScript.enabled = false;
        BerylShotgunScript.enabled = false;
        OnyxSniperScript.enabled = false;
        TopazGunScript.enabled = false;
    }

    void SetUpGunScript(GunType? name)
    {
        gunName = name;
        switch (name)
        {
            case GunType.JasperRifle:
                ToggleGunScript(GunType.JasperRifle, true);
                ToggleGunScript(GunType.BerylShotgun, false);
                ToggleGunScript(GunType.OnyxSniper, false);
                ToggleGunScript(GunType.TopazGun, false);
                AkSoundEngine.SetSwitch("weaponswitch", "gunRifle", player);
                break;
            case GunType.BerylShotgun:
                ToggleGunScript(GunType.JasperRifle, false);
                ToggleGunScript(GunType.BerylShotgun, true);
                ToggleGunScript(GunType.OnyxSniper, false);
                ToggleGunScript(GunType.TopazGun, false);
                AkSoundEngine.SetSwitch("weaponswitch", "gunShotgun", player);
                break;
            case GunType.OnyxSniper:
                ToggleGunScript(GunType.JasperRifle, false);
                ToggleGunScript(GunType.BerylShotgun, false);
                ToggleGunScript(GunType.OnyxSniper, true);
                ToggleGunScript(GunType.TopazGun, false);
                AkSoundEngine.SetSwitch("weaponswitch", "gunSniper", player);
                break;
            case GunType.TopazGun:
                ToggleGunScript(GunType.JasperRifle, false);
                ToggleGunScript(GunType.BerylShotgun, false);
                ToggleGunScript(GunType.OnyxSniper, false);
                ToggleGunScript(GunType.TopazGun, true);
                AkSoundEngine.SetSwitch("weaponswitch", "gunRevolver", player);
                break;
        }
    }

    void ToggleGunScript(GunType? name, bool flag)
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
