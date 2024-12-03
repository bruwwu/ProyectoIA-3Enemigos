using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDmgValues : MonoBehaviour
{
    public HB_Slider hB_Slider;
    public int velocityMagnitude;

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
    private void BossTakeDmg(int dmg)
    {
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(dmg);
        Debug.Log("poqe me pegas si yo te amO Fang");
    }

     public void ApplyWipeDamage(int wipeDamage)
    {
        GameManager.gameManager.playerHealth.DmgUnit(wipeDamage);
        hB_Slider.SetHealth(GameManager.gameManager.playerHealth.Health);
        Debug.Log("Wipe ejecutado, daño aplicado.");
    }
}
