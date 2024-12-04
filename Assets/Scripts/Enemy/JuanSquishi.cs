using System.Collections;
using UnityEngine;

public class JaunSquishi : MonoBehaviour
{
    [SerializeField] HealthGauge healthGauge; // Referencia al HealthGauge
    [SerializeField] Portrait portrait;
    public GameObject playerGameObject;
    public float sphereRadious;
    public float maxSpeed = 5f;
    public float maxAcceleration = 5f;
    public float turnSpeed = 5f;
    public float slowDownRadius = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Lógica para que el enemigo siga al jugador
        if (Utilities.Utility.IsInsideRadius(playerGameObject.transform.position, transform.position, sphereRadious))
        {
            Vector3 directionToPlayer = playerGameObject.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            float currentSpeed = (distanceToPlayer < slowDownRadius)
                ? Mathf.Lerp(maxSpeed, 0, 1 - (distanceToPlayer / slowDownRadius))
                : maxSpeed;

            Vector3 desiredVelocity = directionToPlayer.normalized * currentSpeed;
            Vector3 steering = desiredVelocity - rb.velocity;

            steering = Vector3.ClampMagnitude(steering, maxAcceleration);
            rb.AddForce(steering, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            if (rb.velocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Aplicar daño al jugador
            int damage = 1; // Daño fijo de 1
            GameManager.gameManager.playerHealth.DmgUnit(damage);

            // Actualizar la barra de vida (HealthGauge)
            if (healthGauge != null)
            {
                portrait.OnReceiveDamage(1);
                float newHealthPercentage = GameManager.gameManager.playerHealth.Health;
                healthGauge.OnCurrHealthChanged(newHealthPercentage);
            }

            Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");
        }
    }
}
