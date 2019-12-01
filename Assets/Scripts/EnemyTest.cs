using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public int health = 15;
    private RoomManager room;

    private void Start()
    {
        room = GetComponentInParent<RoomManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.tag == "Projectil")
        {
            Debug.Log("Projectil");
            Ball s = collision.gameObject.GetComponent<Ball>();
            health -= s.damage;
            if (health <= 0)
            {
                if (room != null)
                {
                    room.KillEnemy();
                }
                Destroy(gameObject);
            }
        }
    }
}
