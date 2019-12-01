using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public RoomManager roomManagerToGo;
    public LevelManager lvManager;
    public Transform playerSpawnPoint;
    public bool isBoss = false;
    public bool isBonus = false;

    private bool isTrigger = false;
    private CharacterController player;
    private RoomManager myRoomManager;


    private void Start()
    {
        lvManager = GetComponentInParent<LevelManager>();
        myRoomManager = GetComponentInParent<RoomManager>();
    }

    private void Update()
    {
        if (isTrigger && player != null && Input.GetKeyDown("x") && (myRoomManager.unlockDoors || isBonus) && !isBoss)
        {
            GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Prefab");
            if (!isBonus)
            {
                myRoomManager.resetRoom();
            }
            else
            {
                myRoomManager.StopTimer();
            }
            roomManagerToGo.Initialize();
            player.enabled = false;
            player.transform.position = playerSpawnPoint.position;
            player.enabled = true;
            isTrigger = false;
        }
        else if (isTrigger && player != null && Input.GetKeyDown("x") && lvManager.unlockBoss && isBoss)
        {
            myRoomManager.resetRoom();
            roomManagerToGo.Initialize();
            player.enabled = false;
            player.transform.position = playerSpawnPoint.position;
            player.enabled = true;
            isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTrigger = true;
            player = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTrigger = false;
        }
        }
}
