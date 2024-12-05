using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LockOnSystem : MonoBehaviour
{
    [Header("Settings")]
    public string enemyTag = "Enemy";
    public float maxDistance = 15f;
    public CinemachineFreeLook cinemachineFreeLook;
    public Transform playerTransform;

    private Transform currentTarget;
    private bool isLockedOn = false;
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.CharacterControls.LockOn.performed += OnLockOn;

        if (cinemachineFreeLook == null)
        {
            cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        }

        if (playerTransform == null)
        {
            playerTransform = transform;
        }
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isLockedOn)
            {
                // Desbloquear y restaurar el look at de la cámara al personaje
                isLockedOn = false;
                currentTarget = null;
                cinemachineFreeLook.m_LookAt = playerTransform;
            }
            else
            {
                // Buscar el enemigo más cercano y bloquear
                currentTarget = FindClosestEnemy();
                if (currentTarget != null)
                {
                    isLockedOn = true;
                    cinemachineFreeLook.m_LookAt = currentTarget;
                }
            }
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closestEnemy = null;
        float shortestDistance = maxDistance;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                closestEnemy = enemy.transform;
                shortestDistance = distance;
            }
        }

        return closestEnemy;
    }
}
