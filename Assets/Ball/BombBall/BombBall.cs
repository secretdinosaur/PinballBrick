using System;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : IBall
{
    private int boomCount;
    private int BoomCount
    {
        get
        {
            return boomCount;
        }
        set
        {
            boomCount = value;
            if (boomCount < 0)
            {
                boomCount = 0;
            }
        }
    }
    private List<IBrick> NearbyBricks;
    void OnEnable()
    {
        base.OnEnable();
        BoomCount = 4;
        NearbyBricks = new List<IBrick>();
        BallType = "BombBall";
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        BoomCount--;
        if (BoomCount == 0 && collision.gameObject.CompareTag("Brick"))
        {
            NearbyBricks = collision.gameObject.GetComponent<IBrick>().NearbyBricks;
            collision.gameObject.GetComponent<IBrick>().ArbitraryDamageAttack(
                100 * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                false,
                (100 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"),
                "Fire");
            if(NearbyBricks != null)
            {
                foreach (IBrick brick in NearbyBricks)
                {
                    brick.ArbitraryDamageAttack(
                        100 * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                        false,
                        (100 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"),
                        "Fire");
                }
            }
            BallDied();
        }
        else
        {
            BallManager.Instance.BallShowText(this, BoomCount.ToString(), null, false);
        }
    }
}