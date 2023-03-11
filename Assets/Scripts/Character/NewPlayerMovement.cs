using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    [SerializeField] float speedScaler;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    public string whichGun;
    public Orientation orientation;

    public enum Orientation
    {
        Down,
        DownLeft,
        Left,
        UpLeft,
        Up,
        UpRight,
        Right,
        DownRight

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        Movement = Movement.normalized;
        transform.parent.transform.position += Movement * speedScaler * Time.deltaTime;

        Vector3 characterScale = transform.localScale;
        Vector3 lastOrientation = transform.localScale;

        if (x_input != 0 || y_input != 0)
        {
            animator.SetFloat("xVel", Movement.x);
            animator.SetFloat("yVel", Movement.y);
            animator.SetBool("walk", true);
            orientation = FindOrientation(Movement.x, Movement.y);
            SetAnimationOrientationParameter(orientation);

        }
        else
        {
            animator.SetBool("walk", false);
        }

        
    }

    // Examine orientation
    private Orientation FindOrientation(float input_x, float input_y)
    {
        Orientation dir = Orientation.Down;
        if (input_x == 0f && input_y == -1f)
        {
            dir = Orientation.Down;
        }
        else if (input_x < 0f && input_y < 0f)
        {
            dir = Orientation.DownLeft;
        }
        else if (input_x == -1f && input_y == 0f)
        {
            dir = Orientation.Left;
        }
        else if (input_x < 0f && input_y > 0f)
        {
            dir = Orientation.UpLeft;
        }
        else if (input_x == 0f && input_y == 1f)
        {
            dir = Orientation.Up;
        }
        else if (input_x > 0f && input_y > 0f)
        {
            dir = Orientation.UpRight;
        }
        else if (input_x == 1f && input_y == 0f)
        {
            dir = Orientation.Right;
        }
        else if (input_x > 0f && input_y < 0f)
        {
            dir = Orientation.DownRight;
        }
        return dir;
    }

    // Set animation orientation parameters
    private void SetAnimationOrientationParameter(Orientation orientation)
    {
        switch (orientation)
        {
            case Orientation.Down:
                SetAllOrientationToFalse();
                animator.SetBool("faceDown", true);
                break;
            case Orientation.DownLeft:
                SetAllOrientationToFalse();
                animator.SetBool("faceDownLeft", true);
                break;
            case Orientation.Left:
                SetAllOrientationToFalse();
                animator.SetBool("faceLeft", true);
                break;
            case Orientation.UpLeft:
                SetAllOrientationToFalse();
                animator.SetBool("faceUpLeft", true);
                break;
            case Orientation.Up:
                SetAllOrientationToFalse();
                animator.SetBool("faceUp", true);
                break;
            case Orientation.UpRight:
                SetAllOrientationToFalse();
                animator.SetBool("faceUpRight", true);
                break;
            case Orientation.Right:
                SetAllOrientationToFalse();
                animator.SetBool("faceRight", true);
                break;
            case Orientation.DownRight:
                SetAllOrientationToFalse();
                animator.SetBool("faceDownRight", true);
                break;
        }
    }

    // Set all orientation parameter to false
    public void SetAllOrientationToFalse()
    {
        animator.SetBool("faceDown", false);
        animator.SetBool("faceDownLeft", false);
        animator.SetBool("faceLeft", false);
        animator.SetBool("faceUpLeft", false);
        animator.SetBool("faceUp", false);
        animator.SetBool("faceUpRight", false);
        animator.SetBool("faceRight", false);
        animator.SetBool("faceDownRight", false);
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
