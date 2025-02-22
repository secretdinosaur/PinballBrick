using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : IBall
{
    private int BoomCount;
    void OnEnable()
    {
        base.OnEnable();
        BoomCount = 3;
        BallType = "BombBall";
        BallBaseDamage = 0;
        StartCoroutine(StartBombCount());
    }

    IEnumerator StartBombCount()
    {
        while (BoomCount > 0)
        {
            yield return new WaitForSeconds(1f);
            BallManager.Instance.BallShowText(this, BoomCount.ToString(), null, false);
            BoomCount--;
        }
        yield return new WaitForSeconds(1f);
        BallManager.Instance.BallShowText(this, "Boom", null, false);
        if(detectArea.NearbyBricks != null)
        {
            foreach (IBrick brick in detectArea.NearbyBricks.ToArray())
            {
                if(brick.gameObject.activeInHierarchy)
                brick.ArbitraryDamageAttack(
                    100 * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                    false,
                    (100 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"),
                    "Fire");
            }
        }
        BallDied();
    }
}