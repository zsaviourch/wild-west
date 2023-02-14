using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : MonoBehaviour
{
    // References
    public int health;

    // Start is called before the first frame update
    void Awake()
    {
        health = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
