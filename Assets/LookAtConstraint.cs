using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights to
//Unity LooAtConstraint C# Class 
//This work is published from:
//California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice
public class LookAtConstraint : MonoBehaviour {
    public Transform LookAt;
    private Transform OrientTo;
    private Vector3 rot;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;// head
    }

    void LateUpdate()
    {
        Quaternion lookRotation = Quaternion.LookRotation(LookAt.position - transform.position);
        transform.rotation = lookRotation * initialRotation;
    }


}
