using UnityEngine;
using System.Collections;

public class IKHandControl : MonoBehaviour {


    protected Animator animator;
    public Transform targetA = null;
    public Transform leftHandle = null;
    public Transform rightHandle = null;

    public float lookIKWeight = 1.0f;
    public float bodyWeight = 1.0f;
    public float headWeight = 1.0f;
    public float eyesWeight = 1.0f;
    public float clampWeight = 1.0f;

    float aimIKWeight = 1.0f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void SetAimIK()
    {
        Vector3 pos = targetA.position;
        animator.SetLookAtPosition(pos);
        BlendAimIK(1.0f);
    }

    void BlendAimIK(float targetWeight)
    {

        if (Mathf.Approximately(targetWeight, aimIKWeight))
        {
            aimIKWeight = targetWeight;
        }
        else aimIKWeight = Mathf.Lerp(aimIKWeight, targetWeight, Time.deltaTime * 10.0f);

        //Body Head Eye Weight
        animator.SetLookAtWeight(aimIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
    }

    void OnAnimatorIK(int layerIndex)
    {
        // solve first ik layer
        if (layerIndex == 0)
        {
            if (targetA != null)
            {
                Vector3 target = targetA.transform.position;

                target.y = target.y + 0.2f * (target - animator.rootPosition).magnitude;

                animator.SetLookAtPosition(target);
                animator.SetLookAtWeight(aimIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);

            }
        }

        // solve hands holding on second ik layer
        if (layerIndex == 0)
        {
            if (leftHandle != null)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.transform.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.transform.rotation);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            }

            if (rightHandle != null)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandle.transform.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandle.transform.rotation);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            }
        }
    }
}
