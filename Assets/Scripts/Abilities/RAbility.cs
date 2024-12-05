using System.Collections;
using UnityEngine;

public class RAbility : MonoBehaviour
{
    [SerializeField] private bool isDealingDamage = false;
    public HB_Slider bossHB_Slider;
    public CloudDMG cloudDMG;
    public HealthGauge healthGauge;
    public activationPlaceholder activation;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss") && !isDealingDamage)
        {
            isDealingDamage = true;
            InvokeRepeating(nameof(DealDamageToBoss), 0f, 0.5f);
        }
        else if (other.gameObject.CompareTag("JuanSquishi"))
        {
            DealDamageToJuanSquishi(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            isDealingDamage = false;
            CancelInvoke(nameof(DealDamageToBoss));
        }
    }

    void DealDamageToBoss()
    {
        Debug.Log("R Hit on Boss!");
        GameManager.gameManager.NaomiBossMiViejaWe.DmgUnit(1);
        bossHB_Slider.SetHealth(GameManager.gameManager.NaomiBossMiViejaWe.Health);
        Debug.Log(GameManager.gameManager.NaomiBossMiViejaWe.Health);

        if (GameManager.gameManager.NaomiBossMiViejaWe.Health <= 0)
        {
            Debug.Log("Naomi Killed");
            CancelInvoke(nameof(DealDamageToBoss));
        }
    }

    void DealDamageToJuanSquishi(Collider other)
    {
        Debug.Log("R Hit on JuanSquishi!");
        GameManager.gameManager.juanSquishi.DmgUnit(10);
        Debug.Log(GameManager.gameManager.juanSquishi.Health);

        if (GameManager.gameManager.juanSquishi.Health <= 0)
        {
            Debug.Log("JuanSquishi skibidi ded");
            cloudDMG.StartCoroutine(cloudDMG.ImmunityTimer());
            activation.StartCoroutine(activation.InmuneTime());

            // Restaurar la salud del jugador
            GameManager.gameManager.playerHealth.HealthUnit(10);
            healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);

            // Reubicar y destruir el objeto de JuanSquishi
            Vector3 newPosition = new Vector3(other.gameObject.transform.position.x, 60.0f, other.gameObject.transform.position.z);
            other.gameObject.transform.position = newPosition;
            Destroy(other.gameObject, 2f);
        }
    }
}
