using UnityEngine;
using System.Collections;

public class PreciseTurnOnSpot : MonoBehaviour {


	protected Animator animator;
	public float targetTurn = 100;
	bool doTurn = false;
    Transform leftFoot, rightFoot;


    Quaternion targetRotation;

    Quaternion lFrot, rFrot;


    void Start()
	{
		animator = GetComponent<Animator>();

        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

        lFrot = leftFoot.rotation;
        rFrot = rightFoot.rotation;
    }
    /*
	void OnGUI()
	{

		GUILayout.Label("Simple example to get precise turn on spot while keeping animation as intact as possible");
		GUILayout.Label("Uses a 'Turn On Spot' BlendTree (in Turn state) in conjunction with Mecanim's MatchTarget call");		
		GUILayout.Label("Details in PreciseTurnOnSpot.cs");


		GUILayout.BeginHorizontal();
		targetTurn = GUILayout.HorizontalSlider(targetTurn, -180, 180);
		GUILayout.Label(targetTurn.ToString());		

		if(GUILayout.Button("Do Turn"))
		{
			doTurn = true;
		}
		GUILayout.EndHorizontal();
		
	}
	*/
	void Update()
	{

   

        lFrot = leftFoot.rotation;

        //if angle between root and foot direction (planted) is greater than 45 then turn
        float diffAngle = (rFrot.eulerAngles.y)- (transform.rotation.eulerAngles.y);
        Debug.Log("diff angle " + diffAngle);
        Debug.Log("character angle " + transform.rotation.eulerAngles.y);
        Debug.Log("left foot  angle " + lFrot.eulerAngles.y);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
		{
           

            if ( diffAngle > targetTurn || diffAngle < -targetTurn) // just triggered
			{

                animator.SetBool("Turn", true);
                animator.SetFloat("Direction", diffAngle);
                targetRotation = transform.rotation * Quaternion.AngleAxis(diffAngle, Vector3.up); // Compute target rotation when doTurn is triggered
				
			}
     
		}
		else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Turn"))
		{
			// calls MatchTarget when in Turn state, subsequent calls are ignored until targetTime (0.9f) is reached .
			animator.MatchTarget(Vector3.one, targetRotation, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.zero, 1), animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 0.9f);
            animator.SetBool("Turn", false);
        }

	}
}
