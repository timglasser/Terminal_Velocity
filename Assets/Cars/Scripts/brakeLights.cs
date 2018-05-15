using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brakeLights : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //gameObject.GetComponent<CarController>().
       //gameObject.GetComponentInParent.tag("tailights").setActive(false);

	}
	
	// Update is called once per frame
	void Update () {

        
	}
    public void IsBraking(bool braking)
    {
        if(braking)
        gameObject.GetComponent<Renderer>().material.SetFloat("_Intensity", 3.0f);
        else
        gameObject.GetComponent<Renderer>().material.SetFloat("_Intensity", 0f);
    }
}
