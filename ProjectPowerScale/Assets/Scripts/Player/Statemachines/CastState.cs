using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastState : StateMachineBehaviour
{
    [SerializeField] private GameObject projectile;
    private GameObject blastPoint;
    [SerializeField] private GameObject startingEffects;
    [SerializeField] private float shootHere;
    bool shot;
    [SerializeField] bool rightHand;
    private GameObject effectHolder;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (rightHand)
            blastPoint = animator.GetComponent<Player>().RightHand;
        else {
            blastPoint = animator.GetComponent<Player>().LeftHand;
        }
        shot = false;
        if (startingEffects) {
            effectHolder=Instantiate(startingEffects, blastPoint.transform);
            //effectHolder.transform.SetParent(null);
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime >= shootHere && !shot) {
            Fire();
        }
    }
    void Fire() {
        Instantiate(projectile, blastPoint.transform.position, blastPoint.transform.rotation);
        shot = true;
        effectHolder.SetActive(false);
        //projectile.transform.position = animator.rootPosition;
    }
}
