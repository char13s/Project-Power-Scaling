using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CanvasManager : MonoBehaviour
{
    [SerializeField] protected GameObject canvas;
    [SerializeField] private GameObject firstOnMenu;
    public static event UnityAction<GameObject> sendObj;
    // Start is called before the first frame update

    public virtual void Start() {
        
    }
    private void OnEnable() {
        GameManager.pauseScreen += KillMenus;
    }
    private void OnDisable() {
        GameManager.pauseScreen -= KillMenus;
    }
    public virtual void CanvasControl(bool val) {
        canvas.SetActive(val);
        if (val == true) {
            if(sendObj!=null&&firstOnMenu!=null)
                sendObj.Invoke(firstOnMenu);
            AssignButtons();
        }
        else {
            UnAssignButtons();
        }
    }
    private void KillMenus(bool val) {
        if (GameManager.GetManager().CurrentState == GameManager.GameState.PlayMode) {
            CancelCanvas();
        }
    }
    public virtual void CancelCanvas() {
        CanvasControl(false);
    }
    public virtual void AssignCircle() {
        //close this Canvas and go back to main pause menu

    }
    public virtual void AssignButtons() { 

    }
    public virtual void UnAssignButtons() {

    }
}
