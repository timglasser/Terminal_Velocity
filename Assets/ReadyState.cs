using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyState : StateMachineBehaviour {
     
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        GameObject canvas;
        canvas = GameObject.FindGameObjectWithTag("Canvas");// disable title 
        Transform[] UiObjects;
        // enable game widgets
        UiObjects = canvas.GetComponentsInChildren<Transform>(true);
        

        foreach (Transform ui in UiObjects)
        {
           // Debug.Log("ui element is " + ui.name);
         
            string name = ui.name;
            switch (name)
            {
                case "Title":
                    ui.gameObject.SetActive(false);
                    break;
                case "Start Button":
                    ui.gameObject.SetActive(false);
                    break;
                case "Text":
                    ui.gameObject.SetActive(false);
                    break;
                case "MiniMapCamera":
                    ui.gameObject.SetActive(true);
                    break;
                case "Speedometer":
                    ui.gameObject.SetActive(true);
                    break;
                case "Tachometer":
                    ui.gameObject.SetActive(true);
                    break;
                case "Ready":
                    ui.gameObject.SetActive(true);
                    break;
                case "Credits Scroller":
                    ui.gameObject.SetActive(false);
                    break;
            }

        }
        GameMan.Reset();
   
        Camera.main.GetComponent<LookAtConstraint>().enabled = false;
        Camera.main.GetComponent<ParentChildConstraint>().enabled = false;
        Camera.main.GetComponent<CarCamera>().enabled = true;
        Camera.main.GetComponent<CarCamera>().target = RaceInfo.Instance.players[0].transform; // needs attention for  2 player game

        
    }

	 //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
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
