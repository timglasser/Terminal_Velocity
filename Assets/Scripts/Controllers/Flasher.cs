using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flasher : MonoBehaviour {
    public float timeOf= 2.0f;
	// Use this for initialization
	void Start () {
        // activate skid here (best to use a listener as we may need UI and sound update)
        Debug.Log("timer activated on the " + gameObject.name);
        //onSkid.Send(this); // listener- on skid
        StartCoroutine(Wait(timeOf));

    }
	
	
    // Use IEnumerator interface when you want somethis to happen over several frames or at a later time. Perform the deactivate after the wait time.
    // Effectively runs in parallel to OnTriggerEnter. 
    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // this method is frozen for for the given time                                                 
                                                   // deactivate skid here
                                                   // offSkid.Send(this); // listener- off skid

        Debug.Log("button deactivated after " + (Time.time) + " seconds");
    }
}
