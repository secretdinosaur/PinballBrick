using UnityEngine;

/*
public class Example : StatAugment
{
    public Example() : base( "STAT_n", "Example" , "Just Example" ) { }

    public override void Apply(){ Augment Effect (Plus) }

    public override void Remove(){ Augment Effect (Minus) }
}
 */

public class Atk30Up : StatAugment
{
    public Atk30Up() : base("STAT_1","Atk Up", "Ball Atk 30% Up") { }

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
    public Atk70Up() : base("STAT_2","Super Atk Up", "Ball Atk 70% Up") { }

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
    public CritChanceUp() : base("STAT_3","Critical Chance Up", "Ball Critical Chance 50% Up") { }

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
    public CritDamage30Up() : base( "STAT_4","Critical Damage Up" , "Ball Critical Damage 30% Up" ) { }

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
    public CritDamage70Up() : base( "STAT_5","Super Critical Damage Up" , "Ball Critical Damage 70% Up" ) { }

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