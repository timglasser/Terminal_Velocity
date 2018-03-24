using UnityEngine;
using System.Collections;

public class setKinematic : MonoBehaviour 
{
	void SetKinematic(bool newValue)
	{
		Rigidbody[] bodies=GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic=newValue;
		}
	}
	void Start ()
    {
		SetKinematic(true);
	}
	
	void Update () 
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SetKinematic(false);
			GetComponent<Animator>().enabled=false;
		}
	}
}
