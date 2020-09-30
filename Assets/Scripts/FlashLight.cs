using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashLight : MonoBehaviour
{
    private bool isFlashlight = false;

    private void Start()
    {
        GetComponent<Light>().enabled = isFlashlight;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("FlashLight"))
        {
            isFlashlight = !isFlashlight;
            GetComponent<Light>().enabled = isFlashlight;
        } 
    }
}
