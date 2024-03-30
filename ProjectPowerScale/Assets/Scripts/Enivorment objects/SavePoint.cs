using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SavePoint : Interactable
{
    [SerializeField] private GameObject suggestion;
    public static event UnityAction saveGame;
    public static event UnityAction<bool> enterEvent;
    public override void EnterEvent() {
        if(enterEvent!=null)
            enterEvent(true);
        suggestion.SetActive(true);
    }

    public override void ExitEvent() {
        if (enterEvent != null)
            enterEvent(false);
        suggestion.SetActive(false);
    }

    public override void Interact() {
        if (saveGame != null) {
            saveGame();
        }
        //Send An event to interaction canvas to display the correct obj
    }
}
