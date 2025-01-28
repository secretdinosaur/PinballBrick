using System;
using UnityEngine;

public class BlackAndWhiteBrick : IBrick
{
    public IBrick GetBlackBrick;
    public IBrick GetWhiteBrick;
    
    public BlackBrick BlackBrick;
    public WhiteBrick WhiteBrick;

    private void OnEnable()
    { 
        BrickType = "BlackAndWhiteBrick";
        
        GetBlackBrick = BrickManager.Instance.SpawnSpecificBrick("BlackBrick");
        GetWhiteBrick = BrickManager.Instance.SpawnSpecificBrick("WhiteBrick");
        BlackBrick = GetBlackBrick.GetComponent<BlackBrick>();
        BlackBrick.BlackAndWhiteBrick = this;
        WhiteBrick = GetWhiteBrick.GetComponent<WhiteBrick>();
        BlackBrick.transform.SetParent(this.transform);
        WhiteBrick.transform.SetParent(this.transform);
    }

    private void Update()
    {
        if (BlackBrick.HP <= 0)
        {
            isBlackBrickDied();
        }

        if (isTouchedBlackHole)
        {
            Debug.Log("Active 1");
            TouchedObject.transform.position = WhiteBrick.transform.position;
            isTouchedBlackHole = false;
        }
    }

    public void isBlackBrickDied()
    {
        if (BlackBrick.RebirthCount > 0)
        {
            BlackBrick.RebirthCount--;
            BrickManager.Instance.TakeBrick(BlackBrick);
            GetBlackBrick = BrickManager.Instance.SpawnSpecificBrick("BlackBrick");
            BlackBrick = GetBlackBrick.GetComponent<BlackBrick>();
        }
        else
        {
            BrickManager.Instance.TakeBrick(BlackBrick);
            BrickManager.Instance.TakeBrick(WhiteBrick);
            BrickDie();
        }
    }
}