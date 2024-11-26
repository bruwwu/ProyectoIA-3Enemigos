using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;

    Animator animator;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    public float rotationFPS = 1.0f;
    public float walkingVelocity;
    public float runningVelocity;
    public float groundGravity = -0.05f;
    public float Gravity = -9.8f;

    public float dashSpeed;

    public float dashTime;

    PlayerBaseState _currentState;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


        playerInput.CharacterControls.Move.started += onMovementInput;

        playerInput.CharacterControls.Move.canceled += onMovementInput;

        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Run.started += OnRun;

        playerInput.CharacterControls.Run.canceled += OnRun;
    }

    void OnRun (InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();

    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkingVelocity;
        currentMovement.z = currentMovementInput.y * walkingVelocity;
        currentRunMovement.x = currentMovementInput.x * runningVelocity;
        currentRunMovement.z = currentMovementInput.y * runningVelocity;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }

     void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;


        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed){
            Quaternion targetposition = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetposition, rotationFPS * Time.deltaTime);
        }
        

    }

    void Update()
    {
        handleRotation();
    }
}
