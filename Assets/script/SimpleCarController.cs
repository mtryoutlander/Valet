using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}


public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float breakForce = 20;
    public bool playerInCar = false;
    public Camera carCam;
    public GameObject spawnPoint;

    //events
    public delegate void TimmerStart();
    public static event TimmerStart carParkingStart;
    public delegate void TimmerStop();
    public static event TimmerStop carParkingStop;


    [SerializeField] private GameObject player; // will remove serializeField later
    [SerializeField] private Camera playerCam;
    private bool isParked = false;


    public void Start()
    {
        if (playerInCar)  // only for testing
            PlayerEnterCar(player, playerCam);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInCar)
        {
            PlayerExitCar();
        }
    }

    private void PlayerExitCar()
    {
        Debug.Log("Trying to exit car");
        player.SetActive(true);
        player.transform.position = spawnPoint.transform.position;
        carCam.enabled = false;
        playerInCar = false;
        playerCam.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish") && carParkingStop != null)
        {
            carParkingStop.Invoke();
        }
    }
    public void PlayerEnterCar(GameObject player, Camera playerCam)
    {
        Debug.Log("player in car");
        if (!isParked && carParkingStart != null)
            carParkingStart.Invoke();
        player.transform.position = transform.position;
        player.SetActive(false);
        playerCam.enabled = false;
        this.player = player;
        this.playerCam = playerCam;
        carCam.enabled = true;
        playerInCar = true;
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {

        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }



    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //Debug.Log("Motor: "+motor + " Steering: " + steering);
        //Debug.Log("RPM: " + axleInfos[0].leftWheel.rpm);
        if (playerInCar)
        {
            moveCar(steering, motor);
        }
    }

    private void moveCar(float steering, float motor)
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                axleInfo.rightWheel.brakeTorque = breakForce;
                axleInfo.leftWheel.brakeTorque = breakForce;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }
            if (axleInfo.steering )
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor && axleInfo.leftWheel.brakeTorque <= 0)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
           
            
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    private void Stop(AxleInfo axleInfo)
    {
        float targetBreakForce = 0;

        if (axleInfo.leftWheel.motorTorque > 0)
        {
            targetBreakForce = axleInfo.leftWheel.motorTorque - breakForce;
            if (axleInfo.leftWheel.rpm != 0)
                axleInfo.leftWheel.motorTorque = targetBreakForce;
            else
                axleInfo.leftWheel.motorTorque = 0;
        }
        else
        {
            targetBreakForce = axleInfo.leftWheel.motorTorque + breakForce;
            if (axleInfo.leftWheel.rpm != 0)
                axleInfo.leftWheel.motorTorque = targetBreakForce;
            else
                axleInfo.leftWheel.motorTorque = 0;

        }
        if (axleInfo.rightWheel.motorTorque > 0)
        {
            targetBreakForce = axleInfo.rightWheel.motorTorque - breakForce;
            if (axleInfo.rightWheel.rpm != 0)
                axleInfo.rightWheel.motorTorque = targetBreakForce;
            else
                axleInfo.rightWheel.motorTorque = 0;
        }
        else
        {
            targetBreakForce = axleInfo.rightWheel.motorTorque + breakForce;
            if (axleInfo.rightWheel.rpm != 0)
                axleInfo.rightWheel.motorTorque = targetBreakForce;
            else
                axleInfo.rightWheel.motorTorque = 0;
        }
    }
}


