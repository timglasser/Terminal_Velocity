﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TimAI
{
    public class MoveState : StateMachineBehaviour
    {
      

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // save last target pos
        }

    }
}
