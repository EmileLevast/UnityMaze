using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;



public class ShootableBox : MonoBehaviour {

    //The box's current health point total
    public int currentHealth = 3;
	private Maze maze;
	public GameObject impactPrefab;
	private List<GameObject> listImpacts = new List<GameObject>();
	
	private Animator anim;
	public float delayDeath = 2f;



	void Start(){
		anim = GetComponent<Animator>();
	}
	
    public void Damage(int damageAmount, Vector3 position)
    {
		GameObject impact = Instantiate(impactPrefab);
		impact.GetComponent<Transform>().parent = transform.Find("Base HumanPelvis");
		impact.GetComponent<Transform>().position = position;
		impact.SetActive(true);
		
		listImpacts.Add(impact);
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0) 
        {
            //if health has fallen below zero, deactivate it
			/*Zombie zombie = GetComponent<Zombie>();
			if(zombie != null){
				maze = GameObject.FindWithTag("Maze").GetComponent<Maze>();
				maze.CreateZombie();
				maze.CreateZombie();
			}
            gameObject.SetActive (false);
			Destroy(gameObject);*/
			StartCoroutine(Die());

        }
    }
	
	private IEnumerator Die(){
		anim.SetTrigger("Die");
		NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
		navAgent.isStopped = true;
		Zombie zombie = GetComponent<Zombie>();
		zombie.isAlive = false;
		yield return new WaitForSeconds(delayDeath);
		maze = GameObject.FindWithTag("Maze").GetComponent<Maze>();
		maze.CreateZombie();
		maze.CreateZombie();
		gameObject.SetActive (false);
		Destroy(gameObject);
	}
}