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

    public GameObject targetSpawn;

    void Start()
    {
        juanitoRenderer = GetComponent<Renderer>();
    }

    public void Spawner()
    {
        Debug.Log("AHHH ME DISTE CLICK");
        difficulty();
        Instantiate(juanitoSpawn_object, targetSpawn.transform.position, Quaternion.identity);
    }


    void difficulty()
    {
        dificultad = (juanitoTorreta.idleRotationSpeed * Random.Range(0.0f, 1.0f)) + (juanitoTorreta.maxRotationAngle * Random.Range(0.0f, 1.0f)) + (juanitoTorreta.BalaVelocidad * Random.Range(0.0f, 1.0f));
        enemyColor(dificultad);
    }

    void enemyColor(float _dificultad)
    {
        if (dificultad >= 465) 
        {
            juanitoRenderer.material.color = Color.red; // Dificultad alta
        } 
        else if (dificultad >= 310) 
        {
            juanitoRenderer.material.color = Color.yellow; // Dificultad media
        } 
        else 
        {
            juanitoRenderer.material.color = Color.green; // Dificultad baja
        }

    }

}
