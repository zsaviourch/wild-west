using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get { return instance; }
    }

    private HealthAndEnergy healthAndEnergy;
    private Rigidbody2D rigidbody2D;

    private Vector3 pushDirection;
    private float pushForce;
    private bool isStunned = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        healthAndEnergy = GetComponent<HealthAndEnergy>();
    }

    public void Push(Vector2 direction, float force)
    {
        if (!isStunned)
        {
            rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    public void PushAndStun(Vector3 direction, float force)
    {
        Debug.Log("Push: Push and stun");
        if (!isStunned)
        {
            pushDirection = direction;
            pushForce = force;
            StartCoroutine(PerformPush());
        }
    }

    IEnumerator PerformPush()
    {   
        Debug.Log("Push: Perform push");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

        yield return new WaitForFixedUpdate();

        // Apply stun after push is completed
        StunPlayer(1.0f);
    }

    public void StunPlayer(float duration)
    {
        Debug.Log("Push: Stun player");
        isStunned = true;
        StartCoroutine(PerformStun(duration));
    }

    IEnumerator PerformStun(float duration)
    {
        // Stop player movement and disable input during stun
        DisablePlayerInput();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(duration);

        // End stun
        isStunned = false;
        EnablePlayerInput();
    }

    private void DisablePlayerInput()
    {
        // Disable player input here
    }

    private void EnablePlayerInput()
    {
        // Enable player input here
    }


    public void TakeDamage(int damage)
    {
        healthAndEnergy.TakeDamage(damage);
        if (healthAndEnergy.currentHealth <= 0)
        {
            Die();
            // Add your logic here
        }
    }

    private void Die()
    {
        Debug.Log("Die!");
        // Get all the components attached to the game object
        Component[] components = gameObject.GetComponents<Component>();

        // Loop through all the components
        for (int i = 0; i < components.Length; i++)
        {
            // Disable all components except the current script component
            if (components[i] != this)
            {
                Behaviour behavior = components[i] as Behaviour;
                if (behavior != null)
                {
                    behavior.enabled = false;
                }
                else
                {
                    components[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void UnDie()
    {
        Debug.Log("UnDie!");
        // Get all the components attached to the game object
        Component[] components = gameObject.GetComponents<Component>();

        // Loop through all the components
        for (int i = 0; i < components.Length; i++)
        {
            // Disable all components except the current script component
            if (components[i] != this)
            {
                Behaviour behavior = components[i] as Behaviour;
                if (behavior != null)
                {
                    behavior.enabled = true;
                }
                else
                {
                    components[i].gameObject.SetActive(true);
                }
            }
        }
        gameObject.SetActive(true);
    }
}
