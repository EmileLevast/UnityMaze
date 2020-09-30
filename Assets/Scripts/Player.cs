using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int moneyPlayer = 0;
    public int vieMax = 1;
    private int vieJoueur;
    public GameController gameController;
    public bool isPlayerDead = false;
    public Text moneyDisplay;
    public HealthBar healthBar;

    void Start()
    {
        vieJoueur = vieMax;
        healthBar.SetMaxHealth(vieMax);
    }


    public void Heal(int nombreDePV)
    {
        vieJoueur += nombreDePV;
        if (vieJoueur > vieMax)
        {
            vieJoueur = vieMax;
        }
    }
    
    public void IncrementMoney(int value)
    {
        moneyPlayer += value;
        string money = moneyPlayer.ToString();
        if (moneyPlayer < 10)
        {
            money = "0" + moneyPlayer.ToString();
        }
        moneyDisplay.text = money + "$";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie")
        {
            vieJoueur -= collision.gameObject.GetComponent<Zombie>().forceZombie;

            healthBar.SetHealth(vieJoueur);
			
            if (vieJoueur <= 0)
            {
                isPlayerDead = true;
                gameController.PlayerDies();
            }
        }
    }
}
