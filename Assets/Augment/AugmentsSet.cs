using UnityEngine;

public class TestAtkUp1 : IAugment
{
    public string Name => "TestAtkUp1";
    public string Description => "Ball Atk 30% Up";

    public void Apply(Temporary_Stat stat, StatMultiplier multiplier)
    {
        multiplier.AttackDamageMultiplier += 0.3f;
    }

    public void Remove(Temporary_Stat stat, StatMultiplier multiplier)
    {
        multiplier.AttackDamageMultiplier -= 0.3f;
    }
}

public class TestAtkUp2 : IAugment
{
    public string Name => "TestAtkUp2";
    public string Description => "Ball Atk 70% Up";

    public void Apply(Temporary_Stat stat, StatMultiplier multiplier)
    {
        multiplier.AttackDamageMultiplier += 0.7f;
    }

    public void Remove(Temporary_Stat stat, StatMultiplier multiplier)
    {
        multiplier.AttackDamageMultiplier -= 0.7f;
    }
}

public class TestCritChacneUp : IAugment
{
    public string Name => "TestCritChacneUp";
    public string Description => "Ball Crit Chacne 50% Up";

    public void Apply(Temporary_Stat stat, StatMultiplier multiplier)
    {
        stat.CriticalChance += 0.5f;
    }

    public void Remove(Temporary_Stat stat, StatMultiplier multiplier)
    {
        stat.CriticalChance += 0.5f;
    }
}