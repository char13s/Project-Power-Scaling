using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerLockOn : MonoBehaviour
{
    [SerializeField] private static List<Enemy> enemies = new List<Enemy>(16);
    private Plane[] planes;
    [SerializeField] private LayerMask layerMask;
    //[SerializeField] private float radius;
    [SerializeField] private Image aimIcon;
    private Camera cam;
    private Player player;
    private Enemy enemyTarget;
    //EnemyDetection detector;
    public static event UnityAction<bool> onLockOn;
    public static event UnityAction<Enemy> onTargetFound;
    public static event UnityAction<int> playBattleTheme;
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
    public static List<Enemy> Enemies { get => enemies; set => enemies = value; }
    public int T { get => t; set { t = value; Mathf.Clamp(t, 0, Enemies.Count); } }
    public Enemy EnemyTarget { get => enemyTarget; set { enemyTarget = value; if (value != null) { if (onTargetFound != null) onTargetFound(value); EnemyLockedTo(); } } }
    public float RotateSpeed { get => rotateSpeed; set { rotateSpeed = value; Mathf.Clamp(value, 5, 8); } }
    public Enemy ClosestEnemy { get => closestEnemy; set => closestEnemy = value; }
    public bool Takedown { get => takedown; set => takedown = value; }
    // Start is called before the first frame update
    private void OnEnable() {
        Enemy.onAnyDefeated += RemoveTheDead;
        //NewZend.findEnemy += firstLocked;
        //AttackStates.autoLocked += LockToClosest;
        //AttackStates.locked += LockToTarget;
        //NewZend.onPlayerDeath += RemoveAllEnemies;
    }
    private void OnDisable() {
        Enemy.onAnyDefeated -= RemoveTheDead;
        //NewZend.findEnemy -= firstLocked;
        //AttackStates.autoLocked -= LockToClosest;
        //AttackStates.locked -= LockToTarget;
        //NewZend.onPlayerDeath -= RemoveAllEnemies;
    }
    void Start() {
        cam = Camera.main;
        player = Player.GetPlayer();
        //detector = GetComponent<EnemyDetection>();
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
    private void OnTriggerEnter(Collider other) {
        print("Entered");
        if (other.GetComponent<Enemy>() && !Enemies.Contains(other.GetComponent<Enemy>()))
            Enemies.Add(other.GetComponent<Enemy>());
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Enemy>())
            Enemies.Remove(other.GetComponent<Enemy>());
    }
    // Update is called once per frame
    void Update() {
        inputDirection = transform.forward * 10;
        count = Enemies.Count;
        //if (Takedown) {
        //    StayLockedToTarget();
        //}
        if (enemies.Count > 0) {
            player.HasTarget = true;
        }
        else {
            ClosestEnemy = null;
            player.HasTarget = false;
        }
        GetClosestEnemy();
        if (player.LockedOn) {
            //if (player.Attacking)
            print("player is locked");
            if (Enemies.Count >0)
                player.AimminPoint.transform.position = EnemyTarget.transform.position + new Vector3(0, 1.5f,0);
            else {
                player.AimminPoint.transform.position = transform.position + new Vector3(0, 1.5f, 10);
            }
            GetInput();
        }
    }
    public void SwitchTarget(int val) {
        T += val;
        //GetClosestEnemy();
        if (t >= enemies.Count && t > 0) {
            T--;
        }
        if (T == enemies.Count) {
            T = 0;
        }
        enemyTarget = Enemies[T];
    }
    private void GetInput() {
       // if (Enemies.Count != 0 && T < Enemies.Count) {
            LockOn(player.AimminPoint);
        //}
    }
    private void firstLocked() {
        EnemyTarget = enemies[0];
    }
    private void EnemyLockedTo() {

            //EnemyTarget = enemies[T];
            EnemyTarget.LockedOn = true;
            if (player.LockedOn&&aimIcon!=null)
                aimIcon.transform.position = Camera.main.WorldToScreenPoint(EnemyTarget.transform.position);


        //Enemy.GetEnemy(enemies.IndexOf(enemies[T])); stupid code -_-
        //player.BattleCamTarget.transform.position = EnemyTarget.transform.position;
    }
    /*private void FindEnemyInFrontOfPlayer() {
        RaycastHit info;
        inputDirection = transform.forward * 10;
        if (Physics.SphereCast(transform.position, 2, inputDirection, out info, radius, layerMask)) {
            //if (info.collider.transform.GetComponent<Enemy>().IsAttackable())

            ClosestEnemy = info.collider.transform.GetComponent<Enemy>();
            T = Enemies.IndexOf(closestEnemy);
            //lockon.Enemies.Add(currentTarget);
        }
        //return null;
    }*/
    public void Findtarget() {
        //GetClosestEnemy();
    }
    private void GetClosestEnemy() {
        if (Enemies.Count == 1) {
            //if(enemies[T].Model.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
            ClosestEnemy = enemies[0];
            EnemyTarget= enemies[0];
            //EnemyLockedTo();
        }
        else {
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (T < enemies.Count && enemies[T] != null) {
                float enDist = EnDist(enemies[T].gameObject);
                foreach (Enemy en in Enemies) {
                    if (EnDist(en.gameObject) < enDist && GeometryUtility.TestPlanesAABB(planes, en.CharCon.bounds)) {//ReturnAngle(en) < closestAngle 
                        T = Enemies.IndexOf(en);
                        //if (enemies[T].Model.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
                        ClosestEnemy = en;
                        EnemyTarget = en;
                        //EnemyLockedTo();
                    }
                }
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
    //private void GetCombatMovement(float x, float y) {
    //    if (x == 0 && y == 0) {
    //        //Player.GetPlayer().CombatAnimations = 0;
    //    }
    //    if (x == 0) {
    //        Debug.Log("Combat Jump");
    //        if (y <= -0.3f) {
    //            Debug.Log("Combat BackJump");
    //            player.CombatAnimations = 1;
    //        }
    //        if (y >= 0.3f) {
    //            Debug.Log("Combat BackJump");
    //            player.CombatAnimations = 5;
    //        }
    //    }
    //    if (y == 0) {
    //        if (x <= -0.5f) {
    //            player.CombatAnimations = 2;
    //        }
    //        if (x >= 0.5f) {
    //            player.CombatAnimations = 3;
    //        }
    //    }
    //}
    private float EnDist(GameObject target) => Vector3.Distance(target.transform.position, player.transform.position);
    private void SetLock() {
        rotLock = true;
    }
    private void ResetLock() {
        rotLock = false;
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
        
        LockOff();
        EnemyLockedTo();
        if (target != null) {
            //target.LockedOn = true;
            RotateSpeed = 18 - EnDist(target);
            Vector3 delta = target.transform.position - player.transform.position;
            delta.y = 0;
            //if (!rotLock && player.CharCon.isGrounded) {
            transform.RotateAround(delta, Vector3.up, RotateSpeed);
                transform.rotation = Quaternion.LookRotation(delta, Vector3.up);
                print("is locked"); ;
            //}
        }
        else {
            print("target is null"); ;
        }
    }
    private void StayLockedToTarget() {
        Vector3 delta = enemyTarget.transform.position - player.transform.position;
        delta.y = 0;
        if (!rotLock) {
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
