using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthPlayer
{
    //Parametros
    int currentHealth;
    int currentMaxHealth;

    //Propiedades
    public int Health
    {
        get{
            return currentHealth;
        }
        set{
            currentHealth = value;
        }
    }

    public int MaxHealth
    {
        get{
            return currentMaxHealth;
        }
        set{
            currentMaxHealth = value;
        }
    }

    //Constructor
    public UnitHealthPlayer(int health, int MaxHealth)
    {
        currentHealth = health;
        currentMaxHealth = MaxHealth;
        
    }

    //metodos

    //Metodo para recibir daño
    public void DmgUnit(int dmgAmount, GameObject ui, GameObject scene)
    {
        if (currentHealth > 0)
        {
            currentHealth -= dmgAmount;
        }
        else if (currentHealth <= 0)
        {
            scene.SetActive(false);
            ui.SetActive(true);
        }

    }

    //En caso de hacer un sistema de curacion, se implementa la misma logica, pero con MaxHealth
    
    /*public void HealthUnit(int healthAmount)
    {
        if (currentHealth < currentMaxHealth)
        {
            currentHealth += healthAmount;
        }
        else if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }

    }*/
}
