using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootmotionConfirmation : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.applyRootMotion = true;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.applyRootMotion = true;
    }
}
