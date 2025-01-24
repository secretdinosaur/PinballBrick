using UnityEngine;

public class HPOneItem : Item
{
    public override void UseItem()
    {
        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            if (brick.Type == "Shield") brick.GetComponent<ShieldBrick>().ShieldCount = 0;
            brick.HP = 1;
        }
        ItemManager.Instance.TakeHPOneItem(this);
    }
}
