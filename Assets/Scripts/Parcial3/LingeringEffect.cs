using System.Collections;
using UnityEngine;

public class LingeringBullet : MonoBehaviour
{
    [Header("Lingering Damage Settings")]
    public float damageInterval = 0.5f; // Tiempo entre cada tick de daño
    public int damageAmount = 1; // Cantidad de daño por tick
    public float lingerDuration = 5f; // Duración total del efecto persistente
    public HealthGauge healthGauge; // Referencia al HealthGauge
    public Portrait portrait; // Referencia al Portrait

    private bool isDealingDamage = false; // Bandera para controlar el daño
    private bool isLingering = true; // Control de la duración del efecto
    private Collider target; // Objetivo actual

    private void Start()
    {
        // Destruir el proyectil después de su duración
        Destroy(gameObject, lingerDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDealingDamage)
        {
            target = other; // Guardar el objetivo
            isDealingDamage = true;
            StartCoroutine(ApplyLingeringDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isDealingDamage)
        {
            isDealingDamage = false; // Detener el daño
            target = null; // Limpiar el objetivo
        }
    }

    private IEnumerator ApplyLingeringDamage()
    {
        while (isLingering && isDealingDamage)
        {
            // Aplicar daño al jugador
            if (target != null)
            {
                GameManager.gameManager.playerHealth.DmgUnit(damageAmount);
                healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
                portrait.OnReceiveDamage(damageAmount);

                Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }
}
