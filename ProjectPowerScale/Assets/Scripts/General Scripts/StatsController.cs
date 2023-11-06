using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class StatsController 
{
    public event UnityAction staggercheck;
    private int health;
    private int attack;
    private int defense;
    [SerializeField]private int stagger;
    private int mp;
    private int intellect;
    private int healthLeft;
    private int staggerLeft;
    private int mpLeft;
    //private byte level = 1;
    private int exp = 0;

    #region Base Stats
    [SerializeField] private int level;
    [SerializeField] private int baseHealth;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseMp;
    [SerializeField] private int expYield;
    #endregion
    //Events

    //Properties
    public int Health { get { return health; } set { health = Mathf.Max(0, value); } }
    public int HealthLeft { get { return healthLeft; } set { healthLeft = Mathf.Clamp(value, 0, health);  } }
    public int MPLeft { get { return mpLeft; } set { mpLeft = Mathf.Clamp(value, 0, mp);  } }

    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    public int Intellect { get { return intellect; } set { intellect = value; } }

    public int Level { get => level; set => level = value; }
    public int Exp { get => exp; set => exp = value; }
    public int BaseHealth { get => baseHealth; set => baseHealth = value; }
    public int BaseAttack { get => baseAttack; set => baseAttack = value; }
    public int BaseDefense { get => baseDefense; set => baseDefense = value; }
    public int BaseMp { get => baseMp; set => baseMp = value; }
    public int ExpYield { get => expYield; set => expYield = value; }
    public int Stagger { get => stagger; set => stagger = value; }
    public int StaggerLeft { get => staggerLeft; set { staggerLeft = Mathf.Clamp(value, 0, stagger);
            if (staggercheck != null) { staggercheck(); }
        } }
}
