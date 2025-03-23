using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletEnemy : MonoBehaviour
{
    // Asegúrate de arrastrar un GameObject que tenga el script HB_Slider
    public HealthGauge healthGauge;
    public Portrait portrait;

    public GameObject floatingText_prefab;

    public GameObject damageTarget;

    private int damage = 1;

    void OnTriggerEnter(Collider collison)
    {
        if (collison.gameObject.tag == "Player")
        {
            Debug.Log("Me follan");
            
            // Aplicar daño al jugador
            GameManager.gameManager.playerHealth.DmgUnit(damage);
            if(floatingText_prefab)
            {
                ShowingDamage();
            }

            healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
            portrait.OnReceiveDamage(damage);

            Debug.Log(GameManager.gameManager.playerHealth.Health);
            Destroy(gameObject); //Destruir bala al chocar con el Player
            
        }
        else if(collison.gameObject.tag == "Wall")
        {
            Destroy(gameObject); //Destruir bala al chocar con el Player
        }
        else if(GameManager.gameManager.playerHealth.Health <= 0)
        {
             SceneManager.LoadScene(1);
        }
    }

    void ShowingDamage()
    {
        var go = Instantiate(floatingText_prefab, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }

}
