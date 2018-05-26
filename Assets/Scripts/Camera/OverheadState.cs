using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadState : StateMachineBehaviour {

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // set camera to overhead

        Transform[] UiObjects;
        GameObject canvas;

        // set up the camera constraints
        Camera.main.GetComponent<ParentChildConstraint>().ConstrainTo = animator.GetComponentInParent<CameraMan>().GetLocator(6);// above
        Camera.main.GetComponent<LookAtConstraint>().LookAt = animator.GetComponentInParent<CameraMan>().GetLookAt(0);// above
        Camera.main.GetComponent<CarCamera>().enabled = false;

        canvas = GameObject.FindGameObjectWithTag("Canvas");// disable title 

        // enable game widgets
        UiObjects = canvas.GetComponentsInChildren<Transform>(true);

        foreach (Transform ui in UiObjects)
        {
            Debug.Log("ui element is " + ui.name);

            string name = ui.name;
            switch (name)
            {
                case "Title":
                    ui.gameObject.SetActive(true);
                    break;
                case "Start Button":
                    ui.gameObject.SetActive(true);
                    break;
                case "Text":
                    ui.gameObject.SetActive(true);
                    break;
                case "MiniMapCamera":
                    ui.gameObject.SetActive(false);
                    break;
                case "Speedometer":
                    ui.gameObject.SetActive(false);
                    break;
               
            }
        }
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
