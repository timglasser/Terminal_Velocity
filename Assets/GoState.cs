using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoState : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetTrigger("GameStart");
     

        // free the rigid body of all the participants
        GameMan.Enable();
        GameObject canvas;
        canvas = GameObject.FindGameObjectWithTag("Canvas");// disable title 
        Transform[] UiObjects;
        // enable game widgets
        UiObjects = canvas.GetComponentsInChildren<Transform>(true);


        foreach (Transform ui in UiObjects)
        {
            //Debug.Log("ui element is " + ui.name);

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
                case "Ready":
                    ui.gameObject.SetActive(false);
                    break;
                case "Set":
                    ui.gameObject.SetActive(false);
                    break;
                case "GO":
                    ui.gameObject.SetActive(true);
                    break;
            }

        }
        //GameMan.Reset();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

       
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
