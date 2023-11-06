using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimminPoint : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.LockedOn&&!player.HasTarget)
            transform.position = player.transform.forward * 10;// new Vector3(0, 2, 10);
    }
}
