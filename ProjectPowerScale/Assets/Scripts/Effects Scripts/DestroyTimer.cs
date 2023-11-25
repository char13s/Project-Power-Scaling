using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private int timer=5;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,timer);
    }
}
