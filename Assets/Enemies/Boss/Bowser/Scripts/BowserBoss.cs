using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowserBoss : IBoss
{

    public BowserBodyPart[] bodyParts;

    public Transform SeePoint;

    public float WalkSpeed = 1f;
    public float RunSpeed = 2f;
    public float RotateSpeed = 35f;
    public bool IsRunning = false;
    public bool SeePlayer = false;
    public bool FarFromPlayer = true;
    public bool ToDistanceFromPlayer = true;

    protected override void Start()
    {
        base.Start();
        IsRunning = false;
        SeePlayer = false;
        FarFromPlayer = true;
    }

    protected override void Update()
    {
        base.Update();
        CheckFarFromPlayer();
        CheckSeePlayer();
    }

    bool CheckFarFromPlayer()
    {
        float distance = Utils.Euclidian3DDistance(transform.position, player.GetComponent<CharacterController>().ClosestPoint(transform.position));
        float factor = (IsRunning ? 370 : 450);
        if (distance < Mathf.Max(transform.localScale.x / factor, transform.localScale.z / factor))
        {
            FarFromPlayer = false;
        }
        else
        {
            FarFromPlayer = true;
        }
        if (distance < Mathf.Max(transform.localScale.x / 400, transform.localScale.z / 400)*2)
        {
            ToDistanceFromPlayer = false;
        }
        else
        {
            ToDistanceFromPlayer = true;
        }
        return FarFromPlayer;
    }

    bool CheckSeePlayer()
    {
        RaycastHit hit;
        if (!FarFromPlayer)
        {
            Debug.DrawLine(SeePoint.position, player.transform.position, Color.red);
        }
        else if(!ToDistanceFromPlayer)
        {
            Debug.DrawLine(SeePoint.position, player.transform.position, Color.green);
        }
        else
        {
            Debug.DrawLine(SeePoint.position, player.transform.position);
        }

        if (Physics.Linecast(SeePoint.position, player.transform.position, out hit) && hit.collider.tag == "Player")
        {
            SeePlayer = true;
        }
        else
        {
            SeePlayer = false;
        }
        return SeePlayer;
    }

    public void Move()
    {
        Vector3 dest = player.transform.position;
        navmesh.SetDestination(dest);
        Vector3 moveTowardsPosition = dest;
        moveTowardsPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, moveTowardsPosition, (life.current < life.max / 3 ? 2f : 1f) * (IsRunning ? RunSpeed:WalkSpeed) * Time.deltaTime);

        LookAtPlayer();

    }

    void LookAtPlayer()
    {
        Vector3 mtp = player.transform.position;
        mtp.y = transform.position.y;

        if (mtp - transform.position != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(mtp - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
