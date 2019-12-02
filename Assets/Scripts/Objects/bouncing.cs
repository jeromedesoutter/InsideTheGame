using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class bouncing : MonoBehaviour
{
    public int jumpForce = 13;


    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            collide.gameObject.GetComponent<FirstPersonController>().bounce = jumpForce;
        }
    }
}
