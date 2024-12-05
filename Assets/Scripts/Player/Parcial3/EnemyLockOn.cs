using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyLockOn : MonoBehaviour
{
    public Transform currentTarget;
    Animator anim;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTargetLocator;

    [Tooltip("StateDrivenMethod for switching cameras")]
    [SerializeField] Animator cinemachineAnimator;

    [Header("Settings")]
    [SerializeField] float noticeZone = 10f;
    [SerializeField] float maxNoticeAngle = 60f;
    [SerializeField] PlayerStateMachine playerStateMachine;
    [SerializeField] float rotationSmoothSpeed = 2f;

    bool isLockedOn;
    public Transform cam;
    PlayerInput playerInput;
    float currentYOffset;
    Vector3 pos;
    

    void Awake()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
        playerInput.CharacterControls.LockOn.performed += OnLockOn;
        playerInput.CharacterControls.LockOn.canceled += OnLockOn;
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
        playerInput.CharacterControls.LockOn.performed -= OnLockOn;
        playerInput.CharacterControls.LockOn.canceled -= OnLockOn;
    }

    private void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentTarget)
            {
                ResetTarget();
                return;
            }

            if (currentTarget = ScanForTargets()) FoundTarget(); else ResetTarget();
        }
    }

    private void Update()
    {
        if (isLockedOn)
        {
            if (!TargetInRange()) ResetTarget();
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget();
            return;
        }

        pos = currentTarget.position + new Vector3(0, currentYOffset, 0);
        enemyTargetLocator.position = pos;
        var dir = currentTarget.position - transform.position;
        dir.y = 0;
        var rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmoothSpeed * Time.deltaTime);
    }

    private bool TargetInRange()
    {
        float dis = (transform.position - pos).magnitude;
        return dis / 2 <= noticeZone;
    }

    private void FoundTarget()
    {
        anim.SetLayerWeight(1, 1);
        cinemachineAnimator.Play("Target Camera");
        isLockedOn = true;
        playerStateMachine.LockCameraPosition = true;
    }

    private void ResetTarget()
    {
        isLockedOn = false;
        anim.SetLayerWeight(1, 0);
        cinemachineAnimator.Play("Follow Camera");
        currentTarget = null;
        playerStateMachine.LockCameraPosition = false;
    }

    Transform ScanForTargets()
    {
        var hits = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        var closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        foreach (var hit in hits)
        {
            var dir = hit.transform.position - cam.position;
            dir.y = 0;

            var angle = Vector3.Angle(cam.forward, dir);

            if (angle < closestAngle)
            {
                closestTarget = hit.transform;
                closestAngle = angle;
            }
        }

        if (!closestTarget) return null;

        var collider = closestTarget.GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            var h = collider.height * closestTarget.localScale.y;
            var half_h = (h / 2) / 2;
            currentYOffset = h - half_h;
        }

        return closestTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
