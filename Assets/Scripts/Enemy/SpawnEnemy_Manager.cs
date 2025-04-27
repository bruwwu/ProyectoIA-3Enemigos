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

        LookAtYon[] activeEnemies = GameObject.FindObjectsOfType<LookAtYon>();
        foreach (var enemy in activeEnemies)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Juan Referencia"))
            {
                // Si el enemigo es el de referencia, lo ignoramos
                continue;
            }

            Debug.Log("Ya hay enemigos en la escena. No se puede spawnear más.");
            return;
        }


        // Lista de puntos de spawn disponibles
        List<GameObject> availableSpawnPoints = new List<GameObject>(spawnTargets);

        // Verificar si hay puntos de spawn disponibles
        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No hay puntos de spawn disponibles.");
            return;
        }

        // Iterar sobre los puntos de spawn y generar un enemigo en cada uno
        foreach (GameObject targetSpawn in availableSpawnPoints)
        {
            GameObject spawnedEnemy = Instantiate(juanitoSpawn_object, targetSpawn.transform.position, Quaternion.identity);

            // Cambiar la layer del enemigo instanciado
            spawnedEnemy.layer = LayerMask.NameToLayer("Enemy");

            // Obtener el componente LookAtYon del enemigo instanciado
            LookAtYon lookAtYon = spawnedEnemy.GetComponent<LookAtYon>();
            if (lookAtYon != null)
            {
                // Asignar una dificultad aleatoria
                lookAtYon.difficultyMode = (LookAtYon.Difficulty)Random.Range(0, 4);
                Debug.Log("Dificultad asignada: " + lookAtYon.difficultyMode);
            }
            else
            {
                Debug.LogError("El objeto instanciado no tiene el componente LookAtYon.");
            }
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
