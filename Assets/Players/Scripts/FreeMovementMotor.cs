
// To the extent possible under law, 
// Tim Glasser (tim_glasser@hotmail.com)     https://www.facebook.com/tim.glasser.75 
// has waived all copyright and related or neighboring rights and responsibilties to
// FreeMovementMotor c# classes. This work is published from California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice


using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (MovementMotor))]
public class FreeMovementMotor : MovementMotor {

	public float walkingSnappyness = 50.0f;
	public float turningSmoothing = 0.3f;
    private MovementMotor FMMotor;

    public void Start()
    {

        FMMotor = GetComponent<MovementMotor>();
        FMMotor.MovementDirection = Vector3.zero;
        FMMotor.FacingDirection = Vector3.zero;
        FMMotor.MovementVel = Vector3.zero;

    }

	public void FixedUpdate () {
		// Handle the movement of the character
		Vector3 targetVelocity = FMMotor.MovementDirection * FMMotor.MovementVel.magnitude;

		Vector3 deltaVelocity = targetVelocity - GetComponent<Rigidbody>().velocity;

        if (GetComponent<Rigidbody>().useGravity)
			deltaVelocity.y = 0.0f; 
		GetComponent<Rigidbody>().AddForce (deltaVelocity * walkingSnappyness, ForceMode.Acceleration);
		
		// Setup player to face facingDirection, or if that is zero, then the movementDirection
		Vector3 faceDir = FMMotor.FacingDirection;
		if (faceDir == Vector3.zero)
			faceDir = FMMotor.MovementDirection;
		
		// Make the character rotate towards the target rotation
		if (faceDir == Vector3.zero) {
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		else {	
			float rotationAngle  = AngleAroundAxis (transform.forward, faceDir, Vector3.up);
			GetComponent<Rigidbody>().angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
		}
	}
}

