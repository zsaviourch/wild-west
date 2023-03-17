using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Color;

public class ShowForwardDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.up * 5f, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.right * 5f, Color.green);
    }
}
