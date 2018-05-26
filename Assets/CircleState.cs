using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleState : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Camera.main.GetComponent<LookAtConstraint>().enabled = false;
        Camera.main.GetComponent<ParentChildConstraint>().enabled = false;
        Camera.main.GetComponent<CarCamera>().enabled = false;
        Camera.main.GetComponent<CameraCircle>().enabled = true;
        Camera.main.GetComponent<CameraCircle >().target = RaceInfo.RaceOrder[0].transform; // focus on the winner
        Camera.main.transform.position = RaceInfo.RaceOrder[0].transform.position + new Vector3(3.0f, 3.0f, 0.0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      //  Camera.main.transform.position = RaceInfo.RaceOrder[0].transform.position + new Vector3(6.0f, 3.0f, 0.0f);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Camera.main.GetComponent<CameraCircle>().enabled = false;
        GameMan.Reset();
        GameMan.Enable();
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
