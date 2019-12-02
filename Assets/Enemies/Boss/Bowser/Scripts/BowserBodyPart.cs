﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BowserBodyPartEnum
{
    UPLEG,
    LEG,
    FOOT,
    FOREARM,
    ARM,
    FIST,
    HEAD,
    BODY
};

public class BowserBodyPart : MonoBehaviour
{
    public BowserBodyPartEnum bodyPart;
    public BowserBoss boss;

    float TimeSinceLastDamage = 0;
    float DelayBetweenDamages = .5f;

    private void Start()
    {
        boss = GetComponentInParent<BowserBoss>();
        TimeSinceLastDamage = DelayBetweenDamages;
    }
    private void Update()
    {
        TimeSinceLastDamage += Time.deltaTime;
    }

    public int GetDamage()
    {
        if (TimeSinceLastDamage < DelayBetweenDamages) { return 0; }

        TimeSinceLastDamage = 0;
        switch (bodyPart)
        {
            case BowserBodyPartEnum.FOREARM:
            case BowserBodyPartEnum.UPLEG:
                return 4;
            case BowserBodyPartEnum.ARM:
            case BowserBodyPartEnum.LEG:
                return 6;
            case BowserBodyPartEnum.FOOT:
            case BowserBodyPartEnum.FIST:
                return 8;
            case BowserBodyPartEnum.HEAD:
                return 5;
            default:
                return 2;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            boss.TakeDamage(3f);
        }
    }
}