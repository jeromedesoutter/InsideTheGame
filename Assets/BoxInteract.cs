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
                // TODO call function in Player that get put new items in inventory
                Debug.Log("New item in inventory : " + bonus.bonus + " x" + bonus.quantity);
            }
        }
    }
}