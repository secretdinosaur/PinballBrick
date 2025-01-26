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
    public int Rarity { get; }

    protected StatAugment(string id, string name, string description, int rarity)
    {
        ID = id;
        Name = name;
        Description = description;
        Rarity = rarity;
    }

    public virtual void Apply() { }

    public virtual void Remove() { }
}

public abstract class FieldAugment : IAugment
{
    public string ID { get; }
    public string Name { get; }
    public string Description { get; }
    public int Rarity { get; }

    protected FieldAugment(string id, string name, string description, int rarity)
    {
        ID = id;
        Name = name;
        Description = description;
        Rarity = rarity;
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
    public int Rarity { get; }

    protected SpecialAugment(string id, string name, string description, string type, int rarity)
    {
        ID = id;
        Name = name;
        Description = description;
        Type = type;
        Rarity = rarity;
    }

    public virtual void Apply() { }

    public virtual void Remove() { }
}