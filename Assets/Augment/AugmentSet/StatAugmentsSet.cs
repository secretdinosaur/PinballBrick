using UnityEngine;

/*
public class Example : StatAugment
{
    public Example() : base( "Name" , "Description" ) { }

    public override void Apply(){ Augment Effect (Plus) }

    public override void Remove(){ Augment Effect (Minus) }
}
 */

public class Atk30Up : StatAugment
{
    public Atk30Up() : base("Atk Up", "Ball Atk 30% Up") { }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier += 0.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier -= 0.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class Atk70Up : StatAugment
{
    public Atk70Up() : base("Super Atk Up", "Ball Atk 70% Up") { }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier += 0.7f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier -= 0.7f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChanceUp : StatAugment
{
    public CritChanceUp() : base("Critical Chance Up", "Ball Critical Chance 50% Up") { }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}