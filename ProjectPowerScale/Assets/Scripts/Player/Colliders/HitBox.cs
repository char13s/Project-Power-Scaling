using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class HitBox : MonoBehaviour
{
    [SerializeField] private float additionalDamage=1;
    public float AdditionalDamage { get => additionalDamage; set => additionalDamage = value; }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        
    }
}
