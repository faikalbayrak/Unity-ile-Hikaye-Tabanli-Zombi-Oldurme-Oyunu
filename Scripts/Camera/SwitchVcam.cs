using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SwitchVcam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private int priorityBoostAmount = 10;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && Player.instance.animator.GetBool("isPistol"))
        {
            Debug.Log("girdi");
            StartAim();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && Player.instance.animator.GetBool("isPistol"))
        {
            CancelAim();
        }
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount; 
    }
    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
    }
}
