using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum roomState { none, initialised, finished };
    public RoomManager[] rooms;

    [HideInInspector]
    public ArrayList roomsState;
    [HideInInspector]
    public RoomManager currentRoom;
    [HideInInspector]
    public int currentRoomid;
    [HideInInspector]
    public roomState currentRoomState;
    private int keyNomber;
    private int totalKeyNumber;
    public bool unlockBoss = false;


    // Start is called before the first frame update
    void Start()
    {
        roomsState = new ArrayList();
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].setId(i);
            roomsState.Add(roomState.none);
            if (rooms[i].roomType == RoomManager.roomTypeEnum.extermination || rooms[i].roomType == RoomManager.roomTypeEnum.parcours)
            {
                totalKeyNumber++;
            }
        }
        currentRoom = rooms[0];
        currentRoomState = roomState.finished;
        keyNomber = 0;
       
    }



    public void ChangeCurrentRoom(int id)
    {
        currentRoomid = id;
        currentRoom = rooms[id];
        roomState? r = roomsState[id] as System.Nullable<roomState>;
        currentRoomState = r ?? roomState.none;
    }

    public void ChangeCurrentRoomState(roomState state)
    {
        currentRoomState = state;
        roomsState[currentRoomid] = currentRoomState;
    }

    public void addKey()
    {
        keyNomber++;
        FindObjectOfType<GameController>().WinAKey();
        if (keyNomber == totalKeyNumber)
        {
            unlockBoss = true;
        }
    }

    public void ResetLevel()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            roomsState[i] = roomState.none;
            rooms[i].resetRoom();
        }
        currentRoom = rooms[0];
        currentRoomState = roomState.none;
        keyNomber = 0;
        rooms[0].Initialize();
    }
}
