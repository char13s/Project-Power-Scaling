using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractionCanvas : CanvasManager
{
    
    [SerializeField] private GameObject saveIcon;
    private void OnEnable() {
        GameManager.save += SaveIconControl;
    }
    private void OnDisable() {
        GameManager.save -= SaveIconControl;
    }
    private void SaveIconControl() {
        saveIcon.SetActive(true);
    }
}
