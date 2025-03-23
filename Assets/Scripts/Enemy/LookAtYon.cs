using System.Collections;
using UnityEngine;
using Utilities;
using DebugManager;

public class LookAtYon : MonoBehaviour
{
    [Header("VisionCone")]
    public Transform juanitoTorreta;
    public Transform player;
    public float coneAngle;
    public float coneDistance;
    private bool isDetected = false;
    private bool isRotating = false;  // Para controlar la activación de la rotación

    [Header("Corrutina")]
    public float idleRotationSpeed = 30f;  // Velocidad de la rotación idle
    public float maxRotationAngle = 90f;   // Ángulo máximo de rotación
    private float currentRotationAngle = 0f;
    private int rotationDirection = 1; 
    [Header("Shooting")]
    public GameObject Mira;
    public GameObject BalaPrefabInicio;
    public float BalaVelocidad;

    private bool block;
    // 1 = rotación positiva, -1 = rotación negativa

    [Header("IA Diff")]
    public Renderer enemyRenderer;
    [SerializeField] private float dificultad;
    public Difficulty difficultyMode;
    public float HP;
    public enum Difficulty
    {
        //Le chaqueteamos unos enums para poder seleccionar la dificultad de los enemigos
        modoDiablo, //Dificultad alta
        HPModifier, //Modificador de HP
        darkwarrior777, //Dificultad media no tan cabrona
        elRotador //Le aumenta el rango y velocidad de bala al enemigo, al chile está bien perro este
    }
    
    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        if(enemyRenderer == null)
        {
            Debug.LogError("No se encontró Renderer en el enemigo");
        }
        
        // Se guardan los valores base de los parámetros, ya que los necestiamos para modificarlos de manera local en cada difficultyMode
        float baseIdleRotationSpeed = idleRotationSpeed;
        float baseMaxRotationAngle = maxRotationAngle;
        float baseBalaVelocidad = BalaVelocidad;
        float baseConeDistance = coneDistance;
        float baseHP = GameManager.gameManager.juanitoTorreta.Health;
        
        // Cálculo de dificultad y modificaciones específicas para cada modo
        switch(difficultyMode) //Hola un switch para seleccionar la dificultad
        {
            case Difficulty.modoDiablo: 
            /* 
            Se modifican todos los valores (menos coneDistance) y el escalado de la dificultad es el mas perro aquí, 
            este enemigo es el que tiene los mayores escalados y es raro ver uno de color amarillo*/
                dificultad = GameManager.gameManager.juanitoTorreta.Health * Random.Range(0.8f, 2.0f) +
                             baseIdleRotationSpeed * Random.Range(0.5f, 1.0f) +
                             baseMaxRotationAngle * Random.Range(0.5f, 1.0f) +
                             baseBalaVelocidad * Random.Range(0.5f, 1.0f); 
                // Solo modificamos los parámetros de rotación y disparo
                idleRotationSpeed = baseIdleRotationSpeed * (1 + dificultad / 800f);
                maxRotationAngle = baseMaxRotationAngle * (1 + dificultad / 800f);
                BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 800f);
                HP = baseHP + (1 + dificultad / 800f);

                //No modificado
                coneDistance = baseConeDistance;
              
                break;
                
            case Difficulty.HPModifier:

