using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private float currentHealth;
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get { return instance; }
    }

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
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    { 
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
}
