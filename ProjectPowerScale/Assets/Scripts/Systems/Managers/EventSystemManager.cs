using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EventSystemManager : MonoBehaviour
{
    // Start is called before the first frame updatew
    [SerializeField] private EventSystem eventSystem;
    private GameObject lastSelected;
    private void Start() {
        lastSelected = eventSystem.firstSelectedGameObject;
    }
    private void OnEnable() {
        CanvasManager.sendObj += SetPauseCanvasFirstSelected;

    }
    private void OnDisable() {
        CanvasManager.sendObj -= SetPauseCanvasFirstSelected;

    }
    private void SetPauseCanvasFirstSelected(GameObject selectThis) {
        eventSystem.SetSelectedGameObject(null);
        if (eventSystem.currentSelectedGameObject != selectThis)
            eventSystem.SetSelectedGameObject(selectThis);
        lastSelected = selectThis;
    }
    private void OnButtonSelected() {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(lastSelected);
    }
}