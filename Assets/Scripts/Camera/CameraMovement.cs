//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights to
//Unity CameraMovement C# Classes.
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice

using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

//This script takes care of moving the main camera with the movement keys.

	//bounds to constrain the camera.
	public float minBoundsX;
	public float minBoundsZ;
	public float maxBoundsX;
	public float maxBoundsZ ;
	public float minHeight;
	public float maxHeight ;

	public float height =6.0f;
	public float speed =8.0f;
	public float springiness; //How loose or strong the camera gets closer to the dummy position.

	public Transform targetPos; //The dummy position for the camera to follow.
	private float square  = 1.0f; //A compensate value to maintain equal speed when moving diagonally.
	private Vector3 _targpos;
	void Start () {
		Debug.LogWarning("Target point set " + transform.name);
		_targpos = targetPos.transform.position;
		transform.position=_targpos; //Set dummy position to camera to start.
		height=6.0f;
	}

	void LateUpdate () {

		//Debug.LogWarning("Target point set " + transform.name);
		//If we go diagonal, multiply speed by the inverse of the square root of 2 to maintain equal speed.
		// (1^2 + 1^2 = 2), simple pythagoreum theorum.
		_targpos = targetPos.position;
	
		if( Input.GetAxis("Horizontal") != 0.0f && Input.GetAxis("Vertical") != 0.0f ){
			// this value is unchanged and should be calculated once in Start
			square = 1/Mathf.Sqrt(2.0f);
		} else {
			square = 1.0f;
		}
	
	
		//If the dummy is not going to go past bounds and a certain button is pressed, you can move.
		if (Input.GetAxis("Horizontal") != 0.0f && maxBoundsX >= _targpos.x + (Input.GetAxis("Horizontal") * speed * Time.deltaTime) && _targpos.x + (Input.GetAxis("Horizontal") * speed * Time.deltaTime) >= minBoundsX) {
			_targpos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime * square;
		}
		_targpos.y = height;// height should be constant
		
		if (Input.GetAxis("Vertical") != 0.0f  && maxBoundsZ >= _targpos.z + (Input.GetAxis("Vertical") * speed * Time.deltaTime) && _targpos.z + (Input.GetAxis("Vertical") * speed * Time.deltaTime) >= minBoundsZ ) {
			_targpos.z += Input.GetAxis("Vertical") * speed * Time.deltaTime * square;
		}

		transform.position = Vector3.Lerp(transform.position,_targpos,Time.deltaTime * springiness); //Move the camera closer to dummy target using delta time as the lerp value.
	//	transform.position.y = height;

	}
}