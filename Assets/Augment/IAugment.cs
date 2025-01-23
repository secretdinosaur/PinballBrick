using UnityEngine;

public interface IAugment
{
    string Name { get; }
    string Description { get; }
    void Apply(Temporary_Stat stat, StatMultiplier multiplier);
    void Remove(Temporary_Stat stat, StatMultiplier multiplier);
}

