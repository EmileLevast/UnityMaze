using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bonus : MonoBehaviour
{

    public bool isJumpBoost = false;
    public bool isHeal = false;
    public bool isMoney = false;

    public int moneyValue = 1;
    public int healValue = 1;
    private IEnumerator coroutine;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (isJumpBoost)
            {
                coroutine = jumpBoost(collision.gameObject.GetComponent<PlayerMovement>());
                StartCoroutine(coroutine);
            }

            if (isHeal)
            {
                collision.gameObject.GetComponent<Player>().Heal(1);
            }

            if (isMoney)
            {
                collision.gameObject.GetComponent<Player>().IncrementMoney(moneyValue);
            }

            StartCoroutine("respawnBonus");
        }
    }

    private IEnumerator jumpBoost(PlayerMovement playerMovement)
    {
        //print("Jump boosted");
        playerMovement.jumpForce *= 1.5f;
        yield return new WaitForSeconds(10);
        playerMovement.jumpForce /= 1.5f;
        
    }    
    
    // TODO
    private void Heal(Player player) 
    {
        player.Heal(healValue);
    }

    // Désactive le bonus (trigger et visuel)
    // Puis le réactive 10 secondes apres.
    private IEnumerator respawnBonus()
    {   
        Collider collider = gameObject.GetComponent<Collider>();
        Renderer render = gameObject.GetComponent<Renderer>();

        collider.enabled = false;
        render.enabled = false;
        yield return new WaitForSeconds(10);
        collider.enabled = true;
        render.enabled = true;
    }
}
