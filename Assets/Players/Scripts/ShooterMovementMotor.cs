using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementMotor))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class ShooterMovementMotor : MovementMotor
{
    //  private MovementMotor motor;
    private Rigidbody rb;
    private Animator avatar;

    public float forwardSpeed = 5.0f;
    public float sidewaysSpeed = 3.0f;
    public float walkingSnappyness = 50.0f;
    public float turningSmoothing = 0.3f;

    // Use this for initialization
    void Start()
    {
     
        movementDirection = Vector3.zero;
        facingDirection = Vector3.zero;
        avatar = GetComponent<Animator>();
    }

    void Update()
    {
        // transform to local coordinate system
        Vector3 curVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
 //       Debug.Log("curvel " + curVel);

        // Vector3 curVel = transform.InverseTransformDirection(motor.movementDirection); // for shooters
        avatar.SetFloat("ForwardSpeed", curVel.z);
        avatar.SetFloat("StrafeSpeed", curVel.x);
        avatar.SetFloat("TotalSpeed", (curVel.x * curVel.x) + (curVel.z * curVel.z));
    }

    // Use this for physics operations
    void FixedUpdate()
    {
        Vector3 targetVelocity = movementDirection * forwardSpeed;
        Vector3 deltaVelocity = targetVelocity - GetComponent<Rigidbody>().velocity;
        if (GetComponent<Rigidbody>().useGravity)
            deltaVelocity.y = 0;


        GetComponent<Rigidbody>().AddForce(deltaVelocity * walkingSnappyness, ForceMode.Acceleration);

        // Setup player to face facingDirection, or if that is zero, then the movementDirection
        Vector3 faceDir = facingDirection;
        if (faceDir == Vector3.zero)
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            float rotationAngle = AngleAroundAxis(transform.forward, faceDir, Vector3.up);
            GetComponent<Rigidbody>().angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
            avatar.SetFloat("Direction", avatar.rootRotation.eulerAngles.y);
        }
    }
}