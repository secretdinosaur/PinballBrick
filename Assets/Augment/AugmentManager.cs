using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public partial class AugmentManager : MonoBehaviour
{
    public static AugmentManager instance;
    public List<IAugment> activeAugments = new List<IAugment>();

    void Awake()
    {
        instance = this;
    }
    
    public void AddAugment(IAugment augment)
    {
        if (activeAugments.All(a => a.Name != augment.Name))
        {
            augment.Apply(StatManager.instance.Temporary_stat, StatManager.instance.statMultiplier);
            activeAugments.Add(augment);
            SaveAugment(); 
        }
    }

    public void RemoveAugment(IAugment augment)
    {
        if (activeAugments.Any(a => a.Name == augment.Name))
        {
            augment.Remove(StatManager.instance.Temporary_stat, StatManager.instance.statMultiplier);
            activeAugments.Remove(augment);
            SaveAugment();
        }
    }
    public void ClearAllAugments()
    {
        foreach (var augment in activeAugments)
        {
            augment.Remove(StatManager.instance.Temporary_stat, StatManager.instance.statMultiplier);
        }
        activeAugments.Clear();
        SaveAugment();
    }

    public void SaveAugment()
    {
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}