using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using UnityEngine;

public partial class AugmentManager //Utility
{
    public bool determineSpecialAugmentType(string Type)
    {
        return activeSpecialAugments.Any(a => a.Type == Type);
    }

    public bool determineAugmentID(string ID)
    {
       return activeStatAugments.Any(a => a.ID == ID) || 
            activeFieldAugments.Any(a => a.ID == ID) || 
            activeSpecialAugments.Any(a => a.ID == ID);
    }
    
    
    public List<IAugment> GetRandomAugments(int count)
    {
        List<IAugment> randomAugments = new List<IAugment>();
        List<int> usedIndexes = new List<int>();
        while (randomAugments.Count < count)
        {
            int index = Random.Range(0, augmentsSet.Count);
            
            if (!usedIndexes.Contains(index))
            {
                randomAugments.Add(augmentsSet[index]);
                usedIndexes.Add(index);
            }
        }

        return randomAugments;
    }
}

public partial class AugmentManager
{
    public void AddAugment(IAugment augment)
    {
        if (augment is StatAugment statAugment)
        {
            augment.Apply();
            activeStatAugments.Add(statAugment);
        }
        else if (augment is FieldAugment fieldAugment)
        {
            augment.Apply();
            activeFieldAugments.Add(fieldAugment);
        }
        else if (augment is SpecialAugment specialAugment)
        {
            augment.Apply();
            activeSpecialAugments.Add(specialAugment);
        }
    }


    public void RemoveAugment(IAugment augment)
    {
        if (augment is StatAugment statAugment)
        {
            if (activeStatAugments.Any(a => a.Name == augment.Name))
            {
                augment.Remove();
                activeStatAugments.Remove(statAugment);
            }
        }
        else if (augment is FieldAugment fieldAugment)
        {
            if (activeFieldAugments.Any(a => a.Name == augment.Name))
            {
                augment.Remove();
                activeFieldAugments.Remove(fieldAugment);
            }
        }
        else if (augment is SpecialAugment specialAugment)
        {
            if (activeSpecialAugments.Any(a => a.Name == augment.Name))
            {
                augment.Remove();
                activeSpecialAugments.Remove(specialAugment);
            }
        }
        
    }
    public void ClearAllAugments()
    {
        foreach (var augment in activeStatAugments)
        {
            augment.Remove();
        }
        foreach (var augment in activeFieldAugments)
        {
            augment.Remove();
        }
        foreach (var augment in activeSpecialAugments)
        {
            augment.Remove();
        }
        activeStatAugments.Clear();
        activeFieldAugments.Clear();
        activeSpecialAugments.Clear();
    }
}