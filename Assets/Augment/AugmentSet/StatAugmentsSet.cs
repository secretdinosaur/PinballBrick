using UnityEngine;

public class Atk30Up : IStatAugment
{
    public string Name => "Atk30Up";
    public string Description => "Ball Atk 30% Up";

    public void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier += 0.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier -= 0.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class Atk70Up : IStatAugment
{
    public string Name => "Atk70Up";
    public string Description => "Ball Atk 70% Up";

    public void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier += 0.7f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier -= 0.7f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChanceUp : IStatAugment
{
    public string Name => "CritChanceUp";
    public string Description => "Ball Crit Chacne 50% Up";

    public void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}