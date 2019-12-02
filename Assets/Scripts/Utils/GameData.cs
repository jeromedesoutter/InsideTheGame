using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int lastLevelReached;
    public Statistics stats;

    public GameData()
    {
        lastLevelReached = 1;
        stats = new Statistics();
    }
};