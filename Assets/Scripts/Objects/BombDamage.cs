using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BombDamage : MonoBehaviour
{
    SphereCollider collider;
    private List<GameObject> liste = new List<GameObject>(); 

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

        if (other.gameObject.CompareTag("Enemy"))
        {
            liste.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            liste.Remove(other.gameObject);
        }
    }

    public void explode()
    {
        foreach (GameObject g in liste)
        {
            g.GetComponent<IEnemy>().TakeDamage(10);
        }
    }
}
