using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivation : MonoBehaviour
{
    public GameObject playerPuerta;
  void OnTriggerEnter(Collider other)
  {
    if(other.CompareTag("Player")){
        playerPuerta.SetActive(false);
    }
  }
}
