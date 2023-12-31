using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class Interactable : MonoBehaviour
{
    public static event UnityAction<Interactable> sendThis;
    private void OnTriggerEnter(Collider other) {
        //send timeline to playerInputs
        EnterEvent();
        if(sendThis!=null)
            sendThis(this);
    }
    private void OnTriggerStay(Collider other) {
        //Interact();    
    }
    private void OnTriggerExit(Collider other) {
        if (sendThis != null)
            sendThis(null);
        ExitEvent();
    }
    public abstract void Interact();
    public abstract void EnterEvent();
    public abstract void ExitEvent();
}
