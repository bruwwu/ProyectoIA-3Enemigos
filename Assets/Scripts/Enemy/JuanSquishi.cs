using System.Collections;
using UnityEngine;

public class JaunSquishi : MonoBehaviour
{
    [SerializeField] HealthGauge healthGauge;
    [SerializeField] Portrait portrait;
    public UnitHealthEnemy juanSquishiHealth;
    public GameObject playerGameObject;
    public float sphereRadious;
    public float maxSpeed = 5f;
    public float maxAcceleration = 5f;
    public float inmuneTime = 5f; // Tiempo de inmunidad
    public float turnSpeed = 5f;
    public float slowDownRadius = 3f;

    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        juanSquishiHealth = new UnitHealthEnemy(GameManager.gameManager.juanSquishi.MaxHealth, GameManager.gameManager.juanSquishi.MaxHealth);
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

    public void DmgUnit(int damage)
    {
        juanSquishiHealth.DmgUnit(damage);
        Debug.Log($"Salud actual del JaunSquishi: {juanSquishiHealth.Health}");
        if (juanSquishiHealth.Health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("JaunSquishi eliminado.");
        Destroy(gameObject, 2f); // Destruir esta instancia específica tras un retraso
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Aplicar daño al jugador
            int damage = 1;
            GameManager.gameManager.playerHealth.DmgUnit(damage);
            portrait.OnReceiveDamage(1);
            float newHealthPercentage = GameManager.gameManager.playerHealth.Health;
            healthGauge.OnCurrHealthChanged(newHealthPercentage);
            Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");
        }
    }
}
