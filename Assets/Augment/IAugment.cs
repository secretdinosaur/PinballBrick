using UnityEngine;

public interface IStatAugment
{
    string Name { get; }
    string Description { get; }
    void Apply();
    void Remove();
}

public interface IFieldAugment
{
    string Name { get; }
    string Description { get; }
    void Apply();
    void Remove();
}


public interface ISpecialAugment
{
    string Name { get; }
    string Description { get; }
    string Type { get; }
    void Apply();
    void Remove();
}

