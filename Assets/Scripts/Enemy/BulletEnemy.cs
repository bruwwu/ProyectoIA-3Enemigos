using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletEnemy : MonoBehaviour
{
    // Asegúrate de arrastrar un GameObject que tenga el script HB_Slider
    public HealthGauge healthGauge;
    public Portrait portrait;

    void OnTriggerEnter(Collider collison)
    {
        if (collison.gameObject.tag == "Player")
        {
            Debug.Log("Me follan");
            
            // Aplicar daño al jugador
            GameManager.gameManager.playerHealth.DmgUnit(1);
            

            healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
            portrait.OnReceiveDamage(1);

            Debug.Log(GameManager.gameManager.playerHealth.Health);
            Destroy(gameObject); //Destruir bala al chocar con el Player
            
        }
        else if(GameManager.gameManager.playerHealth.Health <= 0)
        {
             SceneManager.LoadScene(1);
        }
    }
}
