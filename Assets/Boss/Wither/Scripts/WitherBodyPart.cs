using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WitherBodyPartEnum
{
    BIGHEAD,
    SMALLHEAD,
    HEADBAR,
    RIBS,
    SPINE
};

public class WitherBodyPart : MonoBehaviour
{
    public WitherBodyPartEnum bodyPart;
    public WitherBoss boss;
    
    void Start()
    {
        boss = GetComponentInParent<WitherBoss>();
    }

    public int GetDamage()
    {
        switch (bodyPart)
        {
            case WitherBodyPartEnum.BIGHEAD:
                return 8;
            case WitherBodyPartEnum.SMALLHEAD:
                return 5;
            default:
                return 3;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectil"))
        {
            boss.TakeDamage(3f);
            Debug.Log("Hit");
        }
    }

}
