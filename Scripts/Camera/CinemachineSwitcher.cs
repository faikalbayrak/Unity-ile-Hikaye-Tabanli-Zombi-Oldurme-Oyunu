using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    
    private Animator animator;
   


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && Player.instance.animator.GetBool("isPistol"))
        {
            animator.Play("TP Camera Aim");
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && Player.instance.animator.GetBool("isPistol"))
        {
            animator.Play("TP Camera");
        }
    }

}
