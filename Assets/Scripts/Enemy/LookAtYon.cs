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
    
    void Start()
    {
        // Iniciar la rotación de juanitoTorreta al inicio
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

            Destroy(Balatemporal, 1f);
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
}
