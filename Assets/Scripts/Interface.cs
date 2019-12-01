using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public GameObject lifebar;
    double lifeBarWidth;
    public Text lifeTextValue;
    public GameObject shieldBar;
    double shieldBarWidth;

    public GameObject[] keys;

    public Sprite goldKey;
    public Sprite silverKey;

    public Text objective;
    public Timer chronoScript;

    public Image inventoryCurrentObject;

    public void Start()
    {
        lifeBarWidth = lifebar.GetComponent<RectTransform>().rect.width;
        shieldBarWidth = shieldBar.GetComponent<RectTransform>().rect.width;
    }

    public void UpdateLife(float percentageLife, double valueLife)
    {
        lifebar.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(Mathf.Max(0,Mathf.Min(1,percentageLife)) * lifeBarWidth), 0f);
        lifeTextValue.text = ((int)valueLife).ToString();
    }
    public void UpdateShield(float percentageShield)
    {
        shieldBar.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(Mathf.Max(0,Mathf.Min(1,percentageShield)) * shieldBarWidth), shieldBar.GetComponent<RectTransform>().rect.height);
    }

    public void UpdateKeys(int numberOfKeysWon)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (i < numberOfKeysWon)
            {
                keys[i].GetComponent<Image>().sprite = silverKey;
            }
            else
            {
                keys[i].GetComponent<Image>().sprite = goldKey;
            }
        }
    }

    public void SetCurrentObject(Item item)
    {
        if(item.quantity == 0)
        {
            inventoryCurrentObject.color = new Color(255, 255, 255, 0.5f);
        }
        else
        {
            inventoryCurrentObject.color = new Color(255, 255, 255, 1f);
        }
        inventoryCurrentObject.sprite = item.sprite;
    }

    public void HideInventory()
    {
        inventoryCurrentObject.gameObject.SetActive(false);
    }
}
