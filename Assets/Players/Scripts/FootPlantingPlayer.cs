using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]  

// this class implements the whole feet planting solution for a specific animator
public class FootPlantingPlayer : MonoBehaviour
{
	private Animator m_Animator = null;
	
	// body and feet kinematics (position, velocity and acceleration), feet stabilization and body postion smoothing
	private AnimatorKinematics m_AnimatorKinematics = null;

	// plant feets on the ground (uncomment lines 15, 41, 57, 59 and 61 to enable foot planting, it's currently experimental).
	private FootPlanting m_FootPlanting = null;
	
	// make sure this matches humanoid feet size
	public float m_BackFootOffset = -0.0275f;
	public float m_FrontFootOffset = +0.200f;
	
	// play steps sound
	public System.Action OnLeftFootPlantAction = null;
	public System.Action OnRightFootPlantAction = null;

	public bool useFootStepSound = true;
	[HideInInspector]public bool useBodyPositionDamping;
	public LayerMask footFallSoundsLayerMask;

	// The different surfaces and their sounds.
	public AudioSurface[] surfaces =                    
	{
		new AudioSurface ("Carpet"), new AudioSurface ("Glass"),
		new AudioSurface ("matMetal"), new AudioSurface ("MetalLight"),
		new AudioSurface ("Rubber")
	};

	void Start ()
	{
		m_Animator = GetComponent<Animator> ();
		m_AnimatorKinematics = new AnimatorKinematics(m_Animator);
		m_FootPlanting = new FootPlanting (m_Animator, m_BackFootOffset, m_FrontFootOffset, ~m_Animator.gameObject.layer);
		OnLeftFootPlantAction += OnLeftFootPlant;
		OnRightFootPlantAction += OnRightFootPlant;
	}
	
	void OnAnimatorIK (int layerIndex)
	{
		if (layerIndex == 0) 
		{
			float deltaTime = Time.deltaTime;
			
			// update animator kinematics
			m_AnimatorKinematics.Update(deltaTime);
		
			// This is commented out because it is experimental, uncomment it at your own risk!
			// stabilize feet based on kinematics
			m_AnimatorKinematics.StabilizeFeet();
			// foot plant from stable feet position
			m_FootPlanting.FeetPlant(m_AnimatorKinematics.m_LeftFootStabilizer.m_Weight,m_AnimatorKinematics.m_RightFootStabilizer.m_Weight);
			// smooth body position modification induced by foot planting
			m_AnimatorKinematics.SmoothBody(deltaTime);
			
			// foot plant actions for left and right
			if (m_AnimatorKinematics.m_LeftFootStabilizer.m_Stabilizing && OnLeftFootPlantAction != null)
				OnLeftFootPlantAction ();
			if (m_AnimatorKinematics.m_RightFootStabilizer.m_Stabilizing && OnRightFootPlantAction != null)
				OnRightFootPlantAction ();
		}
	}
	
	// get foot bottom position
	Vector3 GetFootPosition(bool left)
	{
		AvatarIKGoal ikGoal = left ? AvatarIKGoal.LeftFoot : AvatarIKGoal.RightFoot;
		float footBottomHeight = left ? m_Animator.leftFeetBottomHeight : m_Animator.rightFeetBottomHeight;
		
		Vector3 footPos = m_Animator.GetIKPosition(ikGoal);
		Quaternion footRot = m_Animator.GetIKRotation(ikGoal);
		
		footPos += footRot * new Vector3(0,-footBottomHeight,0);
		
		return footPos;
	}
	
	void OnLeftFootPlant()
	{
		PlayFootFallSound(true);
	}
	
	void OnRightFootPlant()
	{
		PlayFootFallSound(false);
	}
	
	void PlayFootFallSound(bool left)
	{
		Vector3 position = GetFootPosition(left);
		
		if (!useFootStepSound)
			return;
		
		RaycastHit hit;
		if (!Physics.Raycast(position + Vector3.up, -Vector3.up, out hit, 1.5f, footFallSoundsLayerMask))
			return;
		
		for (int i = 0; i < surfaces.Length; i++)
		{
			if (surfaces[i].tag == hit.collider.tag)
			{
				surfaces[i].PlayRandomClip();
			}
		}
	}
}

