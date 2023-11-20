using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image fillRef;
    [SerializeField] private Slider defSlider;
    [SerializeField] private TextMeshProUGUI powerLeveltext;
    private void Update() {
        FacePlayer();
    }
    private void FacePlayer() { 
        Vector3 direction =  transform.position- Camera.main.transform.position;
        Quaternion qTo;
        qTo = Quaternion.LookRotation(direction);
        transform.rotation = qTo;
    }
    public void SetEnemyHealth() {
        
        if (hpSlider.value < (hpSlider.maxValue / 4)) {
            
            fillRef.color = Color.yellow;
        }

    }
    public void SetPowerLevel(int power) {
        powerLeveltext.text = "BP: "+power.ToString();
    }
    //public void SetDefMeter() { 
    //
    //}
}
