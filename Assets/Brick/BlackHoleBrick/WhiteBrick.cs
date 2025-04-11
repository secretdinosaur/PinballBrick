using System;
using UnityEngine;

public class WhiteBrick : IBrick
{
    public IBrick GetBlackBrick;
    public BlackBrick BlackBrick;
    public int RebirthCount;
    private void OnEnable()
    {
        RebirthCount = 2;
        base.OnEnable();
        BrickType = "WhiteBrick";
        GetComponents();
        HPText.enabled = true;
        GetBlackBrick = BrickManager.Instance.SpawnSpecificBrick("BlackBrick");
        BlackBrick = GetBlackBrick.GetComponent<BlackBrick>();
        BlackBrick.HP = HP;
        BlackBrick.HpTemp = HP;
        BlackBrick.WhiteBrick = this;
        HP = RebirthCount + 1;
    }

    private void Update()
    {
        if (isTouchedBlackHole)
        {
            TouchedObject.transform.position = this.transform.position;
            isTouchedBlackHole = false;
        }
    }

    public void isBlackBrickDied()
    {
        if (RebirthCount > 0)
        {
            RebirthCount--;
            HP = RebirthCount + 1;
            BlackBrick.HP = HpTemp;
            BlackBrick.HPText.enabled = true;
            BrickManager.Instance.TeleportBrickPosition(BlackBrick);
            BlackBrick = GetBlackBrick.GetComponent<BlackBrick>();
        }
        else
        {
            GetBlackBrick.BrickDie();
            BrickDie();
        }
    }
}