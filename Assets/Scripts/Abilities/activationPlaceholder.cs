using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class activationPlaceholder : MonoBehaviour
{
    [Header("Ultimate")]
    public GameObject swainUlt;

    [Header("SwainQ")]
    public GameObject rayoPrefab;
    public Transform[] puntosDisparo;
    public float qCooldown = 5f;
    private bool qDisponible = true;

    [Header("Componentes/UI")]
    public PlayerInput pitufin;
    public Image qImagen;
    public TMP_Text contadorQ;

    // Estado para la activación de habilidades
    private bool isUltActive;

    void Awake()
    {
        pitufin = new PlayerInput();

        // Configuración de Input para la Ultimate
        pitufin.pitufin.RAbility.performed += OnRCast;
        pitufin.pitufin.RAbility.canceled += OnRCast;

        // Configuración de Input para la Q
        pitufin.pitufin.QAbility.performed += OnQCast;
    }

    void Update()
    {
        // Activar o desactivar la ultimate según el estado del botón
        swainUlt.SetActive(isUltActive);
    }

    void QActivation()
    {
        if (!qDisponible) return; // Evitar activar Q si no está disponible

        qDisponible = false;

        // Instanciar rayos en los puntos designados
        foreach (var punto in puntosDisparo)
        {
            Instantiate(rayoPrefab, punto.position, punto.rotation);
        }

        // Actualizar la interfaz visual del cooldown
        qImagen.color = new Color(1, 1, 1, 0.5f);
        StartCoroutine(QIniciarCooldown());
    }

    IEnumerator QIniciarCooldown()
    {
        float tiempoRestante = qCooldown;

        while (tiempoRestante > 0)
        {
            contadorQ.text = Mathf.CeilToInt(tiempoRestante).ToString();
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }

        QReiniciarHabilidad();
    }

    void QReiniciarHabilidad()
    {
        qDisponible = true;
        qImagen.color = Color.white;
        contadorQ.text = "";
    }

    void OnRCast(InputAction.CallbackContext context)
    {
        // Actualizar el estado de activación de la ultimate
        isUltActive = context.ReadValueAsButton();
        Debug.Log($"Ultimate activada: {isUltActive}, Fase:{context.phase}");
    }

    void OnQCast(InputAction.CallbackContext context)
    {
        // Activar Q solo si está disponible
        if (qDisponible && context.ReadValueAsButton())
        {
            QActivation();
            Debug.Log("Habilidad Q activada.");
        }
        else
        {
            Debug.Log("Habilidad Q no disponible.");
        }
    }

    void OnEnable()
    {
        pitufin.pitufin.Enable();
    }

    void OnDisable()
    {
        pitufin.pitufin.Disable();
    }
}
