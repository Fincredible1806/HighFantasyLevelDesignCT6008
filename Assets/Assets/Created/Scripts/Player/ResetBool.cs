using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string interactingBool;
    public bool interactStatus;

    public string usingRootMotionBool;
    public bool usingRootMotionStatus;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(interactingBool, interactStatus);
        animator.SetBool(usingRootMotionBool, usingRootMotionStatus);
    }
}
