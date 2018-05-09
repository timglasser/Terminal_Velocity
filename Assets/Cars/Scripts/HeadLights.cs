using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLights : MonoBehaviour {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("l"))//L
        {
            for (int i = 0; i < 2; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (Input.GetKey("k"))//K
        {
            for (int i = 0; i < 2; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
