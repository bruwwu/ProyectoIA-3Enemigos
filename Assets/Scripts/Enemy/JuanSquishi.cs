using System.Collections;
using UnityEngine;

public class JaunSquishi : MonoBehaviour
{
    [SerializeField] HB_Slider hB_Slider;
    public GameObject playerGameObject;
    public float sphereRadious;
    public float maxSpeed = 5f;
    public float maxAcceleration = 5f;
    public float turnSpeed = 5f; // Velocidad de rotación hacia el jugador
    public float slowDownRadius = 3f; // Radio en el que se reduce la velocidad cuando se acerca al jugador

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
            Vector3 directionToPlayer = playerGameObject.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            // Si está cerca del jugador, reduce la velocidad
            float currentSpeed = (distanceToPlayer < slowDownRadius)
                ? Mathf.Lerp(maxSpeed, 0, 1 - (distanceToPlayer / slowDownRadius))
                : maxSpeed;

            // Calcula la dirección normalizada hacia el jugador
            Vector3 desiredVelocity = directionToPlayer.normalized * currentSpeed;

            // Calcula la fuerza de dirección
            Vector3 steering = desiredVelocity - rb.velocity;

            // Limita la aceleración para que no sea demasiado abrupta
            steering = Vector3.ClampMagnitude(steering, maxAcceleration);

            // Aplica la fuerza de dirección al Rigidbody
            rb.AddForce(steering, ForceMode.Acceleration);

            // Limita la velocidad para no exceder el máximo permitido
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            // Rotación suave hacia la dirección del jugador
            if (rb.velocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }
        else
        {
            // Detiene el movimiento si el jugador está fuera del rango
            rb.velocity = Vector3.zero;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        // Si colisiona con el jugador, causar un daño fijo
        if (collision.gameObject.tag == "Player")
        {
            // Aplicar daño fijo al jugador
            GameManager.gameManager.playerHealth.DmgUnit(1);
            hB_Slider.SetHealth(GameManager.gameManager.playerHealth.Health);
            Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, sphereRadious);
    }
}
