using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public int currentLevelindex = 0;
    public string levelToLoad = "";

    private void OnTriggerEnter(Collider other)
    {
        if (!levelToLoad.Equals("") && other.gameObject.CompareTag("Player"))
        {
            SaveState();
            SceneManager.LoadScene(levelToLoad);
        }
    }


    void SaveState()
    {
        GameData data = SaveSystem.LoadScores();
        data.lastLevelReached = currentLevelindex + 1;
        data.stats.levelsWon += 1;
        SaveSystem.SaveScores(data);
    }
}
