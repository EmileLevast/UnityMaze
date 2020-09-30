using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimension : MonoBehaviour
{
	public static Dimension instance=null;
	public int sizeCell=1;
    // Start is called before the first frame update
	void Awake(){
		if(instance==null){
			instance = this;
		}else if(instance!=this){
			Destroy(gameObject);
		}
	}
	
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
