using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HB_Slider : MonoBehaviour
{
    private Slider hbSlider;

    void Start()
    {
        hbSlider = GetComponent<Slider>();
    }

    // Establecer la salud máxima y el valor inicial
    public void SetMaxHealth(int maxHealth)
    {
        hbSlider.maxValue = maxHealth;
        hbSlider.value = maxHealth;
    }

    // Actualizar la salud actual en el slider
    public void SetHealth(int health)
    {
        hbSlider.value = health;
    }
}
