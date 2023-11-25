using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardThrowAttack : MonoBehaviour
{
    [SerializeField] private float move; 

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.forward * move * Time.deltaTime;
    }
}
