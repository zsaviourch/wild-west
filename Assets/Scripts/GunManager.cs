using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    // References
    [SerializeField] GameObject player;
    [SerializeField] GameObject JasperRifle;
    [SerializeField] GameObject BerylliumShotgun;
    [SerializeField] GameObject OnyxSniper;
    [SerializeField] GameObject TopazGun;
    [SerializeField] GameObject jasperRifleBulletPrefab;


    public enum BulletType
    {
        JasperRifleBullet,
        BerylliumShotgunBullet,
        OnyxSniperBullet,
        TopazGunBullet

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
