using UnityEngine;
using System.Collections;

// this class is used to plant a foot on the ground
// from current foot position and orientation read from animator IK goals, a series of raycasts and hit positions are used to find foot planted position and orientation
public class FootPlanting
{
	private Animator m_Animator = null;
	private int m_LayerMask = -1;
	
	// two points floor contact model for the foot: back and front of the foot 
	private float m_BackFootOffset;
	private float m_FrontFootOffset;
	
	// foot planting won't fix over a certain error which would create absurd extreme pose
	private float m_MaxFootPositionDelta = 0.15f; // cm
	private float m_MaxFootRotationDelta = 15.0f; // degrees
	
	public FootPlanting (Animator animator, float backFootOffset, float frontFootOffset, LayerMask mask)
	{
		m_Animator = animator;
		m_BackFootOffset = backFootOffset;
		m_FrontFootOffset = frontFootOffset;
		m_LayerMask = mask;
	}
	
	// use physic raycast to project foot contact points on the ground (mesh collider)
	Vector3 ProjectPositionOnGround(Vector3 position)
	{
		Vector3 ret = position;
		
		float humanScale = m_Animator.humanScale;
		
		RaycastHit hitInfo = new RaycastHit ();
		
		// raycast down with a starting point half a human scale up (human scale corersponds to mass center height) for a maximum distance of a human scale down 
		if (Physics.Raycast (position + new Vector3 (0, 0.5f * humanScale, 0), new Vector3 (0, -1, 0), out hitInfo, 1.0f * humanScale, m_LayerMask)) 
		{
			ret = hitInfo.point;
		}
		
		return ret;
	}
	
	// plant left or right foot
	float FootPlant (bool left, float plantWeight)
	{
		AvatarIKGoal ikGoal = left ? AvatarIKGoal.LeftFoot : AvatarIKGoal.RightFoot;
		float footBottomHeight = left ? m_Animator.leftFeetBottomHeight : m_Animator.rightFeetBottomHeight;
		
		Vector3 footPosition = m_Animator.GetIKPosition(ikGoal);
		Quaternion footRotation = m_Animator.GetIKRotation(ikGoal);
		
		// the foot planting algo uses a 2 contact points foot model. One at the bottom front and one at the bottom back
		Vector3 footBackOffset = new Vector3 (0, -footBottomHeight, m_BackFootOffset);
		Vector3 footFrontOffset = new Vector3 (0, -footBottomHeight, m_FrontFootOffset);
		
		Vector3 footBackPosition = footPosition + (footRotation * footBackOffset);
		Vector3 footFrontPosition = footPosition + (footRotation * footFrontOffset);
		
		// raycast both contact points with the ground
		Vector3 footBackPositionHit = ProjectPositionOnGround(footBackPosition);
		Vector3 footFrontPositionHit = ProjectPositionOnGround(footFrontPosition);
		
		Vector3 footDir = footFrontPosition - footBackPosition;
		Vector3 footDirHit = footFrontPositionHit - footBackPositionHit;
		
		Quaternion deltaRotation = Quaternion.FromToRotation (footDir, footDirHit);
		
		// weight the delta rotation by scaling imaginary part of the quaternion with plant weight
		deltaRotation.x *= plantWeight;
		deltaRotation.y *= plantWeight;
		deltaRotation.z *= plantWeight;
		
		// compute the new orientation of the feet
		// for a plant weight of 0 the foot rotation remains un changed
		footRotation = deltaRotation * footRotation;
		
		// clamp the deltaRotation so foot does not get into an extreme orientation
		float angle;
		Vector3 axis;
		deltaRotation.ToAngleAxis (out angle, out axis);
		angle = Mathf.Clamp (angle, -m_MaxFootRotationDelta, m_MaxFootRotationDelta);
		deltaRotation = Quaternion.AngleAxis (angle, axis);
		
		footBackPosition = footPosition + (footRotation * footBackOffset);
		footFrontPosition = footPosition + (footRotation * footFrontOffset);
		Vector3 footCenterPosition = (footBackPosition + footFrontPosition) * 0.5f;
		
		footBackPositionHit = ProjectPositionOnGround(footBackPosition);
		footFrontPositionHit = ProjectPositionOnGround(footFrontPosition);
		Vector3 footCenterPositionHit = ProjectPositionOnGround(footCenterPosition);
		
		// compute difference in y between current foot position and hit position
		float footBackDelta = footBackPositionHit.y - footBackPosition.y;
		float footFrontDelta = footFrontPositionHit.y - footFrontPosition.y;
		float footCenterDelta = footCenterPositionHit.y - footCenterPosition.y;
		
		// if difference is greater than 0 then contact is below ground and we do a hard fix
		// else then perform a soft fix using plantWeight
		footBackDelta = footBackDelta > 0 ? footBackDelta : footBackDelta * plantWeight; 
		footFrontDelta = footFrontDelta > 0 ? footFrontDelta : footFrontDelta * plantWeight; 
		footCenterDelta = footCenterDelta > 0 ? footCenterDelta : footCenterDelta * plantWeight; 
		
		// here we pick the worst case, max positive delta. It guaranties that back, center and front foot stays above the ground
		float footDelta = Mathf.Max(footCenterDelta,Mathf.Max (footBackDelta, footFrontDelta));
		
		footDelta = Mathf.Clamp (footDelta, -m_MaxFootPositionDelta * m_Animator.humanScale, m_MaxFootPositionDelta * m_Animator.humanScale);
		
		// adjust feet position with worst case deltaq
		footPosition.y += footDelta;
		
		m_Animator.SetIKPosition(ikGoal,footPosition);
		m_Animator.SetIKRotation(ikGoal,footRotation);
		
		m_Animator.SetIKPositionWeight(ikGoal,1);
		m_Animator.SetIKRotationWeight(ikGoal,1);
		
		return footDelta;
	}
	
	// plant both feet and adjust body postion 
	public void FeetPlant (float leftFootPlantWeight, float rightFootPlantWeight)
	{
		float leftDelta = FootPlant (true,leftFootPlantWeight);
		float rightDelta = FootPlant (false,rightFootPlantWeight);
		
		// move mass center down with worst case foot delta to prevent ik goal to be out of reach and have a leg at maximum extension
		Vector3 bodyPosition = m_Animator.bodyPosition;
		bodyPosition.y += Mathf.Min (leftDelta, rightDelta);
		m_Animator.bodyPosition = bodyPosition;
	}	
}