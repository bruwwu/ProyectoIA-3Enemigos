using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour
{
    [SerializeField] HB_Slider hB_Slider;
    public GameObject playerGameObject;
    public Vector3 Velocity = Vector3.zero;
    
    public float sphereRadious;
    public float maxSpeed = 5f;
    public float maxAcceleration = 5f;
    public float turnSpeed = 5f; // Velocidad de rotación hacia el jugador
    public float slowDownRadius = 3f; // Radio en el que se reduce la velocidad cuando se acerca al jugador
    
    // Variable para almacenar la magnitud de la velocidad
    private int velocityMagnitude;

    // Referencia al Rigidbody del enemigo
    private Rigidbody rb;

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();
    }

   void Update()
{
    // Verificar si el jugador está dentro del radio de acción
    if (Utilities.Utility.IsInsideRadius(playerGameObject.transform.position, transform.position, sphereRadious))
    {
        // Calcula la dirección hacia el jugador (Seek)
        Vector3 directionToPlayer = PuntaMenosCola(playerGameObject.transform.position, transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;

        // Si está cerca del jugador, reduce la velocidad para evitar movimientos bruscos
        if (distanceToPlayer < slowDownRadius)
        {
            maxSpeed = Mathf.Lerp(maxSpeed, 0, Time.deltaTime);
        }
        else
        {
            maxSpeed = 5f; // Restablece la velocidad máxima si está fuera del radio de desaceleración
        }

        // Calcula la dirección normalizada hacia el jugador
        Vector3 desiredVelocity = directionToPlayer.normalized * maxSpeed;

        // Calcula la fuerza de dirección (fuerza necesaria para cambiar la velocidad)
        Vector3 steering = desiredVelocity - rb.velocity;

        // Limita la aceleración para que no sea demasiado abrupta
        steering = Vector3.ClampMagnitude(steering, maxAcceleration);

        // Aplica la fuerza de dirección al Rigidbody usando AddForce
        rb.AddForce(steering, ForceMode.Acceleration);

        // Limita la velocidad para no exceder el máximo permitido
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        // Suaviza la rotación del enemigo hacia la dirección deseada
        if (rb.velocity.magnitude > 0.1f) // Evita rotación innecesaria cuando está casi quieto
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        // Calcula la magnitud de la velocidad y la convierte a entero
        velocityMagnitude = Mathf.RoundToInt(rb.velocity.magnitude);
    }
    else
    {
        maxSpeed = 0;
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

   
}
