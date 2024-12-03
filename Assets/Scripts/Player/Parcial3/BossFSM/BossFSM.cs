using System;
using System.Collections;
using DebugManager;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class BossFSM : MonoBehaviour
{
    [Header("VisionCone - Detección del Jugador")]
    public Transform player;
    public Rigidbody rb;
    public float detectedSphereRadious;
    public float jumpSphereRadious;

    [Header("Movimiento y Física")]
    public float speed;
    public float jumpForce;
    public float jumpCooldown;
    public bool isJumping = true;
    public NavMeshAgent navMeshAgent;
    public Animator naomiAni;

    /*
    [Header("Ataque Especial")]
    public GameObject meteorPrefab;
    public Transform meteorSpawnPoint;
    public float meteorCooldown = 10f;
    [SerializeField] private bool canCastMeteor = true;
    */

    [Header("Wipe")]
    public float wipeRadius;
    public bool wipeActivated = false;
    public int wipeDamage = 9999; // Daño del Wipe
    public float wipeTemporizer;
    public BossDmgValues bossDmgValues;

    // State Machine
    public BossBaseState currentState;
    public BossStateFactory stateFactory;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        naomiAni = GetComponent<Animator>();
        stateFactory = new BossStateFactory(this);

        // Inicializa en el estado Idle
        SwitchState(stateFactory.Idle());
        StartCoroutine(IniciarTempo());
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void SwitchState(BossBaseState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        if (currentState != null)
        {
            currentState.EnterState();
        }
    }

    public bool IsPlayerInRange(float range)
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= range;
    }

    public void DoJump()
    {
        if (!isJumping) return;

        Debug.Log("Realizando salto hacia el jugador.");

        Vector3 direction = (player.position - transform.position).normalized;

        rb.velocity = Vector3.zero;

        // Aplicar fuerza de salto hacia el jugador
        rb.AddForce(direction * jumpForce, ForceMode.Impulse);

        // Iniciar cooldown del salto
        StartCoroutine(JumpCooldown());
    }

    IEnumerator JumpCooldown()
    {
        // Deshabilitar temporalmente el salto
        isJumping = false;

        // Esperar la duración del cooldown
        yield return new WaitForSeconds(jumpCooldown);

        // Rehabilitar el salto
        isJumping = true;

        Debug.Log("Salto listo nuevamente.");
    }

    IEnumerator IniciarTempo()
    {
        yield return new WaitForSeconds(wipeTemporizer);
         // Cambiar al estado de Wipe
        wipeActivated = true;
        Debug.Log("Wipe activado después de 5 minutos.");
        SwitchState(stateFactory.Wipe());
    }

    void OnDrawGizmos()
    {
        if (DebugGizmoManager.VisionCone)
        {
            // Dibujar la esfera de salto
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, jumpSphereRadious);

            // Dibujar la esfera de detección
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectedSphereRadious);

            //Esfera para el wipe 
             // Dibujar la esfera de salto
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, wipeRadius);
        }
    }
}
