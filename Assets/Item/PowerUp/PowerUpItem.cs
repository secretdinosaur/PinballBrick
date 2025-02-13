using UnityEngine;
using System.Collections;
public class PowerUpItem : Item
{
    public override void UseItem()
    {
        StartCoroutine(ItemManager.Instance.UsePowerUpItem(this));
    }
}
