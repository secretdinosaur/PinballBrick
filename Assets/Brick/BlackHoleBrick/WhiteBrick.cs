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

        GetBlackBrick = BrickManager.Instance.SpawnSpecificBrick("BlackBrick");
        BlackBrick = GetBlackBrick.GetComponent<BlackBrick>();
        BlackBrick.WhiteBrick = this;
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
            BlackBrick.HP = HpTemp;
            BrickManager.Instance.TeleportBrickPosition(BlackBrick);
            BlackBrick = GetBlackBrick.GetComponent<BlackBrick>();
        }
        else
        {
            GetBlackBrick.BrickDie();
            this.BrickDie();
        }
    }
}