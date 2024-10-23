using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#pragma warning disable 0649
//[RequireComponent(typeof(EnemyTimelines))]
[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    private EnemyAiStates state;
    public enum EnemyType { soft, hard, absorbent }
    [SerializeField] private EnemyType type;
    public event UnityAction onDefeat;
    public static event UnityAction<int> sendDmg;
    public enum EnemyAiStates { Null, Idle, Attacking, Chasing, ReturnToSpawn, Dead, Hit, UniqueState, UniqueState2, UniqueState3, UniqueState4, StatusEffect, Grabbed, Staggered };
    public enum EnemyHealthStatus { FullHealth, MeduimHealth, LowHealth }
    EnemyHealthStatus healthStatus;

    [Header("Enemy Health Bar")]
    #region Enemy Health Bar
    [SerializeField] private GameObject canvas;
    //[SerializeField] private Text levelText;
    [SerializeField] private GameObject lockOnArrow;
    [SerializeField] private Slider EnemyHp;
    [SerializeField] private Slider defMeter;
    #endregion
    #region Special Effects
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float reach;
    private float distanceGround;
    #endregion
    [Space]
    [Header("Enemy Parameters")]
    //[SerializeField] private int level;
    [SerializeField] private int attackDelay;
    private int powerLevel;
    private int mp;
    //[SerializeField] private int baseExpYield;
    //[SerializeField] private int baseHealth;
    [SerializeField] private float attackDistance;
    [SerializeField] private bool standby;
    [SerializeField] private float gravity;
    [SerializeField] private float speed;
    [SerializeField] private int orbWorth;
    [Space]
    [Header("Object Refs")]

    [SerializeField] private GameObject teleportTo;
    [SerializeField] private GameObject hitSplat;
    [SerializeField] private GameObject drop;//Drop yo sword! 
    [SerializeField] private GameObject dropLocation;
    [SerializeField] private GameObject soul;
    [SerializeField] private GameObject cut;
    private int stagger;

    // [SerializeField] private GameObject model;
    [SerializeField] private GameObject skinnedMesh;
    [SerializeField] private GameObject finisherTrigger;
    [Space]
    [Header("Effects Refs")]
    [SerializeField] private GameObject SpawninEffect;
    #region Script References
    //internal StatusEffects status = new StatusEffects();
    [SerializeField]
    internal StatsController stats = new StatsController();
    private Player pc;
    private Player zara;
    [SerializeField] private Animator anim;
    //private EnemyTimelines timelines;
    //private AudioSource sound;
    private CharacterController charCon;
    #endregion

    #region Coroutines
    private Coroutine hitCoroutine;
    private Coroutine attackCoroutine;
    private Coroutine attackingCoroutine;
    private Coroutine recoveryCoroutine;
    private Coroutine guardCoroutine;
    #endregion
    //private byte eaten;
    private bool attacking;
    private bool attack;
    private bool walk;
    private bool hit;
    private bool lockedOn;
    private bool dead;
    private bool lowHealth;
    private bool parry;
    private bool timelining;
    // [SerializeField] private bool weak;

    private bool striking;
    [SerializeField] private int flip;
    private static List<Enemy> enemies = new List<Enemy>(32);
    private bool grounded;

    [SerializeField] private bool boss;
    private bool frozen;

    public static event UnityAction<Enemy> onAnyDefeated;
    public static event UnityAction onAnyEnemyDead;
    public static event UnityAction onHit;
    public static event UnityAction guardBreak;
    public static event UnityAction<AudioClip> sendsfx;
    public static event UnityAction<int> sendOrbs;
    public static event UnityAction add;
    public static event UnityAction remove;
    #region Getters and Setters
    public int Health { get { return stats.Health; } set { stats.Health = Mathf.Max(0, value); } }
    public int HealthLeft { get { return stats.HealthLeft; } set { stats.HealthLeft = Mathf.Max(0, value); UpdateParameters(); UIMaintence(); if (canvas != null) canvas.GetComponent<EnemyCanvas>().SetEnemyHealth(); if (stats.HealthLeft <= 0 && !dead) { Dead = true; } } }
    public bool Attack { get => attack; set { attack = value; } }
    protected bool Walk { get => walk; set { walk = value;  } }

    public bool Hit {
        get => hit; set {
            hit = value;
            Anim.SetBool("Hurt", hit); if (onHit != null) {
                onHit();
            }
            if (hit) { OnHit(); }
        }
    }
    public EnemyAiStates State { get => state; set { state = value; States(); } }
    public bool Grounded { get => grounded; set { grounded = value; Anim.SetBool("Grounded", grounded); } }
    public bool LockedOn {
        get => lockedOn; set {
            lockedOn = value; if (lockedOn && !dead&&canvas) {

                //canvas.SetActive(true);
                //lockOnArrow.SetActive(true);
            }
            else {
                //lockOnArrow.SetActive(false);
               // canvas.SetActive(false);
            }

        }
    }
    public bool Dead {
        get => dead;
        private set {
            dead = value;
            if (dead) {
                //GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_onOrOff", 1);
                //GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("dead", 1);

                OnDefeat();
                //Anim.SetBool("Hurt", dead);
                if (onAnyDefeated != null) {
                    onAnyDefeated(this);
                }
                if (onAnyEnemyDead != null) {
                    onAnyEnemyDead();
                }

            }
        }
    }
    public static event UnityAction updateEnemyList;
    public bool Boss { get => boss; set => boss = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public static List<Enemy> Enemies { get => enemies; set { enemies = value; } }
    public bool Frozen { get => frozen; set { frozen = value; if (frozen) { FreezeEnemy(); } } }
    private static int totalCount;
    private float distance;

    public int AttackDelay { get => attackDelay; set => attackDelay = value; }
    //public Rigidbody Rbody { get => rbody; set => rbody = value; }
    public bool Standby { get => standby; set { standby = value; StandbyState(); } }

    //public Rigidbody Rbody { get => rbody; set => rbody = value; }
    public bool Parry { get => parry; set { parry = value; if (value) { Anim.Play("Parried"); } } }
    public CharacterController CharCon { get => charCon; set => charCon = value; }
    public Player Zara { get => zara; set => zara = value; }
    public float Distance { get => distance; set => distance = value; }
    // public GameObject Model { get => model; set => model = value; }
    public GameObject SkinnedMesh { get => skinnedMesh; set => skinnedMesh = value; }
    public static int TotalCount { get => totalCount; set { totalCount = value; } }

    public int Stagger { get { return stats.StaggerLeft; } set { stats.StaggerLeft = Mathf.Max(0, value); UIMaintence(); } }

    public bool Timelining { get => timelining; set => timelining = value; }
    public int PowerLevel { get => powerLevel; set { powerLevel = value;canvas.GetComponent<EnemyCanvas>().SetPowerLevel(powerLevel); } }
    public int Mp { get => mp; set { mp = value; UpdateParameters(); } }
    #endregion



    public virtual void Awake() {

        //Anim = Model.GetComponent<Animator>();
        //sound = GetComponent<AudioSource>();
        //StatusEffects.onStatusUpdate += StatusControl;

        StatCalculation();
        state = EnemyAiStates.Idle;
        stats.staggercheck += StaggerCheck;

    }
    // Start is called before the first frame update
    public virtual void OnEnable() {

        //Instantiate(SpawninEffect,transform.position,Quaternion.identity);
       // ZaWarudo.timeFreeze += FreezeEnemy;

        //if(Player.GetPlayer()!=null)
        //zend = Player.GetPlayer();

        //Player.onPlayerDeath += OnPlayerDeath;

        //ReactionRange.dodged += SlowEnemy;
        //Enemies.Add(this);
        //if (add != null) {
        //    add();
        //}
        //timelines = GetComponent<EnemyTimelines>();
        HealthLeft = stats.Health;
        StandbyState();
        
        //Anim.Play("Spawn In");
    }
    private void OnDisable() {
        TotalCount = Enemies.Count;
        //ZaWarudo.timeFreeze -= FreezeEnemy;

        //Player.onPlayerDeath -= OnPlayerDeath;

        //ReactionRange.dodged -= SlowEnemy;
       
        Enemies.Remove(this);
        if (remove != null) {
            remove();
        }
        print("Enemy DIed");
    }
    public virtual void Start() {
        CharCon = GetComponent<CharacterController>();
        Zara = Player.GetPlayer();
        CharCon.center= new Vector3(CharCon.center.x, CharCon.height / 2, CharCon.center.z);
        // distanceGround = GetComponent<Collider>().bounds.extents.y;
        // Zend = NewZend.GetPlayer();
        //#region Grabbing Behaviors here
        //EnemyHitBehavior[] hitBehaviors = Anim.GetBehaviours<EnemyHitBehavior>();
        //for (int i = 0; i < hitBehaviors.Length; i++)
        //    hitBehaviors[i].Enemy = this;
        //EnemyChaseBehavior[] chaseBehaviors = Anim.GetBehaviours<EnemyChaseBehavior>();
        //for (int i = 0; i < chaseBehaviors.Length; i++)
        //    chaseBehaviors[i].Enemy = this;
        //EnemyKnockedUp[] move = Anim.GetBehaviours<EnemyKnockedUp>();
        //for (int i = 0; i < move.Length; i++)
        //    move[i].Enemy = this;
        //#endregion
        //TotalCount = Enemies.Count;
    }
    public void TimeliningControl(bool val) {
        Timelining = val;
        print("Boss timelingggg");
    }
    private void EnemiesNeedToRespawn(int c) {
        Destroy(gameObject);
    }
    // Update is called once per frame
    public virtual void Update() {
        if (Zara != null)
            Distance = Vector3.Distance(Zara.transform.position, transform.position);
        States();
        //if (status.Status != StatusEffects.Statuses.stunned && state != EnemyAiStates.Null) {
        //    //StateSwitch();
        //}

        //canvas.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
    public virtual void FixedUpdate() {

        //transform.LookAt(Zend.transform.position);
        //transform.rotation = Quaternion.LookRotation(delta);
        if (!hit)
            CharCon.Move(new Vector3(0, -gravity, 0) * Time.deltaTime);
    }
   /* private void StatusControl() {
        if (!dead) {
            switch (status.Status) {
                case StatusEffects.Statuses.stunned:
                    State = EnemyAiStates.StatusEffect;
                    if (!dead) {
                        Anim.speed = 0;
                    }
                    break;
            }
        }
    }*/
    private void StatCalculation() {
        Health = (10 * stats.BaseMp) * (stats.BaseAttack); //
        orbWorth=(10*stats.BaseMp* stats.BaseAttack)/4;
        HealthLeft = Health;
        Mp = stats.MPLeft;

        stats.Defense = stats.BaseDefense * stats.Level;
        stats.StaggerLeft = stats.Stagger;
        Stagger = stats.StaggerLeft;
        
    }
    private void UpdateParameters() {
        stats.Attack = ((HealthLeft / 10) + Mp) * stats.BaseAttack; 
        PowerLevel = stats.Attack;
    }
    //public static Enemy GetEnemy(int i) => Enemies[i];
    public void OnPlayerDeath() {
        Enemies.Clear();
    }
    #region Reactions
    public void Knocked() {
        Vector3 delta = (Zara.transform.position - transform.position);
        delta.y = 0;
        // transform.LookAt(Zend.transform.position);
        transform.rotation = Quaternion.LookRotation(delta);
        //timelines.KnockedBack();//swap this with an anim controlled interaction
    }
    public void CancelKnocked() {
        //Anim.Play("Dropping");
        //timelines.CancelKnockUp();
        //StartCoroutine(SendToGround());
        //Anim.SetTrigger("Fall");
        //Rbody.AddForce(new Vector3(0, -150, 0), ForceMode.VelocityChange);
    }
    private IEnumerator SendToGround() {
        while (isActiveAndEnabled && !grounded) {
            yield return null;
            charCon.Move(new Vector3(0, -15, 0));
        }
    }
    public void KnockedUp() {
        print("Knocked up");
        Anim.Play("KnockedUp");
        Anim.ResetTrigger("AirHit");
    }
    public void KnockedBack() {
        Anim.Play("KnockedBack");
    }
    public void KnockedDown() {
        Anim.Play("KnockedDown");
    }
    private void KillEnemy() {
        Destroy(this);
    }
   /* public void Burned() {
        if (status.Status == StatusEffects.Statuses.neutral) {
            status.Status = StatusEffects.Statuses.burned;
            //Add a red glow material
            StartCoroutine(BurnEnemy());
        }
    }
    public void Froze() {
        if (status.Status == StatusEffects.Statuses.neutral) {
            status.Status = StatusEffects.Statuses.frozen;
            //Add a baby blue material and ice particles
            StartCoroutine(FrozeEnemy());
        }
    }
    public void Pararlyzed() {
        if (status.Status == StatusEffects.Statuses.neutral) {
            status.Status = StatusEffects.Statuses.stunned;
            //Add a yellow material and electricity particles
            StartCoroutine(StunEnemy());
        }
    }*/
    #region old freeze
    private void SwitchFreezeOn() {
        Frozen = true;
    }
    private void FreezeEnemy() {
        Debug.Log("Froze");
        Anim.SetFloat("Speed", 0.1f);
        //anim.speed = 0;
        //State = EnemyAiStates.Null;
        StartCoroutine(UnFreeze());
    }
    private IEnumerator UnFreeze() {
        YieldInstruction wait = new WaitForSeconds(4);
        yield return wait;
        Anim.SetFloat("Speed", 0.1f);
        UnFreezeEnemy();
    }
    private void UnFreezeEnemy() {
        anim.speed = 1;
        State = EnemyAiStates.Idle;
    }
    private void NullEnemy() {
        State = EnemyAiStates.Null;
    }
    #endregion
    #endregion
    #region Event handlers

    #endregion

    #region State Logic
    public virtual void StateSwitch() {
        if (!Timelining) {
            if (HealthLeft < Health / 4) {
                lowHealth = true;
                //finisherTrigger.SetActive(true);
            }
            if (state == EnemyAiStates.Chasing) {
                SwitchToAttack();
                BackToIdle();
            }
            if (state == EnemyAiStates.Idle) {//What happens in Idle
                SwitchToAttack();
                ChasePlayer();
            }
            if (state == EnemyAiStates.Attacking) {
                ChasePlayer();
                BackToIdle();
                //SwitchToAttack();
            }
        }
    }
    public virtual void SwitchToAttack() {
        if (Distance < attackDistance && !dead && !Hit) {
            State = EnemyAiStates.Attacking;
        }
        else {
            attacking = false;
        }
    }
    public virtual void ChasePlayer() {
        if (Distance > attackDistance && !dead && !Hit) {
            State = EnemyAiStates.Chasing;
        }
        else {
            Walk = false;
        }
    }
    public virtual void BackToIdle() {
        if (Distance > 10f | AttackDelay > 0) {
            State = EnemyAiStates.Idle;
        }
    }
    public virtual void StaggerCheckState() {
        if (stats.StaggerLeft <= 0) {
            recoveryCoroutine = StartCoroutine(WaitForStaggerRecovery());
        }
    }
    public virtual void SuperAttack() {

    }
    public virtual void States() {
        switch (state) {
            case EnemyAiStates.Idle:
                Idle();
                break;
            case EnemyAiStates.Attacking:
                //Rbody.velocity = new Vector3(0, 0, 0);
                StartCoroutine(waitToAttack());
                break;
            //LowHealth();
            case EnemyAiStates.Chasing:
                Walk = true;

                //Chasing();
                break;
            //case EnemyAiStates.Staggered:
            //    //Call recovery coroutine
            //    recoveryCoroutine = StartCoroutine(WaitForStaggerRecovery());
            //    //set animation to stunned
            //    anim.SetBool("Stagger", true);
            //    break;
            default:
                break;
        }
    }
    IEnumerator waitToSuperAttack() {
        int num = Random.Range(7, 10);
        YieldInstruction wait = new WaitForSeconds(num);
        yield return wait;
        Anim.Play("Super");
    }
    IEnumerator waitToAttack() {
        int num = Random.Range(1, 3);
        YieldInstruction wait = new WaitForSeconds(num);
        yield return wait;
        Anim.SetTrigger("Attack 0");
        AttackDelay = 3;
    }
    public virtual void Idle() {
        Walk = false;
        attackDelay = 0;
    }
    public virtual void Chasing() {

        Walk = true;
        Vector3 delta = Zara.transform.position - transform.position;
        delta.y = 0;
        print("bruh;");
        transform.rotation = Quaternion.LookRotation(delta);
        CharCon.Move(transform.forward * speed * Time.deltaTime);
    }
    public virtual void AttackRot() {
        Vector3 delta = Zara.transform.position - transform.position;
        delta.y = 0;
        transform.rotation = Quaternion.LookRotation(delta);
    }
    private void StandbyState() {
        if (standby) {
            State = EnemyAiStates.Null;
        }
    }
    #endregion
    private void UIMaintence() {

        //levelText.GetComponent<Text>().text = "Lv. " + stats.Level;
        if (canvas != null) { 
        EnemyHp.maxValue = stats.Health;
        EnemyHp.value = stats.HealthLeft;
        defMeter.maxValue = stats.Stagger;
        defMeter.value = stats.StaggerLeft;
        }
    }
    public virtual void OnHit() {
        //StartCoroutine(waitToFall());
        if(anim)
            anim.Play("AirHit");
    }


    #region Coroutines
    IEnumerator waitToFall() {
        YieldInstruction wait = new WaitForSeconds(1);
        yield return wait;
        //rbody.useGravity = true;
    }
    IEnumerator BurnEnemy() {
        int counter = 5;
        YieldInstruction wait = new WaitForSeconds(1);
        while (isActiveAndEnabled && counter > 5) {
            yield return wait;
            counter--;
            //Particle come up
            HealthLeft -= (int)(Health * 0.05f);
        }
        //status.Status = StatusEffects.Statuses.neutral;
    }
    IEnumerator FrozeEnemy() {
        YieldInstruction wait = new WaitForSeconds(5);
        yield return wait;
        Anim.speed = 0;
    }
    IEnumerator StunEnemy() {
        YieldInstruction wait = new WaitForSeconds(1);
        yield return wait;
        Anim.speed = 0;
    }
    #endregion



    //private void OnTriggerStay(Collider other) {
    //    if (other != null && !other.CompareTag("Enemy") && other.CompareTag("Attack")) {
    //        Grounded = true;
    //    }
    //}
    #region status stuff
    private void SlowEnemy() {

        FreezeEnemy();
        print("Wth?????");
    }
    public void OnDefeat() {
        //onAnyDefeated(this);
        if (onDefeat != null) {
            onDefeat();
        }
        Drop();
        Enemies.Remove(this);
        if (deathEffect != null) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        sendOrbs.Invoke(orbWorth);
        //deathEffect.transform.position = transform.position;
        Destroy(gameObject, 0.5f);
        //drop.transform.SetParent(null);
    }
    public void Grabbed() {
        charCon.enabled = false;
    }
    public void UnSetParent() {
        charCon.enabled = true;
        transform.SetParent(null);
    }
    public void UnsetHit() {
        Hit = false;
        //Anim.ResetTrigger("Attack 0");
        State = EnemyAiStates.Idle;
    }
    #endregion
    public void CalculateDamage(float addition) {//, HitBoxType dmgType
        if (!dead) {
            float dmgModifier = 1;
            //dmgModifier = DmgMod(dmgModifier, dmgType);
            int dmg;
            if (Stagger > 0) {
                dmg = (int)Mathf.Clamp(((Zara.stats.Attack * addition) - stats.Defense) * dmgModifier, 1, 999);
                Stagger -= (int)(Zara.stats.Attack * dmgModifier);
            }
            else {
                dmg = (int)Mathf.Clamp(((Zara.stats.Attack * addition)) * dmgModifier, 1, 999);
            }
            print(dmg);
            HealthLeft -= dmg;
            //HitText hitSplat= new HitText();
            //Debug.Log(hitSplat.Text.ToString());
            //hitSplat.GetComponent<HitText>().Text = dmg.ToString();
            //Instantiate(hitSplat, transform.position, Quaternion.identity);
            //Hit = true;

            if (HealthLeft <= Health / 4 && !lowHealth) {
                //StartCoroutine(StateControlCoroutine());
                lowHealth = true;
            }

            //OnHit();
        }

    }//(Mathf.Max(1, (int)
     //(Mathf.Pow(stats.Attack - 2.6f * zend.stats.Defense, 1.4f) / 30 + 3))) / n; }
    /*private float DmgMod(float dmg, HitBoxType dmgType) {
        switch (type) {
            case EnemyType.absorbent:
                switch (dmgType) {
                    case HitBoxType.Heavy:
                        return dmg;
                    case HitBoxType.Magic:
                        return dmg / 4;
                    default:
                        return dmg * 1.5f;
                }
            case EnemyType.soft:
                switch (dmgType) {
                    case HitBoxType.Heavy:
                        return dmg / 4;
                    case HitBoxType.Magic:
                        return dmg;
                    default:
                        return dmg * 1.5f;
                }
            case EnemyType.hard:
                switch (dmgType) {
                    case HitBoxType.Heavy:
                        return dmg * 1.5f;
                    case HitBoxType.Magic:
                        return dmg;
                    default:
                        return dmg / 4;
                }
        }
        return dmg;
    }*/
    public void CalculateAttack(int extDmg) {
        if(sendDmg!=null)
        sendDmg(Mathf.Max(1, stats.Attack+extDmg));
    }
    private void StaggerCheck() {
        print("Stagger check");
        if (stats.StaggerLeft <= 0) {
            print("Stagger broke");
            Anim.Play("Parried");
            recoveryCoroutine = StartCoroutine(WaitForStaggerRecovery());
            state = EnemyAiStates.Staggered;
        }
    }
    public void TeleportPlayer() {
        //turnoff characterController
        Debug.Log("ported");
        //Zara.CharCon.enabled = false;
        //move player to teleportTo
        Zara.transform.position = teleportTo.transform.position;
        StartCoroutine(WaitToCharCon());
    }
    IEnumerator WaitToCharCon() {
        YieldInstruction wait = new WaitForSeconds(0.1f);
        yield return wait;
        //Zara.CharCon.enabled = true;
    }
    IEnumerator WaitForStaggerRecovery() {
        YieldInstruction wait = new WaitForSeconds(5f);
        while (isActiveAndEnabled && Stagger != stats.Stagger) {
            yield return wait;
            Stagger = stats.Stagger;
        }
        if (Stagger == stats.Stagger) {
            state = EnemyAiStates.Idle;
        }
    }
    public void HitGuard() {
        if (Zara.stats.MPLeft > 0) {
            Zara.stats.MPLeft -= Mathf.Max(1, stats.Attack);
            if (sendsfx != null) {

            }
        }
        else {

            if (guardBreak != null) {
                guardBreak();
            }
        }
    }
    private void Drop() {
        //Zara.stats.AddExp(orbWorth);
        
        if (drop != null) {
            Instantiate(drop, transform.position + new Vector3(0, 0.14f, 0), Quaternion.identity);
            drop.transform.position = transform.position;

        }
        if (soul != null) {
            Instantiate(soul, transform.position + new Vector3(0, 0.18f, 0), Quaternion.identity);
            soul.transform.position = transform.position;
        }

    }

}
