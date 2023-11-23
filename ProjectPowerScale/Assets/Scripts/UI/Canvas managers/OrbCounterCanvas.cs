using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OrbCounterCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.sendOrbs += CountOrb;
    }
    private void OnDisable() {
        GameManager.sendOrbs -= CountOrb;
    }
    private void CountOrb(int val) {
        text.text = ": "+val.ToString();
    }
}
