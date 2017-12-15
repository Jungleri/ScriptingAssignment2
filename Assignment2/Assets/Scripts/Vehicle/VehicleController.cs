using UnityEngine;
using UnityEngine.UI;
using System.Collections;


enum DriveType { FrontWheelDrive, RearWheelDrive, AllWheelDrive }

public class VehicleController : MonoBehaviour
{


    [Header("Wheel Colliders")]
    [SerializeField] WheelCollider wcFL;
    [SerializeField] WheelCollider wcFR;
    [SerializeField] WheelCollider wcRL;
    [SerializeField] WheelCollider wcRR;

    [Header("Vehicle Settings")]
    [SerializeField] DriveType driveType = DriveType.FrontWheelDrive;
    [SerializeField] float enginePower = 100f;
    [SerializeField] float breakPower = 100f;
    [SerializeField] float steeringForce = 50f;
    [SerializeField] float nitrousMultiplier = 3f;
    [SerializeField] float nitrousSteeringEffect = 0.5f;

    bool nitrousUsed = false;
    WheelCollider[] myDriveWheels;


    void Start()
    {
        if(driveType == DriveType.FrontWheelDrive)
        {

            myDriveWheels = new WheelCollider[2];
            myDriveWheels[0] = wcFL;
            myDriveWheels[1] = wcFR;

        }
        else if(driveType == DriveType.RearWheelDrive)
        {
            myDriveWheels = new WheelCollider[2];
            myDriveWheels[0] = wcRL;
            myDriveWheels[1] = wcRR;

            Debug.Log("Set3");
        }
        else if (driveType == DriveType.AllWheelDrive)
        {
            myDriveWheels = new WheelCollider[4];
            myDriveWheels[0] = wcFL;
            myDriveWheels[1] = wcFR;
            myDriveWheels[3] = wcRL;
            myDriveWheels[4] = wcRR;

            Debug.Log("Set2");
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Vertical") != 0f)
        {   //Apply forward/backward drive to the vehicle.
            DriveVehicle(Input.GetAxisRaw("Vertical"));
        }
        else
        {
            for (int i = 0; i < myDriveWheels.Length; i++)
            {
                myDriveWheels[i].motorTorque = 0f;
            }
        }

        if (Input.GetAxisRaw("Horizontal") != 0f)
        {   //Steer the vehicle left/right.
            SteerVehicle(Input.GetAxisRaw("Horizontal"));
        }
        else
        {   //If I am not steering in a direction, I want to move forward, reset the steering column.
            ResetSteering();
        }

        if (Input.GetKey(KeyCode.LeftShift))
            //Simple 'On whilst held' for the nitrous.
            nitrousUsed = true;
        else
            nitrousUsed = false;

        if (Input.GetKey(KeyCode.Space))
        {   //Apply breaks to the vehicle.
            VehicleBreak();
        }
    }


    private void DriveVehicle(float _driveDirection)
    {   //Remove any break torque and apply engine power.
        wcFL.brakeTorque = 0f;
        wcFR.brakeTorque = 0f;  
        wcRL.brakeTorque = 0f;
        wcRR.brakeTorque = 0f;


        if(nitrousUsed)
        {   //Apply motor torque to the wheels.

            for (int i = 0; i < myDriveWheels.Length; i++)
            {
                myDriveWheels[i].motorTorque = enginePower * nitrousMultiplier * _driveDirection;
            }
        }
        else
        {
            for (int j = 0; j < myDriveWheels.Length; j++)
            {
                myDriveWheels[j].motorTorque = enginePower * nitrousMultiplier * _driveDirection;
            }
        }
    }


    private void VehicleBreak()
    {   //Reset the engine torque and apply break power.
        wcFL.motorTorque = 0f;
        wcFR.motorTorque = 0f;
        wcFL.brakeTorque = breakPower;
        wcFR.brakeTorque = breakPower;
        wcRL.brakeTorque = breakPower;
        wcRR.brakeTorque = breakPower;
    }


    private void SteerVehicle(float _steerDir)
    {   //Steer the vehicle in the given direction, based on A/D or Left/Right input.
        if (nitrousUsed)
        {   //If nitrous used, steering is more restricted.
            wcFL.steerAngle = Mathf.Lerp(wcFL.steerAngle, _steerDir * steeringForce * nitrousSteeringEffect, Time.deltaTime * 3);
            wcFR.steerAngle = Mathf.Lerp(wcFR.steerAngle, _steerDir * steeringForce * nitrousSteeringEffect, Time.deltaTime * 3);
        }
        else
        {   //Otherwise, apply reegular steering to the front wheels.
            wcFL.steerAngle = Mathf.Lerp(wcFL.steerAngle, _steerDir * steeringForce, Time.deltaTime * 3);
            wcFR.steerAngle = Mathf.Lerp(wcFR.steerAngle, _steerDir * steeringForce, Time.deltaTime * 3);
        }

    }


    private void ResetSteering()
    {   //Reset the steering column back to centre.
        wcFL.steerAngle = Mathf.Lerp(wcFL.steerAngle, 0, Time.deltaTime * 2);
        wcFR.steerAngle = Mathf.Lerp(wcFR.steerAngle, 0, Time.deltaTime * 2);
    }
}
