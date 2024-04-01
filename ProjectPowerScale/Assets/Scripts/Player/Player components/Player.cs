using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    private static Player instance;

    public static event UnityAction<bool> lockon;
    public static event UnityAction onPlayerDeath;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject aimminPoint;
    [SerializeField] private GameObject body;
    private bool lockedOn;
    private bool skillsUp;
    bool attacking;
    private bool hasTarget;
    private bool flying;
    bool blocking;

    Animator anim;
    CharacterController charCon;
    internal Stats stats = new Stats();
    PlayerLockOn pLock;
    public bool LockedOn { get => lockedOn; set { lockedOn = value; Anim.SetBool("LockedOn",value);  } }
    public bool SkillsUp { get => skillsUp; set => skillsUp = value; }
    public GameObject AimminPoint { get => aimminPoint; set => aimminPoint = value; }
    public bool Attacking { get => attacking; set => attacking = value; }
    public bool HasTarget { get => hasTarget; set => hasTarget = value; }
    public CharacterController CharCon { get => charCon; set => charCon = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public bool Flying { get => flying; set => flying = value; }
    public bool Blocking { get => blocking; set => blocking = value; }
    public GameObject Body { get => body; set => body = value; }
    public GameObject RightHand { get => rightHand; set => rightHand = value; }
    public GameObject LeftHand { get => leftHand; set => leftHand = value; }
    public PlayerLockOn PLock { get => pLock; set => pLock = value; }

    public static Player GetPlayer() => instance;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        Anim = GetComponent<Animator>();
        charCon = GetComponent<CharacterController>();
        pLock = GetComponent<PlayerLockOn>();
        stats.Start();
    }
    private void OnEnable() {
        ParryBox.parried += PlayParry;
    }
    private void OnDisable() {
        ParryBox.parried -= PlayParry;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void PlayParry() {
        anim.Play("");
    }
}
