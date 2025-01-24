using UnityEngine;

public class BangBangItem : Item
{
    public override void UseItem()
    {
        ItemManager.Instance.CreateBangBang();
        ItemManager.Instance.TakeBangBangItem(this);
    }
}
