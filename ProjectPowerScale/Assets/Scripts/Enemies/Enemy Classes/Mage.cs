using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage :Enemy
{
    bool healActive;
    //public static Mage GetMage() => instance;
    public override void Awake() {
        base.Awake();
    }
    public override void OnEnable() {
        base.OnEnable();
    }
    // Start is called before the first frame update
    public override void Start() {
        base.Start();


        StartCoroutine(waitToUlt());
    }
    IEnumerator waitToUlt() {
        //int time = Random.Range(45, 60);
        YieldInstruction wait = new WaitForSeconds(AttackDelay);
        while (isActiveAndEnabled) {
            yield return wait;
            Anim.SetTrigger("Ult");
        }
    }
    public override void States() {
        Anim.SetFloat("Distance",Distance);
        if (HealthLeft < Health * 0.45f&&!healActive) {
            StartCoroutine(waitToHeal());
            healActive = true;
        }
    }
    IEnumerator waitToHeal() {
        YieldInstruction wait = new WaitForSeconds(5);
        while (isActiveAndEnabled && HealthLeft < Health * 0.45f) { 
        yield return wait; 
        Anim.Play("HealCast");}
        healActive = false;
    }
}
