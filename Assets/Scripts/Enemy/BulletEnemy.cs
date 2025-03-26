using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletEnemy : MonoBehaviour
{
    public HealthGauge healthGauge;
    public Portrait portrait;

    public GameObject floatingText_prefab;
    public GameObject damageTarget;

    private int damage = 1;

    void OnTriggerEnter(Collider collison)
    {
        if (collison.gameObject.tag == "Player")
        {
            // ✅ Mostrar texto flotante SIEMPRE
            if (floatingText_prefab)
            {
                ShowingDamage();
            }

            // 🛡️ Verificar si el jugador es invencible
            if (activationPlaceholder.isInvincible)
            {
                Debug.Log("Jugador invencible. Solo se muestra el daño visual.");
                Destroy(gameObject);
                return;
            }

            // ✅ Aplicar daño real
            Debug.Log("Me follan");

            GameManager.gameManager.playerHealth.DmgUnit(damage);

            healthGauge.OnCurrHealthChanged(GameManager.gameManager.playerHealth.Health);
            portrait.OnReceiveDamage(damage);

            Debug.Log(GameManager.gameManager.playerHealth.Health);

            Destroy(gameObject); // Destruir bala
        }
        else if (collison.gameObject.tag == "Wall")
        {
            Destroy(gameObject); // Destruir bala al chocar contra pared
        }
        else if (GameManager.gameManager.playerHealth.Health <= 0)
        {
            SceneManager.LoadScene(1); // Reiniciar escena si el jugador muere
        }
    }

    void ShowingDamage()
    {
        var go = Instantiate(floatingText_prefab, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }
}
