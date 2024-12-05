using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloudDMG : MonoBehaviour
{
    public HealthGauge healthGauge;
    public Portrait portrait;
    public bool cloudDmg = false;
    public float duration;

    public bool isImmune = false; // Bandera para el período de inmunidad

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !cloudDmg && !isImmune)
        {
            cloudDmg = true;
            InvokeRepeating(nameof(DealDamage), 0f, 0.5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cloudDmg = false;
            CancelInvoke(nameof(DealDamage));
        }
    }

    void DealDamage()
    {
        GameManager.gameManager.playerHealth.DmgUnit(1);
        healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
        portrait.OnReceiveDamage(1);

        Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");

        // Si el jugador muere, cancelar el daño
        if (GameManager.gameManager.playerHealth.Health <= 0)
        {
            Debug.Log("fang tiesa");
            SceneManager.LoadScene(1);
            CancelInvoke(nameof(DealDamage));
        }
    }


    public IEnumerator ImmunityTimer()
    {
        isImmune = true; // Activar inmunidad
        cloudDmg = false; // Cancelar cualquier daño activo
        CancelInvoke(nameof(DealDamage)); // Asegurar que no se invoque el daño

        yield return new WaitForSeconds(duration);

        isImmune = false; // Desactivar inmunidad
        Debug.Log("Inmunidad terminada.");
    }
}
