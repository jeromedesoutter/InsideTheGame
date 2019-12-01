using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitherBoss : IBoss
{

    public WitherBodyPart[] bodyParts;
    public float yPositionWhenWeak = 1.5f;
    public float ySpeedWhenWeak = 0.075f;
    private Vector3 initialPos = Vector3.zero;

    public GameObject skull;
    public float speed = 2f;
    public float rotateSpeed = 2.5f;
    public float minFollowDistance = 2f;
    public float maxShootDistance = 6f;
    public float shootRate = 2f;
    float timeSinceLastShoot = 0f;

    bool beginFight = false;
    bool see_player = false;

    protected override void Awake()
    {
        base.Awake();
        initialPos = transform.position;
    }

    protected override void Start()
    {
        base.Start();
        timeSinceLastShoot = 0f;
    }

    void CheckBeginFight()
    {
        if (beginFight) return;
        beginFight = CheckSeePlayer();
    }
    bool CheckSeePlayer()
    {
        RaycastHit hit;
        Debug.DrawLine(GameObject.Find("BigHead").transform.position, player.transform.position);
        if (Physics.Linecast(GameObject.Find("BigHead").transform.position, player.transform.position, out hit) && hit.collider.transform.tag == "Player")
        {
            beginFight = true;
            if (!see_player) { see_player = true; animator.SetBool("see_player", see_player); }
        }
        else
        {
            if (see_player) { see_player = false; animator.SetBool("see_player", see_player); }
        }
        return see_player;
    }

    protected override void Update()
    {
        base.Update();
        timeSinceLastShoot += Time.deltaTime;
        transform.position = initialPos;

        CheckBeginFight();
        if (!beginFight)
        {
            return;
        }
        
        if (life.current < life.max / 3)
        {
            Vector3 dest = new Vector3(transform.position.x, yPositionWhenWeak, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, dest, ySpeedWhenWeak);
            initialPos = transform.position;
        }

        CheckSeePlayer();

        float distance = Utils.Euclidian3DDistance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z));
        if (see_player)
        {
            Move(distance);
            Shoot(distance);
        }
    }

    void Move(float distance)
    {
        Vector3 dest = player.transform.position;

        dest = (transform.position - player.transform.position).normalized * (life.current < life.max / 3 ? 0.75f : 1f) * minFollowDistance + player.transform.position;

        navmesh.SetDestination(dest);
        Vector3 moveTowardsPosition = dest;
        moveTowardsPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, moveTowardsPosition, (life.current < life.max / 3 ? 2f : 1f) * speed);

        LookAtPlayer();
        initialPos = transform.position;

    }

    void LookAtPlayer()
    {
        Vector3 mtp = player.transform.position;
        mtp.y = transform.position.y;

        if (mtp - transform.position != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(mtp - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void Shoot(float distance)
    {
        if (distance < maxShootDistance && timeSinceLastShoot > (life.current < life.max / 3 ? 0.85f : 1f) / shootRate)
        {
            Instantiate(skull, GameObject.Find("BigHead").transform.position, GameObject.Find("BigHead").transform.rotation);
            timeSinceLastShoot = 0f;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
