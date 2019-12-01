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

    public Image hitPanel;
    public bool hitFlash;
    private bool resetFlash;
    private float TimeSinceBeginFeedback;
    public float duration = 0.3f;

    public void Start()
    {
        lifeBarWidth = lifebar.GetComponent<RectTransform>().rect.width;
        shieldBarWidth = shieldBar.GetComponent<RectTransform>().rect.width;
        TimeSinceBeginFeedback = 0;
        resetFlash = false;
        hitFlash = false;
    }

    private void Update()
    {
        TimeSinceBeginFeedback += Time.deltaTime;
        if (resetFlash)
        {
            resetFlash = false;
            TimeSinceBeginFeedback = 0;
        }
        if (hitFlash)
        {
            UpdateFlash();
        }
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

    public void PlayerHit()
    {
        if (hitPanel != null)
        {
            hitFlash = true;
            resetFlash = true;
        }
    }

    void UpdateFlash()
    {
        Debug.Log("in flash " + TimeSinceBeginFeedback);
        if (TimeSinceBeginFeedback >= duration) { hitFlash = false; return; }

        Color oldColor = new Color(1, 0, 0, 0);
        Color newColor = new Color(1, 0, 0, 0.2f);
        
            
            if (TimeSinceBeginFeedback < duration / 5)
            {
                float lerp = 5 * TimeSinceBeginFeedback / duration;
                hitPanel.color = Color.Lerp(oldColor, newColor, lerp);
            }
            else
            {
                float lerp = 5 * (TimeSinceBeginFeedback - duration / 5) / (duration * 4);
                hitPanel.color = Color.Lerp(newColor, oldColor, lerp);
            }

    }
}
