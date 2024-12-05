using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public LockOnSystem Lock;
     void OnTriggerEnter(Collider collider)
    {

        if(collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Disparaste a un amogus");
            GameManager.gameManager.baseEnemyHealth.DmgUnit(20);
            Debug.Log(GameManager.gameManager.baseEnemyHealth.Health);
            if(GameManager.gameManager.baseEnemyHealth.Health == 0)
            {
                Debug.Log("Heavy Muerto");
                Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.5f);
                Lock.cinemachineFreeLook.m_LookAt = Lock.playerTransform;
            }
        }
        else if(collider.gameObject.CompareTag("juanitoTorreta"))
        {
            Debug.Log("Disparaste a un juanitoTorreta");
            GameManager.gameManager.juanitoTorreta.DmgUnit(15);
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
            GameManager.gameManager.bruwuFem.DmgUnit(20);
            Debug.Log(GameManager.gameManager.bruwuFem.Health);
            if(GameManager.gameManager.bruwuFem.Health == 0)
            {
                Debug.Log("BruwuFlee Muerto");
                Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.5f);
            }
        }
    }
}
