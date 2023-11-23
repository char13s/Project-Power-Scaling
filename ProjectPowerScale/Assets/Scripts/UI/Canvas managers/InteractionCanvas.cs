using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCanvas : CanvasManager
{
    [SerializeField] private GameObject saveIcon;

    private void Start() {
        SavePoint.enterEvent += SaveIconControl;
    }
    private void OnDisable() {
        SavePoint.enterEvent -= SaveIconControl;
    }
    private void SaveIconControl(bool val) {
        saveIcon.SetActive(val);
    }
}
