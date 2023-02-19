using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrystal : MonoBehaviour
{
    [SerializeField] private int crystalDamage = 20;
    [SerializeField] private Rigidbody2D rb;

// hit enemy or obstacles
private void OnTriggerEnter2D(Collider2D collision)
{
    // hit the enemy
    if (collision.gameObject.CompareTag("Player"))
    {
        collision.gameObject.GetComponent<PlayerController>().TakeDamage(crystalDamage);
        rb.isKinematic = true;
    }

    // hit the obstacle
    if (collision.gameObject.CompareTag("obstacle"))
    {
        Destroy(collision.gameObject);
    }
    Destroy(this.gameObject);
}

}
