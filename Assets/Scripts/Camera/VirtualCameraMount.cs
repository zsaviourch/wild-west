using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraMount : MonoBehaviour
{
    void Start()
    {
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = PlayerController.Instance.transform;
        }
    }
}
