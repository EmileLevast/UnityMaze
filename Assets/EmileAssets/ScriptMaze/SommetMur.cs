using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SommetMur : MonoBehaviour
{
	public Vector3 speed;
	
	private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "player")
        {
                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(speed);           
        }
    }
}
