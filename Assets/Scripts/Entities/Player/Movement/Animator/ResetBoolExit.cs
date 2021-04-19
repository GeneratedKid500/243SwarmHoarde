using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoolExit : StateMachineBehaviour
{
    public string theBool;
    public bool set;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(theBool, set);
    }
}
