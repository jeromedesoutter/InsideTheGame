using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static float Euclidian3DDistance(Vector3 source, Vector3 target)
    {
        float result = Mathf.Pow(source.x - target.x, 2) + Mathf.Pow(source.y - target.y, 2) + Mathf.Pow(source.z - target.z, 2);
        return Mathf.Sqrt(result);
    }
};
