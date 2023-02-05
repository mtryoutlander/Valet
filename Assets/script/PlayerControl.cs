using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerControl : MonoBehaviour
{
    public float walkSpeed, sprintSpeed, sensitivity;
    public float minTurnAngle = -90, maxTurnAngle =90;
    public Camera playerCam;

    //events
    public delegate void SpawnDrivableCar();
    public static event SpawnDrivableCar spawnDrivableCar;



    private float pitch, yaw;
    private bool isSprinting = false, nextToCar = false, talkingToMan = false;
    private Rigidbody rb;
    private GameObject car;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        Look();
        InputControls();
        

    }

    private void InputControls()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
            isSprinting = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;
        if (Input.GetKeyDown(KeyCode.F))
            EnterCar();
        if (Input.GetKeyDown(KeyCode.E))
            TalkToNpc();
    }

    private void FixedUpdate()
    {
       
        Move();
        
    }
    //controlls moveing the camra to look around 
    private void Look()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity; // get y axis of the mouse input
        pitch = Mathf.Clamp(pitch, minTurnAngle, maxTurnAngle);                // clamps or set the pitch to number between the two given number rounding to close if its over or under
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;   //get x axis of the mouse input
        playerCam.transform.localRotation = Quaternion.Euler(pitch, yaw, 0); //moves the camra useing local roation Quaternion.Euler dose some kind of math
    }
    //this method handles all movement code
    private void Move()
    {
        float speed;
        if(isSprinting)
            speed = sprintSpeed;
        else
            speed = walkSpeed;

        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * speed;
        Vector3 forward = new Vector3(-playerCam.transform.right.z,0,playerCam.transform.right.x);/// use this cause the camra is updated sooner and if use body forward it be out of synic use .right so don't fly into the air
        Vector3 wishDirection = (forward * axis.x + playerCam.transform.right * axis.y + Vector3.up * rb.velocity.y); // multiplying forward vector and axis vector not sure how the cam and comes into play velocity should be getting current velocity
        rb.velocity = wishDirection;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SimpleCarController>())
        {
            Debug.Log("Touching car");
            nextToCar = true;
            car = other.gameObject;
        }
        if (other.gameObject.GetComponent<SpawnControler>()) {
            Debug.Log("talking to man");
            talkingToMan = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<SimpleCarController>())
        {
            Debug.Log("Step away from car");
            nextToCar = false;
            car = new GameObject();
        }
        if (other.gameObject.GetComponent<SpawnControler>())
        {
            Debug.Log("leaving man");
            talkingToMan = false;
        }
    }

    private void EnterCar()
    {
        //will use this method to check if their a near by car and switch control of it to the player 
        if (nextToCar)
        {
            SimpleCarController carControls = car.gameObject.GetComponent<SimpleCarController>();
            carControls.PlayerEnterCar(this.gameObject, playerCam);
            nextToCar= false;
        }
    }

    private void TalkToNpc()
    {
        if (talkingToMan)
        {
            if(spawnDrivableCar !=null)
                spawnDrivableCar.Invoke();
        }

        // will use this method to interact with npcs
    }
}
