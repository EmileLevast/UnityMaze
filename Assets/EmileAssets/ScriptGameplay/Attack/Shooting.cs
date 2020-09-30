using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform projectileSpawn;
    public GameObject projectilePrefab;
    private bool isReadyToShot = true;
    public float Cooldown = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire"))
        {
            if (isReadyToShot)
            {
                //RaycastHit rh;
                Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
                /*if (Physics.Raycast(GetComponentInParent<Transform>().transform.position, GetComponent<Transform>().transform.forward, out rh))
                {
                    if (rh.collider != null)
                    {
                        print("BOOM raycast");
                        rh.collider.GetComponent<Plateforme>().SetUsedThenNormalTexture();
                    }
                }*/
                isReadyToShot = false;
                StartCoroutine("isAvailable");
            }
        }
    }

    private IEnumerator isAvailable()
    {
        yield return new WaitForSeconds(Cooldown);
        isReadyToShot = true;
    }
}
