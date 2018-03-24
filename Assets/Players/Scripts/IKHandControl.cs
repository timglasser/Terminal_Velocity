using UnityEngine;
using System.Collections;

public class IKHandControl : MonoBehaviour {


    protected Animator animator;
    public GameObject targetA = null;
    public GameObject leftHandle = null;
    public GameObject rightHandle = null;
  //  public GameObject weapon = null;

    private bool load = false;

 

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void OnAnimatorIK(int layerIndex)
    {
        float aim = 0.5f;// animator.GetFloat("Aim");

        // solve lookat and update bazooka transform on first il layer
        if (layerIndex == 0)
        {
            if (targetA != null)
            {
                Vector3 target = targetA.transform.position;

                target.y = target.y + 0.2f * (target - animator.rootPosition).magnitude;

                animator.SetLookAtPosition(target);
                animator.SetLookAtWeight(aim, 0.5f, 0.5f, 0.0f, 0.5f);
/*
                if (weapon != null)
                {
                    float fire = animator.GetFloat("Fire");
                    Vector3 pos = new Vector3(0.195f, -0.0557f, -0.155f);
                    Vector3 scale = new Vector3(0.2f, 0.8f, 0.2f);
                    pos.x -= fire * 0.2f;
                  //  scale = scale * aim;
                   // weapon.transform.localScale = scale;
                    weapon.transform.localPosition = pos;
                }
 */

            }
        }

        // solve hands holding on second ik layer
        if (layerIndex == 1)
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
