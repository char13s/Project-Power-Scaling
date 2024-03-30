using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ParryBox : MonoBehaviour
{
    [SerializeField] private GameObject parrySound;
    public static event UnityAction parried;
    private void OnTriggerEnter(Collider other) {
        EnemyHitBox enemyHitBox;
        if (enemyHitBox=other.GetComponent<EnemyHitBox>()) {
            enemyHitBox.Enemy.Parry=true;
            if (parried != null) {
                parried();
            }
            if (parrySound != null) {
                Instantiate(parrySound);
            }
        }
    }
}
