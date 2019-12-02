using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int damage = 5;
    public GameObject explosionEffect;
    private GameObject particles;


    private void OnCollisionEnter(Collision collision)
    {
        particles = Instantiate(explosionEffect, transform.position, transform.rotation);
        Invoke("DestroyEffect", 1);
        gameObject.SetActive(false);
    }

    private void DestroyEffect()
    {
        Destroy(particles);
        Destroy(gameObject);
    }
}
