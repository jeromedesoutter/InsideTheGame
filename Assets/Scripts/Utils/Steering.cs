using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Steering
{
    public float distanceMaxToPlayer = 3f;
    public float speed = 0.01f;
    // Wander variables
    public Wandering wander;
};

[System.Serializable]
public class Wandering
{
    public float wanderRadius = 1f;
    public float wanderTimer = 3f;
};
