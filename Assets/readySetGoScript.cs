using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readySetGoScript : MonoBehaviour {
    public GameObject readyObject;
    public GameObject setObject;
    public GameObject goObject;
    public int time;

	// Use this for initialization
	void Start () {
        time = 0;
        //readyObject.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {
        time++;
        //if (time < 50)
        {

        }
        // after 1 second intervals thes ready obeject shouldd be set to deactive and the set object should be set to active
        //else if (time >= 50 && time <= 125)
        {
            //readyObject.SetActive(false);
            //setObject.SetActive(true);
        }
        // after 2 seconds the set ogject should be set to deactive and the go object should be activated
       // else if (time >= 125 && time <= 200)
        {
           // setObject.SetActive(false);
           // goObject.SetActive(true);
        }
        //finally the go object should be deactivated and the actuall object itself should be set to deactive
        //else
        {
            //readyObject.SetActive(false);
           // setObject.SetActive(false);
           // goObject.SetActive(false);
        }
       
	}
}
