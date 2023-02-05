using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerControlCar : MonoBehaviour
{
    public Camera carCam;
    //public float speed;
    public GameObject spawnPoint;
    public float forwardAccerate, maxSpeed, turnStrength, reverseAccel;
    [SerializeField] private GameObject player; // will remove serializeField later
    [SerializeField] private Camera playerCam;
    public bool playerInCar = false;


    //car controler fields 
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float currentSteerAngle;
    [SerializeField]private float currentbreakForce;
    private bool isBreaking, acceratingForward, acceratingBackward, turnLeft, turnRight;





    void Start()
    {
        rb.transform.parent = null;
        if (playerInCar)  // only for testing
            PlayerEnterCar(player,playerCam);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerExitCar();
        }
        if (playerInCar)
        {
            GetInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerInCar)
        {
            
            moveCar();
        }
        
    }

    private void moveCar()
    {
        Debug.Log("acceratingForward: " + acceratingForward);

        if(acceratingForward)
            rb.AddForce(transform.forward * forwardAccerate * 100f);
        if (acceratingBackward)
            rb.AddForce(transform.forward * -reverseAccel * 100f);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,horizontalInput*turnStrength*Time.deltaTime,0));
        /*

        if (acceratingForward && rb.transform.forward.z < maxSpeed)
        {
            Debug.Log("going forward");
            rb.AddRelativeForce(rb.transform.forward * accerate);
        }
        else
            Debug.Log("didn't go farword");
        if(acceratingBackward && rb.transform.forward.z > -maxSpeed/2)
            rb.AddRelativeForce(rb.transform.forward * -accerate/2);
        if (isBreaking && (rb.transform.forward.z >0 ))
            rb.AddRelativeForce(rb.velocity.x - currentbreakForce, rb.velocity.y, rb.velocity.z-currentbreakForce);
        if (turnLeft)
        {
           // rb.velocity = new Vector3(rb.velocity.x, currentSteerAngle, rb.velocity.z);
        }
        if (turnRight)
        {
           // rb.velocity = new Vector3(rb.velocity.x, -currentSteerAngle, rb.velocity.z);
        }
        if (!turnRight && !turnLeft) { }
            // rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);


        
        

        //Vector2 axis = new Vector2(verticalInput, horizontalInput);
        //Vector3 forward = rb.transform.forward * verticalInput * accerate;
        // Vector3 forward = new Vector3(-carCam.transform.right.z, 0, carCam.transform.right.x);/// use this cause the camra is updated sooner and if use body forward it be out of synic use .right so don't fly into the air
        //Vector3 wishDirection = (forward * axis.x * axis.y + Vector3.up * rb.velocity.y); // multiplying forward vector and axis vector not sure how the cam and comes into play velocity should be getting current velocity
        //rb.AddForce(wishDirection *accerate);
        */
    }

    public void PlayerEnterCar(GameObject player, Camera playerCam)
    {
        player.transform.position = transform.position;
        player.SetActive(false);
        playerCam.enabled= false;
        this.player = player;
        this.playerCam= playerCam;
        carCam.enabled= true;
        playerInCar = true;
    }
    private void PlayerExitCar()
    {
        Debug.Log("Trying to exit car");
        player.SetActive(true);
        player.transform.position = spawnPoint.transform.position;
        carCam.enabled = false;
        playerInCar = false;
        playerCam.enabled= true;
        
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
        acceratingForward = Input.GetKey(KeyCode.W);
        acceratingBackward= Input.GetKey(KeyCode.S);

    }

}
