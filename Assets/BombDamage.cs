using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BombDamage : MonoBehaviour
{
    SphereCollider collider;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

    public void ActivateCollision()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
