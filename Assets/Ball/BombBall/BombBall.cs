using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombBall : IBall
{
    private int BoomCount;
    void OnEnable()
    {
        base.OnEnable();
        BoomCount = 3;
        BallType = "BombBall";
        BallBaseDamage = 0;
        StartCoroutine(StartBoomCount());
    }

    IEnumerator StartBoomCount()
    {
        while (BoomCount > 0)
        {
            yield return new WaitForSeconds(1f);
            BallManager.Instance.BallShowText(this, BoomCount.ToString(), null, false);
            BoomCount--;
        }
        yield return new WaitForSeconds(1f);
        BallManager.Instance.BallShowText(this, "Boom", null, false);
        if(detectArea.BoomArea != null)
        {
            foreach (IBrick brick in detectArea.BoomArea.ToArray())
            {
                if(brick.gameObject.activeInHierarchy)
                brick.ArbitraryDamageAttack(
                    (100 + StatManager.instance.Persistent_stat.EffectDamage + StatManager.instance.Temporary_stat.EffectDamage) 
                    * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                    false,
                    ((100 + StatManager.instance.Persistent_stat.EffectDamage + StatManager.instance.Temporary_stat.EffectDamage)
                    * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"),
                    "Explosion");
            }
        }
        VisualEffectManager.Instance.SpawnParticle("BombBall", this.transform.position, 1);
        if (AugmentManager.instance.determineAugmentID("SPECIAL_48"))
        {
            if(Random.value < 0.5f)
            {
                BallManager.Instance.CreateSpecificBall(1, "BombBall");
            }
        }
        BallDied();
    }
}