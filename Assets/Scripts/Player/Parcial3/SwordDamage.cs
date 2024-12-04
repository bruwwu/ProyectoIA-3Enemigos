using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public HB_Slider bossHB_Slider;
     void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Disparaste a un amogus");
            GameManager.gameManager.baseEnemyHealth.DmgUnit(10);
            Debug.Log(GameManager.gameManager.baseEnemyHealth.Health);
            if(GameManager.gameManager.baseEnemyHealth.Health == 0)
            {
                Debug.Log("Heavy Muerto");
                Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.5f);
            }
        }
        else if(collider.gameObject.CompareTag("juanitoTorreta"))
        {
            Debug.Log("Disparaste a un juanitoTorreta");
            GameManager.gameManager.juanitoTorreta.DmgUnit(10);
            Debug.Log(GameManager.gameManager.juanitoTorreta.Health);
            if(GameManager.gameManager.juanitoTorreta.Health == 0)
            {
                Debug.Log("juanitoTorreta Muerto");
                 Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.5f);
            }
        }
          else if(collider.gameObject.CompareTag("bruwuFem"))
        {
            Debug.Log("Disparaste a un flee");
            GameManager.gameManager.bruwuFem.DmgUnit(10);
            Debug.Log(GameManager.gameManager.bruwuFem.Health);
            if(GameManager.gameManager.bruwuFem.Health == 0)
            {
                Debug.Log("BruwuFlee Muerto");
                Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.5f);
            }
        } else if(collider.gameObject.CompareTag("Boss")){
            Debug.Log("Pijaso Hit!");
            GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(10);
            bossHB_Slider.SetHealth(GameManager.gameManager.NaomiBossMiViejaWe.Health);
            Debug.Log(GameManager.gameManager.NaomiBossMiViejaWe.Health);
            if(GameManager.gameManager.NaomiBossMiViejaWe.Health == 0)
            {
                Debug.Log("NaomiKilled");
                Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60.0f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.6f);
            }
        }
    }
    
}
