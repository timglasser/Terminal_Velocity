using UnityEngine;
using System.Collections;

// This structure is used to compute position, velocity and acceleration of a body part
public struct Kinematics
{
	public Vector3 m_Position;
	public Vector3 m_Velocity;
	public Vector3 m_Acceleration;
	
	public bool m_FirstEval;
	public Vector3 m_MaxVelocity;
	public Vector3 m_MaxAcceleration;
	
	public void Init()
	{
		m_Acceleration = Vector3.zero;
		m_Velocity = Vector3.zero;
		m_Position = Vector3.zero;
		
		m_FirstEval = true;
		
		m_MaxVelocity = Vector3.zero;
		m_MaxAcceleration = Vector3.zero;
	}
	
	// update position, velocity and acceleration
	public void UpdatePosition(Vector3 position, float deltaTime)
	{
		Vector3 velocity = (position - m_Position) / deltaTime;
		m_Acceleration = (velocity - m_Velocity) / deltaTime;
		m_Velocity = velocity;
		m_Position = position;
	}
	
	// this is used to smooth animation change induced by foot planting
	// update position, velocity and acceleration with clamped velocity and acceleration
	// velocity and acceleration potentially modified by foot planting
	// original velocity and acceleration are used as clamp values so that dynamic of motion is respected, but noise is cleaned out
	// a minimum clamp velocity and acceleration is used to ensure damped position will stabilize to desired position
	public void UpdatePositionClamp(Vector3 position, Vector3 maxVelocity, Vector3 maxAcceleration, float deltaTime)
	{
		if (m_FirstEval) {
			m_FirstEval = false;
			m_Position = position;
		} else {
			
			float maxVelocityY = 2.0f * Mathf.Abs(maxVelocity.y);
			float maxVelocityXZ = 2.0f * Mathf.Sqrt(maxVelocity.x*maxVelocity.x+maxVelocity.z*maxVelocity.z);
			
			float maxAccelerationY = 2.0f * Mathf.Abs(maxAcceleration.y);
			float maxAccelerationXZ = 2.0f * Mathf.Sqrt(maxAcceleration.x*maxAcceleration.x+maxAcceleration.z*maxAcceleration.z);
			
			maxVelocity = Vector3.Max(m_MaxVelocity,new Vector3(maxVelocityXZ,maxVelocityY,maxVelocityXZ));
			maxAcceleration = Vector3.Max(m_MaxAcceleration,new Vector3(maxAccelerationXZ,maxAccelerationY,maxAccelerationXZ));
			
			Vector3 velocity = (position - m_Position) / deltaTime;
			m_Acceleration = (velocity - m_Velocity) / deltaTime;
			
			m_Acceleration = Vector3.Min (m_Acceleration, maxAcceleration);
			m_Acceleration = Vector3.Max (m_Acceleration, -maxAcceleration);
			
			m_Velocity += m_Acceleration * deltaTime;
			
			m_Velocity = Vector3.Min (m_Velocity, maxVelocity);
			m_Velocity = Vector3.Max (m_Velocity, -maxVelocity);
			
			m_Position += m_Velocity * deltaTime;
		}
	}
}

// this structure is used to stabilize feet for foot planting
// stabilisation is need to prevent foot planting to switch between different hit points when planted. ex: a foot is close to the edge of a stair 
public struct FootStabilizer
{
	public float m_Weight; 			// stabilization weight that ramps between 1 (stable) and 0 (moving)
	public bool m_Stabilized;		// true when the foot is stabilized
	public bool m_Stabilizing; 		// true when the is changing state to stabilized. Use to trigger foot planting events
	public float m_ReleaseDuration;	// the time allowed to recover from stabilized foot position and rotation to current foot position and rotation
	public float m_ReleaseTimer;	// the timer used to recover from stabilization. m_ReleaseTimer / m_ReleaseDuration is used to lerp between stabilized and current position and rotation
	public Vector3 m_Position;		// the stabilized foot position
	private Quaternion m_Rotation;	// the stabilized foot rotation
	
	public void Init()
	{
		m_Weight = 0;
		m_Stabilized = false;
		m_Stabilizing = false;
		m_ReleaseDuration = 0.25f;
		m_ReleaseTimer = 0;
		m_Position = Vector3.zero;
		m_Rotation = Quaternion.identity ;
	}
	
	// for high speeds the stabilzed weight is computed as the relative stability of both feet
	float ComputeStabilizeWeight(float footSpeed, float sumSpeed, float speedMax)
	{
		float movingWeight = footSpeed / Mathf.Max(sumSpeed,speedMax);
		return Mathf.Clamp01(1.3f-movingWeight);
	}
	
	// this update the stabilized state of a foot (weight, stabilized, stabilizing), but does not modify the position or rotation
	public void Update(float speed, float sumSpeed, float maxSpeed, float deltaTime, Quaternion rot)
	{
		m_ReleaseTimer -= deltaTime;
		m_ReleaseTimer = Mathf.Max (m_ReleaseTimer, 0);
		
		m_Weight = ComputeStabilizeWeight (speed, sumSpeed, maxSpeed);
		
		//float deltaAngle = Mathf.Abs(Quaternion.Angle(rot,m_Rotation));
		
		m_Stabilizing = false;
		
		if (m_Stabilized) {
			
			if(m_Weight < 0.5f)// || deltaAngle > 20.0f)
			{
				m_Stabilized = false;
				m_ReleaseTimer = m_ReleaseDuration;
			}
		} else {
			
			if(m_Weight >= 1)// && deltaAngle < 15.0f && m_ReleaseTimer <= 0) 
			{
				m_Stabilizing = true;
				m_Stabilized = true;
			}
		}
	}
	
