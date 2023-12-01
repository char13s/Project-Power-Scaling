using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
#pragma warning disable 0649
public class EnemyHitBox : MonoBehaviour
{
    private Enemy enemy;
    //[SerializeField] private GameObject effect;
    private bool occured;
    [SerializeField] private bool isProjectile;
    [SerializeField] private int extraDmg;
    public Enemy Enemy { get => enemy; set => enemy = value; }

    public static event UnityAction hit;
    public static event UnityAction guardHit;
    private void OnDisable() {
        occured = false;
    }
    // Start is called before the first frame update
    void Start() {
        //player = Player.GetPlayer();
        if (!isProjectile)
            Enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider other) {

        // Instantiate(effect, transform.position, Quaternion.identity);
        if (other.GetComponent<HurtBox>()) {
            if (Player.GetPlayer().Blocking) {

            }
            else {
                Enemy.CalculateAttack(extraDmg);
            }
        }
        if (other.GetComponent<ParryBox>() && !isProjectile) {
            Enemy.Parry = true;
        }
    }
}
