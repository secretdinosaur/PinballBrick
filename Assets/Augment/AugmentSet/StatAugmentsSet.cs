using UnityEngine;

/*
public class Example : StatAugment
{
    private void OnEnable()
    {
        SetAugment("Stat_N", "Example", "Example", 3);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public virtual void BeforeAttackEffect(IBrick Brick) { }
    
    public virtual void AfterAttackEffect(IBrick Brick) { }

    public virtual void CriticalAttackEffect(IBrick Brick) { }
}
*/

public class Atk30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_1", "Atk Up", "Ball Atk 30% Up", 4);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class Atk70Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_2", "Super Atk Up", "Ball Atk 70% Up", 3);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.7f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.7f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChanceUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_3", "Critical Chance Up", "Ball Critical Chance 50% Up", 3);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class CritDamage30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_4", "Critical Damage Up", "Ball Critical Damage 30% Up", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalDamage += 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalDamage -= 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class CritDamage70Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_5", "Super Critical Damage Up", "Ball Critical Damage 70% Up", 3);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalDamage += 0.7f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalDamage -= 0.7f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class BossDamageUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_6", "Boss Additional Damage Up", "Boss Additional Damage 30% Up", 4);
    }

    public override void Apply(){  }

    public override void Remove(){  }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            Brick.TotalMultiple *= 1.3f;
        }
    }
}

public class AtkUpWhenCritical : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_7", "AtkUpWhenCritical", "On Critical Hit, Ball Attack Damage increases for a few seconds", 2);
    }

    public override void Apply()
    { 

    }

    public override void Remove()
    { 

    }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf)
        {
            StatManager.instance.RunAttackDamageUpFewSec(3f, 0.2f);
        }
    }
}

public class EffectCritical : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_8", "EffectCritical", "EffectCritical", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.EffectCriticalDamage += 0.2f;
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.EffectCriticalDamage -= 0.2f;
    }
}