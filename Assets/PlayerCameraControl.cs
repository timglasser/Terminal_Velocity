using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : StateMachineBehaviour {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        /*
        if (stateInfo.IsName("TP") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[0].SetActive(true);
        }
        if (stateInfo.IsName("Seat") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[1].SetActive(true);
        }
        if (stateInfo.IsName("Hood") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[2].SetActive(true);
        }
        if (stateInfo.IsName("Rear") == true)
        {
            animator.GetComponentInParent<CameraMan>().Cameras[3].SetActive(true);
        }
        */
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

 //       animator.SetInteger("Player Cameras", animator.GetComponentInParent<CameraMan>().PlayerCams);
  //      animator.SetInteger("Current Cameras", animator.GetComponentInParent<CameraMan>().CurrentCamerasPar);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (stateInfo.IsName("TP") == true)
        {
            animator.GetComponentInParent<CameraMan>().tempInt = 0;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
        }
        if (stateInfo.IsName("Seat") == true)
        {
            animator.GetComponentInParent<CameraMan>().tempInt = 1;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
        }
        if (stateInfo.IsName("Hood") == true)
        {
            animator.GetComponentInParent<CameraMan>().tempInt = 2;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
        }

        if (stateInfo.IsName("Rear") == true)
        {
            animator.GetComponentInParent<CameraMan>().tempInt = 3;
            animator.GetComponentInParent<CameraMan>().Cameras[animator.GetComponentInParent<CameraMan>().tempInt].SetActive(false);
        }
        */
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
