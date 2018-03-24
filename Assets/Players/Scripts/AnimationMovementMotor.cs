// To the extent possible under law, 
// Tim Glasser (tim_glasser@hotmail.com)     https://www.facebook.com/tim.glasser.75 
// has waived all copyright and related or neighboring rights and responsibilties to
// AnimationMovementMotor c# classes. This work is published from California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementMotor))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class AnimationMovementMotor : MovementMotor
{

    private Rigidbody rb;
    private Animator avatar;
    private MovementMotor FMMotor;

    public float walkingSnappyness = 50.0f;
    public float turningSmoothing = 0.3f;

    // Use this for initialization
    void Start()
    {
        FMMotor = GetComponent<MovementMotor>();
        FMMotor.MovementDirection = Vector3.zero;
        FMMotor.FacingDirection = Vector3.zero;
        avatar = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // transform to local coordinate system
        Vector3 curVel = transform.InverseTransformDirection(FMMotor.MovementVel); 
        avatar.SetFloat("ForwardSpeed", curVel.z);
        avatar.SetFloat("StrafeSpeed", curVel.x);
        avatar.SetFloat("TotalSpeed", (curVel.x * curVel.x) + (curVel.z * curVel.z));
    }

    // Use this for physics operations
    void FixedUpdate()
    {
        Vector3 targetVelocity = FMMotor.MovementDirection * FMMotor.MovementVel.magnitude;
        Vector3 deltaVelocity = targetVelocity - rb.velocity;
        if (rb.useGravity)
            deltaVelocity.y = 0;

        rb.AddForce(deltaVelocity * walkingSnappyness, ForceMode.Acceleration);

        // Setup player to face facingDirection, or if that is zero, then the movementDirection
        Vector3 faceDir = FMMotor.FacingDirection;
        if (faceDir == Vector3.zero)
            faceDir = FMMotor.MovementDirection;

        // Setup player to face facingDirection, or if that is zero, then the movementDirection
      //  Vector3 faceDir = FMMotor.FacingDirection;
        if (faceDir == Vector3.zero)
        {
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            float rotationAngle = AngleAroundAxis(transform.forward, faceDir, Vector3.up);
            rb.angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
         //   avatar.SetFloat("Direction", avatar.rootRotation.eulerAngles.y);
        }
    }
}