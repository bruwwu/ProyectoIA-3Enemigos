using System.Collections;
using System.Collections.Generic;
using DebugManager;
using UnityEngine;
using Utilities;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random; //QuickFix de VScode para poder usar Random en el respawn jiji

public class NavMesh : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform movePosition; //Posicion del Miko/Target
    
    [SerializeField] private Transform agentTransform; //Posicion del agente del NavMesh/Zdraada

    [Header("GameObjects")]
    public GameObject moveTarget; //GameObject de Miko, para poder desactivarlo y activarlo libremente dentro del codigo 
    private bool isDetected = false; //Está aqui solo para existir y hacer funcionar el cono xd
    private NavMeshAgent navMeshAgent; //El navmesh 

    public Transform[] respawnPoints; //Array de puntos de respawn :D


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();   //Inicializamos el NavMeshAgent

    }

    // Update is called once per frame
    void Update()
    {
        
        navMeshAgent.destination = movePosition.position; //Hace que el destino del Zdraada/Agente sea igual a la de Miko/Target

        respawnBol(); //Llamamos a la funcion para respawnear la bolita/Miko
    }

    void OnDrawGizmos()
    {
        if(DebugGizmoManager.VisionCone){
            Utility.DrawVisionCone(agentTransform.position, 45.0f, 10.0f, isDetected, agentTransform); //Dibuja el cono en la posicion del Zdraada
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Player")){ 
            
            moveTarget.SetActive(false); //Desactiva a Miko

            movePosition.position = respawnPoints[Random.Range(0, respawnPoints.Length)].position; //Asigna un respawn random dentro del array (y)
            
        }
    }

    void respawnBol()
    {
        if(Input.GetKeyDown(KeyCode.Space)) //Le damos al space para volverlo a aparecer
        {
            moveTarget.SetActive(true); //Activa a Miko
        }
    }
}
