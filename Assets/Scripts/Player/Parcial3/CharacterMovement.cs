using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public PlayerInput playerInput;
    public CharacterController characterController;

    public Animator animator;

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
    public float groundGravity = -0.05f;
    public float Gravity = -9.8f;
    public float dashSpeed = 1;
    bool isDashing;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


        playerInput.CharacterControls.Move.started += onMovementInput;

        playerInput.CharacterControls.Move.canceled += onMovementInput;

        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.DodgeRoll.performed += OnDodge;

        playerInput.CharacterControls.DodgeRoll.canceled += OnDodge;
    }

    void OnDodge (InputAction.CallbackContext context)
    {
        isDodgeTap = context.ReadValueAsButton();

    }



    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkingVelocity;
        currentMovement.z = currentMovementInput.y * walkingVelocity;

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

    void handleAnimation()
    {
        float speed = 0f; // Inicializa la velocidad en 0

        // Verifica si hay movimiento
        if (isMovementPressed)
        {
            speed = 0.5f;
            animator.SetTrigger("move"); // 1 para correr, 0.5 para caminar
        }

        // Establece el valor de speed en el animator
        
        // Maneja el dodge
        if (isDodgeTap && isMovementPressed)
        {   
            animator.SetTrigger("Dodge"); 
            speed = 1.0f;
            // Usa un trigger para el dodge
            //StartCoroutine(Dash());
        }
        
        animator.SetFloat("speed", speed);
    }

    bool AnimatorInState(string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
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
        handleAnimation();
        handleRotation();
        if(isDodgeTap)
        {
            StartCoroutine(Dash());
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        

    }

    IEnumerator Dash()
    {
        isDodgeTap = false;
        characterController.Move(currentDodgeMovement * dashSpeed * Time.deltaTime);
        yield return new WaitForSeconds(2f);
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
