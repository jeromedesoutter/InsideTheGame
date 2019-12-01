﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public Sprite sprite;
    public GameObject prefab;
    public int quantity = 0;
};

public class Inventory
{
    List<Item> items;
    int currentIndex = -1;

    public Inventory(Item[] listItems)
    {
        items = new List<Item>();
        foreach(Item item in listItems)
        {
            items.Add(item);
        }
    }

    public Item At(int index)
    {
        if (items.Count < index || index < 0)
            return null;
        else
            return items[index];
    }

    public int Length()
    {
        return items.Count;
    }
    public List<Item> GetItems()
    {
        return items;
    }

    public Item Next()
    {
        if (items.Count == 0)
        {
            return null;
        }
        else
        {
            currentIndex++;
            if(currentIndex>=items.Count) currentIndex = currentIndex % items.Count;
            return Current();
        }
    }

    public Item Previous()
    {
        if (items.Count == 0)
        {
            return null;
        }
        else
        {
            if (currentIndex != -1) { currentIndex--; }
            if (currentIndex < 0) currentIndex = (currentIndex+items.Count) % items.Count;
            return Current();
        }
    }

    public Item Current()
    {
        return At(currentIndex);
    }
};