using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoint : MonoBehaviour
{	
	private Maze maze;
	
	 // Start is called before the first frame update
    void Start()
    {
		maze = GameObject.FindWithTag("Maze").GetComponent<Maze>();
    }

	 private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "player")
        {
			maze.GenerateEnnemies();
			Destroy(gameObject);
        }
    }
}
