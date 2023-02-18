using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 200;
    [SerializeField] protected int damagePerHit = 10;

    [SerializeField] protected Animator animator;

    protected int currentHealth;
    protected bool isDead = false;

    public bool IsDead 
    {
        get { return isDead; }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log("AIController.TakeDamage" + currentHealth + damage);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            animator.SetBool("isDead", true);
        }
    }
}