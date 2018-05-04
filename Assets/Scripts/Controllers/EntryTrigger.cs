using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntryTrigger : MonoBehaviour {

    
    //private bool showPopUp = false;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Tunnel")
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        Debug.Log(obj.tag);
        if (obj.gameObject.tag == "Tunnel")
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
            transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
        }
    }
}
