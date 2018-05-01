//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights and responsibilties to
//Unity PhysX3.3 MotorBike C# Classes -  from the7347@hotmail.com
//This work is published from:
//Oakland, California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice

using System;
using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof (Rigidbody))]
public class MotorBike : MonoBehaviour
{
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider mCollider;
        public Transform mMesh;
        public bool motor;
        public bool steering;
    }

    public AxleInfo[] axleInfos = new AxleInfo[2]; // the information about each individual axle
	public float MaxTorque; // maximum torque the motor can apply to wheel
	public float MaxSteeringAngle; // maximum steer angle the wheel can have
	public float AngularDrag = 6.0f;//agility of the bike
    private float BodyTorque; // torque applied to the axis of the bike
    private float Torque; // torque applied to wheel
	private float SteeringAngle; // steer angle applied to wheel
    private float wheelxrot;

    // Attributes
    public Transform CoM; // center of mass
	private float CurSpeed ; 

	public int[] Gears = {15, 30, 60, 120, 240, 350, 400}; // (7 gears)
	private int CurGear  = 0; 
	

	public float resetTime = 5.0f;
	private float resetTimer = 0.0f;
    private  Quaternion Frotation;
    public float forkangle = 66.0f;
    public Transform UpperFork ;

    // finds the corresponding visual wheel
    // correctly applies the transform
    private void ApplyLocalPositionToFWheelVisual(AxleInfo axle)
    {
        Transform visualWheel = axle.mMesh;
        Vector3 position;

        axle.mCollider.GetWorldPose(out position, out Frotation);
        visualWheel.position = position;

        // spin the front wheel according to relative velocity
        float zVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z;
        if (zVel > 0.0f)
        {
           // Debug.Log("forward " + Torque);
            wheelxrot += -3.0f * CurSpeed;
            axle.mMesh.localRotation = UpperFork.localRotation * Quaternion.AngleAxis(wheelxrot, Vector3.left);
        }
        else {
           // Debug.Log("back " + Torque);
            wheelxrot += 3.0f * CurSpeed;
            axle.mMesh.localRotation = UpperFork.localRotation * Quaternion.AngleAxis(wheelxrot, Vector3.left);
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    private void ApplyLocalPositionToRWheelVisual(AxleInfo axle)
    {
        Transform visualWheel = axle.mMesh;

        Vector3 position;
        Quaternion rotation;
    
        axle.mCollider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
      
    }

    void OnGUI ()  {

    	GUI.Label(new Rect(20, 20, 300, 300), "Speed: " + CurSpeed);
    	GUI.Label(new Rect(20, 47, 300, 300),  "Torque: " + Torque);  
    	GUI.Label(new Rect(20, 74, 300, 300), "SteerAngle: " + SteeringAngle);
    	GUI.Label(new Rect(20, 101, 300, 300), "AngularDrag: " + GetComponent<Rigidbody>().angularDrag);
    	
	} 
	
	void Start (){
		// adjust the center of mass
		GetComponent<Rigidbody>().centerOfMass = new Vector3(CoM.localPosition.x, CoM.localPosition.y, CoM.localPosition.z);
		GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
    }
	
	void FixedUpdate (){

	 	// assume the front wheel is the first and the steering wheel
		axleInfos[0].mCollider.steerAngle = SteeringAngle;

		// modify the angular drag  on the steering wheel to stabilise the bike
		if (CurSpeed > 50 && axleInfos[0].mCollider.isGrounded){
			GetComponent<Rigidbody>().angularDrag = 8.0f;
		}	
		else if (CurSpeed > 50 && !axleInfos[0].mCollider.isGrounded){
			//WaitForSeconds(1.5f);
			GetComponent<Rigidbody>().angularDrag = 3.0f;
		}
	
		axleInfos[1].mCollider.motorTorque = Torque;
        /*
                // if both wheels are grounded
                if (axleInfos[0].mCollider.isGrounded && axleInfos[1].mCollider.isGrounded){
                    Debug.Log ("Grounded");
                    if ( Vector3.Angle( Vector3.up, transform.up ) > 45.0f){

                        GetComponent<Rigidbody>().AddRelativeTorque (Vector3.forward * 50000.0f * Input.GetAxis("Horizontal"));
                    }
                    else {  
                        GetComponent<Rigidbody>().angularDrag = AngularDrag;  
                    }	 
                }  
                // apply the forces when the bike is in the air
                else {	       
                    if (Input.GetKey (KeyCode.W)){   	
                        GetComponent<Rigidbody>().AddRelativeTorque (Vector3.right * 150000.0f * Input.GetAxis("Vertical"));
                    }
                    if (Input.GetKey (KeyCode.S)){  		
                        GetComponent<Rigidbody>().AddRelativeTorque (Vector3.left * -150000.0f * Input.GetAxis("Vertical"));
                    }   
                    if (Input.GetKey (KeyCode.D)) {		
                        GetComponent<Rigidbody>().AddRelativeTorque (Vector3.back * 55000.0f * Input.GetAxis("Horizontal")); 		       
                    }
                    if (Input.GetKey (KeyCode.A)){		
                        GetComponent<Rigidbody>().AddRelativeTorque (Vector3.forward * -55000.0f * Input.GetAxis("Horizontal")); 		
                    }	    	
                }  
        */
        ApplyLocalPositionToFWheelVisual(axleInfos[0]);
        ApplyLocalPositionToRWheelVisual(axleInfos[1]);
	}
	
	void Update () {
		
		SteeringAngle = MaxSteeringAngle * Input.GetAxis ("Horizontal");	// steering is the left/right key
		Torque = MaxTorque * Input.GetAxis ("Vertical");        // throttle is the up/down key

        // at higher speeds the bike turns by leaning rather than steering
        CurSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        if (CurSpeed > 30.0f){
			MaxSteeringAngle = 8.0f; 
		}	
		if (CurSpeed > 60.0f){
			MaxSteeringAngle = 4.0f;
		}	
		if (CurSpeed > 100.0f){
			MaxSteeringAngle = 2.0f;
		}	
		if (CurSpeed > 150.0f){
			MaxSteeringAngle = 1.0f;
		}						
		if (CurSpeed < 30.0f){
			MaxSteeringAngle = 30.0f;
		}		
	}

    void LateUpdate()
    {
        UpperFork.localRotation = Quaternion.Euler(forkangle, 0.0f, -SteeringAngle);
        /*
               UpperFork.localRotation = Quaternion.Euler(forkangle, 0.0f, -SteeringAngle);
               float wheelxrot;// = Frotation.eulerAngles.x;

               wheelxrot = Frotation.eulerAngles.x;

               Debug.Log("front rot = " + wheelxrot);
               axleInfos[0].mMesh.localRotation = UpperFork.localRotation * Quaternion.AngleAxis(wheelxrot, Vector3.right);
       */
    }

    void Check_If_Bike_Is_Flipped()
	{
        if (transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 280)
            resetTimer += Time.deltaTime;
        else
            resetTimer = 0;
		
		if(resetTimer > resetTime)
			FlipBike();
	}
	
	void FlipBike()
	{
		transform.rotation = Quaternion.LookRotation(transform.forward);
		transform.position += Vector3.up * 0.5f;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		resetTimer = 0;
		Torque = 0;
	}
}


