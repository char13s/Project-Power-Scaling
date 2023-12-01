using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRootControl : StateMachineBehaviour
{
    [Tooltip("When to turn the root motion back on.")]
    [SerializeField] private float start;

    [SerializeField] private bool turnRootBackOn;
    private Enemy enemy;
    // [SerializeField] private float end;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        enemy = animator.gameObject.GetComponent<Enemy>();
        if (!enemy.Timelining)
            animator.applyRootMotion = false;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime > start && turnRootBackOn) {
            animator.applyRootMotion = true;
        }
        else {
            animator.GetComponent<Enemy>().AttackRot();
        }
        Debug.Log("root is off");
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.applyRootMotion = true;
    }
}
