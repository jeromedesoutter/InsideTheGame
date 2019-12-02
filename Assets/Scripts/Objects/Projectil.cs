using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectil : MonoBehaviour
{
    public int lifetime = 3;
    public int damage = 5;
    public string creatorName = "";
    void Start()
    {
        Invoke("DestroyProjectil", lifetime);
    }

    public void DestroyProjectil()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != creatorName)
        {
            Destroy(gameObject);
        }
    }
}
