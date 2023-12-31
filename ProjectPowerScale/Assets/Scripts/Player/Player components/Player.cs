using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    private static Player instance;

    public static event UnityAction<bool> lockon;

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
    public bool LockedOn { get => lockedOn; set { lockedOn = value; Anim.SetBool("LockedOn",value); lockon.Invoke(value); } }
    public bool SkillsUp { get => skillsUp; set => skillsUp = value; }
    public GameObject AimminPoint { get => aimminPoint; set => aimminPoint = value; }
    public bool Attacking { get => attacking; set => attacking = value; }
    public bool HasTarget { get => hasTarget; set => hasTarget = value; }
    public CharacterController CharCon { get => charCon; set => charCon = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public bool Flying { get => flying; set => flying = value; }
    public bool Blocking { get => blocking; set => blocking = value; }
    public GameObject Body { get => body; set => body = value; }

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
    }
    // Start is called before the first frame update
    void Start()
    {
        stats.Start();
    }

}
