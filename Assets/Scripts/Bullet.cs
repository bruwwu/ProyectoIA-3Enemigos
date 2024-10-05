using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Disparaste a un amogus");
            GameManager.gameManager.baseEnemyHealth.DmgUnit(15);
            Debug.Log(GameManager.gameManager.baseEnemyHealth.Health);
            if(GameManager.gameManager.baseEnemyHealth.Health == 0)
            {
                Debug.Log("Amogus Muerto");
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("Alv, la bala choca con la pared");
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Rocks")
        {
            Debug.Log("Le pegaste a una piedraaaaaa");
            GameManager.gameManager.rocaHealth.DmgUnit(20);
            Debug.Log(GameManager.gameManager.rocaHealth.Health);
            if(GameManager.gameManager.rocaHealth.Health == 0)
            {
                Debug.Log("Mate una roca");
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
