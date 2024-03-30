using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SavePointMenu : CanvasManager
{
    public static event UnityAction fullRestore;
    private void OnEnable() {
        SavePoint.saveGame += CanvasUp;

    }
    private void OnDisable() {
        SavePoint.saveGame -= CanvasUp;
    }
    private void CanvasUp() {
        CanvasControl(true);
        fullRestore();
    }

}
