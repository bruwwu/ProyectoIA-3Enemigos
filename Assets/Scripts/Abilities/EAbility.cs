using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAbility : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Boss")){
        Debug.Log("Q Hit!");
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(20);
        Debug.Log(GameManager.gameManager.NaomiBossMiViejaWe.Health);
        if(GameManager.gameManager.NaomiBossMiViejaWe.Health == 0)
            {
                Debug.Log("NaomiKilled");
                Vector3 newPosition = new Vector3(other.gameObject.transform.position.x, 60.0f, other.gameObject.transform.position.z);
                other.gameObject.transform.position = newPosition;
                Destroy(other.gameObject, 0.6f);
            }
        }
        
    }
}
