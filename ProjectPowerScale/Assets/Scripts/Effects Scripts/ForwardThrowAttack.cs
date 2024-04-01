using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardThrowAttack : MonoBehaviour
{
    [SerializeField] private float move;
    private Vector3 direction;
    private void Start() {
        direction = Player.GetPlayer().transform.forward;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position+=direction * move * Time.deltaTime;
    }
}
