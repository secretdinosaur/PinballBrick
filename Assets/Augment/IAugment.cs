using UnityEngine;
using System.Collections.Generic;

public interface IAugment
{
    string Name { get; }
    string Description { get; }
    void Apply();
    void Remove();
}

public abstract class StatAugment : IAugment
{
    public string Name { get; }
    public string Description { get; }

    protected StatAugment(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Apply() { }

    public void Remove() { }
}

public abstract class FieldAugment : IAugment
{
    public string Name { get; }
    public string Description { get; }

    protected FieldAugment(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Apply() { }

    public void Remove() { }
}

public abstract class SpecialAugment : IAugment
{
    public string Name { get; }
    public string Description { get; }
    public string Type { get; }

    protected SpecialAugment(string name, string description, string type)
    {
        Name = name;
        Description = description;
        Type = type;
    }

    public void Apply() { }

    public void Remove() { }
}