using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Brian Hodges
/// updated 5/15/2018
/// This Activates the brake light material
/// Assisted by Phantom John
/// </summary>

public class brakeLights : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

        IsBraking(); //This checks ever frame for braking. Then activates the brake lights when braking, and doesnt when its not avtive.

    }
    public void IsBraking()// This methood sets up the brake lights.
    {
        if (GetComponentInParent<UnityStandardAssets.Vehicles.Car.CarController>().BrakeInput > 0)// this gets the brake input from the CarController.
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_Intensity", 3.0f);// this activates the material
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_Intensity", 0f);// this deactivates the material.
        }
    }
}
