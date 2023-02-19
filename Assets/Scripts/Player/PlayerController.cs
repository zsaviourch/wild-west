using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get { return instance; }
    }

    private HealthAndEnergy healthAndEnergy;

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
        healthAndEnergy = GetComponent<HealthAndEnergy>();
    }

    public void TakeDamage(int damage)
    {
        healthAndEnergy.TakeDamage(damage);
        if (healthAndEnergy.currentHealth <= 0)
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
