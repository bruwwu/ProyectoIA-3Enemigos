using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class HabiliDAM : MonoBehaviour
{
    public GameObject rayoPrefab; // Prefab del sistema de partículas
    public Transform[] puntosDisparo; // Puntos desde donde saldrán los rayos
    public float cooldown = 5f; // Tiempo de enfriamiento

    private bool puedeDisparar = true;

    // Referencias UI
    public Image iconoHabilidad; // El ícono de la habilidad
    public TMP_Text contadorHabilidad; // El contador sobre el ícono

    private void Update()
    {
        if (puedeDisparar && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ActivarHabilidad();
        }
    }

    void ActivarHabilidad()
    {
        puedeDisparar = false;

        // Instanciar partículas en cada punto de disparo
        foreach (var punto in puntosDisparo)
        {
            Instantiate(rayoPrefab, punto.position, punto.rotation);
        }

        // Deshabilitar el ícono
        iconoHabilidad.color = new Color(1, 1, 1, 0.5f); // Poner el ícono en gris
        StartCoroutine(IniciarEnfriamiento());
    }

    System.Collections.IEnumerator IniciarEnfriamiento()
    {
        float tiempoRestante = cooldown;

        while (tiempoRestante > 0)
        {
            // Actualizar el contador
            contadorHabilidad.text = Mathf.CeilToInt(tiempoRestante).ToString();
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }

        // Reiniciar la habilidad
        ReiniciarHabilidad();
    }

    void ReiniciarHabilidad()
    {
        puedeDisparar = true;
        iconoHabilidad.color = Color.white; // Restaurar el ícono
        contadorHabilidad.text = ""; // Limpiar el contador
    }
}