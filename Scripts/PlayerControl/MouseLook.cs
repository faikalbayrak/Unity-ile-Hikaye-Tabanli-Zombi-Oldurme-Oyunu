using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //#region singleton

    //public static MouseLook instance;
    //private void Awake()
    //{

    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //}

    //#endregion
    //[Range(50,500)]
    //public float sens;
    //public Transform body;
    //Transform skaleton;
    //Animator animator;
    //float xRot = 0f;
    //public float rotX;
    //public float rotY;

    //void Start()
    //{
    //    animator = GetComponentInParent<Animator>();
    //    skaleton = animator.GetBoneTransform(HumanBodyBones.Chest);
    //}

    private void LateUpdate()
    {
       
         //if(!Player.instance.animator.GetBool("Crouching"))
         //       skaleton.rotation = Quaternion.Euler(xRot,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);

        
    }
    // Update is called once per frame
    void Update()
    {
        //if (!Player.instance.InventoryPanel.activeSelf)
        //{
        //    rotX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        //    rotY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;

        //    xRot -= rotY;
        //    xRot = Mathf.Clamp(xRot, -40f, 60);
        //    transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);



        //    body.Rotate(Vector3.up * rotX);
        //}




    }
}
