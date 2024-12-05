using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDmgValues : MonoBehaviour
{
    public HealthGauge healthGauge;
    public Portrait portrait;
    public int velocityMagnitude;

    public void OnTriggerEnter(Collider collision)
    {
        // Si colisiona con el jugador, aplicar daño basado en la velocidad
        if (collision.gameObject.tag == "Player")
        {
           if (healthGauge != null)
            {
                GameManager.gameManager.playerHealth.DmgUnit(2);
                healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
                portrait.OnReceiveDamage(2);
                Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");
            }
        }
    }
    private void BossTakeDmg(int dmg)
    {
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(dmg);
        Debug.Log("poqe me pegas si yo te amO Fang");
    }

     public void ApplyWipeDamage(int wipeDamage)
    {
        GameManager.gameManager.playerHealth.DmgUnit(wipeDamage);
        healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
        portrait.OnReceiveDamage(9999);
        Debug.Log($"Player Health: {GameManager.gameManager.playerHealth.Health}");
        Debug.Log("Wipe ejecutado, daño aplicado.");
    }
}
