using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class ZaraInput : MonoBehaviour
{
    private Animator anim;
    private Player player;

    public static event UnityAction onpause;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    #region
    private void OnMove(InputValue value) {
        Vector2 move = value.Get<Vector2>();
        anim.SetFloat("XInput", move.x);
        anim.SetFloat("YInput",move.y);
    }
    private void OnSquare() {
        anim.SetTrigger("Attack");
    }
    private void OnTriangle() {
        anim.SetTrigger("StrongAttack");
    }
    private void OnCircle() {
        anim.SetTrigger("Teleport");
        //maybe make an object that when it dies you teleport to its location
    }
    private void OnJump() {
        if (player.LockedOn) {
            print("Locked JUMP");
            anim.SetTrigger("Jump");
        }
        print("Jump girl");
    }
    private void OnPause() {
        if (onpause != null) {
            onpause();
        }
    }
    private void OnLockOn(InputValue value) {
        if (value.isPressed) {
            player.LockedOn = true;
        }
        else {
            player.LockedOn = false;
        }
    }
    private void OnSkills(InputValue value) {
        if (value.isPressed) {
            player.SkillsUp = true;
        }
        else {
            player.SkillsUp = false;
        }
    }
    private void OnParry() {
        anim.Play("Parry");
    }
    private void OnTeleport() {
        anim.Play("Dash");
    }
    #endregion
    #region Transformation slots
    private void OnDpadUp() {
        //Maybe transformation slots
    }
    private void OnDpadDown() {

    }
    private void OnDpadRight() {

    }
    private void OnDpadLeft() {

    }
    #endregion
}
