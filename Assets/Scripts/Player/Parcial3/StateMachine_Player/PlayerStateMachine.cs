using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerInput playerInput;
    public CharacterController characterController;

    public Animator animator;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 currentDodgeMovement;
    Vector3 currentHitMovement;
    Vector3 _cameraRelativeMovement;
    bool Attackwait;
    bool _isMovementPressed;
    bool isRunPressed;
    bool _isDodgeTap;
    public bool isAttacking;
    public float rotationFPS = 1.0f;
    public float walkingVelocity;
    public float groundGravity = -0.05f;
    public float Gravity = -9.8f;
    public float dashSpeed = 1;
    float _speed = 0f;

    float timePassed;
    float clipLength;

    float clipSpeed;

    bool attack;

    bool isDashing;

    public bool dodgeBlock;

    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    public PlayerBaseState CurrentState{ get {return _currentState;} set {_currentState = value;}}
    public Animator Animator {get{return animator;}}
    public bool isMovementPressed { get {return _isMovementPressed;}}
    public bool isDodgeTap{get {return _isDodgeTap;} set{_isDodgeTap = value;}}
    public bool _AttackAction{get{return isAttacking;} set{isAttacking = value;}}

    public float speed {get {return _speed;} set {_speed = value;}}

    public float AppliedMovementX {get {return currentMovement.x;} set{currentMovement.x = value;}}
    public float AppliedMovementZ {get {return currentMovement.z;} set{currentMovement.z = value;}}
    public float currentDodgeMovementX {get {return currentDodgeMovement.x;} set{currentDodgeMovement.x = value;}}

    public float currentDodgeMovementZ {get {return currentDodgeMovement.z;} set{currentDodgeMovement.z = value;}}

    public float walkingMultiplier {get {return walkingVelocity;}}

    public Vector2 CurrentMovementInput {get {return currentMovementInput;}}

    public float GroundedGravity {get {return groundGravity;}}

    public float CurrentMovementY {get {return currentMovement.y;} set {currentMovement.y = value;}}
    public float CurrentDodgeMovementY {get {return currentDodgeMovement.y;} set {currentDodgeMovement.y = value;}}

    public float _TimePassed {get{return timePassed;} set{timePassed = value;}}
    public float _ClipLength {get {return clipLength;} set{clipLength = value;}}
    public float _ClipSpeed {get{return clipSpeed;} set{clipSpeed = value;}}
    public bool _Attack{get{return attack;} set{attack = value;}}

    public bool _DodgeBlock{get{return dodgeBlock;} set{dodgeBlock = value;}}

    public bool LockCameraPosition { get; set; }

    EnemyLockOn enemyLockOn;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();


        playerInput.CharacterControls.Move.started += onMovementInput;

        playerInput.CharacterControls.Move.canceled += onMovementInput;

        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.DodgeRoll.performed += OnDodge;

        playerInput.CharacterControls.DodgeRoll.canceled += OnDodge;

        playerInput.CharacterControls.Hit.started += onAttack;

        playerInput.CharacterControls.Hit.canceled += onAttack;
    }

    
    
    

    void OnDodge (InputAction.CallbackContext context)
    {
        _isDodgeTap = context.ReadValueAsButton();

    }

    void onAttack (InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton();

    }


    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        _isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _cameraRelativeMovement.z;


        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed){
            Quaternion targetposition = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetposition, rotationFPS * Time.deltaTime);
        }
        

    }
    IEnumerator Dash()
    {
        _isDodgeTap = false;
        dodgeBlock = true;
        characterController.Move(_cameraRelativeMovement * ( walkingVelocity * dashSpeed ) * Time.deltaTime);
        StartCoroutine(DashBoost());
        yield return new WaitForSeconds(1.5f);
        dodgeBlock = false;
    }

    IEnumerator DashBoost()
    {
        float restoreChilito = walkingVelocity;
        walkingVelocity = walkingVelocity + 1.8f;
        yield return new WaitForSeconds(1.0f);
        walkingVelocity = restoreChilito;
    }

    void Update()
    {
        if (_currentState == null) { 
            Debug.LogError("_currentState is not set!"); 
            return; 
            } 
            if (characterController == null) { 
                Debug.LogError("characterController is not set!"); 
                return;
            }

            _cameraRelativeMovement = ConverToCameraSpace(currentMovement);
        handleRotation();
        _currentState.UpdateStates();
        if(_isDodgeTap && !dodgeBlock)
        {
            StartCoroutine(Dash());
        }
        else if(isAttacking && !Attackwait)
        {
            attack = true;
            if(isMovementPressed && isAttacking)
            {
                attack = true;
                characterController.Move(_cameraRelativeMovement * walkingVelocity * Time.deltaTime);
            }
        }
        else if(_isMovementPressed)
        {
            characterController.Move(_cameraRelativeMovement * walkingVelocity * Time.deltaTime);
        }
    }

    Vector3 ConverToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
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
