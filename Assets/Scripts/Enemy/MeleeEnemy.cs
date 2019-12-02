using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : IEnemy
{

    // Steering variables
    public Steering steering;
    bool SeePlayer = false;
    float timeSinceLastWanderCommand = 0;
    Vector3 lastDestination;
    Vector3 nextDestination;

    public int attack = 10;
    public float timeBetweenAttack = 1.5f;
    private float delayBeforeAttack = 0f;
    private bool isAttacking = false;
    private Player p = null;
    private Vector3 offsetRay = new Vector3(0, 1.5f, 0);

    //Smooth rotation when enemy sees player
    float rotateSpeed = 3;

    protected override void Start()
    {
        base.Start();
        ShowSphere(debug.showDebugSphere);
        OnFirstNotSeePlayer();
    }
    protected override void Update()
    {
        base.Update();

        if (!IsDead())
        {
            Move();
        }
        if (delayBeforeAttack > 0)
        {
            delayBeforeAttack -= Time.deltaTime;
        }

        if (isAttacking && delayBeforeAttack <= 0 && !IsDead())
        {
            if (p != null)
            {
                p.TakeDamage(attack);
            }
            delayBeforeAttack = timeBetweenAttack;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    void Move()
    {
        debug.debugForwardPoint.transform.position = transform.position + transform.forward;
        bool canFollowPlayer = false;

        // Check if the player is within sight of the enemy and follow if possible
        if (IsPlayerEnoughClose())
        {
            canFollowPlayer = TryToFollowPlayer();
        }

        // If the enemy can follow the player let's make him wander
        if (!canFollowPlayer)
        {
            if (SeePlayer)
            {
                OnFirstNotSeePlayer();
            }
            SeePlayer = false;
            Wander();
        }
    }


    bool IsPlayerEnoughClose() { return Utils.Euclidian3DDistance(transform.position, player.transform.position) < steering.distanceMaxToPlayer; }

    void OnFirstSeePlayer()
    {
        soundsManager.PlaySound("see_player");
    }
    void OnFirstNotSeePlayer()
    {
        timeSinceLastWanderCommand = steering.wander.wanderTimer;
    }

    void ShowSphere(bool show)
    {
        if (show)
        {
            debug.debugSphere.transform.localScale = new Vector3(steering.distanceMaxToPlayer / 2, steering.distanceMaxToPlayer / 2, steering.distanceMaxToPlayer / 2) * 10 * debug.scaleFactor;
        }
        else
        {
            debug.debugSphere.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    bool TryToFollowPlayer()
    {
        // Check if the enemy can see the player
        Vector3 shootRay = player.transform.position;
        if (debug.showRay) { Debug.DrawLine(raycastPoint.position + offsetRay, shootRay); }
        bool canFollowPlayer = false;
        RaycastHit hit;
        if (Physics.Linecast(raycastPoint.position + offsetRay, player.transform.position, out hit) && hit.transform.tag == "Player")
        {
            // Find Player's position and set as destination
            navmesh.SetDestination(player.transform.position);

            canFollowPlayer = true;
            // Player became followable by the enemy
            if (!SeePlayer) { OnFirstSeePlayer(); }
            SeePlayer = true;
        }

        // Follow the player if possible
        if (canFollowPlayer)
        {
            FollowPlayer();
        }

        return canFollowPlayer;
    }

    void FollowPlayer()
    {
        Vector3 moveTowardsPosition = player.transform.position;
        moveTowardsPosition.y = transform.position.y;
        Vector3 lastPos = transform.position;
        if (Vector3.Distance(moveTowardsPosition, transform.position) > 1.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTowardsPosition, steering.speed);
        }
        if (lastPos.Equals(transform.position))
        {
            animator.SetBool("walk", false);
        }
        else
        {
            animator.SetBool("walk", true);
        }

        if (moveTowardsPosition - transform.position != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(moveTowardsPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void Wander()
    {
        timeSinceLastWanderCommand += Time.deltaTime;

        if (timeSinceLastWanderCommand >= steering.wander.wanderTimer)
        {
            Vector3 randomPoint = Random.onUnitSphere * steering.wander.wanderRadius + transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, steering.wander.wanderRadius, NavMesh.AllAreas))
            {
                lastDestination = nextDestination;
                nextDestination = hit.position;
            }
            else
            {
                nextDestination = lastDestination;
            }
            navmesh.SetDestination(nextDestination);
            ResetWanderTimer();
        }
        if (nextDestination != null)
        {
            Vector3 moveTowardsPosition = nextDestination;
            moveTowardsPosition.y = transform.position.y;

            Vector3 lastPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, moveTowardsPosition, steering.speed);

            if (lastPos.Equals(transform.position))
            {
                animator.SetBool("walk", false);
            }
            else
            {
                animator.SetBool("walk", true);
            }

            if (moveTowardsPosition - transform.position != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(moveTowardsPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }


    }

    void ResetWanderTimer() { timeSinceLastWanderCommand = 0; }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
    }

    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }
    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            p = collision.gameObject.GetComponent<Player>();
            animator.SetTrigger("attack");
            isAttacking = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            isAttacking = false;
        }
    }
}
