using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonState : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // set up the camera constraints
        // Camera.main.GetComponent<ParentChildConstraint>().ConstrainTo = animator.GetComponentInParent<CameraMan>().GetLocator(6);// above
        //  Camera.main.GetComponent<LookAtConstraint>().LookAt = animator.GetComponentInParent<CameraMan>().GetLookAt(0);// above
        Camera.main.GetComponent<ParentChildConstraint>().ConstrainTo = animator.GetComponentInParent<CameraMan>().GetLocator(7);// above
        Camera.main.GetComponent<LookAtConstraint>().LookAt = animator.GetComponentInParent<CameraMan>().GetLookAt(1);// above

        Camera.main.GetComponent<CarCamera>().enabled = false;
        Camera.main.GetComponent<LookAtConstraint>().enabled = false;
        Camera.main.GetComponent<ParentChildConstraint>().enabled = true;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger("InGameCameras", animator.GetComponentInParent<CameraMan>().IGC());
        switch (animator.GetInteger("InGameCameras"))
        {
            case 0:
                animator.SetTrigger("Camera Up");
                animator.ResetTrigger("Camera Down");
                break;
            case 1:
                animator.SetTrigger("Camera Down");
                animator.ResetTrigger("Camera Up");
                break;
            case 2:
                animator.SetTrigger("Camera Down");
                animator.ResetTrigger("Camera Up");
                break;

        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
