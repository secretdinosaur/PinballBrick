using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public partial class AugmentManager : MonoBehaviour
{
    public static AugmentManager instance;
    private List<IStatAugment> activeStatAugments = new List<IStatAugment>();
    private List<IFieldAugment> activeFieldAugments = new List<IFieldAugment>();
    private List<ISpecialAugment> activeSpecialAugments = new List<ISpecialAugment>();
    
    void Awake()
    {
        instance = this;
    }
}

public partial class AugmentManager //Utility
{
    public bool determineSpecialAugmentType(string Type)
    {
        return activeSpecialAugments.Any(a => a.Type == Type);
    }
}

public partial class AugmentManager //StatAugment
{
    public void AddStatAugment(IStatAugment augment)
    {
        if (activeStatAugments.All(a => a.Name != augment.Name))
        {
            augment.Apply();
            activeStatAugments.Add(augment);
        }
    }

    public void RemoveStatAugment(IStatAugment augment)
    {
        if (activeStatAugments.Any(a => a.Name == augment.Name))
        {
            augment.Remove();
            activeStatAugments.Remove(augment);
        }
    }
    public void ClearAllStatAugments()
    {
        foreach (var augment in activeStatAugments)
        {
            augment.Remove();
        }
        activeStatAugments.Clear();
    }
}

public partial class AugmentManager //FieldAugment
{
    public void AddFieldAugment(IFieldAugment augment)
    {
        if (activeFieldAugments.All(a => a.Name != augment.Name))
        {
            augment.Apply();
            activeFieldAugments.Add(augment);
        }
    }

    public void RemoveFieldAugment(IFieldAugment augment)
    {
        if (activeFieldAugments.Any(a => a.Name == augment.Name))
        {
            augment.Remove();
            activeFieldAugments.Remove(augment);
        }
    }
    public void ClearFieldAllAugments()
    {
        foreach (var augment in activeFieldAugments)
        {
            augment.Remove();
        }
        activeFieldAugments.Clear();
    }
}

public partial class AugmentManager //SpecialAugment
{
    public void AddSpecialAugment(ISpecialAugment augment)
    {
        if (activeSpecialAugments.All(a => a.Name != augment.Name))
        {
            augment.Apply();
            activeSpecialAugments.Add(augment);
        }
    }

    public void RemoveSpecialAugment(ISpecialAugment augment)
    {
        if (activeSpecialAugments.Any(a => a.Name == augment.Name))
        {
            augment.Remove();
            activeSpecialAugments.Remove(augment);
        }
    }
    public void ClearAllSpecialAugments()
    {
        foreach (var augment in activeSpecialAugments)
        {
            augment.Remove();
        }
        activeSpecialAugments.Clear();
    }
}