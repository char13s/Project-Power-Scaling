using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveIcon : MonoBehaviour
{
    private void OnEnable() {
        StartCoroutine(waitToDisable());
    }

    IEnumerator waitToDisable() {
        YieldInstruction wait = new WaitForSeconds(5);
        yield return wait;
        gameObject.SetActive(false);
    }
}
