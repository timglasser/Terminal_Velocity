using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Oil Slick")
        {

        }
    }

    void OnTriggerEnter(Collider obj)
    {
        Debug.Log(obj.tag);
        if (obj.gameObject.tag == "Oil Slick")
        {
            
        }
    }

}
