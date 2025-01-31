using UnityEngine;

public class GoldItem : Item
{
    public override void UseItem()
    {
        StatManager.instance.Persistent_stat.Gold += 10 + (10 * StatManager.instance.Temporary_stat.GoldGain);
        StatManager.instance.SavePersistentStats(StatManager.instance.Persistent_stat);
        Debug.Log("Now Gold is " + StatManager.instance.Persistent_stat.Gold);
        ItemManager.Instance.TakeItem(this);
    }
}