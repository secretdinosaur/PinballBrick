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

    public IAugment GetRandomAugment()
    {
        if (augmentsSet == null || augmentsSet.Count == 0) return null;
        
        Dictionary<int, float> rarityWeights = new Dictionary<int, float>
        {
            { 1, 2f },
            { 2, 5f },
            { 3, 13f },
            { 4, 26f },
            { 5, 54f }
        };
        float totalWeight = 0f;
        float[] Weights = new float[augmentsSet.Count];
        
        for (int i = 0; i < augmentsSet.Count; i++)
        {
            totalWeight += rarityWeights[augmentsSet[i].Rarity];
            Weights[i] = totalWeight;
        }
        
        float randomValue = Random.Range(0f, totalWeight);
        
        for (int i = 0; i < augmentsSet.Count; i++)
        {
            if (randomValue <= Weights[i])
            {
                IAugment ChosenAugment;
                ChosenAugment = augmentsSet[i];
                //augmentsSet[i] = null;
                return ChosenAugment;
            }
        }
        return augmentsSet[0];
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