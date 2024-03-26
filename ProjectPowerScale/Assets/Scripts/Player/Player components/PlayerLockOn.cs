using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerLockOn : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>(16);
    private Plane[] planes;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius;
    [SerializeField] private Image aimIcon;
    private Camera cam;
    private Player player;
    private Enemy enemyTarget;

    public static event UnityAction<bool> onLockOn;
    public static event UnityAction<Enemy> onTargetFound;
    public static event UnityAction<int> playBattleTheme;
    public static event UnityAction<GameObject> switchTarget;
    private Enemy closestEnemy;
    private bool locked;
    private float rotateSpeed;
    private GameObject aimPoint;
    Vector3 inputDirection;
    //private GameObject leftPoint;
    private bool rotLock;
    private float rotationSpeed;
    private bool takedown;
    private int t;
    private int count;
    private Vector3 displacement;
    public float RotationSpeed { get => rotationSpeed; set { rotationSpeed = value; Mathf.Clamp(value, 5, 8); } }
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }
    public int T { get => t; set { t = Mathf.Clamp(value, 0, Enemies.Count); } }
    public Enemy EnemyTarget { get => enemyTarget; set { enemyTarget = value; if (value != null) { if (onTargetFound != null) onTargetFound(value); } } }
    public float RotateSpeed { get => rotateSpeed; set { rotateSpeed = value; Mathf.Clamp(value, 5, 8); } }

    public Enemy ClosestEnemy { get => closestEnemy; set => closestEnemy = value; }
    public bool Takedown { get => takedown; set => takedown = value; }
    public bool RotLock { get => rotLock; set => rotLock = value; }

    // Start is called before the first frame update
    private void OnEnable() {
        Enemy.onAnyDefeated += RemoveTheDead;
        //NewZend.findEnemy += firstLocked;
        //AttackStates.autoLocked += LockToClosest;
        //AttackStates.locked += LockToTarget;
        ThirdPersonCameraWithLockOn.ThirdPersonCamera.sendThese += AddThese;
        //LockedOnState.autoLocked += LockToTarget;
        Player.onPlayerDeath += RemoveAllEnemies;
        ThirdPersonCameraWithLockOn.ThirdPersonCamera.sendTarget += EnemyLockedTo;
    }
    private void OnDisable() {
        Enemy.onAnyDefeated -= RemoveTheDead;
        //NewZend.findEnemy -= firstLocked;
        //AttackStates.autoLocked -= LockToClosest;
        //AttackStates.locked -= LockToTarget;
        //LockedOnState.autoLocked -= LockToTarget;
        ThirdPersonCameraWithLockOn.ThirdPersonCamera.sendThese -= AddThese;
        Player.onPlayerDeath -= RemoveAllEnemies;
        ThirdPersonCameraWithLockOn.ThirdPersonCamera.sendTarget -= EnemyLockedTo;
        RemoveAllEnemies();
    }
    void Start() {
        cam = Camera.main;
        player = GetComponent<Player>();

    }
    private void RemoveTheDead(Enemy enemy) {
        Enemies.Remove(enemy);
    }
    private void RemoveAllEnemies() {
        Enemies.Clear();
    }
    private void CheckDisplacement(Vector2 val) {
        displacement = val;
    }

    private void AddThese(GameObject[] newlist) {
        for (int i = 0; i < newlist.Length; i++) {
            Enemy temp;
            if (temp = newlist[i].GetComponent<Enemy>()) {
                if (!enemies.Contains(temp)) {
                    Enemies.Add(newlist[i].GetComponent<Enemy>());
                }
            }
        }
    }
 
    void Update() {
        //inputDirection = transform.forward * 10;
        //count = Enemies.Count;
        //if (Takedown) {
        //    StayLockedToTarget();
        //}
        if (enemies.Count > 0) {
            player.HasTarget = true;
        }
        else {
            //ClosestEnemy = null;
            player.HasTarget = false;
        }
        //GetClosestEnemy();
        if (player.LockedOn) {
            //    //if (player.Attacking)
            //    print("player is locked");
            if (Enemies.Count > 0 && EnemyTarget != null)
                player.AimminPoint.transform.position = EnemyTarget.transform.position + new Vector3(0, 1.5f, 0);
            else {
                player.AimminPoint.transform.position = transform.position + new Vector3(0, 1.5f, 10);
            }
        }
        if (player.LockedOn)
            GetInput();
        //}
        //if (Enemies.Count > 0 && EnemyTarget == null) {
        //    SwitchTarget(1);
        //}
        SwitchTarget();

        //if (t > enemies.Count && t > 0) {
        //    T--;
        //}
        //if (T == enemies.Count) {
        //    T = 0;
        //}
    }
    private void SwitchTarget() {
        if (Enemies.Count > 0 && EnemyTarget && EnemyTarget.Dead) {
            SwitchTarget(-1);
        }
    }
    public void SwitchTarget(int val) {
        T++;
        //GetClosestEnemy();
        // if (t >= enemies.Count && t > 0) {
        //     T--;
        // }
        if (T == enemies.Count) {
            T = 0;
        }
        //if (t < 0) {
        //    T = 0;
        //}
        //EnemyTarget = Enemies[T];
        print(T);
        if (switchTarget != null) {
            switchTarget(Enemies[T].gameObject);
        }
        //EnemyTarget = enemies[T];
        //if (player.Lockedon&&aimIcon!=null)
        // aimIcon.transform.position = Camera.main.WorldToScreenPoint(EnemyTarget.transform.position);

    }
    private void GetInput() {
        if (EnemyTarget) {
            LockOn(enemyTarget.gameObject);
            print("locked on");
        }
        else {
            LockOn(aimPoint);
        }
    }
    private void firstLocked() {
        EnemyTarget = enemies[0];
    }
    private void EnemyLockedTo(GameObject target) {

        if (target != null) {
            EnemyTarget = target.GetComponent<Enemy>();

        }
        //Enemy.GetEnemy(enemies.IndexOf(enemies[T])); stupid code -_-
        //player.BattleCamTarget.transform.position = EnemyTarget.transform.position;
    }
    private void FindEnemyInFrontOfPlayer() {
        RaycastHit info;
        inputDirection = transform.forward * 10;
        if (Physics.SphereCast(transform.position, 2, inputDirection, out info, radius, layerMask)) {
            //if (info.collider.transform.GetComponent<Enemy>().IsAttackable())

            ClosestEnemy = info.collider.transform.GetComponent<Enemy>();
            T = Enemies.IndexOf(closestEnemy);
            //lockon.Enemies.Add(currentTarget);
        }
        //return null;
    }
    public void Findtarget() {
        //GetClosestEnemy();
    }
    private void GetClosestEnemy() {
        if (Enemies.Count == 1) {
            //if(enemies[T].Model.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
            ClosestEnemy = enemies[0];
            T = 0;
            ;
        }
        else {
            if (T < enemies.Count && enemies[0] != null) {
                float enDist = EnDist(enemies[0].gameObject);
                foreach (Enemy en in enemies) {
                    if (EnDist(en.gameObject) < enDist) {//ReturnAngle(en) < closestAngle 
                        T = Enemies.IndexOf(en);
                        enDist = EnDist(en.gameObject);
                        //if (enemies[T].Model.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
                        ClosestEnemy = en;

                    }
                }
                //EnemyLockedTo();
            }
        }
    }

    private float ReturnAngle(Enemy en) {
        Vector3 targetDir = en.gameObject.transform.position - Camera.main.transform.position;
        var cameraFor = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        var newTargetDir = new Vector2(targetDir.x, targetDir.z);
        float angle = Vector2.Angle(cameraFor, newTargetDir);
        return angle;
    }
    private float EnDist(GameObject target) => Vector3.Distance(target.transform.position, player.transform.position);
    private void SetLock() {
        RotLock = true;
    }
    private void ResetLock() {
        RotLock = false;
    }
    private void BasicMovement() {
        //print("Basic moving");
        float x = displacement.x;
        float y = displacement.y;
        //MovementInputs(x, y);
        //RotateSpeed = 18 - EnDist(player.PlayerBody.BattleCamTarget);
        Vector3 delta = new Vector3(05, 0, 0);
        delta.y = 0;
        transform.rotation = Quaternion.LookRotation(delta, Vector3.up);
        //transform.position = Vector3.MoveTowards(transform.position, aimPoint.transform.position, player.MoveSpeed * y * Time.deltaTime);
    }
    public Vector3 TargetOffset(Transform target) {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }
    private void LockOn(GameObject target) {
        // LockOff();
        //EnemyLockedTo();
        if (target != null) {
            //target.LockedOn = true;
            RotateSpeed = 18 - EnDist(target.gameObject);
            Vector3 delta = target.transform.position - player.transform.position;
            delta.y = 0;

            print("bro your def locked on rn/");
            transform.rotation = Quaternion.LookRotation(delta, Vector3.up);

        }
    }
    private void StayLockedToTarget() {
        Vector3 delta = enemyTarget.transform.position - player.transform.position;
        delta.y = 0;
        if (!RotLock) {
            transform.rotation = Quaternion.LookRotation(delta, Vector3.up);
        }
    }
    private void LockToTarget() {
        Vector3 delta = transform.position;
        if (EnemyTarget) {
            delta = EnemyTarget.transform.position - transform.position;
        }
        delta.y = 0;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(delta), 5 * Time.deltaTime);
    }
    private void LockToClosest() {
        Vector3 delta;
        if (ClosestEnemy) {
            delta = ClosestEnemy.transform.position - player.transform.position;
            print("should be auto locking");
        }
        else {
            delta = transform.forward;
        }
        delta.y = 0;
        print(delta);
        transform.rotation = Quaternion.LookRotation(delta, Vector3.up);//Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(delta), 5 * Time.deltaTime);
    }
    private void LockOff() {
        foreach (Enemy en in Enemies) {
            if (Enemies[T] != en) {
                en.LockedOn = false;
                //onLockOn.Invoke(false);
            }
        }
    }
}
