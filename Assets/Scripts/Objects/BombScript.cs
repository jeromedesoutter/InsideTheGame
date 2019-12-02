using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public int damage = 5;
    public float durationBeforeExplode = 4f;
    public GameObject explosionEffect;
    private GameObject particles;

    private BombDamage explosionScript;

    float timeSinceInstanciation = 0f;
    bool exploded = false;

    public AudioSource audioBoum;
    public AudioSource audioMinuterie;

    //Flash variables
    Renderer[] renderers;
    public Color color = Color.red;
    int count = 4;
    float duration = 1;
    float TimeSinceBeginFeedback = 0;
    float TotalTime = 0;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        timeSinceInstanciation = 0;
        exploded = false;

        explosionScript = GetComponentInChildren<BombDamage>();
        //explosionScript.enabled = false;
        count = 4;
        duration = 1;

        TimeSinceBeginFeedback = 0;
        TotalTime = 0;

        audioMinuterie.Play();
    }

    private void Update()
    {
        timeSinceInstanciation += Time.deltaTime;

        if (!exploded && timeSinceInstanciation > durationBeforeExplode)
        {
            Explode();
        }

        if (timeSinceInstanciation < durationBeforeExplode)
        {
            UpdateFlash();
        }
    }

    void Explode()
    {
        exploded = true;

        particles = Instantiate(explosionEffect, transform.position, transform.rotation);
        audioMinuterie.Stop();
        audioBoum.Play();
        
        explosionScript.enabled = true;
        explosionScript.explode();
        Invoke("DestroyEffect", 2);
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
        GetComponent<Collider>().enabled = false;
    }

    private void DestroyEffect()
    {
        audioBoum.Stop();
        Destroy(particles);
        Destroy(gameObject);
    }

    void UpdateFlash()
    {
        TotalTime += Time.deltaTime;
        TimeSinceBeginFeedback += Time.deltaTime;
        if (TotalTime > durationBeforeExplode) { return; }
        if (TimeSinceBeginFeedback >= duration) { count++; duration = durationBeforeExplode / count; TimeSinceBeginFeedback = 0; }


        foreach (Renderer renderer in renderers)
        {
            Material old_mat = new Material(renderer.material);
            Material new_mat = new Material(renderer.material);
            new_mat.color = color;
            if (TimeSinceBeginFeedback < duration / 2)
            {
                float lerp = 2 * TimeSinceBeginFeedback / duration;
                renderer.material.Lerp(old_mat, new_mat, lerp);
            }
            else
            {
                float lerp = 2 * (TimeSinceBeginFeedback - duration / 2) / duration;
                renderer.material.Lerp(new_mat, old_mat, lerp);
            }
        }

    }
}