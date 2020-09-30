using System.Collections;
using UnityEngine;

public class Plateforme : MonoBehaviour
{
    public Texture2D usedTexture, normalTexture;
    // Start is called before the first frame update
    public void SetUsedThenNormalTexture()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", usedTexture);
        StartCoroutine("SetNormalTexture");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            GetComponent<Renderer>().material.SetTexture("_MainTex", usedTexture);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            StartCoroutine("SetNormalTexture");           
        }
    }

    private IEnumerator SetNormalTexture()
    {
        yield return new WaitForSeconds(10);
        GetComponent<Renderer>().material.SetTexture("_MainTex", normalTexture);
    }
}
