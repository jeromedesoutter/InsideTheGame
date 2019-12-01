using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotateSpeed = 35.0f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        transform.position += transform.forward*moveVertical* speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, moveHorizontal, 0) * rotateSpeed*Time.deltaTime);

        /*

        rb.AddForce(transform.forward * moveVertical * speed, ForceMode.Force);

        transform.Rotate(new Vector3(0, moveHorizontal, 0) * rotationSpeed * Time.deltaTime);*/
    }
}
