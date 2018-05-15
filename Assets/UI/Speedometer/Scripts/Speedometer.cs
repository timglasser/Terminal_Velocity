using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Speedometer : MonoBehaviour {

	public Rigidbody vehicle;
	public Text speedometer;
	private float MPH;

	// Use this for initialization
	void Start () {

        // set the speedometer needle object here instead of update
        // this should be here
        //speedometer.text = "Night Ryder";

    }
	
	// Update is called once per frame
	void Update () {
		MPH = vehicle.velocity.magnitude * 2.237f; // what is this magic number?
		speedometer.text = "Night Ryder";
        // do not do a find in Update . Very slow
		transform.Find("SpeedometerNeedle").localEulerAngles = new Vector3(0f, 0f, Mathf.Round(-MPH * 100f / 100f) );
	}
}