            /* 
            Este es puro escalado de vida, algo sencillito
            */
                dificultad = (GameManager.gameManager.juanitoTorreta.Health * Random.Range(0.0f, 1.0f) + 
                              baseBalaVelocidad * Random.Range(0.0f, 1.0f)) / 2;
                // Se aplica la modificación principalmente a la HP
                HP = baseHP * (1 + dificultad / 400f);
                BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 300f);

                //No modificado
                idleRotationSpeed = baseIdleRotationSpeed;
                maxRotationAngle = baseMaxRotationAngle;
                coneDistance = baseConeDistance;
                break;
                
            case Difficulty.darkwarrior777:
            /* 
                Este es un enemigo con dificultad media, no tan cabrona, no conteine escalado de vida
            */
                dificultad = (baseIdleRotationSpeed * Random.Range(0.0f, 1.0f) +
                              baseMaxRotationAngle * Random.Range(0.0f, 1.0f) +
                              baseBalaVelocidad * Random.Range(0.0f, 1.0f) * 2);
                // Se aplican modificaciones a casi todos los parámetros, pero con distintos factores
                idleRotationSpeed = baseIdleRotationSpeed * (1 + dificultad / 600f);
                maxRotationAngle = baseMaxRotationAngle * (1 + dificultad / 600f);
                BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 600f);
                //No modificado
                coneDistance = baseConeDistance;
                HP = baseHP;
                break;
                
            case Difficulty.elRotador:
            /* 
            Borren a este wey porfas, tiene un rango de disparo muy grande, rotación rápida y balas muy veloces
            */
                dificultad = baseIdleRotationSpeed * Random.Range(2.0f, 5.0f) * 5f + 
                             coneDistance * Random.Range(0.5f, 1.0f) * 2f + 
                             BalaVelocidad * Random.Range(0.5f, 1.0f) * 2f;
                //ola yonesi y niggawarrior
                idleRotationSpeed = baseIdleRotationSpeed * (1 + dificultad / 800f);
                BalaVelocidad = baseBalaVelocidad * (1 + dificultad / 200);
                coneDistance = baseConeDistance * (1 + dificultad / 200);
                //No modificado
                maxRotationAngle = baseMaxRotationAngle;
                HP = baseHP;
                break;
                
            default:
                Debug.LogError("Modo de dificultad no reconocido");
                break;
        }
        
        // Debug: Mostrar los valores modificados
        Debug.Log("Valores alterados: " +
                  "idleRotationSpeed: " + idleRotationSpeed +
                  ", maxRotationAngle: " + maxRotationAngle + 
                  ", BalaVelocidad: " + BalaVelocidad +
                  ", HP: " + HP +
                  ", coneDistance: " + coneDistance);
        
        SetEnemyColor(dificultad); //Color en base a la dificultad calculada en cada una de las
        StartCoroutine(rotateIdle());
    }

    void Update()
    {
        Vector3 juanitoDirection = juanitoTorreta.forward;

        // Verificar si el jugador está dentro del cono de visión
        if (Utilities.Utility.inInsideCone(coneAngle, coneDistance, player.position, juanitoTorreta.position, juanitoDirection))
        {
            if (!isDetected)
            {
                isDetected = true;
                 // Parar la rotación si se detecta al jugador
                StartCoroutine(lockIn());      // Iniciar la corutina de fijación de vista (lock-in)
            }
        }
        else
        {
            if (isDetected)
            {
                isDetected = false;
                StopCoroutine(lockIn());       // Parar la fijación de vista
                 // Volver a iniciar la rotación
            }
        }
    }

    void OnDrawGizmos()
    {
        if (DebugGizmoManager.VisionCone)
        {
            Utility.DrawVisionCone(juanitoTorreta.position, coneAngle, coneDistance, isDetected, juanitoTorreta);
        }
    }
    void yonBombing()
    {
        if(!block){
            GameObject Balatemporal = Instantiate(Mira, BalaPrefabInicio.transform.position, BalaPrefabInicio.transform.rotation) as GameObject;
            Rigidbody rb = Balatemporal.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * BalaVelocidad);

            Destroy(Balatemporal, 2f);
            StartCoroutine(WaitFor());
        }
    }

    IEnumerator rotateIdle()
    {
        isRotating = true;
        while (!isDetected)
        {
            // Calcular el nuevo ángulo
            currentRotationAngle += idleRotationSpeed * rotationDirection * Time.deltaTime;

            // Si alcanza el ángulo máximo o mínimo, cambiar la dirección de la rotación
            if (Mathf.Abs(currentRotationAngle) >= maxRotationAngle)
            {
                rotationDirection *= -1;  // Invertir la dirección de la rotación
            }

            // Aplicar la rotación usando RotateAround
            juanitoTorreta.Rotate(0, idleRotationSpeed * rotationDirection * Time.deltaTime, 0);
            yield return null;
            
        }
        isRotating = false;
    }

    IEnumerator lockIn()
    {
        while (isDetected)  // Mientras el jugador esté dentro del rango de visión
        {
            // Hacer que juanitoTorreta mire al jugador
            Vector3 relativePos = player.position - juanitoTorreta.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            juanitoTorreta.rotation = Quaternion.Slerp(juanitoTorreta.rotation, toRotation, Time.deltaTime * 5f);
            yonBombing();
            yield return null; // Continuar cada frame
        }
        yield return new WaitForSeconds(2f);
        yonBombing();
        StartCoroutine(rotateIdle());
    }

    IEnumerator WaitFor()
    {
        block = true;
        yield return new WaitForSeconds(0.5f);
        block = false;
    }


    #region IA Diff
    void SetEnemyColor(float diff)
{
    if(diff >= 800) 
    {
        enemyRenderer.material.color = Color.red; // Dificultad alta
    } 
    else if(diff >= 500) 
    {
        enemyRenderer.material.color = Color.yellow; // Dificultad media
    } 
    else 
    {
        enemyRenderer.material.color = Color.green; // Dificultad baja
    }
}
    #endregion
}
