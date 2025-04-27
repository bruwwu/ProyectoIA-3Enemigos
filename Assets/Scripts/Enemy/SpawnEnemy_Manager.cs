using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy_Manager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject juanitoSpawn_object;
    public GameObject[] spawnTargets; // Puntos de spawn
    public PlayerInput pitufin;

    [Header("Control de Rondas")]
    private int currentRound = 1;
    private int enemiesAlive = 0;
    private float difficultyWeight = 0.1f;

    void Awake()
    {
        pitufin = new PlayerInput();
        pitufin.pitufin.KSpawner.performed += OnKCast;
        pitufin.pitufin.CRestart.performed += OnCCast;
        pitufin.Enable();
    }

    void OnKCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // No usamos este directamente. Controlamos desde Update()
    }

    void OnCCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Restart();
    }

    public void Spawner()
    {
        if (juanitoSpawn_object == null)
        {
            Debug.LogError("juanitoSpawn_object no está asignado. No se puede spawnear.");
            return;
        }

        if (spawnTargets.Length == 0)
        {
            Debug.LogWarning("No hay puntos de spawn disponibles.");
            return;
        }

        foreach (GameObject targetSpawn in spawnTargets)
        {
            GameObject spawnedEnemy = Instantiate(juanitoSpawn_object, targetSpawn.transform.position, Quaternion.identity);
            spawnedEnemy.layer = LayerMask.NameToLayer("Enemy");

            LookAtYon lookAtYon = spawnedEnemy.GetComponent<LookAtYon>();
            if (lookAtYon != null)
            {
                lookAtYon.difficultyMode = (LookAtYon.Difficulty)Random.Range(0, 4);
                Debug.Log("Dificultad asignada: " + lookAtYon.difficultyMode);
                Debug.Log($"Spawneado enemigo con stats: " +
                            $"HP={lookAtYon.HP}, " +
                            $"BulletSpeed={lookAtYon.BalaVelocidad}, " +
                            $"RotationSpeed={lookAtYon.idleRotationSpeed}, " +
                            $"RotationAngle={lookAtYon.maxRotationAngle}, " +
                            $"ConeDistance={lookAtYon.coneDistance}");

            }
            else
            {
                Debug.LogError("El objeto instanciado no tiene el componente LookAtYon.");
            }

            enemiesAlive++;
        }

        Debug.Log("¡Ronda " + currentRound + " iniciada con " + enemiesAlive + " enemigos!");

    }

    public void EnemyDied()
    {
        enemiesAlive--;

        // Aquí ya NO subimos dificultad, solo mostramos que murió.
    }

    void SpawnNextWave()
    {
        LookAtYon.difficultyWeight += 0.1f;
        LookAtYon.difficultyWeight = Mathf.Clamp(LookAtYon.difficultyWeight, 0.1f, 0.5f);

        currentRound++;
        Debug.Log("Subiendo a ronda " + currentRound + " con dificultadWeight ahora en: " + LookAtYon.difficultyWeight);

        Spawner(); 
    }


    public void Restart()
    {
        SceneManager.LoadScene("EscenaFinal");
    }

    void Update()
    {
        if (pitufin.pitufin.KSpawner.triggered)
        {
            SpawnNextWave();
        }
    }
}
