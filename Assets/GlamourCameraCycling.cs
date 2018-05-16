using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlamourCameraCycling : StateMachineBehaviour {
    int a =0;
    int tI;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        if (stateInfo.IsName("Scenic") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[4].SetActive(true);
        }
        if (stateInfo.IsName("Ai Profile") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[5].SetActive(true);
        }
        if (stateInfo.IsName("Ai Follow") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[6].SetActive(true);
        }
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        a++;
        animator.SetInteger("Glamour Camera Cycle", a);
        animator.SetInteger("Current Cameras", animator.GetComponentInParent<CameraMan>().CurrentCamerasPar);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        a = 0;
        animator.SetInteger("Glamour Camera Cycle", a);
        if (stateInfo.IsName("Scenic") == true)
        {
            //animator.GetComponentInParent<CameraMan>().Cameras[8] = animator.GetComponentInParent<CameraMan>().Cameras[0];
            animator.GetComponentInParent<CameraMan>().tempInt = 4;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
            // animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().testInt].

        }
        if (stateInfo.IsName("Ai Profile") == true)
        {
            //            animator.GetComponentInParent<CameraMan>().Cameras[8] = animator.GetComponentInParent<CameraMan>().Cameras[1]; 
            animator.GetComponentInParent<CameraMan>().tempInt = 5;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
        }
        if (stateInfo.IsName("Ai Follow") == true)
        {
            //            animator.GetComponentInParent<CameraMan>().Cameras[8] = animator.GetComponentInParent<CameraMan>().Cameras[1]; 
            animator.GetComponentInParent<CameraMan>().tempInt = 6;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //  }

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //  }
}
