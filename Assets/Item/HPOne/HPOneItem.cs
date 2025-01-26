using UnityEngine;

public class HPOneItem : Item
{
    public override void UseItem()
    {
        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            if(!brick.IsBoss && brick.Type != "Shield") 
                brick.HP /= 2;
        }
        ItemManager.Instance.TakeHPOneItem(this);
    }
}
