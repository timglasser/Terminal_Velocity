using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class TriggerSkid : MonoBehaviour {

    public float timeOfSkid = 2.0f;
    private float startTime;
    private CarController skiddingCar;
    public Listener onSkid, offSkid;  // listeners ( user interface and sound )

    public void OnTriggerEnter(Collider obj)
    {
        Debug.Log(obj.gameObject.tag); // the Collider obj is the collision box on the car not the car itself
     
        if (obj.gameObject.tag == "Player")
        {
            // the car controller is in the grandparent of the collision boxes. See scene graph
            skiddingCar = obj.transform.parent.GetComponentInParent<CarController>();
            if (skiddingCar)
            {
                startTime = Time.time;
                // activate skid here (best to use a listener as we may need UI and sound update)
                Debug.Log("skid activated on the " + skiddingCar.name);
                //onSkid.Send(this); // listener- on skid
                StartCoroutine(Wait(timeOfSkid));
            }
        }
    }

    // Use IEnumerator interface when you want somethis to happen over several frames or at a later time. Perform the deactivate after the wait time.
    // Effectively runs in parallel to OnTriggerEnter. 
    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // this method is frozen for for the given time                                                 // deactivate skid here
       // offSkid.Send(this); // listener- off skid
        Debug.Log("skid deactivated after " + (Time.time - startTime) + " seconds");
    }
}
