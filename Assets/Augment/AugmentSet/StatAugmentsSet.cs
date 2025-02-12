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
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class Atk50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_2", "Super Atk Up", "Ball Atk 50% Up", 2);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.5f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.5f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChance50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_3", "Super Critical Chance Up", "Ball Critical Chance 50% Up", 2);
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

public class CritDamage50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_5", "Super Critical Damage Up", "Ball Critical Damage 50% Up", 2);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalDamage += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalDamage -= 0.5f;
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
        SetAugment("STAT_7", "AtkUpWhenCritical", "AtkUpWhenCritical", 4);
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

public class Atk10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_9", "Weak Atk Up", "Ball Atk 10% Up", 5);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.1f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.1f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChance10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_10", "Weak Critical Chance Up", "Ball Critical Chance 10% Up", 5);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class CritChance20Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_11", "Critical Chance Up", "Ball Critical Chance 20% Up", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.2f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.2f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class GoldGain10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_12", "Weak Gold Gain Up", "Gold Gain 10% Up", 5);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.GoldGain += 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class GoldGain30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_13", "Gold Gain Up", "Gold Gain 30% Up", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.GoldGain += 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class GoldGain50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_14", "Super Gold Gain Up", "Gold Gain 50% Up", 3);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.GoldGain += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class PowerButSlow : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_15", "PowerButSlow", "Atk Up Max Speed Decrease", 4);
    }

    public override void Apply()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 10f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 15f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class WeakButFast : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_16", "WeakButFast", "Atk Down Max Speed Increase", 4);
    }

    public override void Apply()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 20f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 0.7f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 15f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 0.7f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class WeakButStrongEffect : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_17", "WeakButStrongEffect", "Atk Down Effect Damage Increase", 4);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 0.33f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier *= 1.6f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 0.33f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier /= 1.6f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class WeakButStrongAtk : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_18", "WeakButStrongAtk", "Atk Up Effect Damage decrease", 4);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.6f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier *= 0.33f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.6f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier /= 0.33f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class DecreaseFieldCooldown5 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_19", "DecreaseFieldCooldown", "DecreaseFieldCooldown", 5);
    }

    public override void Apply()
    {
        AugmentManager.instance.FieldGlobalCooldown *= 0.95f;
    }

    public override void Remove()
    {
        AugmentManager.instance.FieldGlobalCooldown /= 0.95f;
    }
}

public class DecreaseFieldCooldown30 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_20", "DecreaseFieldCooldown30", "DecreaseFieldCooldown30", 3);
    }

    public override void Apply()
    {
        AugmentManager.instance.FieldGlobalCooldown *= 0.7f;
    }

    public override void Remove()
    {
        AugmentManager.instance.FieldGlobalCooldown /= 0.7f;
    }
}

