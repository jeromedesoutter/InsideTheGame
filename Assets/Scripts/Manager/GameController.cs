using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


public class GameController : MonoBehaviour
{

    public GameObject menuPause;
    bool isPaused = false;
    public GameObject menuGameOver;


    public GameObject gameInterface;
    Interface interfaceScript;

    public Player player;
    public LevelManager lvManager;

    public int numberOfKeysWon = 0;

    public Item[] itemsAtBeginning;
    Inventory inventory;

    private EventSystem es;

    private float delayBeforeTimeDamage = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        es = EventSystem.current;

        interfaceScript = gameInterface.GetComponent<Interface>();
        numberOfKeysWon = 0;
        inventory = new Inventory(itemsAtBeginning);
        if (inventory.Length() != 0)
        {
            interfaceScript.SetCurrentObject(inventory.Next());
        }
        else
        {
            interfaceScript.HideInventory();
        }
        interfaceScript.UpdateKeys(numberOfKeysWon);
        UIFocus(false);
    }

    public void GameOver()
    {
        menuGameOver.SetActive(true);
        UIFocus(true);
        SaveState(false);
        EventSystem.current.SetSelectedGameObject(menuGameOver);
    }

    void SaveState(bool win = true)
    {
        GameData data = SaveSystem.LoadScores();
        if (win) data.stats.roomsWon += 1;
        else data.stats.roomsLost += 1;
        SaveSystem.SaveScores(data);
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (!isPaused && Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        if(isPaused && Input.GetKeyDown(KeyCode.UpArrow))
        {
            ContinueGame();
        }
        if(isPaused && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Quit();
        }

        if (delayBeforeTimeDamage > 0)
        {
            delayBeforeTimeDamage -= Time.deltaTime;
        }

        if (interfaceScript.chronoScript.timeOver && delayBeforeTimeDamage <=0)
        {
            player.TakeDamage(1);
            delayBeforeTimeDamage = 1;
        }

        interfaceScript.UpdateLife((float)(player.life.current / player.life.max), Mathf.Min((float)player.life.current, (float)player.life.max));
        interfaceScript.UpdateShield((float)(player.shield.current / player.shield.max));
    }


    void Pause()
    {
        menuPause.SetActive(true);
        isPaused = !isPaused;
        UIFocus(true);
        EventSystem.current.SetSelectedGameObject(menuPause);
    }

    public void ContinueGame()
    {
        UIFocus(false);
        menuPause.SetActive(false);
        isPaused = !isPaused;
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void WinAKey()
    {
        numberOfKeysWon++;
        interfaceScript.UpdateKeys(numberOfKeysWon);
    }

    private void UIFocus(bool focus)
    {
        if (focus)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.GetComponent<FirstPersonController>().mouseLookEnabled = false;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.GetComponent<FirstPersonController>().mouseLookEnabled = true;
            EventSystem.current = es;
        }
    }

    public void PreviousObject()
    {
        Item item = inventory.Previous();
        interfaceScript.SetCurrentObject(item);
    }
    public void NextObject()
    {
        Item item = inventory.Next();
        interfaceScript.SetCurrentObject(item);
    }

    public void setInterfaceTimer(int second, int minute)
    {
        interfaceScript.chronoScript.ResetTimer(second, minute);
    }

    public void setInterfaceObjective(string s)
    {
        interfaceScript.objective.text = s;
    }

    public void ActiveTimer(bool b)
    {
        if (b && interfaceScript != null) {
            interfaceScript.chronoScript.ContinueTimer();
        }
        else if (interfaceScript != null)
        {
            interfaceScript.chronoScript.StopTimer();
        }
    }

    public void setHitFeedback()
    {
        interfaceScript.PlayerHit();
    }
}
