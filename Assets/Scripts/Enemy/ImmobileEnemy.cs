using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using System;

public class ImmobileEnemy : IEnemy
{

    // Steering variables
    public float delayBetweenChangingOrientation = 2f;
    public float distanceMaxToPlayer = 4f;
    bool SeePlayer = false;
    float timeSinceLastWanderCommand = 0;
    bool canFollowPlayer = false;

    // en degrés/seconde
    public Transform ToRotate;
    public float rotateSpeed = 30f;
    public float minAngle = 0f;
    public float maxAngle = 360f;
    float destinationRotationAngle = 0f;

    //Shoot properties
    public GameObject prefabProjectil;
    public float projectilSpeed = 10f;
    float timeSinceLastShoot = 0;
    public float shootRatePerSecond = .5f;
    public Transform shootSource;

    private Vector3 initialPosition = Vector3.zero;
    private Vector3 initialPos = Vector3.zero;

    private Vector3 offsetRay = new Vector3(0, 1, 0);

    protected override void Start()
    {
        initialPosition = ToRotate.position;
        initialPos = transform.position;
        base.Start();
        if (minAngle > maxAngle)
        {
            Debug.Log("Error Min Max Angles -> Set default");
            minAngle = 0f;
            maxAngle = 360f;
        }
        timeSinceLastShoot = 0;
        ShowSphere(debug.showDebugSphere);
    }
    protected override void Update()
    {
        base.Update();

        ToRotate.position = initialPosition;
        transform.position = initialPos;

        if (!IsDead())
        {
            Move();
            timeSinceLastShoot += Time.deltaTime;
            if (canFollowPlayer && timeSinceLastShoot > 1f/shootRatePerSecond)
            {
                Shoot();
            }
        }
    }
    
    void Move()
    {
        debug.debugForwardPoint.transform.position = transform.position + transform.forward;
        canFollowPlayer = false;

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

    async void Shoot()
    {
        animator.SetTrigger("attack");
        timeSinceLastShoot = 0;
        await Task.Delay(TimeSpan.FromSeconds(0.4f * 1f / shootRatePerSecond));
        Rigidbody instantiatedProjectile = Instantiate(prefabProjectil, shootSource.position, shootSource.rotation).GetComponent<Rigidbody>();
        instantiatedProjectile.transform.LookAt(player.transform);
        instantiatedProjectile.freezeRotation = true;
        instantiatedProjectile.velocity = (player.transform.position - shootSource.position).normalized*projectilSpeed;
        instantiatedProjectile.gameObject.GetComponent<Projectil>().creatorName = gameObject.name;
        soundsManager.PlaySound("shoot");
        timeSinceLastShoot = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    bool IsPlayerEnoughClose() { return Utils.Euclidian3DDistance(transform.position, player.transform.position) < distanceMaxToPlayer; }

    void OnFirstSeePlayer()
    {
        animator.SetBool("see_the_player", true);
        soundsManager.PlaySound("see_player");
    }
    void OnFirstNotSeePlayer()
    {
        animator.SetBool("see_the_player", false);
        soundsManager.PlaySound("see_player");
        timeSinceLastWanderCommand = delayBetweenChangingOrientation;
    }

    void ShowSphere(bool show)
    {
        if (show)
        {
            debug.debugSphere.transform.localScale = new Vector3(distanceMaxToPlayer / 2, distanceMaxToPlayer / 2, distanceMaxToPlayer / 2) * 10 * debug.scaleFactor;
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

        if (moveTowardsPosition - transform.position != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(moveTowardsPosition - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * 0.1f * Time.deltaTime);
        }
    }

    void Wander()
    {
        timeSinceLastWanderCommand += Time.deltaTime;

        if (timeSinceLastWanderCommand >= delayBetweenChangingOrientation)
        {
            destinationRotationAngle = UnityEngine.Random.Range(minAngle, maxAngle);
            navmesh.SetDestination(ToRotate.position);
            ToRotate.rotation = Quaternion.Euler(ResetAngle());
            ResetTimer();
        }
        else
        {
            ToRotate.rotation = Quaternion.RotateTowards(
                ToRotate.rotation,
                Quaternion.Euler(new Vector3(0, destinationRotationAngle, 0)),
                Mathf.Min(rotateSpeed * Time.deltaTime, Mathf.Abs(ToRotate.rotation.eulerAngles.y - destinationRotationAngle))
                );
        }
    }

    void ResetTimer() { timeSinceLastWanderCommand = 0; }
    Vector3 ResetAngle()
    {
        Vector3 rot = ToRotate.rotation.eulerAngles;
        if (rot.y < 0) rot.y += 360f * ((int)Mathf.Abs(rot.y) / 360f);
        rot.y = (rot.y + 360) % 360;
        return rot;
    }

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
}

