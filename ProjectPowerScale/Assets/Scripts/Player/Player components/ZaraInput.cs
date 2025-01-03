using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class ZaraInput : MonoBehaviour
{
    [SerializeField] private GameObject pierceSword;
    [SerializeField] private GameObject heavySwordPincer;
    [SerializeField] private GameObject protectionSwing;
    [SerializeField] private GameObject holdSwordProtection;
    [SerializeField] private GameObject effectsPoint;
    private Animator anim;
    private Player player;
    private PlayerInput map;

    private Interactable interactObj;
    public static event UnityAction onpause;
    public static event UnityAction onclose;
    // Start is called before the first frame update
    private void OnEnable() {
        Interactable.sendThis += RecieveInteractable;
        SavePointMenu.switchMapping += SwitchMaps;
    }
    private void OnDisable() {
        Interactable.sendThis -= RecieveInteractable;
        SavePointMenu.switchMapping -= SwitchMaps;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        map = GetComponent<PlayerInput>();
    }
    #region Gameplay Controls
    private void OnMove(InputValue value) {
        Vector2 move = value.Get<Vector2>();
        anim.SetFloat("XInput", move.x);
        anim.SetFloat("YInput",move.y);
    }
    private void OnSquare() {
        anim.SetTrigger("Attack");
    }
    private void OnHoldAttack() {
        anim.SetTrigger("HoldAttack");
    }
    private void OnTriangle() {
        if (anim.GetFloat("YInput") > 0.5) {
            Instantiate(pierceSword, effectsPoint.transform.position, player.Body.transform.rotation);
        }
        else if (anim.GetFloat("YInput") < 0.5) {
            Instantiate(heavySwordPincer, effectsPoint.transform.position, Quaternion.identity);
        }
        else {
            Instantiate(protectionSwing, effectsPoint.transform.position, Quaternion.identity);
        }      
    }
    private void OnHoldTriangle() {
        Instantiate(holdSwordProtection, effectsPoint.transform.position, player.Body.transform.rotation);
    }
    private void OnCircle() {
        if (interactObj == null)
            anim.Play("Big Bang Attack"); //anim.Play("Parry");
        else {
            interactObj.Interact();
        }
        //maybe make an object that when it dies you teleport to its location
    }
    private void OnJump() {
        if (player.LockedOn&&!player.Flying) {
            print("Locked JUMP");
            
        }
        print("Jump girl");
    }
    private void OnLaunch() {
        //anim.Play("LaunchUp");
        //player.Flying = true;
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
        
    }
    private void OnTeleport() {
        anim.Play("Dash");
    }
    #endregion
    private void RecieveInteractable(Interactable obj) {
        print("interacted");
        interactObj = obj;
    }

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
    #region Menu Controls
    private void OnCancel() {
        onclose();
    }
    #endregion
    private void SwitchMaps(int val) {
        switch (val) {
            case 0:
                map.SwitchCurrentActionMap("DefaultControls");
                break;
            case 1:
                map.SwitchCurrentActionMap("Menu");
                break;
        }
    }
}
