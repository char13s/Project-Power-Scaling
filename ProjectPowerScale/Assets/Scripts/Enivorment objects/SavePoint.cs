using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SavePoint : Interactable
{
    public static event UnityAction saveGame;
    public static event UnityAction<bool> enterEvent;
    public override void EnterEvent() {
        if(enterEvent!=null)
            enterEvent(true);
    }

    public override void ExitEvent() {
        if (enterEvent != null)
            enterEvent(false);
    }

    public override void Interact() {
        if (saveGame != null) {
            saveGame();
        }
        //Send An event to interaction canvas to display the correct obj
    }
}
