using UnityEngine;
using System.Collections;

public class ConstraintScript : MonoBehaviour {
	
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
