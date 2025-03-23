using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy_Manager : MonoBehaviour
{
    [Header("Dificultad")]
    float dificultad;
    private LookAtYon juanitoTorreta;
    public GameObject juanitoSpawn_object;
    public Renderer juanitoRenderer;

    public PlayerInput pitufin; 
    public GameObject[] spawnTargets;

    void Awake()
    {
        pitufin = new PlayerInput(); // Instanciar aquí
        pitufin.pitufin.KSpawner.performed += OnKCast;
        pitufin.Enable(); // Habilitar las acciones
    }
    void OnKCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Spawner();
    }

    void Start()
    {
        if(juanitoTorreta == null)
        {
            Debug.LogError("LookAtYon component not found in the GameObject");
        }
    }

    public void Spawner()
    { 
        Debug.Log("penesotes");
        GameObject targetSpawn = spawnTargets[Random.Range(0, spawnTargets.Length)];
        Instantiate(juanitoSpawn_object, targetSpawn.transform.position, Quaternion.identity);
    }

    void Update()
    {
        if(pitufin.pitufin.KSpawner.triggered)
        {
            Spawner();   
        }
    }


}
