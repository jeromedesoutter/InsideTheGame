using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject[] gravels;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Reset();
        }
    }

    public void Reset()
    {
        foreach (GameObject gravel in gravels)
        {
            if (gravel.GetComponent<gravel>() != null && gravel.GetComponent<Rigidbody>() != null)
            {
                gravel.transform.position = gravel.GetComponent<gravel>().position;
                gravel.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
