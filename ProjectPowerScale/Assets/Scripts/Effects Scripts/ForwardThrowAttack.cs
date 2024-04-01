using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardThrowAttack : MonoBehaviour
{
    [SerializeField] private float move;
    private Vector3 direction;
    PlayerLockOn playerLock;
    Player player;
    Vector3 target;
    private void Start() {
        player = Player.GetPlayer();
        playerLock = player.PLock;
        direction = player.transform.forward;
        if(player.LockedOn)
            target = playerLock.EnemyTarget.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(player.LockedOn)
            Vector3.MoveTowards(transform.position, target, 1000);
        transform.position+=direction * move * Time.deltaTime;
    }
}
