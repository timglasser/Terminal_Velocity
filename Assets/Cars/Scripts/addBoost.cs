using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class addBoost : MonoBehaviour {

    // Use this for initialization

    
    private CarController boostCar;

    public void OnTriggerEnter(Collider obj)
    {
        //Debug.Log(obj.gameObject.tag);

        if (obj.gameObject.tag == "Player")
        {
            boostCar = obj.transform.parent.GetComponentInParent<CarController>();
            boostCar.addBoost();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
