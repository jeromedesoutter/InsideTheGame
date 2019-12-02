using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravel : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 position;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        position = transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke("Fall", 0.5f);
        }
    }

    private void Fall()
    {
        rb.isKinematic = false;
    }    
}
