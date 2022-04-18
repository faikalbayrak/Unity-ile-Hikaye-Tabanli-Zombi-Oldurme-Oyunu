using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontal;
    private float vertical;
    private float currentBreakForce;
    private float steerAngle;
    private bool isBreaking;



    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform rearLeftTransform;
    [SerializeField] private Transform rearRightTransform;
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontal = Input.GetAxis(HORIZONTAL);
        vertical = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeft.motorTorque = vertical * motorForce;
        frontRight.motorTorque = vertical * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();


    }

    private void ApplyBreaking()
    {
        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        rearRight.brakeTorque = currentBreakForce;
        rearLeft.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        steerAngle = maxSteerAngle * horizontal;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftTransform);
        UpdateSingleWheel(frontRight, frontRightTransform);
        UpdateSingleWheel(rearLeft, rearLeftTransform);
        UpdateSingleWheel(rearRight, rearRightTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        Debug.Log(wheelTransform.rotation);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
