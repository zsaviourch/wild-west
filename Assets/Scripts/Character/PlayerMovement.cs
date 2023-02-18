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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.Log("No animator");
        }
        else
        {
            Debug.Log(animator.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Shoot();

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
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("shoot", true);
        }
        else
        {
            animator.SetBool("shoot", false);
        }

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
    
}
