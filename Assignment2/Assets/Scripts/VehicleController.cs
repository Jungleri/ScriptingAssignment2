using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField]
    private WheelCollider wcFL;
    [SerializeField]
    private WheelCollider wcFR;
    [SerializeField]
    private WheelCollider wcRL;
    [SerializeField]
    private WheelCollider wcRR;


    [Header("Vehicle Settings")]
    [SerializeField]
    private float enginePower = 100f;
    [SerializeField]
    private float breakPower = 100f;
    [SerializeField]
    private float steeringForce = 50f;
    private bool nitrousUsed = false;
    [SerializeField]
    private float nitrousMultiplier = 3f;
    [SerializeField]
    private float nitrousSteeringEffect = 0.5f;

    [Header("Other")]
    public Text rpmText;



    void FixedUpdate()
    {
        UpdateSpeed();
        if (Input.GetAxisRaw("Vertical") != 0f)
        {
            DriveVehicle(Input.GetAxisRaw("Vertical"));
        }
        else
        {
            //If I am not moving forwards, apply an automatic handbreak, so I can stop and park on hills.
            //VehicleBreak();
        }

        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            SteerVehicle(Input.GetAxisRaw("Horizontal"));
        }
        else
        {
            //If I am not steering in a direction, I want to move forward, reset the steering column.
            ResetSteering();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            nitrousUsed = true;
        }
        else
        {
            nitrousUsed = false;
        } 

        if (Input.GetKey(KeyCode.Space))
        {
            VehicleBreak();
        }
}


    private void DriveVehicle(float _driveDirection)
    {
        //Remove any break torque and apply engine power.
        wcFL.brakeTorque = 0f;
        wcFR.brakeTorque = 0f;  
        wcRL.brakeTorque = 0f;
        wcRR.brakeTorque = 0f;

        if(nitrousUsed)
        {
            wcFL.motorTorque = enginePower * nitrousMultiplier * _driveDirection;
            wcFR.motorTorque = enginePower * nitrousMultiplier * _driveDirection;
        }
        else
        {
            wcFL.motorTorque = enginePower * _driveDirection;
            wcFR.motorTorque = enginePower * _driveDirection;
        }
    }


    private void VehicleBreak()
    {
        //Reset the engine torque and apply break power.
        wcFL.motorTorque = 0f;
        wcFR.motorTorque = 0f;
        wcFL.brakeTorque = breakPower;
        wcFR.brakeTorque = breakPower;
        wcRL.brakeTorque = breakPower;
        wcRR.brakeTorque = breakPower;
    }


    private void SteerVehicle(float _steerDir)
    {
        //Steer the vehicle in the given direction, based on A/D or Left/Right input.
        if (nitrousUsed)
        {
            wcFL.steerAngle = _steerDir * steeringForce * nitrousSteeringEffect;
            wcFR.steerAngle = _steerDir * steeringForce * nitrousSteeringEffect;
        }
        else
        {
            wcFL.steerAngle = Mathf.Lerp(wcFL.steerAngle, _steerDir * steeringForce, Time.deltaTime * 3);
            wcFR.steerAngle = Mathf.Lerp(wcFR.steerAngle, _steerDir * steeringForce, Time.deltaTime * 3);
        }

    }

    private void ResetSteering()
    {
        //Reset the steering column back to centre.
        wcFL.steerAngle = Mathf.Lerp(wcFL.steerAngle, 0, Time.deltaTime * 2);
        wcFR.steerAngle = Mathf.Lerp(wcFR.steerAngle, 0, Time.deltaTime * 2);
    }


    private void UpdateSpeed()
    {
        //rpmText.text = Mathf.RoundToInt(wcFL.rpm).ToString();
    }
}
