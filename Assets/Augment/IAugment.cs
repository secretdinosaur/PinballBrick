using UnityEngine;
using System;
using System.Collections;

public abstract class IAugment : MonoBehaviour
{
    public string ID;
    public string Name;
    public string Description;
    public int Rarity;
    public virtual void Apply() { }

    public virtual void Remove() { }
    public virtual void BeforeAttackEffect(IBrick Brick) { }
    public virtual void AfterAttackEffect(IBrick Brick) { }

    public virtual void CriticalAttackEffect(IBrick Brick) { }
}

public abstract class StatAugment : IAugment
{
    protected void SetAugment(string id, string name, string description, int rarity)
    {
        ID = id;
        Name = name;
        Description = description;
        Rarity = rarity;
    }
}

public abstract class FieldAugment : IAugment
{
    protected void SetAugment(string id, string name, string description, int rarity)
    {
        ID = id;
        Name = name;
        Description = description;
        Rarity = rarity;
    }
}

public abstract class SpecialAugment : IAugment
{
    public string Type;

    protected void SetAugment(string id, string name, string description, string type, int rarity)
    {
        ID = id;
        Name = name;
        Description = description;
        Type = type;
        Rarity = rarity;
    }
}