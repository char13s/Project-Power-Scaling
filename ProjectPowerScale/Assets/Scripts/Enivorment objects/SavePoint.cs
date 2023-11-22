using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SavePoint : Interactable
{
    public static event UnityAction saveGame;
    public override void Interact() {
        if (saveGame != null) {
            saveGame();
        }
        //Send An event to interaction canvas to display the correct obj
    }
}
