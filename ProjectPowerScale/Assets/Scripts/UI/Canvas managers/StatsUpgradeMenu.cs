using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsUpgradeMenu : CanvasManager
{
    private Player player;
    [SerializeField] private Button strength;
    [SerializeField] private Button auraControl;
    [SerializeField] private Button numberOfSwords;
    [SerializeField] private Button energyOutput;
    [SerializeField] private Button dmgReduction;
    [SerializeField] private Button expMultipler;

    [SerializeField] private TextMeshProUGUI strLvl;
    [SerializeField] private TextMeshProUGUI aurConLvl;
    [SerializeField] private TextMeshProUGUI numOfSwordLvl;
    [SerializeField] private TextMeshProUGUI energyOutLvl;
    [SerializeField] private TextMeshProUGUI dmgReducLvl;
    [SerializeField] private TextMeshProUGUI expMultiLvl;
    private void OnEnable() {
        

    }
    private void Start() {
        player = Player.GetPlayer();
        CheckWhatsAvailable();
        SetDisplayLevels();
    }
    private void OnDisable() {

    }
    private void ActivateCanvas() {
        CanvasControl(true);
    }
    public void LevelStrength() {
        player.stats.StrLvl++;
        SetDisplayLevels();
    }
    public void LevelAuraControl() {
        player.stats.AurConLvl++;
        SetDisplayLevels();
    }
    public void LevelNumberOfSwords() {
        player.stats.NumSwordsLvl++;
        SetDisplayLevels();
    }
    public void LevelEnergyOutput() {
        player.stats.EnergyAttLvl++;
        SetDisplayLevels();
    }
    public void LevelDmgReduction() {
        player.stats.DmgReductionLvl++;
        SetDisplayLevels();
    }
    public void LevelExpMultipler() {
        player.stats.ExpModLvl++;
        SetDisplayLevels();
    }
    private void CheckWhatsAvailable() {
        strength.interactable = player.stats.IsStrLevelAvailable();
        auraControl.interactable = player.stats.IsAuraConLevelAvailable();
        numberOfSwords.interactable = player.stats.IsNumOfSwordsLevelAvailable();
        energyOutput.interactable = player.stats.IsEnergyAttLevelAvailable();
        dmgReduction.interactable = player.stats.IsDmgReductionLevelAvailable();
        expMultipler.interactable = player.stats.IsExpMultiLevelAvailable();
    }
    private void SetDisplayLevels() {
        strLvl.text = player.stats.StrLvl.ToString();
        aurConLvl.text = player.stats.AurConLvl.ToString(); ;
        numOfSwordLvl.text = player.stats.NumSwordsLvl.ToString();
        energyOutLvl.text = player.stats.EnergyAttLvl.ToString();
        dmgReducLvl.text = player.stats.DmgReductionLvl.ToString();
        expMultiLvl.text = player.stats.ExpModLvl.ToString();
    }
}
