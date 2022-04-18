using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public enum Axel
{
    Front,
    Rear
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class CarController2 : MonoBehaviour
{
    #region singleton

    public static CarController2 instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    [SerializeField]
    private float maxAcceleration = 20.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 45.0f;
    [SerializeField]
    private Vector3 _centerOfMass;
    [SerializeField]
    private List<Wheel> wheels;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject carCam;

    public Transform getOutPos;
    public bool isDriving = false;
    private float inputX, inputY;

    private Rigidbody _rb;

    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;
        Cursor.visible = false;
        
    }

    
    private void Update()
    {
        AnimateWheels();
        GetInputs();

        if (isDriving)
        {
            if (Input.GetKey(KeyCode.G))
            {
                getOutCar();
            }
        }
        
    }

    private void LateUpdate()
    {
        Move();
        Turn();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ActivateBreaking();
        if (Input.GetKeyUp(KeyCode.Space))
            DeactivateBreaking();


    }

    IEnumerator WaitForGetOut()
    {
        yield return new WaitForSeconds(2f);

        


    }

    private void getOutCar()
    {
        isDriving = false;
        
        player.GetComponent<Player>().isDriving = false;
        player.transform.position = getOutPos.position;
        carCam.SetActive(false);
        GetComponent<CarController2>().enabled = false;
        player.SetActive(true);
    }

    private void ActivateBreaking()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.brakeTorque = 3000;
            
        }
        
    }

    private void DeactivateBreaking()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.brakeTorque = 0;

        }

    }


    private void GetInputs()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.motorTorque = inputY * maxAcceleration * 500 * Time.deltaTime;
        }
    }

    private void Turn()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = inputX * turnSensitivity * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle,_steerAngle,0.5f);
            }
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }
    }
}
