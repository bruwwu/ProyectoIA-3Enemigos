using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoAnimCharacterMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;

    Animator animator;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 currentDodgeMovement;
    Vector3 currentHitMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isDodgeTap;
    public float rotationFPS = 1.0f;
    public float walkingVelocity;
    public float runningVelocity;
    public float groundGravity = -0.05f;
    public float Gravity = -9.8f;
    public float dashSpeed = 1;
    
    // Start is called before the first frame update
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

        playerInput.CharacterControls.DodgeRoll.started += OnDodge;

        playerInput.CharacterControls.DodgeRoll.canceled += OnDodge;
    }

    void OnDodge (InputAction.CallbackContext context)
    {
        isDodgeTap = context.ReadValueAsButton();

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

        currentDodgeMovement.x = currentMovementInput.x;
        currentDodgeMovement.z = currentMovementInput.y;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

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
    
    void handleGravity()
    {
        if(characterController.isGrounded)
        {
            currentMovement.y = groundGravity * Time.deltaTime;
            currentRunMovement.y = groundGravity * Time.deltaTime;
            currentDodgeMovement.y = groundGravity * Time.deltaTime;
        }
        else
        {
            currentMovement.y += Gravity * Time.deltaTime;
            currentRunMovement.y += Gravity * Time.deltaTime;
            currentDodgeMovement.y += Gravity * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleGravity();
        handleRotation();
        if(isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
            /*if(isDodgeTap)
            {
                
            }*/
            if(isDodgeTap && isRunPressed)
            {
                StartCoroutine(Dash());
            }
        }
        else if(isDodgeTap && isMovementPressed)
        {
            
            StartCoroutine(Dash());
            //characterController.Move(currentDodgeMovement * dashSpeed * Time.deltaTime);
            
        }
    
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }

    }

    IEnumerator Dash()
    {
         //bool isButtonPressed = playerInput.CharacterControls.Run.ReadValue<float>() > 0;

        playerInput.CharacterControls.Run.Disable();
        characterController.Move(currentDodgeMovement * dashSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        playerInput.CharacterControls.DodgeRoll.Disable();
        yield return new WaitForSeconds(0.5f);
        playerInput.CharacterControls.Run.Enable();
        /*if(isButtonPressed)
        {
            isRunPressed = true;
        }*/

        yield return new WaitForSeconds(2f);
        playerInput.CharacterControls.DodgeRoll.Enable();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
