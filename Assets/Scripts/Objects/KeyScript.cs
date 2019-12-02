using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{

    private RoomManager room;
    void Start()
    {
        room = GetComponentInParent<RoomManager>();
        RoomManager.roomTypeEnum type = room.roomType;
        switch (type)
        {
            case RoomManager.roomTypeEnum.extermination:
                gameObject.SetActive(false);
                break;
            case RoomManager.roomTypeEnum.parcours:
                gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            room.FinishRoom();
            gameObject.SetActive(false);
            //Add key to the interface
        }
    }

    public void setKeyVisible()
    {
        Debug.Log("Key visible");
        gameObject.SetActive(true);
    }
}
