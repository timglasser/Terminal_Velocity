using UnityEngine;
using System.Collections;

public class WaypointScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Script for waypoints. Mainly holds data of what the next waypoint is for the enemy to interpret.
	float radius = 0.5f;
	public GameObject waypointTarget;


	void OnDrawGizmos () { //Draws a line to next waypoint and a sphere 
		Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
		Gizmos.DrawSphere(transform.position, radius);
		if (waypointTarget != null)
			Gizmos.DrawLine(transform.position, waypointTarget.transform.position);
	}

	void OnDrawGizmosSelected () { //Draws a line to next waypoint and a sphere with alternate color when selected.
		Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
		Gizmos.DrawSphere(transform.position, radius);
		if (waypointTarget != null)
			Gizmos.DrawLine(transform.position, waypointTarget.transform.position);
	}	
}
