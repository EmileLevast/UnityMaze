using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Zombie : MonoBehaviour
{
	public Texture2D usedTexture, normalTexture;
    public int forceZombie=1;
	// Start is called before the first frame update
	private NavMeshAgent navAgent;
	private Transform player;
	public float delayPathFinding;
	private WaitForSeconds delay;
	private Animator anim;
	public bool isAlive = true;
	
    void Start()
    {
		player = GameObject.FindGameObjectWithTag ("player").transform;
		delay = new WaitForSeconds(delayPathFinding);
        navAgent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();

		StartCoroutine(FollowPlayer());
    }


    public IEnumerator FollowPlayer()
    {
		float distance;
        while (isAlive)
        {
			distance = Vector3.Distance(transform.position, new Vector3(player.position.x,.5f,player.position.z));
			if (distance < 10)
            {
				if(distance < 1.5f)
				{
					anim.SetTrigger("Attack");
				}else{
					anim.SetTrigger("BeginRun");
				}
				
				if(distance < 0.9f){
					GetComponent<Collider>().enabled = false;
				}else{
					GetComponent<Collider>().enabled = true;
				}					
                navAgent.SetDestination(player.position);
            }else
			{
					anim.SetTrigger("StopRun");
			}
            yield return delay;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if (collision.gameObject.GetComponent<Renderer>() != null)
            {
                collision.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", usedTexture);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if (collision.gameObject.GetComponent<Renderer>() != null)
            {
                IEnumerator fonctionSetNormalTexture = SetNormalTexture(collision.gameObject);
                StartCoroutine(fonctionSetNormalTexture);
            }
        }
    }

    private IEnumerator SetNormalTexture(GameObject go)
    {
        yield return new WaitForSeconds(1);
        go.GetComponent<Renderer>().material.SetTexture("_MainTex", normalTexture);
    }


}
