using UnityEngine;

public class HPOneItem : Item
{
    public override void UseItem()
    {
        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            if(!brick.IsBoss && (brick.BrickType == "Shield" && brick.GetComponent<ShieldBrick>().IsShieldDestroyed)) 
                brick.HP /= 2;
        }
        ItemManager.Instance.TakeItem(this);
    }
}
