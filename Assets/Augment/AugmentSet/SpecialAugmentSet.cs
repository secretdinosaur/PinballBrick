using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
public class Example : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_N", "Example", "Example", "Example", 3);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public virtual void BeforeAttackEffect(IBrick Brick) { }
    
    public virtual void AfterAttackEffect(IBrick Brick) { }

    public virtual void CriticalAttackEffect(IBrick Brick) { }
}
 */

public class FireBall : SpecialAugment
{
    private float FireDamage; //EffectDamage
    private float FireDuration; //EffectDuration

    private void OnEnable()
    {
        SetAugment("SPECIAL_1", "FireBall", "Time to Cast Magic", "Effect", 3);
        FireDamage = 0.5f;
        FireDuration = 0.5f;
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.EffectDamage += FireDamage;
        StatManager.instance.Temporary_stat.EffectDuration += FireDuration;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.EffectDamage -= FireDamage;
        StatManager.instance.Temporary_stat.EffectDuration -= FireDuration;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            Brick.EffectStack += (Brick.EffectStack != 5) ? 1 : 0;
            StartCoroutine(Brick.DamageByEffect(StatManager.instance.Temporary_stat.EffectDuration));
        }
    }
}
public class Homing : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_2", "Homing Ball", "Lock On!", "Homing", 1);
    }
    public override void Apply()
    { 
        
    }

    public override void Remove(){  }
}
public class BouncePower: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_3", "Bounce = Power", "Get Stronger with Bounces", "Bounce", 3);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.Ball.BounceTime * StatManager.instance.statMultiplier.AttackDamageMultiplier;
        Brick.Ball.BounceTime = 1;
    }
}

public class AdditionalThreeAttack: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_4", "Ninja", "Blink of an Eye", "AdditionalAttack", 1);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            StartCoroutine(Brick.AdditionalAttack(0.3f, 0.1f));
        }
    }
}

public class CriticalAdditionalAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_5", "CriticalAdditionalAttack", "CriticalAdditionalAttack", "AdditionalAttack", 4);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf) //Critical Additional Attack
        {
            Brick.AdditionalAttack(0.5f, 0.5f);
        }
    }
}

public class Execution : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_6", "Execution", "Execution", "Special", 1);
    }

    public override void Apply(){ }

    public override void Remove(){ }
}

public class PercentDamage : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_7", "Percent Damage", "Percent Damage", "Special", 2);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf)
        { 
            Brick.ArbitraryDamageAttack(Brick.HpTemp * 0.01f, false, "1%");
        }
    }
}

public class EffectStackBlast : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_8", "EffectStackBlast", "EffectStackBlast", "Effect", 3);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.EffectStack >= 5)
        {
            Brick.EffectStack -= 3;
            foreach (IBrick nearbyBrick in Brick.NearbyBricks)
            {
                nearbyBrick.EffectStack += (nearbyBrick.EffectStack != 5) ? 2 : 0;
                StartCoroutine(nearbyBrick.DamageByEffect(StatManager.instance.Temporary_stat.EffectDuration));
            }
        }
    }
}

public class ChainLightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_9", "ChainLightning", "ChainLightning", "AdditionalAttack", 3);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        List<IBrick> OrderedTargetBricks = new List<IBrick>();

        IBrick targetBrick = Brick;
        if (Brick.NearbyBricks.Count == 0)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            List<IBrick> BrickToBrick = targetBrick.NearbyBricks;
            OrderedTargetBricks.Add(targetBrick);

            if (BrickToBrick.Count == 0)
            {
                break;
            }
            int RandomNumber = Random.Range(0, BrickToBrick.Count);
            BrickToBrick[RandomNumber].ArbitraryDamageAttack(1 * StatManager.instance.statMultiplier.EffectDamageMultiplier, 
                false, (1 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"));
            BrickToBrick[RandomNumber].NearbyBricks.Remove(targetBrick);

            targetBrick = BrickToBrick[RandomNumber];

        }
        VisualEffectManager.Instance.SpawnChainLightning(OrderedTargetBricks);
    }
}

public class DeathExp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_10", "Death = Exp", "Death = Exp", "Death", 5);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect()
    {
        LevelManager.Instance.Exp += 30 * StatManager.instance.Persistent_stat.BonusExp;
    }
}

public class DeathGold : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_11", "Death = Gold", "Death = Gold", "Death", 5);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect()
    {
        StatManager.instance.Persistent_stat.Gold += 18 + (18 * StatManager.instance.Temporary_stat.GoldGain);
    }
}

public class SpeedPower : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_12", "Speed = Power", "Speed = Power", "Speed", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.Ball.TransformationMultiple * StatManager.instance.statMultiplier.AttackDamageMultiplier;
    }
}

public class SpeedCritical : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_13", "Speed = Critical Chance", "Speed = Critical Chance", "Speed", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.Ball.TransformationMultiple * StatManager.instance.statMultiplier.AttackDamageMultiplier;

    }
}

public class Lightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_14", "Lightning", "Lightning", "Lightning", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterAttackEffect(IBrick brick)
    {
        brick.ArbitraryDamageAttack(1 * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                false, (1 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"));
        VisualEffectManager.Instance.SpawnParticle("Lightning", brick.transform.position, 1);

    }
}