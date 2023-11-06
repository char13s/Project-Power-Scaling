using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LockOnCam : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    private void Awake() {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    private void OnEnable() {
        Player.lockon += SwitchCam;
    }
    private void OnDisable() {
        Player.lockon -= SwitchCam;
    }
    void Start() {
        
    }
    private void SwitchCam(bool val) {
        if (val) {
            cam.Priority = 100;
        }
        else {
            cam.Priority = 1;
        }
    }
}
