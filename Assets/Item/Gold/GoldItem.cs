using UnityEngine;

public class GoldItem : Item
{
    public override void UseItem()
    {
        StatManager.instance.GetGold(10);
        StatManager.instance.SavePersistentStats(StatManager.instance.Persistent_stat);
        Debug.Log("Now Gold is " + StatManager.instance.Persistent_stat.Gold);
        ItemManager.Instance.TakeItem(this);
    }
}