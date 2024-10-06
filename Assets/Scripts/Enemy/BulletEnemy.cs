using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    // Asegúrate de arrastrar un GameObject que tenga el script HB_Slider
    [SerializeField] HB_Slider hBb_Slider; 

    void OnCollisionEnter(Collision collison)
    {
        if (collison.gameObject.tag == "Player")
        {
            Debug.Log("Me follan");
            
            // Aplicar daño al jugador
            GameManager.gameManager.playerHealth.DmgUnit(15, GameManager.gameManager.UI, GameManager.gameManager.Scene);
            

            // Actualizar la barra de salud con el valor actual de la salud
            hBb_Slider.SetHealth(GameManager.gameManager.playerHealth.Health);

            Debug.Log(GameManager.gameManager.playerHealth.Health);
            Destroy(gameObject); //Destruir bala al chocar con el Player
            
        }
    }
}
