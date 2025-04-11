using UnityEngine;

public class DiceItem : Item
{
    public override void UseItem()
    {
        ItemManager.Instance.TakeItem(this);
    }
}
