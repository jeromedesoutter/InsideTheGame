using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject obj;
    [HideInInspector]
    public GameObject gSpawn;
    public int Number;
    public int RadiusSpawn;


    public  int SpawnObject()
    {
        for (int i = 0; i < Number; i++)
        {
            Vector3 v = Random.insideUnitSphere * RadiusSpawn;
            v += transform.position;
            v.y = transform.position.y;
            gSpawn = Instantiate(obj, v, transform.rotation);
            gSpawn.transform.parent = transform;
        }
        return Number;
    }
}
