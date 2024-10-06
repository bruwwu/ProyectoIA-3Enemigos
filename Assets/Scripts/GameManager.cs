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

    public UnitHealthObject rocaHealth = new UnitHealthObject(30, 30);

    public UnitHealthEnemy baseEnemyHealth = new UnitHealthEnemy(100, 100);

    public UnitHealthEnemy juanitoTorreta = new UnitHealthEnemy(60, 60);
    public UnitHealthEnemy bruwuFem = new UnitHealthEnemy(40, 40);

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
