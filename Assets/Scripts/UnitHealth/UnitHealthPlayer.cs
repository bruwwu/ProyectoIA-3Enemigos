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
    public void DmgUnit(int dmgAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= dmgAmount;
        }

    }
          public void VelDmgUnit(int dmgAmount, int Velocity)
    {
        if (currentHealth > 0)
        {
            currentHealth -= dmgAmount * Velocity;
        }
    }

    //En caso de hacer un sistema de curacion, se implementa la misma logica, pero con MaxHealth
    
    public void HealthUnit(int healthAmount)
    {
        currentHealth += healthAmount;

        // Asegurarse de que la salud no exceda el valor máximo
        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
    }

}
