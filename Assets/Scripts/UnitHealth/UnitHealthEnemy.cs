using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthEnemy
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
    public UnitHealthEnemy(int health, int MaxHealth)
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
            if(currentHealth <= 0){
                currentHealth = 0;
            }
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
