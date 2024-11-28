using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using DebugManager;
using Utilities;
using Unity.AI.Navigation;
using UnityEditor;

public class NavMeshBoss : MonoBehaviour
{
    [Header("VisionCone - Detección del Jugador")]
    public Transform Enemy;
    public Transform player;

    private bool isDetected = false; 
    [SerializeField] HB_Slider hB_Slider;
    public GameObject playerGameObject; 
    public float detectedSphereRadious;
    public float jumpSphereRadious; 
    public float coneAngle;
    public float coneDistance;

    [Header("Movement - Movimiento y Física")]
    public Vector3 Velocity = Vector3.zero; 
    public float maxSpeed = 5f; 
    public float maxAcceleration = 5f; 

    [Header("Shooting - Sistema de Disparo")]
    public GameObject Mira; 
    public GameObject BalaPrefabInicio; 
    public float BalaVelocidad; 

    [Header("Behavior - Estado y Comportamiento")]
    private bool block; 
    private bool isChasing; 

    [Header("Components - Componentes")]
    private NavMeshAgent navMeshAgent; // Agente de navegación para el movimiento
    public Transform agentTransform; 
    private Rigidbody rb;
    public Animator naomiAni; 
    public float speed;
    // Variable para almacenar la magnitud de la velocidad
    private int velocityMagnitude;

    [Header("Salto del jefe")]
    public float jumpForce;
    public float jumpCooldown;
    public bool isJumping = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        naomiAni = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();  
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerGameObject.transform.position, transform.position);

        // Detectar si el jugador está dentro del rango de detección
        if (distanceToPlayer <= detectedSphereRadious)
        {
            isDetected = true;
            
            
            // Mover hacia el jugador si está dentro del rango de detección
            navMeshAgent.SetDestination(player.position);

            // Activar animación de caminar
            if (naomiAni != null)
            {
                naomiAni.SetTrigger("Walking");
                speed = 1;
                naomiAni.SetFloat("Speed", speed);
            }
        }
        // Detectar si el jugador está fuera de rango de detección pero dentro del rango de salto
        else if (distanceToPlayer > detectedSphereRadious && distanceToPlayer <= jumpSphereRadious)
        {
            // Realizar un salto hacia el jugador si está en rango de salto y no está en cooldown
            if (isJumping)
            {
        
                Debug.Log("Jugador en rango de salto. Ejecutando DoJump()");
                DoJump();
            }
        }
        else
        {
            // Fuera de ambas esferas: estado de búsqueda
            isDetected = false;
            StartCoroutine(lockIn());

            // Detener animación de caminar
            if (naomiAni != null)
            {
                naomiAni.SetTrigger("Walking");
                speed = 0;
                naomiAni.SetFloat("Speed", speed);
            }
        }
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

            // Dibujar el cono de visión
            Utility.DrawVisionCone(agentTransform.position, coneAngle, coneDistance, isChasing, agentTransform);
        }
    }

    void DoJump()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        rb.velocity = Vector3.zero;
        // Aplicar fuerza de salto hacia el jugador
        rb.AddForce(direction * jumpForce, ForceMode.Impulse);
        navMeshAgent.SetDestination(player.position);

        // Desactivar la capacidad de saltar temporalmente
        isJumping = false;
        StartCoroutine(JumpCooldown());

        // Activar animación de salto
        if (naomiAni != null)
        {
            naomiAni.SetTrigger("Jumping");
            speed = 2; // Velocidad aumentada temporalmente para el salto
            naomiAni.SetFloat("Speed", speed);
        }
    }

    IEnumerator JumpCooldown()
    {
        // Aumentar el tamaño de la esfera de detección durante el cooldown
        float originalDetectedSphereRadious = detectedSphereRadious;
        detectedSphereRadious = jumpSphereRadious;

        yield return new WaitForSeconds(jumpCooldown);

        // Restaurar el tamaño original de la esfera de detección
        detectedSphereRadious = originalDetectedSphereRadious;
        rb.velocity = Vector3.zero;
        isJumping = true;
    }

    public void OnTriggerEnter(Collider collision)
    {
        // Si colisiona con el jugador, aplicar daño basado en la velocidad
        if (collision.gameObject.tag == "Player")
        {
            // Asegúrate de que GameManager y playerHealth están correctamente configurados
            GameManager.gameManager.playerHealth.VelDmgUnit(2, velocityMagnitude);
            hB_Slider.SetHealth(GameManager.gameManager.playerHealth.Health);
            Debug.Log(GameManager.gameManager.playerHealth.Health);
        }
    }

    IEnumerator lockIn() //Corrutina para fijar al player
    {
        while (!isDetected)  // Mientras el jugador esté dentro del rango de visión
        {
            // Hacer que juanitoTorreta mire al jugador
            Vector3 relativePos = player.position - Enemy.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            Enemy.rotation = Quaternion.Slerp(Enemy.rotation, toRotation, Time.deltaTime * 5f);
            yonBombing();
            yield return null; // Continuar cada frame
        }
        yield return new WaitForSeconds(2f);
    }

    void yonBombing() //Disparo
    {
        if(!block){
            GameObject Balatemporal = Instantiate(Mira, BalaPrefabInicio.transform.position, BalaPrefabInicio.transform.rotation) as GameObject;
            Rigidbody rb = Balatemporal.GetComponent<Rigidbody>();
            Vector3 relativePos = PredictPos(Enemy.position, player.position, 1f);
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            rb.AddForce(transform.forward * BalaVelocidad);
            Destroy(Balatemporal, 1f);
            StartCoroutine(WaitFor());
        }
    }

    Vector3 PredictPos(Vector3 InitiaPos, Vector3 Velocity, float TimePrediction)
    {
        return InitiaPos + Velocity * TimePrediction; // Retorna la posición futura
    }
    IEnumerator WaitFor()
    {
        block = true;
        yield return new WaitForSeconds(5f);
        block = false;
    }
}
