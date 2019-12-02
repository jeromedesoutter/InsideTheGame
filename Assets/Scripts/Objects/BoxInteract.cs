using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteract : Interact
{
    public void GiveBonusToPlayer(Bonus bonus)
    {
        if (bonus.quantity == 0)
        {
            MessageToDisplay = "Bonus already received";
            DisplayMessage(true);
        }
        else
        {
            if (player != null)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().gController.AddBonusToInventory(bonus);      
                Debug.Log("New item in inventory : " + bonus.bonus + " x" + bonus.quantity);
            }
        }
    }
}