using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy_Manager : MonoBehaviour
{
    [Header("Dificultad")]
    private LookAtYon lookAtYon;
    public GameObject juanitoSpawn_object;
    public Renderer juanitoRenderer;

    public PlayerInput pitufin; 
    public GameObject[] spawnTargets; //Array de los puntos de spawn

    void Awake()
    {
        pitufin = new PlayerInput(); // Instanciar aquí
        pitufin.pitufin.KSpawner.performed += OnKCast;
        pitufin.pitufin.CRestart.performed += OnCCast;
        pitufin.Enable(); // Habilitar las acciones
    }
    void OnKCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Spawner();
    }

    void OnCCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Restart();
    }

    void Start()
    {

    }

    public void Spawner()
    { 
        /*Muy bien, aqui al dar la K los enemigos harán spawn en alguno de los puntos asignados en el inspector
        Se intancia el enemigo en la posicion del targetspawn con la rotacion base.
        Se obtiene el componente (script) de juanito torreta
        despues se verifica que no esté nulo para de ahi poder asignarle una de las 4 dificultades que tenemos 
        tambien se imprime en consola cual se asignó :p*/
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
        // Si se presiona la tecla K, se invoca el método Spawner sencillito 
        if(pitufin.pitufin.KSpawner.triggered)
        {
            Spawner();   
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("EscenaFinal");
    }


}
