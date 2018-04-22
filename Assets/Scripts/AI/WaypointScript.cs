using UnityEngine;
using System.Collections;

public class WaypointScript : MonoBehaviour {
	
	//Script for waypoints. Mainly holds data of what the next waypoint is for the enemy to interpret.
	float radius = 0.5f;

	void OnDrawGizmos () { //Draws a sphere 
		Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
		Gizmos.DrawSphere(transform.position, radius);
	}

	void OnDrawGizmosSelected () { //Draws a  sphere with alternate color when selected.
		Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
		Gizmos.DrawSphere(transform.position, radius);
	}	
}
