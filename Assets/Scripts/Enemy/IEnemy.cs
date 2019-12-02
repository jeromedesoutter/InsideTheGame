﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using System;

[System.Serializable]
public class DebugMode
{
    public bool showRay = true;
    public bool showDebugSphere = true;
    public float scaleFactor = 1f;
    public GameObject debugSphere;
    public GameObject debugForwardPoint;
};

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class IEnemy : MonoBehaviour
{
    public GameObject player;

    protected NavMeshAgent navmesh;
    public Transform raycastPoint;
    protected Rigidbody rb;
    protected Collider collider;
    protected Animator animator;

    // Life
    public Life life;
    public float delayBeforeDisappearingWhenDead = 3f;
    bool AlreadyDead = false;

    private RoomManager room;

    // Debug
    public DebugMode debug;

    //Sounds
    public AudioManager soundsManager;

    //Color on damage
    public Color color = Color.red;
    public float duration = 0.7f;
    Renderer[] renderers;
    bool HitFeedback = false;
    float TimeSinceBeginFeedback = 0;

    public GameObject particles;

    protected virtual void Start()
    {
        room = GetComponentInParent<RoomManager>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.isKinematic = true;
        collider = GetComponent<Collider>();
        raycastPoint = GetComponent<Transform>();
        navmesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        navmesh.speed = 0.01f;
        renderers = gameObject.GetComponentsInChildren<Renderer>();

    }
    protected virtual void Update() {
        if (IsDead())
        {
            if (!AlreadyDead)
            {
                if (animator != null) { animator.SetTrigger("die"); }
                Die();
            }
        }

        if (TimeSinceBeginFeedback <= duration)
        {
            TimeSinceBeginFeedback += Time.deltaTime;
        }

        if (HitFeedback)
        {
            Flash();
        }
    }
    protected virtual void FixedUpdate() { }

    protected bool IsDead()
    {
        return life.current <= 0;
    }
    protected async void Die()
    {
        soundsManager.PlaySound("die");
        AlreadyDead = true;
        if (collider != null)
            collider.enabled = false;
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
        if (GetComponent<Renderer>()!= null)
        {
            GetComponent<Renderer>().enabled = false;
        }
        GameObject part = null;
        if (particles != null)
        {
            part = Instantiate(particles, transform.position, transform.rotation);
        }
        room.KillEnemy();
        await Task.Delay(TimeSpan.FromSeconds(delayBeforeDisappearingWhenDead));
        if (part != null)
        {
            Destroy(part);
        }
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        HitFeedback = true;
        TimeSinceBeginFeedback = 0;

        soundsManager.PlaySound("hurt");

        life.current -= damage;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {

            Ball s = collision.gameObject.GetComponent<Ball>();
            if (s != null)
            {
                TakeDamage(s.damage);
            }
        }
    }

    protected virtual void OnCollisionStay(Collision collision) { }
    protected virtual void OnCollisionExit(Collision collision) { }

    void Flash()
    {
        if (TimeSinceBeginFeedback >= duration) { HitFeedback = false; return; }
        for (int index = 0; index < renderers.Length; index++)
        {
            Material old_mat = new Material(renderers[index].material);
            Material new_mat = new Material(renderers[index].material);
            new_mat.color = color;
            if (TimeSinceBeginFeedback < duration / 5)
            {
                float lerp = 5 * TimeSinceBeginFeedback / duration;
                renderers[index].material.Lerp(old_mat, new_mat, lerp);
            }
            else
            {
                float lerp = 5 * (TimeSinceBeginFeedback - duration / 5) / (duration * 4);
                renderers[index].material.Lerp(new_mat, old_mat, lerp);
            }
        }

    }
}
