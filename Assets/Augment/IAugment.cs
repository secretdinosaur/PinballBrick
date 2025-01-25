using UnityEngine;
using System.Collections.Generic;

public interface IAugment
{
    string ID { get; }
    string Name { get; }
    string Description { get; }
    void Apply();
    void Remove();
}

public abstract class StatAugment : IAugment
{
    public string ID { get; }
    public string Name { get; }
    public string Description { get; }

    protected StatAugment(string id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }

    public virtual void Apply() { }

    public virtual void Remove() { }
}

public abstract class FieldAugment : IAugment
{
    public string ID { get; }
    public string Name { get; }
    public string Description { get; }

    protected FieldAugment(string id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }

    public virtual void Apply() { }

    public virtual void Remove() { }
}

public abstract class SpecialAugment : IAugment
{
    public string ID { get; }
    public string Name { get; }
    public string Description { get; }
    public string Type { get; }

    protected SpecialAugment(string id, string name, string description, string type)
    {
        ID = id;
        Name = name;
        Description = description;
        Type = type;
    }

    public virtual void Apply() { }

    public virtual void Remove() { }
}