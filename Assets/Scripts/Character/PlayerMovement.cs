using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    [SerializeField] float speedScaler;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    public string whichGun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("No animator");
        }
        else
        {
            Debug.Log(animator.name);
        }
        whichGun = FindGunName();
    }

    // Update is called once per frame
    private void Update()
    {

        if (!Input.GetMouseButtonDown(0)) Move();

        if (EnoughAmmo(whichGun)) Shoot();

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Reload());
        }
    }

    // Move
    private void Move()
    {
        float x_input = Input.GetAxisRaw("Horizontal");
        float y_input = Input.GetAxisRaw("Vertical");

        Vector3 Movement = new Vector3(x_input, y_input, 0f);
        transform.position += Movement * speedScaler * Time.deltaTime;

        Vector3 characterScale = transform.localScale;
        Vector3 lastOrientation = transform.localScale;

        if (x_input != 0 || y_input != 0)
        {
            animator.SetBool("walk", true);

        }
        else
        {
            animator.SetBool("walk", false);
        }

        if (x_input < 0)
        {
            characterScale.x = -1;
            transform.localScale = characterScale;
            lastOrientation = characterScale;
        }
        else if (x_input > 0)
        {
            characterScale.x = 1;
            transform.localScale = characterScale;
            lastOrientation = characterScale;
        }
        else
        {
            transform.localScale = lastOrientation;
        }
    }

    // Shoot
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && StartToShot(whichGun))
        {
            Debug.Log("animation");
            StartCoroutine(PlayShootAnimation());
        }

    }

    private IEnumerator PlayShootAnimation()
    {
        animator.SetBool("shoot", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("shoot", false);
    }

    // MassiveShoot
    private void MassiveShoot()
    {
        animator.SetBool("massive_shoot", true);
    }

    // Reload (shoot_reload, massive_shoot_reload, walk_reload)
    private IEnumerator Reload()
    {
        animator.SetBool("shoot_reload", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("shoot_reload", false);        

    }

    // Find the gun name
    private string FindGunName()
    {
        string gunName = null;
        if (GetComponent<JasperRifle>() != null) gunName = "JasperRifle";
        else if (GetComponent<BerylShotgun>() != null) gunName = "BerylShotgun";
        else if (GetComponent<OnyxSnipper>() != null) gunName = "OnyxSnipper";
        else if (GetComponent<TopazGun>() != null) gunName = "TopazGun";
        return gunName;
    }

    // Find the gun shot initiation variable
    private bool StartToShot(string gunName)
    {
        bool startToShoot = false;
        switch (gunName)
        {
            case "JasperRifle":
                startToShoot = GetComponent<JasperRifle>().shootInitiated;
                break;
            case "BerylShotgun":
                startToShoot = GetComponent<BerylShotgun>().shootInitiated;
                break;
            case "OnyxSnipper":
                startToShoot = GetComponent<OnyxSnipper>().shootInitiated;
                break;
            case "TopazGun":
                startToShoot = GetComponent<TopazGun>().shootInitiated;
                break;
        }
        return startToShoot;
    }

    // Find whether there is enough ammo
    private bool EnoughAmmo(string gunName)
    {
        bool enoughEmmo = false;
        int energy = GetComponent<HealthAndEnergy>().currentEnergyAmount;
        switch (gunName)
        {
            case "JasperRifle":
                enoughEmmo = energy >= GetComponent<JasperRifle>().energyConsumePerBullet;
                break;
            case "BerylShotgun":
                enoughEmmo = energy >= GetComponent<BerylShotgun>().energyConsumePerBullet;
                break;
            case "OnyxSnipper":
                enoughEmmo = energy >= GetComponent<OnyxSnipper>().energyConsumePerBullet;
                break;
            case "TopazGun":
                enoughEmmo = energy >= GetComponent<TopazGun>().energyConsumePerBullet;
                break;
        }
        return enoughEmmo;
    }

}
