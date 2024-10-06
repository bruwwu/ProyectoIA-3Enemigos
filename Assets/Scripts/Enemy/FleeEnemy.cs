using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemy : MonoBehaviour
{
    [Header("VisionCone")]
    public Transform Enemy;
    public Transform player;
    private bool isDetected = false;
    [SerializeField] HB_Slider hB_Slider;
    public GameObject playerGameObject;
    public Vector3 Velocity = Vector3.zero;
    public float sphereRadious;
    public float maxSpeed = 5f;
    public float maxAcceleration = 5f;

    [Header("Shooting")]
    public GameObject Mira;
    public GameObject BalaPrefabInicio;
    public float BalaVelocidad;

    private bool block;
    
    // Variable para almacenar la magnitud de la velocidad
    private int velocityMagnitude;

    void Update()
    {
        if (Utilities.Utility.IsInsideRadius(playerGameObject.transform.position, transform.position, sphereRadious))
        {
            isDetected = true;
            // Calcula la dirección hacia el objetivo (Seek)
            Vector3 PosToTarget = -PuntaMenosCola(playerGameObject.transform.position, transform.position);
            
            // Calcula la fuerza de dirección y la aplica a la velocidad actual
            Velocity += maxAcceleration * Time.deltaTime * PosToTarget.normalized;
            
            // Limita la magnitud de la velocidad para no superar la velocidad máxima
            Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);

            // Calcula la magnitud de la velocidad y la convierte a entero
            velocityMagnitude = Mathf.RoundToInt(Velocity.magnitude);
            
            // Actualiza la posición del objeto según la velocidad calculada
            transform.position += Velocity * Time.deltaTime;
        }
        else
        {
            isDetected = false;
            StartCoroutine(lockIn());
        }
    }

    // Método para calcular la dirección de un punto a otro
    public Vector3 PuntaMenosCola(Vector3 Punta, Vector3 Cola)
    {
        return new Vector3(Punta.x - Cola.x, Punta.y - Cola.y, Punta.z - Cola.z);
    }

    public void OnTriggerEnter(Collider collision)
    {
        // Si colisiona con el jugador, aplicar daño basado en la velocidad
        if (collision.gameObject.tag == "Player")
        {
            // Asegúrate de que GameManager y playerHealth están correctamente configurados
            GameManager.gameManager.playerHealth.VelDmgUnit(2, velocityMagnitude);
            hB_Slider.SetHealth(GameManager.gameManager.playerHealth.Health);
            Debug.Log(GameManager.gameManager.playerHealth.Health);
        }
    }

    IEnumerator lockIn()
    {
        while (!isDetected)  // Mientras el jugador esté dentro del rango de visión
        {
            // Hacer que juanitoTorreta mire al jugador
            Vector3 relativePos = player.position - Enemy.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            Enemy.rotation = Quaternion.Slerp(Enemy.rotation, toRotation, Time.deltaTime * 5f);
            yonBombing();
            yield return null; // Continuar cada frame
        }
        yield return new WaitForSeconds(2f);
    }

    void yonBombing()
    {
        if(!block){
            GameObject Balatemporal = Instantiate(Mira, BalaPrefabInicio.transform.position, BalaPrefabInicio.transform.rotation) as GameObject;
            Rigidbody rb = Balatemporal.GetComponent<Rigidbody>();
            Vector3 relativePos = PredictPos(Enemy.position, player.position, 1f);
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            rb.AddForce(transform.forward * BalaVelocidad);
            Destroy(Balatemporal, 1f);
            StartCoroutine(WaitFor());
        }
    }

    Vector3 PredictPos(Vector3 InitiaPos, Vector3 Velocity, float TimePrediction)
    {
        return InitiaPos + Velocity * TimePrediction; // Retorna la posición futura
    }
    IEnumerator WaitFor()
    {
        block = true;
        yield return new WaitForSeconds(3f);
        block = false;
    }
}
