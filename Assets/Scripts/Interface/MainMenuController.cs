using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Statistics
{
    public int roomsWon = 0;
    public int roomsLost = 0;
    public int levelsWon = 0;
    public int objectsPickedUp = 0;

};

public class MainMenuController : MonoBehaviour
{
    int levelReachedInPreviousGame = 1;
    Statistics stats;

    public Text roomsWon;
    public Text roomsLost;
    public Text levelsWon;
    public Text objectsPickedUp;

    void Start()
    {
        LoadData();
        UpdateStats();
    }

    void LoadData()
    {
        GameData data = SaveSystem.LoadScores();
        stats = data.stats;
        levelReachedInPreviousGame = data.lastLevelReached;
    }

    void UpdateStats()
    {
        roomsWon.text = stats.roomsWon.ToString();
        roomsLost.text = stats.roomsLost.ToString();
        levelsWon.text = stats.levelsWon.ToString();
        objectsPickedUp.text = stats.objectsPickedUp.ToString();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(levelReachedInPreviousGame);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Scenes/Minecraft");
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
