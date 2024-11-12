using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using DebugManager;
using Utilities;
using Unity.AI.Navigation;

public class NavMeshEscapistEnemy : MonoBehaviour
{

[Header("VisionCone - Detección del Jugador")]
public Transform Enemy;
public Transform player; 
private bool isDetected = false; 
[SerializeField] HB_Slider hB_Slider;
public GameObject playerGameObject; 
public float sphereRadious; 
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
public NavMeshSurface ARTURO;

    // Variable para almacenar la magnitud de la velocidad
    private int velocityMagnitude;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();  
    }

    void Update()
    {
        // Detectar si el jugador está dentro del radio
        if (Utilities.Utility.IsInsideRadius(playerGameObject.transform.position, transform.position, sphereRadious))
        {
            isDetected = true;

            // Calcula la dirección hacia el objetivo (Seek)
            Vector3 PosToTarget = -PuntaMenosCola(playerGameObject.transform.position, transform.position);
            Vector3 dirToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            // Establecer la nueva posición de destino
            navMeshAgent.SetDestination(newPos);

            // Recalcular el NavMesh dinámicamente si es necesario
            if (ARTURO != null)
            {
                ARTURO.BuildNavMesh(); // Reconstruye la malla de navegación
            }
        }
        else
        {
            isDetected = false;
            StartCoroutine(lockIn());
        }
    }

     void OnDrawGizmos()
    {
        if(DebugGizmoManager.VisionCone){
            Utility.DrawVisionCone(agentTransform.position, coneAngle, coneDistance, isChasing, agentTransform); //Dibuja el cono en la posicion del Zdraada
        }
    }


    // Método para calcular la dirección de un punto a otro
    public Vector3 PuntaMenosCola(Vector3 Punta, Vector3 Cola)
    {
        return new Vector3(Punta.x - Cola.x, Punta.y - Cola.y, Punta.z - Cola.z);
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

    IEnumerator lockIn()
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

    void yonBombing()
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
