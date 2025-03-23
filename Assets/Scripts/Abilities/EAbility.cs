using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EAbility : MonoBehaviour
{
    public HB_Slider bossHB_Slider;
    public CloudDMG cloudDMG;
    public HealthGauge healthGauge;
    public activationPlaceholder activation;
    public JaunSquishi jaunSquishi;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Boss")){
        Debug.Log("Q Hit!");
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(6);
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
            else if (collider.gameObject.CompareTag("JuanSquishi"))
        {
            Debug.Log("Disparaste a un JuanSquishi");

            // Obtener la instancia específica de JaunSquishi
            JaunSquishi juan = collider.gameObject.GetComponent<JaunSquishi>();
            if (juan != null)
            {
                // Aplicar daño a la instancia específica
                juan.DmgUnit(6);

                // Verificar si el enemigo ha muerto
                if (juan.juanSquishiHealth.Health == 0)
                {
        

                    Debug.Log("JuanSquishi Muerto");

                    // Activar efectos de inmunidad
                    cloudDMG.StartCoroutine(cloudDMG.ImmunityTimer());
                    activation.StartCoroutine(activation.InmuneTime());

                    // Curar al jugador
                    GameManager.gameManager.playerHealth.HealthUnit(10);
                    healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);

                    // Mover y destruir el enemigo
                    Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                    collider.gameObject.transform.position = newPosition;
                    Destroy(collider.gameObject, 2f);
                }
            }
        else if(collider.gameObject.CompareTag("juanitoTorreta"))
        {
            Debug.Log("Disparaste a un juanitoTorreta");
            GameManager.gameManager.juanitoTorreta.DmgUnit(6);
            Debug.Log(GameManager.gameManager.juanitoTorreta.Health);
            if(GameManager.gameManager.juanitoTorreta.Health == 0)
            {
                Debug.Log("juanitoTorreta Muerto");
                 Vector3 newPosition = new Vector3(collider.gameObject.transform.position.x, 60f, collider.gameObject.transform.position.z);
                collider.gameObject.transform.position = newPosition;
                Destroy(collider.gameObject, 0.5f);
            }
        }
        }
    }
}
