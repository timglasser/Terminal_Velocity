using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderHeadControl : MonoBehaviour {
    public Transform LookAt;
    public Transform OrientTo;
    private Vector3 rot;

 //   var headTransform : Transform;
//var target : Transform;
private Quaternion initialRotation ;
 
    void Start()
    {
        initialRotation = transform.rotation;// head
    }

     void LateUpdate()
    {
        Quaternion lookRotation = Quaternion.LookRotation(LookAt.position - transform.position);
        transform.rotation = lookRotation * initialRotation;
    }

 

    /* Late Update is called after all animation etc.
    void LateUpdate()
    {
        //Quaternion.LookRotation(transform.forward);
        //transform.LookAt(LookAt, Vector3.back);

        Quaternion lookRotation = Quaternion.LookRotation(target.position - headTransform.position);
        headTransform.rotation = lookRotation * initialRotation;

    }
    */
}
