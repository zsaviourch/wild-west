using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAndEnergy : MonoBehaviour
{
    // References
    public int energyInitialAmount;
    public int currentEnergyAmount;
    [SerializeField] int health;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergyAmount = energyInitialAmount;
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDied?.Invoke(this);
        }
    }

    public delegate void OnDiedHandler(HealthAndEnergy sender);
    public event OnDiedHandler OnDied;
}
