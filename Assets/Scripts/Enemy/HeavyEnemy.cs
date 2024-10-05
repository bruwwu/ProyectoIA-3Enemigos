using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour
{
    [SerializeField] HB_Slider hB_Slider;
    public GameObject playerGameObject;
    public Vector3 Velocity = Vector3.zero;
    public float sphereRadious;
    public float maxSpeed = 5f;
    public float maxAcceleration = 5f;
    
    // Variable para almacenar la magnitud de la velocidad
    private int velocityMagnitude;

    void Update()
    {
        if (Utilities.Utility.IsInsideRadius(playerGameObject.transform.position, transform.position, sphereRadious))
        {
            // Calcula la dirección hacia el objetivo (Seek)
            Vector3 PosToTarget = PuntaMenosCola(playerGameObject.transform.position, transform.position);
            
            // Calcula la fuerza de dirección y la aplica a la velocidad actual
            Velocity += maxAcceleration * Time.deltaTime * PosToTarget.normalized;
            
            // Limita la magnitud de la velocidad para no superar la velocidad máxima
            Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);

            // Calcula la magnitud de la velocidad y la convierte a entero
            velocityMagnitude = Mathf.RoundToInt(Velocity.magnitude);
            
            // Actualiza la posición del objeto según la velocidad calculada
            transform.position += Velocity * Time.deltaTime;
        }
    }

    // Método para calcular la dirección de un punto a otro
    public Vector3 PuntaMenosCola(Vector3 Punta, Vector3 Cola)
    {
        return new Vector3(Punta.x - Cola.x, Punta.y - Cola.y, Punta.z - Cola.z);
    }

    public void OnTriggerEnter(Collider collision)
    {
        // Si colisiona con el jugador, aplicar daño basado en la velocidad
        if (collision.gameObject.tag == "Player")
        {
            // Asegúrate de que GameManager y playerHealth están correctamente configurados
            GameManager.gameManager.playerHealth.VelDmgUnit(2, velocityMagnitude);
            hB_Slider.SetHealth(GameManager.gameManager.playerHealth.Health);
            Debug.Log(GameManager.gameManager.playerHealth.Health);
        }
    }
}
