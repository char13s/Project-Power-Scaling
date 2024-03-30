using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
[System.Serializable]
public class Stats
{

    //Variables
    private float health;
    private int attack;
    private float defense;

    private int mp;
    private float speed;
    private float healthLeft;

    private int mpLeft;
    private float transformationMod = 1;
    private float expMod;
    private float energyAttackMod;

    private int baseAttack;
    private int baseDefense;
    private int baseMp;
    private int baseHealth;

    private float attackBoost;
    private int defenseBoost;
    private int mpBoost;

    private int strLvl;
    private int aurConLvl;
    private int numSwordsLvl;
    private int energyAttLvl;
    private int dmgReductionLvl;
    private int expModLvl;
    private int abilitypoints;

    //Events
    public static event UnityAction onHealthChange;
    public static event UnityAction onMPLeft;


    public static event UnityAction onBaseStatsUpdate;

    public static event UnityAction<int> onPowerlv;
    public static event UnityAction sendSpeed;
    public static event UnityAction<int> onOrbGain;
    public static event UnityAction<int> sendMp;
    public static event UnityAction<int> updateMP;
    public static event UnityAction increase;
    public static event UnityAction decrease;
    //Properties
    #region Getters and Setters
    public float Health { get { return health; } set { health = Mathf.Max(0, value); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public float HealthLeft { get { return healthLeft; } set { healthLeft = Mathf.Clamp(value, 0, health); CalculateStatsOutput(); if (onHealthChange != null) { onHealthChange(); } } }
    //public int MPLeft { get { return MpLeft; } set { MpLeft = Mathf.Clamp(value, 0, mp); CalculateStatsOutput(); if (onMPLeft != null) { onMPLeft(); } } }

    public int Attack { get { return attack; } set { attack = value; if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public float Defense { get { return defense; } set { defense = value; } }
    public int MP { get { return mp; } set { mp = value; if (sendMp != null) sendMp(mp); CalculateStatsOutput(); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }//
    //public float Speed { get { return speed; } set { speed = value; sendSpeed.Invoke(); } }
    public int BaseAttack { get => baseAttack; set { baseAttack = Mathf.Clamp(value, 0, 9999999); CalculateStatsOutput(); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public int BaseDefense { get => baseDefense; set { baseDefense = Mathf.Clamp(value, 0, 9999999); CalculateStatsOutput(); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public int BaseMp { get => baseMp; set { baseMp = Mathf.Clamp(value, 0, 9999999); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public int BaseHealth { get => baseHealth; set { baseHealth = Mathf.Clamp(value, 0, 9999999); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public float AttackBoost { get => attackBoost; set { attackBoost = Mathf.Clamp(value, 0, 2); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); SetStats(); CalculateStatsOutput(); } }
    public int DefenseBoost { get => defenseBoost; set { defenseBoost = Mathf.Clamp(value, 0, 9999999); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); SetStats(); } }
    public int MpBoost { get => mpBoost; set { mpBoost = Mathf.Clamp(value, 0, 9999999); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); SetStats(); } }
    public int Abilitypoints { get => abilitypoints; set { abilitypoints = value; if (onBaseStatsUpdate != null) onBaseStatsUpdate(); if (onOrbGain != null) onOrbGain(abilitypoints); } }
    public int MPLeft {
        get => mpLeft; set {
            mpLeft = Mathf.Clamp(value, 1, mp); if (updateMP != null) updateMP(MPLeft); SetSpeed();
        }
    }
    public float TransformationMod { get => transformationMod; set { Mathf.Clamp(transformationMod, 1, 10000); CalculateStatsOutput(); } }

    public float Speed { get => speed; set { speed = Mathf.Clamp(value, 1, 1.5f); Player.GetPlayer().Anim.SetFloat("AttackSpeed", speed); } }

    public float ExpMod { get => expMod; set => expMod = value; }
    public float EnergyAttackMod { get => energyAttackMod; set => energyAttackMod = value; }
    public int StrLvl { get => strLvl; set => strLvl = value; }
    public int AurConLvl { get => aurConLvl; set => aurConLvl = value; }
    public int NumSwordsLvl { get => numSwordsLvl; set => numSwordsLvl = value; }
    public int EnergyAttLvl { get => energyAttLvl; set => energyAttLvl = value; }
    public int DmgReductionLvl { get => dmgReductionLvl; set => dmgReductionLvl = value; }
    public int ExpModLvl { get => expModLvl; set => expModLvl = value; }

    //public int CalculateExpNeed() { int expNeeded = 4 * (Level * Level * Level); return Mathf.Abs(Exp - expNeeded); }
    //public int ExpCurrent() { return Exp - (4 * ((Level - 1) * (Level - 1) * (Level - 1))); }
    #endregion
    public void AddExp(int points) {
        MpBoost += points / 4;
    }
    public void Start() {
        SetStats();
        CalculateStatsOutput();
        Enemy.sendOrbs += AdjustOrbs;
        if (onHealthChange != null) {
            onHealthChange();
        }

        Enemy.sendDmg += CalculateDmg;
        SavePointMenu.fullRestore += FullRestore;
    }
    private void UpdateUi() {
        if (onHealthChange != null) {
            onHealthChange();
        }


    }
    private void SetStats() {
        // + mpBoost
        //baseHealth = 120;
        //healthLeft = baseHealth;
        ExpMod = 1;
        baseMp = 100;
        //Health = baseHealth;// + healthBoost
        MP = baseMp + mpBoost;
        FullRestore();
        BaseAttack = 1;
        BaseDefense = 5;

        TransformationMod = 1;
        if (onHealthChange != null) {
            onHealthChange();
        }

        //onMPLeft.Invoke();
        //CalculateStatsOutput();

    }
    private void FullRestore() {
        HealthLeft = health;
        MPLeft = MP;
    }
    private void SetSpeed() {
        Speed = 1 + (mpLeft / mp);
    }
    //private void ChangeMpLeft(int amt) => MPLeft += amt;
    private void CalculateStatsOutput() {
        //calculated everytime health or Mp is changed.
        //Health=(10*mp)* (baseAttack); VERY CLEAN ONE
        //Attack = (1+ mpLeft)*(healthLeft/health) * baseAttack+(attackBoost)*transformationMod; Interesting curve maybe will use

        //Attack = (((healthLeft / 10) + (mpLeft)) * (baseAttack))+attackBoost*(healthLeft/health)); A simple one I didnt know how to make stronger
        //Attack = (((healthLeft / 10) + mpLeft) * (baseAttack) * transformationMod)+AttackBoost;  A chaotic one
        Health = (10 * mp);//Attack got taken out for simplicity
        Attack = (int)((mpLeft) * ((healthLeft / health) + attackBoost) * transformationMod);// Until I think of how to handle upgrading base attack values if theres time.
    }
    private void AddToAttackBoost(int val) {
        //Upgrading Attacks on Attack boost affect here
        AttackBoost += val;
        CalculateStatsOutput();
    }
    private void AddToDefenseBoost(int val) {
        //Upgrading Defense boost affect here
        //
        defenseBoost += val;
        CalculateStatsOutput();
    }
    private void OnTransformation(bool val) {
        if (val) {
            AttackBoost = BaseAttack;
        }
        else {
            AttackBoost = 0;
        }
        //An Mp boost should be given here which would contribute to an attack otput boost
        //but also drains Mp and stamina the longer its held.
        //CalculateStatsOutput();
    }
    private void AdjustOrbs(int val) {
        Abilitypoints += (int)(val * expMod);
    }
    public void IncreaseHealth() {
        Health += 10;
        //Debug.Log(Health);
    }
    public void IncreaseMPSlowly() {
        MPLeft += (mp / 100);
        increase.Invoke();
    }
    public void AdjustMp(int amt) {
        MPLeft += amt;
    }
    public void DecreaseMPSlowly() {
        MPLeft -= (mp / 100);
        decrease.Invoke();
    }
    private void AddToStats() {
        if (mpLeft / (mp / 2) > 1)
            AttackBoost = mpLeft / (mp / 2);
    }
    private void Heal() {
        HealthLeft += (int)(Health * 0.2);
    }
    private void OutsideDamage(int val) {
        HealthLeft -= val;
    }
    private void CalculateDmg(int val) {
        HealthLeft -= val - (val * Defense);
    }
    public bool IsStrLevelAvailable() => abilitypoints >= 5 * Mathf.Pow(strLvl, 3) / 4;
    public bool IsAuraConLevelAvailable() => abilitypoints >= 4 * Mathf.Pow(aurConLvl, 3) / 1.75f;
    public bool IsNumOfSwordsLevelAvailable() => abilitypoints >= 5 * Mathf.Pow(numSwordsLvl, 6);
    public bool IsEnergyAttLevelAvailable() => abilitypoints >= 5 * Mathf.Pow(energyAttLvl, 3) / 2;
    public bool IsDmgReductionLevelAvailable() => abilitypoints >= 5 * Mathf.Pow(dmgReductionLvl, 3) / 4;
    public bool IsExpMultiLevelAvailable() => abilitypoints >= 5 * Mathf.Pow(expModLvl, 3) / 4;
}
