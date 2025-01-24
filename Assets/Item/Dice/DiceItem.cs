using UnityEngine;

public class DiceItem : Item
{
    public override void UseItem()
    {
        DiceManager.Instance.RollDice(1);
        ItemManager.Instance.TakeItem(this);
    }
}
