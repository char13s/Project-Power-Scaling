using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsUpgradeMenu : CanvasManager
{
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
        CheckWhatsAvailable();
        SetDisplayLevels();
    }
    public void LevelStrength() {
        Player.GetPlayer().stats.StrLvl++;
        SetDisplayLevels();
    }
    public void LevelAuraControl() {
        Player.GetPlayer().stats.AurConLvl++;
        SetDisplayLevels();
    }
    public void LevelNumberOfSwords() {
        Player.GetPlayer().stats.NumSwordsLvl++;
        SetDisplayLevels();
    }
    public void LevelEnergyOutput() {
        Player.GetPlayer().stats.EnergyAttLvl++;
        SetDisplayLevels();
    }
    public void LevelDmgReduction() {
        Player.GetPlayer().stats.DmgReductionLvl++;
        SetDisplayLevels();
    }
    public void LevelExpMultipler() {
        Player.GetPlayer().stats.ExpModLvl++;
        SetDisplayLevels();
    }
    private void CheckWhatsAvailable() {
        strength.interactable = Player.GetPlayer().stats.IsStrLevelAvailable();
        auraControl.interactable = Player.GetPlayer().stats.IsAuraConLevelAvailable();
        numberOfSwords.interactable = Player.GetPlayer().stats.IsNumOfSwordsLevelAvailable();
        energyOutput.interactable = Player.GetPlayer().stats.IsEnergyAttLevelAvailable();
        dmgReduction.interactable = Player.GetPlayer().stats.IsDmgReductionLevelAvailable();
        expMultipler.interactable = Player.GetPlayer().stats.IsExpMultiLevelAvailable();
    }
    private void SetDisplayLevels() {
        strLvl.text= Player.GetPlayer().stats.StrLvl.ToString();
        aurConLvl.text=Player.GetPlayer().stats.AurConLvl.ToString(); ;
        numOfSwordLvl.text = Player.GetPlayer().stats.NumSwordsLvl.ToString();
        energyOutLvl.text = Player.GetPlayer().stats.EnergyAttLvl.ToString();
        dmgReducLvl.text = Player.GetPlayer().stats.DmgReductionLvl.ToString();
        expMultiLvl.text = Player.GetPlayer().stats.ExpModLvl.ToString();
    }
}
