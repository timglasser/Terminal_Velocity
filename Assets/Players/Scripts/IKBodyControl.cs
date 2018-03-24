using UnityEngine;
using System.Collections;

public class IKBodyControl : MonoBehaviour {

    Animator anim;
    public float ikWeight;

   // public Transform leftHandIKTarget;
   // public Transform rightHandIKTarget;

    public Transform hintLeft;
    public Transform hintRight;
    public GameObject target;

    Vector3 lFpos;
    Vector3 rFpos;
    Quaternion lFrot, rFrot;

    float lFweight, rFweight;

    Transform leftFoot, rightFoot;

    public float offsetY;

    public float lookIKWeight;
    public float bodyWeight;
    public float headWeight;
    public float eyesWeight;
    public float clampWeight;

    public Transform lhIKTarget, rhIKTarget;

    Vector3 rightHandPos;
    float rightHandWeight;

    public Transform pointer;
    public Transform lookPos;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);

        lFrot = leftFoot.rotation;
        rFrot = rightFoot.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        /*
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 15);

        lookPos.position = ray.GetPoint(15);
        */
        /*
        code for raising right hand
        RaycastHit rightHand;
        if (Physics.Raycast(pointer.position , pointer.right, out rightHand, 1))
        {
            anim.SetBool("Raise", true);
        }
        else
        {
            anim.SetBool("Raise", false);
        }
        */
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 lpos = leftFoot.TransformPoint(Vector3.zero);
        Vector3 rpos = rightFoot.TransformPoint(Vector3.zero);

        if (Physics.Raycast(lpos, -Vector3.up, out leftHit, 1))
        {
            lFpos = leftHit.point;
            lFrot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }

        if (Physics.Raycast(rpos, -Vector3.up, out rightHit, 1))
        {
            rFpos = rightHit.point;
            rFrot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }
    }

    void OnAnimatorIK()
    {
        anim.SetLookAtWeight(lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
        anim.SetLookAtPosition(lookPos.position);


        lFweight = anim.GetFloat("LeftFoot");
        rFweight = anim.GetFloat("RightFoot");

        // positions

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lFweight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rFweight);

        anim.SetIKPosition(AvatarIKGoal.LeftFoot, lFpos + new Vector3(0.0f, offsetY,0.0f));
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rFpos + new Vector3(0.0f, offsetY, 0.0f));

        // Leg hints
        anim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, ikWeight);
        anim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, ikWeight);

        anim.SetIKHintPosition(AvatarIKHint.LeftKnee, hintLeft.position);
        anim.SetIKHintPosition(AvatarIKHint.RightKnee, hintRight.position);

        // rotations
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, lFweight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rFweight);

        anim.SetIKRotation(AvatarIKGoal.LeftFoot, lFrot);
        anim.SetIKRotation(AvatarIKGoal.RightFoot, rFrot);

        /* Hand Inverse Kinematics

        float lHweight = anim.GetFloat("LeftHand");
        float rHweight = anim.GetFloat("RightHand");

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, lHweight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rHweight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, lhIKTarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand,rhIKTarget.position);


        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, lHweight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rHweight);

        anim.SetIKRotation(AvatarIKGoal.LeftHand, lFrot);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rFrot);
        */
    }
}
