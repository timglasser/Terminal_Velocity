using UnityEngine;
using System.Collections;

public class CameraFollowCS : MonoBehaviour {


/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.

There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/

	// The target we are following
	public Transform target;
	
	// what we are looking at
	public Transform lookat;
	
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	
	public float height = 5.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	// Place the script in the Camera-Control group in the component menu
	//@script AddComponentMenu("Camera-Control/Smooth Follow")

	// Use this for initialization
	void Start () {
	
	}
	
	/* Update is called once per frame
	void LateUpdate () {
		// Early out if we don't have a target
		if (!target)
			return;
	
		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
	
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0.0f, currentRotationAngle, 0.0f);
	
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		gameObject.transform.position = target.position ;	// Set the height of the camera
		gameObject.transform.position -= currentRotation * Vector3.forward * distance;
		gameObject.transform.position += Vector3.up*currentHeight;	
		// Always look at the target
		gameObject.transform.LookAt (target);	
		//return;
	}
	*/
	// Update is called once per frame
	void LateUpdate () {
		// Early out if we don't have a target
		if (!target)
			return;
	
		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
	
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
	
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		gameObject.transform.position = target.position;
		gameObject.transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		gameObject.transform.position +=  new Vector3(0.0f,wantedHeight,0.0f);
	
		// Always look at the target
		gameObject.transform.LookAt (lookat);	
		//return;
	}	
}

