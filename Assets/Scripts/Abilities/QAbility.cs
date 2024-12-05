using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAbility : MonoBehaviour
{
    
    public HB_Slider bossHB_Slider;
    public CloudDMG cloudDMG;
    public HealthGauge healthGauge;
    public activationPlaceholder activation;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Boss")){
        Debug.Log("Q Hit!");
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(10);
        bossHB_Slider.SetHealth(GameManager.gameManager.NaomiBossMiViejaWe.Health);
        Debug.Log(GameManager.gameManager.NaomiBossMiViejaWe.Health);
        if(GameManager.gameManager.NaomiBossMiViejaWe.Health == 0)
            {
                Debug.Log("NaomiKilled");
                Vector3 newPosition = new Vector3(other.gameObject.transform.position.x, 60.0f, other.gameObject.transform.position.z);
                other.gameObject.transform.position = newPosition;
                Destroy(other.gameObject, 0.6f);
            }
        } else if(other.gameObject.CompareTag("JuanSquishi")){
            Debug.Log("Q Hit!");
            GameManager.gameManager.juanSquishi.DmgUnit(10);
            Debug.Log(GameManager.gameManager.juanSquishi.Health);
            if(GameManager.gameManager.juanSquishi.Health == 0)
                {
                    cloudDMG.StartCoroutine(cloudDMG.ImmunityTimer());
                    activation.StartCoroutine(activation.InmuneTime());
                    Debug.Log("JuanSquishi skibidi ded");

                    GameManager.gameManager.playerHealth.HealthUnit(10);
                    healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);

                    Vector3 newPosition = new Vector3(other.gameObject.transform.position.x, 60.0f, other.gameObject.transform.position.z);
                    other.gameObject.transform.position = newPosition;
                    Destroy(other.gameObject, 2f);
                }
        }
        
    }
}
