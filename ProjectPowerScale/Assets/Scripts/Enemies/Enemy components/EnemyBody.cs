using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBody : MonoBehaviour
{
    [SerializeField] private Enemy body;
    [SerializeField] private Material bodyMat;
    [SerializeField] private Material attMat;
    [SerializeField] private GameObject hitSound;
    [SerializeField] private SkinnedMeshRenderer mesh;
    public Enemy Body { get => body; set => body = value; }
    public static event UnityAction<float> force;
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<HitBox>()) {
            if(mesh!=null)
                Attacked(other);
            if (!Body.Parry&&hitSound!=null)
                Instantiate(hitSound);
            Body.CalculateDamage(other.GetComponent<HitBox>().AdditionalDamage);
        }
        if (!Body.Grounded) {
            if (Body.Anim != null)
                Body.Anim.SetTrigger("AirHit");
            Body.OnHit();
        }
        else {
            print("hit was herrrrr");
            if(Body.Anim!=null)
                Body.Anim.Play("Hurt");
            Body.OnHit();
        }
    }
    private void Attacked(Collider other) {
        mesh.material = attMat;
        StartCoroutine(waitToReset());
    }
    IEnumerator waitToReset() {
        YieldInstruction wait = new WaitForSeconds(1);
        yield return wait;
        mesh.material = bodyMat;
    }
}
