using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{

    public float fireForce = 20f;
    public float fireRate = 2f;
    public float fireHeightOffset = 0.2f;
    private Vector3 fireOffset;

    public GameObject projectilPrefab;
    public Transform shotPos;
    public GameObject ballInHand;

    private float nextShoot = 0f;


    private void Start()
    {
        fireOffset = new Vector3(0, fireHeightOffset, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (nextShoot != 0)
        {
            nextShoot -= Time.deltaTime;
        }
        if (Input.GetMouseButton(0) && nextShoot <= 0)
        {
            Fire();
            nextShoot = 1 / fireRate;
        }
    }

    void Fire()
    {
        GameObject projectil = Instantiate(projectilPrefab, shotPos.position, shotPos.rotation);
        Rigidbody rb =  projectil.GetComponent<Rigidbody>();
        rb.AddForce((transform.forward + fireOffset) * fireForce, ForceMode.VelocityChange);
        ballInHand.SetActive(false);
        Invoke("BallVsible", 0.2f);
    }

    private void BallVsible()
    {
        ballInHand.SetActive(true);
    }
}
