using System.Threading;
using UnityEngine;
using System.Collections;
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
        FireDamage = 3f;
        FireDuration = 5f;
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

    public override void AfterAttackEffect(IBrick Brick)
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
        Brick.TotalMultiple = Ball.BounceTime * StatManager.instance.statMultiplier.AttackDamageMultiplier;
        Ball.BounceTime = 1;
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

    public override void AfterAttackEffect(IBrick Brick)
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
        SetAugment("SPECIAL_5", "Critical", "Additional Attack when Attack Hit Critical", "AdditionalAttack", 3);
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

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf)
        {
            if (Brick.HP <= Brick.HpTemp * 0.15)
            {
                if (Brick.BrickType == "ShieldBrick" && GetComponent<ShieldBrick>().IsShieldDestroyed)
                {
                    Brick.ArbitraryDamageAttack(999999, true);
                }
                Brick.ArbitraryDamageAttack(999999, true);
            }
        }
    } 
}

public class EffectCritical : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_7", "EffectCritical", "EffectCritical", "Special", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }
}