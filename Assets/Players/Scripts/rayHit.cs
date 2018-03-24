using UnityEngine;
using System.Collections;

// The raycast code / revive code.

public class rayHit : MonoBehaviour
{
	float impactEndTime=0;
	Rigidbody impactTarget=null;
	Vector3 impact;
	
	void Update () { 
    /*
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			if (Physics.Raycast(ray,out hit))
			{
				if (hit.rigidbody!=null)
				{
                    ragdollActivation helper = GetComponent<ragdollActivation>();
					helper.ragdolled=true;
					impactTarget = hit.rigidbody;
					impact = ray.direction * 2.0f;
					impactEndTime=Time.time+0.25f;
				}
			}
		}
*/
		if (Input.GetKeyDown(KeyCode.Space))
		{
            ragdollActivation helper = GetComponent<ragdollActivation>();
			helper.ragdolled=false;
		}	
/*		
		if (Time.time<impactEndTime)
		{
			impactTarget.AddForce(impact,ForceMode.VelocityChange);
		}
        */
	}
}
