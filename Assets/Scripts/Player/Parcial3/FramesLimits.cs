using UnityEngine;

public class FramesLimits : MonoBehaviour
{
    public Animator animator; // Referencia al componente Animator
    public float choppyRate = 0.1f; // Tasa de actualización de la animación (en segundos)

    private float timer = 0f; // Temporizador para controlar la actualización

    void Update()
    {
        timer += Time.deltaTime;

        // Solo actualizamos la animación cuando el temporizador supera la tasa de actualización
        if (timer >= choppyRate)
        {
            animator.Update(timer); // Actualiza el Animator
            timer = 0f; // Reinicia el temporizador
        }
    }
}
