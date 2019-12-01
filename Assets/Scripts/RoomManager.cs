using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [HideInInspector]
    public int id;

    public GameObject objectsPrefabs;
    public GameObject spawn;
    public LevelManager lvManager;
    public enum roomTypeEnum { extermination, parcours, boss, other};
    public roomTypeEnum roomType;

    public string Objectif;

    public int myTimer = 0;

    public bool unlockDoors;
    private int numberOfEnemis;

    private GameController gcontroller = null;

    //set an Id to each roomManager by the levelManager
    public void setId(int i)
    {
        id = i;
    }

    private void Start()
    {
        //get the levelManager component
        lvManager = GetComponentInParent<LevelManager>();
        gcontroller = FindObjectOfType<GameController>();
        //(un)lock the doors corresponding on the room type
        InitializeRoom();
    }

    private void InitializeRoom()
    {
        switch (roomType)
        {
            case roomTypeEnum.extermination:
            case roomTypeEnum.boss:
            case roomTypeEnum.parcours:
                unlockDoors = false;
                break;
            case roomTypeEnum.other:
                unlockDoors = true;
                StopTimer();
                break;
        }
    }

    public void Initialize()
    {
        //set the room background visible
        objectsPrefabs.SetActive(true);
        //change current room
        lvManager.ChangeCurrentRoom(id);
        //spawn enemies and key if necessary
        if (lvManager.currentRoomState == LevelManager.roomState.none)
        {
            if (roomType == roomTypeEnum.other)
            {
                lvManager.ChangeCurrentRoomState(LevelManager.roomState.finished);
            }
            else
            {
                numberOfEnemis = SpawnEnemies();
                lvManager.ChangeCurrentRoomState(LevelManager.roomState.initialised);
                setTimer(myTimer);
            }
            gcontroller.setInterfaceObjective(Objectif);
        }
        else if (lvManager.currentRoomState == LevelManager.roomState.finished)
        {
            unlockDoors = true;
            StopTimer();
        }
        else if (lvManager.currentRoomState == LevelManager.roomState.initialised)
        {
            if (!(roomType == roomTypeEnum.extermination && numberOfEnemis == 0))
            {
                gcontroller.ActiveTimer(true);
                gcontroller.setInterfaceObjective(Objectif);
            }
        }
        
        
    }

    private int SpawnEnemies()
    {
        int enemies = 0;
        for (int i = 0; i < spawn.transform.childCount; i++)
        {
            Spawn sp = spawn.transform.GetChild(i).GetComponent<Spawn>();
            if (sp != null && sp.obj.CompareTag("Enemy")) {
                enemies += sp.SpawnObject();
            }
            else if (sp != null)
            {
                sp.SpawnObject();
            }
        }
        if (enemies == 0 && roomType == roomTypeEnum.extermination)
        {
            unlockDoors = true;
        }
        Debug.Log("Ennemis number : " + enemies);
        return enemies;
    }

    private void dispawnEnemies()
    {
        GameObject[] enemiesToDispawn = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject g in enemiesToDispawn)
        {
            if (g.transform.IsChildOf(this.transform))
            {
                Destroy(g);
            }           
        }
    }

    private void setTimer(int timer)
    {
        if (timer != 0)
        {
            int minute = timer / 60;
            int seconds = timer % 60;
            gcontroller.setInterfaceTimer(seconds, minute);
        }
    }

    public void StopTimer()
    {
        gcontroller.ActiveTimer(false);
    }

    public void KillEnemy()
    {
        numberOfEnemis--;
        if (numberOfEnemis == 0 && roomType == roomTypeEnum.extermination)
        {
            SpawnKey();
            Debug.Log("room Clear");
         
        }
    }

    public void KillBoss()
    {
        FinishRoom();
    }

    public void FinishRoom()
    {
        gcontroller.setInterfaceObjective("Go to the next room");
        gcontroller.ActiveTimer(false);
        unlockDoors = true;
        lvManager.ChangeCurrentRoomState(LevelManager.roomState.finished);
        if (roomType != roomTypeEnum.boss)
        {
            lvManager.addKey();
        }
    }

    private void SpawnKey()
    {
        for (int i = 0; i < spawn.transform.childCount; i++)
        {
            Spawn sp = spawn.transform.GetChild(i).gameObject.GetComponent<Spawn>();
            if (sp.gSpawn != null && sp.gSpawn.CompareTag("Key"))
            {
                sp.gSpawn.GetComponent<KeyScript>().setKeyVisible();
                gcontroller.setInterfaceObjective("Get the key");
                gcontroller.ActiveTimer(false);
            }
        }
    }

    public void resetRoom()
    {
        dispawnEnemies();
        GameObject[] toReset = GameObject.FindGameObjectsWithTag("Reset");
            foreach (GameObject g in toReset)
            {
                PressurePlate p = g.GetComponent<PressurePlate>();
                if (p != null)
                {
                    p.Reset();
                }
            }
        objectsPrefabs.SetActive(false);
        InitializeRoom();
    }
}
