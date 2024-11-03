using System.Collections;
using System.Collections.Generic;
using DebugManager;
using UnityEngine;
using Utilities;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random; //QuickFix de VScode para poder usar Random en el respawn jiji

public class NavMeshCone : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform movePosition; //Posicion del Miko/Target
    
    [SerializeField] private Transform agentTransform; //Posicion del agente del NavMesh/Zdraada

    [Header("GameObjects")]
    public GameObject moveTarget; //GameObject de Miko, para poder desactivarlo y activarlo libremente dentro del codigo 
    private NavMeshAgent navMeshAgent; //El navmesh 

    public Transform[] respawnPoints; //Array de puntos de respawn :D

    public Transform navRespawn;

    public float coneAngle;
    public float coneDistance;

    private bool isChasing = false;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();   //Inicializamos el NavMeshAgent
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 navDirection = agentTransform.forward; //Asignamos nuestra mirada al frente de nuestro agente
        if (Utilities.Utility.inInsideCone(coneAngle, coneDistance, movePosition.position, agentTransform.position, navDirection)) //Verificar si está dentro del metodo VisionCone
        {
            if (!isChasing)
            {
                isChasing = true;
                navMeshAgent.destination = movePosition.position; //Si esta persiguiendo al personaje, le damos su objetivo
                StartCoroutine(navMesh_Chase());
            }
        }
        else{
            if(isChasing)
            {
                isChasing = false;
                StartCoroutine(navMesh_Chase());
                
            }
        }

        if(!moveTarget.activeInHierarchy)
        {
            respawnBol();//Verificamos si nuestro objeto esta en pantalla para poder reaparecerlo
        }
        
        
    }

    void OnDrawGizmos()
    {
        if(DebugGizmoManager.VisionCone){
            Utility.DrawVisionCone(agentTransform.position, coneAngle, coneDistance, isChasing, agentTransform); //Dibuja el cono en la posicion del Zdraada
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Player"))
        { 
            moveTarget.SetActive(false); //Desactiva a Miko
            movePosition.position = respawnPoints[Random.Range(0, respawnPoints.Length)].position; //Asigna un respawn random dentro del array (y)
        }
    }

    IEnumerator navMesh_Chase()
    {
        yield return new WaitForSeconds(3f);
        navMeshAgent.destination = agentTransform.position; //despues de tres segundos, el personaje se quedará en su lugar
        yield return new WaitForSeconds(3f);
        navMeshAgent.destination = navRespawn.position;//aqui el personaje ira a su respawn
        yield return new WaitForSeconds(6f);
    }

    void respawnBol()
    {
        if(Input.GetKeyDown(KeyCode.Space)) //Le damos al space para volverlo a aparecer
        {
            moveTarget.SetActive(true); //Activa a Miko
        }
    }
}
