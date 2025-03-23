using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy_Manager : MonoBehaviour
{
    [Header("Dificultad")]
    float dificultad;
    private LookAtYon lookAtYon;
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

    }

    public void Spawner()
    { 
        Debug.Log("penes gordos y jugosos");
        GameObject targetSpawn = spawnTargets[Random.Range(0, spawnTargets.Length)];
        Instantiate(juanitoSpawn_object, targetSpawn.transform.position, Quaternion.identity);

        LookAtYon lookAtYon = juanitoSpawn_object.GetComponent<LookAtYon>();
        if(lookAtYon != null)
        {
            // Random.Range(0,3) devolverá 0, 1 o 2, mapeando así a las tres opciones del enum
            lookAtYon.difficultyMode = (LookAtYon.Difficulty)Random.Range(0, 4);
            Debug.Log("Dificultad: " + lookAtYon.difficultyMode);
        }
    }

    void Update()
    {
        if(pitufin.pitufin.KSpawner.triggered)
        {
            Spawner();   
        }
    }


}
