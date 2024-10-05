using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Me follan");
            GameManager.gameManager.playerHealth.DmgUnit(20,GameManager.gameManager.UI, GameManager.gameManager.Scene);
            Debug.Log(GameManager.gameManager.playerHealth.Health);
        }
    }
}
