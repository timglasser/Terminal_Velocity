using UnityEngine;
using System.Collections;

public class Rider : MonoBehaviour {

	// use the rigid body of the bike to set the animation states of the rider
	public GameObject rb;
	private Animator avatar;

	// layer 2 is the steering and shooting layers

	// lean is created by mixing/lerping in the lean and the rb/bike angles
	void Start () {
		//Cache the animation controller
		// set layer weights
		avatar = GetComponent<Animator> ();
		avatar.SetLayerWeight(1,0);

		// cache the steering transform
	}
	
	// Update is called once per frame
	void Update () {
		// get speed from rigid body
		// transform to local coordinate system to control state engine
		Vector3 curVel = transform.InverseTransformDirection(rb.GetComponent<Rigidbody>().velocity);
		//avatar.SetFloat ("run", curVel.z);
		
//		Debug.Log( "local velocity " + curVel);
		transform.position = rb.transform.position;
		//avatar.SetFloat ("strafe", curVel.x);
		avatar.SetFloat ("speed", (curVel.x + curVel.z)*(curVel.x + curVel.z));//, DirectionDampTime, Time.deltaTime);

	
	}
}
