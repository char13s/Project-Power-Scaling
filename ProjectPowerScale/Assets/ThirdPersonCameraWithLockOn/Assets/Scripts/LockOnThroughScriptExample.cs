using UnityEngine;
using System.Collections;
using ThirdPersonCameraWithLockOn;
using UnityEngine.InputSystem;

public class LockOnThroughScriptExample : MonoBehaviour
{

    private ThirdPersonCamera camScript;
    Player zend;
    [SerializeField] private GameObject lockOnObject;
    private void OnEnable() {
        PlayerLockOn.switchTarget += OnChangeTarget;
    }
    private void OnDisable() {
        PlayerLockOn.switchTarget -= OnChangeTarget;
    }
    // Use this for initialization
    void Start() {
        zend = GetComponent<Player>();
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCamera>();
        //StartCoroutine("ToggleLockOn");
    }
    void OnLockOn(InputValue value) {
        if (value.isPressed) {//
            camScript.InitiateLockOn(lockOnObject);
        }
        else
            camScript.ExitLockOn();
    }
    void OnChangeTarget(GameObject target) {
        camScript.NewTarget(target);
    }
    IEnumerator ToggleLockOn() {
        //while (true)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("Initiating lock on");
            camScript.InitiateLockOn(lockOnObject);
            yield return new WaitForSeconds(2f);
            Debug.Log("Exiting lock on");
            camScript.ExitLockOn();

        }

    }
}
