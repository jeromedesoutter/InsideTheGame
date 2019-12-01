using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Bonus
{
    public string bonus;
    public int quantity;
}

public class BonusBox : MonoBehaviour
{
    class BonusFactory
    {
        public static Bonus Create()
        {
            Bonus b = new Bonus();
            switch (Random.Range(0, 3))
            {
                case 1:
                    b.bonus = "potion";
                    b.quantity = Random.Range(2, 4);
                    break;
                case 2:
                    b.bonus = "shield";
                    b.quantity = 1;
                    break;
                default:
                    b.bonus = "bomb";
                    b.quantity = Random.Range(5, 10);
                    break;
            }
            return b;
        }
    };

    public KeyCode keyCode;
    BoxInteract triggerSphere;
    Bonus bonus;

    private void Start()
    {
        triggerSphere = GetComponentInChildren<BoxInteract>();
        bonus = BonusFactory.Create();
    }

    private void Update()
    {
        if (triggerSphere != null && triggerSphere.IsAvailable() && Input.GetKeyDown(keyCode))
        {
            triggerSphere.GiveBonusToPlayer(bonus);
            bonus.quantity = 0;
        }
    }

}
