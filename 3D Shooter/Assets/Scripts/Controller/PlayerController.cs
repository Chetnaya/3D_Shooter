using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]

    [Tooltip("The speed at which the player moves")]
    public float moveSpeed = 2f;

    [Tooltip("he speed at which the player rotates left or right (calculated in degrees)")]
    public float lookSpeed = 60f;

    [Tooltip("The power at which the player jumps")]
    public float jumpPower = 8f;
    
    [Tooltip("The strength of gravity")]
    public float gravity = 9.81f;

    [Header("Required Refrences")]
    [Tooltip("The player shooter script that fires projectiles")]
    public Shooter playerShooter;
    

    //The Character controller component on the player
    private CharacterController controller;
    private InputManager inputManager;

    void Start()
    {
        setUpCharacterController();
        setUpInputManager();
    }

    private void setUpCharacterController()
    {
        controller = GetComponent<CharacterController>();
        if(controller == null)
        {
            Debug.LogError("The Player controller script does not have a character controller on the same game object!");
        }
    }

    void setUpInputManager()
    {
        inputManager = InputManager.instance;
    }

    void Update()
    {
        ProcessMovement();
        ProcessRotation();
    }
    
    Vector3 moveDirection;
    
    void ProcessMovement()
    {
        //Get input from input manager
        float leftRightInput = inputManager.horizontalMoveAxis;
        float forwardBackwardInput = inputManager.verticalMoveAxis;
        bool jumpPressed = inputManager.jumpPressed;

        //Handle the control of the player while it is on the ground
        if(controller.isGrounded)
        {
            //Set the movement direction to be the received input, set y to zero as the player is on the ground
            moveDirection = new Vector3 (leftRightInput, 0, forwardBackwardInput);
            //set the move direction in relation to the transform
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * moveSpeed;

            if (jumpPressed)
            {
                moveDirection.y = jumpPower;

            } 

        }
        else
        {
            moveDirection = new Vector3(leftRightInput * moveSpeed, moveDirection.y, forwardBackwardInput * moveSpeed);
            moveDirection = transform.TransformDirection(moveDirection);

        }
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
    void ProcessRotation()
    {
        float horizontalLookInput = inputManager.horizontalLookAxis;
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + horizontalLookInput * lookSpeed * Time.deltaTime, playerRotation.z));
    }
}
