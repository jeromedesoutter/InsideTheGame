using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BoxCollider))]
public class MovingPlatform : MonoBehaviour
{
    Vector3[] points;
    Vector3 current_start;
    Vector3 current_dest;
    int current_index;
    bool canMove;
    public float Speed = 0.01f;
    Vector3 last_translation;

    GameObject player;
    bool playerOnPlatform;

    void Start()
    {
        points = GetComponentsInChildren<Transform>().Select<Transform, Vector3>(x => x.position).ToArray();
        player = GameObject.FindGameObjectWithTag("Player");
        playerOnPlatform = false;

        current_index = 0;
        last_translation = Vector3.zero;
        if(points.Length > 0)
        {
            current_start = points[current_index];
            current_dest = points[current_index];
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    void Update()
    {
        if (canMove) MovePlatform();
    }

    void MovePlatform()
    {
        if (Utils.Euclidian3DDistance(current_start,transform.position)>=Utils.Euclidian3DDistance(current_start,current_dest))
        {
            current_index = (current_index + 1) % points.Length;
            current_start = current_dest;
            current_dest = points[current_index];
        }
        Vector3 old_pos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, current_dest, Speed);
        last_translation = transform.position - old_pos;
    }

    public Vector3 GetTranslation()
    {
        return last_translation;
    }
}

//Add in FirstCharacterController

/*

    As attribute:

            private MovingPlatform platform = null;


    In private void Update(): (At the end of the function)

            if(platform != null)
            {
                m_CharacterController.enabled = false;
                transform.position = transform.position + platform.GetTranslation();
                m_CharacterController.enabled = true;
            }


    In private void OnControllerColliderHit(ControllerColliderHit hit): (At the beginning of the function)

        if (hit.collider.CompareTag("MovingPlatform"))
        {
            platform = hit.collider.gameObject.GetComponent<MovingPlatform>();
        }
        else
        {
            platform = null;
        }




 */
