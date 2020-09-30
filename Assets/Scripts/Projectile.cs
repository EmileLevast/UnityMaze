using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * -speed, Space.Self);
    }
}
