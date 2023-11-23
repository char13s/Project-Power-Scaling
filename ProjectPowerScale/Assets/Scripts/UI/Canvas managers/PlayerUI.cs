using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI battlePowerDisplay;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthDisplay;
    [SerializeField] private Slider mpBar;
    [SerializeField] private TextMeshProUGUI mpDisplay;
    // Start is called before the first frame update
    void Start() {
        Stats.onBaseStatsUpdate += UpdateUI;
    }
    private void UpdateUI() {
        healthBar.value = Player.GetPlayer().stats.HealthLeft;
        healthBar.maxValue = Player.GetPlayer().stats.Health;
        mpBar.value = Player.GetPlayer().stats.MPLeft;
        mpBar.maxValue = Player.GetPlayer().stats.MP;
        battlePowerDisplay.text = "BP. " + Player.GetPlayer().stats.Attack.ToString();
        healthDisplay.text = Player.GetPlayer().stats.HealthLeft.ToString() + "/" + Player.GetPlayer().stats.Health.ToString();
        mpDisplay.text = Player.GetPlayer().stats.MPLeft.ToString() + "/" + Player.GetPlayer().stats.MP.ToString();
    }
}
