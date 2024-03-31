using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SavePointMenu : CanvasManager
{
    public static event UnityAction fullRestore;
    public static event UnityAction<int> switchMapping;
    private void OnEnable() {
        SavePoint.saveGame += CanvasUp;
        
    }
    private void OnDisable() {
        SavePoint.saveGame -= CanvasUp;
    }
    private void CanvasUp() {
        switchMapping(1);
        CanvasControl(true);
        fullRestore();
        ZaraInput.onclose += CancelCanvas;
    }
    public override void CancelCanvas() {
        base.CancelCanvas();
        ZaraInput.onclose -= CancelCanvas;
        switchMapping(0);
    }
}
