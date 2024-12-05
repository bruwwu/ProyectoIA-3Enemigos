using System;
using System.Collections;
using DebugManager;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using UnityEngine.SceneManagement;

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
    public bool isJumping;
    public NavMeshAgent navMeshAgent;
    public Animator naomiAni;

    [Header("Melee")]
    public float areaDmg;
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
    private bool block;
    
    public GameObject cameraSync;

    public GameObject Vfx;

    [Header("Disparo")]
    public GameObject Mira;
    public float shootRadius;
    public GameObject balaCloud;
    public bool isLockingIn = false; 
    public float balaVelocity;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        naomiAni = GetComponent<Animator>();
        stateFactory = new BossStateFactory(this);
        isJumping = false;
        cameraSync.SetActive(false);
        Vfx.SetActive(false);
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

    public bool IsPlayerInShootRange(float shootRadius)
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= shootRadius;
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
        detectedSphereRadious /= 10;
        rb.velocity = Vector3.zero;
        // Calcular dirección y aplicar fuerza de salto
        navMeshAgent.SetDestination(player.position);
        Vector3 direction = (player.position - transform.position).normalized;
        rb.AddForce(direction * jumpForce, ForceMode.Impulse);
        

        // Deshabilitar salto hasta que termine el cooldown
        StartCoroutine(JumpCooldown());
    }

    IEnumerator JumpCooldown()
    {
        // Restaurar el rango de detección original
        
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(jumpCooldown);
        detectedSphereRadious *= 10;
        isJumping = false;
    }

    IEnumerator IniciarTempo()
    {
        yield return new WaitForSeconds(wipeTemporizer);
         // Cambiar al estado de Wipe
        wipeActivated = true;
        Debug.Log("Wipe activado después de 5 minutos.");
        SwitchState(stateFactory.Wipe());
    }

    public IEnumerator lockIn() // Corrutina para fijar al jugador
    {
        while (isLockingIn && IsPlayerInRange(shootRadius))
        {
            Vector3 relativePos = player.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);

            // Llama a yonBombing opcionalmente si es necesario
            yonBombing();

            yield return null; // Continuar en el siguiente frame
        }

        yield return new WaitForSeconds(2f);
    }


    public void yonBombing()
    {
        if(!block){
        GameObject Balatemporal = Instantiate(Mira, balaCloud.transform.position, balaCloud.transform.rotation);
        Rigidbody rb = Balatemporal.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * balaVelocity, ForceMode.Impulse);
        Destroy(Balatemporal, 4f);
        StartCoroutine(WaitFor());
        
        }
    }

    IEnumerator WaitFor()
    {
        block = true;
        yield return new WaitForSeconds(0.5f);
        block = false;
    }

    void OnDrawGizmos()
    {
        if (DebugGizmoManager.VisionCone)
        {
            //deteccion dispro
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, shootRadius);
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
