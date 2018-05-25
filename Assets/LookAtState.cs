using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtState : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // set camera to overhead
        // Transform _Locator = GameObject.FindWithTag("Player").transform;

        // set up the camera constraints
        Camera.main.GetComponent<ParentChildConstraint>().ConstrainTo = animator.GetComponentInParent<CameraMan>().GetLocator(0);// above
        Camera.main.GetComponent<LookAtConstraint>().LookAt = animator.GetComponentInParent<CameraMan>().GetLookAt(0);// above

        Camera.main.GetComponent<LookAtConstraint>().enabled = false;
        Camera.main.GetComponent<ParentChildConstraint>().enabled = false;
        Camera.main.GetComponent<CarCamera>().enabled = true;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
