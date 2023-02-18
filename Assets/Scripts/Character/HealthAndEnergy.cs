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

    // Update is called once per frame
    void Update()
    {
        
    }
}
