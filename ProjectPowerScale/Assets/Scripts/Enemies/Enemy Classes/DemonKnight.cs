using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonKnight : Enemy
{
    public override void Start() {
        base.Start();
        StartCoroutine(waitToUlt());
    }
    IEnumerator waitToUlt() {
        
        YieldInstruction wait = new WaitForSeconds(AttackDelay);
        while (isActiveAndEnabled) { 
        yield return wait;
        Anim.SetTrigger("Ult");
        }
    }
    public override void StateSwitch() {
        //base.StateSwitch();
    }
    public override void States() {
        Anim.SetFloat("Distance",Distance);
        Anim.SetInteger("Hp",HealthLeft/Health);
    }
}
