
//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights and responsibilties to
//Unity 5.1 MotorBike C# Classes. - ported from the7347@hotmail.com
//This work is published from:
//Oakland, California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice
using UnityEngine;
using System.Collections;

public class ParentChildConstraint: MonoBehaviour {
	
	public Transform ConstrainTo;
	public bool pos=false;
	public bool rot=false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Late Update is called after all animation etc.
	void LateUpdate () {
		if( pos){
			transform.position=ConstrainTo.position;
		}
		if( rot){
			
			transform.rotation=ConstrainTo.rotation;
		}		
	}
}
