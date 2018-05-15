using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights to
//Unity RiderHeadControl C# Class (modified from an old JS Unity Demo).
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice


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
