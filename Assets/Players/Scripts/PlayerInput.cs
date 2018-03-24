using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (MovementMotor))]
[RequireComponent(typeof(PreciseTurnOnSpot))]
public class PlayerInput : MonoBehaviour {


	private MovementMotor motor;
	private Transform character;
	private Rigidbody rb;
	protected Animator avatar;
    public Sender keyDownEvent;
    public Sender keyUpEvent;

/*

	// Use this for initialization
	void Start () 
	{
		if (!character)
			character = transform;

		if (!motor)
			motor = GetComponent<ShooterMovementMotor> ();

		motor.MovementDirection = Vector3.zero;
		motor.FacingDirection = Vector3.zero;

		avatar = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            keyDownEvent.Send(this);
			avatar.SetBool ("Shoot", true);
			avatar.SetLayerWeight(1,1);// this 
    
        }

        else if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            keyUpEvent.Send(this);
			avatar.SetBool ("Shoot", false);
			avatar.SetLayerWeight(1,0);
        }

        Vector3 heading_fore = transform.TransformDirection(Vector3.forward * CrossPlatformInputManager.GetAxis("Vertical"));
        Vector3 heading_right = transform.TransformDirection(Vector3.right * CrossPlatformInputManager.GetAxis("Horizontal"));
		motor.MovementDirection = heading_right + heading_fore;


        //camera.
        Camera.main.GetComponent<PlayerCamera>().Rotation += CrossPlatformInputManager.GetAxis("Horizontal2") ;
        GetComponent<MouseLook>().YRot += CrossPlatformInputManager.GetAxis("Vertical2");

    }
   
    */
  

}
