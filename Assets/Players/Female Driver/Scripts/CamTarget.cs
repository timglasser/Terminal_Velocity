using UnityEngine;
using System.Collections;

public class CamTarget : MonoBehaviour 
{

	public Transform target;
	float camSpeed = 50.0f;
	Vector3 lerpPos;

	void Update() 
	{
		lerpPos = (target.position-transform.position)* Time.deltaTime * camSpeed;
		transform.position += lerpPos;
	}
}
