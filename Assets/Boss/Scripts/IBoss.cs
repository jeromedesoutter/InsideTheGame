using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class IBoss : MonoBehaviour
{
    public GameObject player;

    public NavMeshAgent navmesh;
    public Rigidbody rb;
    public Animator animator;

    // Life
    public Life life;
    public float delayBeforeDisappearingWhenDead = 5f;
    bool AlreadyDead = false;


    //Color on damage
    public Color color = Color.red;
    public float duration = 0.7f;
    Renderer[] renderers;
    bool HitFeedback = false;
    float TimeSinceBeginFeedback = 0;

    protected virtual void Awake() { }
    protected virtual void Start()
    {
        life.current = life.max;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
        navmesh = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        navmesh.speed = 0.01f;
        player = GameObject.FindGameObjectWithTag("Player");
        renderers = gameObject.GetComponentsInChildren<Renderer>();
    }

    protected virtual void Update()
    {
        
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
        AlreadyDead = true;
        await Task.Delay(TimeSpan.FromSeconds(delayBeforeDisappearingWhenDead));
        Destroy(gameObject);
        GetComponentInParent<RoomManager>().KillBoss();
    }

    public void TakeDamage(float damage)
    {
        life.current -= Mathf.Min(damage, life.current);
        if (IsDead())
        {
            if (!AlreadyDead)
            {
                if (animator != null)
                {
                    animator.SetTrigger("die");
                    animator.SetBool("dead", true);
                }
                Die();
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            HitFeedback = true;
            TimeSinceBeginFeedback = 0;

            Ball s = collision.gameObject.GetComponent<Ball>();
            TakeDamage(s.damage);
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
