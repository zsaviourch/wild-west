using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematicCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        this.gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
