using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Speedometer : MonoBehaviour {

	public GameObject vehicle;
	public Text speedometer;
	private float MPH;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		MPH = vehicle.GetComponent<Rigidbody>().velocity.magnitude * 2.237f;
		speedometer.text = "Name: RYDER";

		transform.Find("SpeedometerNeedle").localEulerAngles = new Vector3(0f, 0f, Mathf.Round(-MPH * 100f / 100f) );
	}
}
