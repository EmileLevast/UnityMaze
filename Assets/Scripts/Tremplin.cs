    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tremplin : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 speed;

    public Texture2D usedTexture, normalTexture;
    
    private bool isTremplinAvailable = true;

    // Start is called before the first frame update

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "player")
        {
            if (isTremplinAvailable)
            {
                rb = collider.gameObject.GetComponent<Rigidbody>();
                rb.GetComponent<PlayerMovement>().secondJump = true;
                rb.AddForce(speed);
                GetComponent<Renderer>().material.SetTexture("_MainTex", usedTexture);
                isTremplinAvailable = false;
                StartCoroutine("isAvailable");
            }
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "player")
        {
            if (isTremplinAvailable)
            {
                rb = coll.gameObject.GetComponent<Rigidbody>();
                rb.GetComponent<PlayerMovement>().secondJump = true;
                rb.AddForce(speed);
                GetComponent<Renderer>().material.SetTexture("_MainTex", usedTexture);
                isTremplinAvailable = false;
                StartCoroutine("isAvailable");
            }
        }
    }
    private IEnumerator isAvailable()
    {
        yield return new WaitForSeconds(2);
        isTremplinAvailable = true;
        GetComponent<Renderer>().material.SetTexture("_MainTex", normalTexture);
    }
}