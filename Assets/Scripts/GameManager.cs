using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject UI;

    public GameObject Scene;

    public static GameManager gameManager{ get; private set; }

    public UnitHealthPlayer playerHealth = new UnitHealthPlayer(100, 100);

    public UnitHealthObject rocaHealth = new UnitHealthObject(100, 100);

    public UnitHealthEnemy baseEnemyHealth = new UnitHealthEnemy(100, 100);

    public UnitHealthEnemy juanitoTorreta = new UnitHealthEnemy(50, 50);

    void Awake()
    {
        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else{
            gameManager = this;
        }
    }

    public void RestartB()
    {
        SceneManager.LoadScene("Proyecto Primer Parcial part 1");
    }
}
