using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAbility : MonoBehaviour
{
    [SerializeField] private bool isDealingDamage = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss") && !isDealingDamage)
        {
            isDealingDamage = true;
            InvokeRepeating(nameof(DealDamage), 0f, 0.5f);
        }
        else{
            isDealingDamage = false;
            CancelInvoke(nameof(DealDamage));
        }
    }

    void DealDamage()
    {
        // Lógica de daño
        Debug.Log("R Hit!");
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(1);
        Debug.Log(GameManager.gameManager.NaomiBossMiViejaWe.Health);

        if (GameManager.gameManager.NaomiBossMiViejaWe.Health == 0)
        {
            Debug.Log("Naomi Killed");
            CancelInvoke(nameof(DealDamage));
        }
    }
}