	// this is used to stabilize and recover from stabilization for foot position and orientation
	public void Stabilize(ref Vector3 position, ref Quaternion rotation, float weight)
	{
		float releaseWeight = m_Stabilized ? weight : weight * m_ReleaseTimer / m_ReleaseDuration;
		
		m_Weight = Mathf.Lerp (m_Weight, 1, releaseWeight);
		m_Position = position = Vector3.Lerp(position,m_Position,releaseWeight);
		m_Rotation = rotation = Quaternion.Lerp(rotation,m_Rotation,releaseWeight);
	}
}

// this class compute body and feet postion, velocity and acceleration
// it smooths body position modified by foot plantng adjustments
// it stabilizes feet 
public class AnimatorKinematics
{
	private Animator m_Animator = null;
	
	// original body and feet position, velocity and acceleration
	private Kinematics m_BodyKinematics = new Kinematics();
	public Kinematics m_LeftFootKinematics = new Kinematics();
	public Kinematics m_RightFootKinematics = new Kinematics();
	
	// smoothed body position
	private Kinematics m_BodyKinematicsSmooth = new Kinematics();
	
	// feet stabilizers
	public FootStabilizer m_LeftFootStabilizer = new FootStabilizer();
	public FootStabilizer m_RightFootStabilizer = new FootStabilizer();
	
	public AnimatorKinematics (Animator animator)
	{
		m_Animator = animator;
		
		m_BodyKinematics.Init();
		m_LeftFootKinematics.Init();
		m_RightFootKinematics.Init();
		
		m_BodyKinematicsSmooth.Init();
		m_BodyKinematicsSmooth.m_MaxVelocity = new Vector3 (0.5f,0.5f,0.5f);
		m_BodyKinematicsSmooth.m_MaxAcceleration = new Vector3 (5.0f,5.0f,5.0f);
		
		m_LeftFootStabilizer.Init();
		m_RightFootStabilizer.Init();
	}
	
	// updates body and feet kinematics and foot stabilizer
	public void Update(float deltaTime)
	{
		m_BodyKinematics.UpdatePosition(m_Animator.bodyPosition,deltaTime);
		m_LeftFootKinematics.UpdatePosition(m_Animator.GetIKPosition (AvatarIKGoal.LeftFoot),deltaTime);
		m_RightFootKinematics.UpdatePosition(m_Animator.GetIKPosition (AvatarIKGoal.RightFoot),deltaTime);
		
		float leftFootSpeed = m_LeftFootKinematics.m_Velocity.magnitude;
		float rightFootSpeed = m_RightFootKinematics.m_Velocity.magnitude;
		
		float sumSpeed = leftFootSpeed + rightFootSpeed;
		
		m_LeftFootStabilizer.Update (leftFootSpeed, sumSpeed, m_Animator.humanScale, deltaTime,m_Animator.GetIKRotation (AvatarIKGoal.LeftFoot));
		m_RightFootStabilizer.Update (rightFootSpeed, sumSpeed, m_Animator.humanScale, deltaTime,m_Animator.GetIKRotation (AvatarIKGoal.RightFoot));
	}
	
	// stabilizes animator feet IK goals position and rotation
	public void StabilizeFeet()
	{
		float bodySpeed = m_BodyKinematics.m_Velocity.magnitude;
		float weight = bodySpeed < m_Animator.humanScale ? 1 : Mathf.Clamp01 (2.0f - bodySpeed / m_Animator.humanScale);
		
		Vector3 leftFootPosition = m_Animator.GetIKPosition (AvatarIKGoal.LeftFoot);
		Quaternion leftFootRotation = m_Animator.GetIKRotation (AvatarIKGoal.LeftFoot);
		
		m_LeftFootStabilizer.Stabilize ( ref leftFootPosition, ref leftFootRotation, weight);
		
		m_Animator.SetIKPosition (AvatarIKGoal.LeftFoot, leftFootPosition);
		m_Animator.SetIKRotation (AvatarIKGoal.LeftFoot, leftFootRotation);
		
		Vector3 rightFootPosition = m_Animator.GetIKPosition (AvatarIKGoal.RightFoot);
		Quaternion rightFootRotation = m_Animator.GetIKRotation (AvatarIKGoal.RightFoot);
		
		m_RightFootStabilizer.Stabilize (ref rightFootPosition, ref rightFootRotation, weight);
		
		m_Animator.SetIKPosition (AvatarIKGoal.RightFoot, rightFootPosition);
		m_Animator.SetIKRotation (AvatarIKGoal.RightFoot, rightFootRotation);
	}	
	
	// smooths animator body position 
	public void SmoothBody(float deltaTime)
	{	
		m_BodyKinematicsSmooth.UpdatePositionClamp(m_Animator.bodyPosition,m_BodyKinematics.m_Velocity,m_BodyKinematics.m_Acceleration,deltaTime);
		m_Animator.bodyPosition = m_BodyKinematicsSmooth.m_Position;
	}	
}