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
    
    private List<IAugment> alreadyChosenAugments = new List<IAugment>();
    public IAugment GetRandomAugment()
    {
        if (augmentsSet == null || augmentsSet.Count == 0) return null;
        var availableAugments = augmentsSet.Where(a => !alreadyChosenAugments.Contains(a)).ToList();
        if (availableAugments.Count == 0) return null;
        
        Dictionary<int, float> rarityWeights = new Dictionary<int, float>
        {
            { 1, 2f },
            { 2, 5f },
            { 3, 13f },
            { 4, 26f },
            { 5, 54f }
        };
        float totalWeight = 0f;
        float[] Weights = new float[availableAugments.Count];
        
        for (int i = 0; i < availableAugments.Count; i++)
        {
            totalWeight += rarityWeights[availableAugments[i].Rarity];
            Weights[i] = totalWeight;
        }
        
        float randomValue = Random.Range(0f, totalWeight);
        
        for (int i = 0; i < availableAugments.Count; i++)
        {
            if (randomValue <= Weights[i])
            {
                IAugment ChosenAugment = availableAugments[i];
                alreadyChosenAugments.Add(ChosenAugment);
                return ChosenAugment;
            }
        }
        return availableAugments[0];
    }
}

public partial class AugmentManager
{
    public void AddAugment(IAugment augment)
    {
        alreadyChosenAugments.Add(augment);
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
        IAugment OppositeAugment = GetOppositeAugment(augment);
        if (OppositeAugment != null)
        {
            alreadyChosenAugments.Add(OppositeAugment);
        }
    }


    public void RemoveAugment(IAugment augment)
    {
        if (augment is StatAugment statAugment)
        {
            if (activeStatAugments.Any(a => a.Name == augment.Name))
            {
                augmentsSet.Remove(statAugment);
            }
        }
        else if (augment is FieldAugment fieldAugment)
        {
            if (activeFieldAugments.Any(a => a.Name == augment.Name))
            {
                augmentsSet.Remove(fieldAugment);
            }
        }
        else if (augment is SpecialAugment specialAugment)
        {
            if (activeSpecialAugments.Any(a => a.Name == augment.Name))
            {
                augmentsSet.Remove(specialAugment);
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
    
    private IAugment GetOppositeAugment(IAugment ChosenAugment)
    {
        IAugment oppositeAugment;
        if (ChosenAugment.ID == "STAT_15")
        {
            oppositeAugment = augmentsSet.FirstOrDefault(a => a.ID == "STAT_16");
            if (oppositeAugment != null)
            {
                return oppositeAugment;
            }
        }
        else if (ChosenAugment.ID == "STAT_16")
        {
            oppositeAugment = augmentsSet.FirstOrDefault(a => a.ID == "STAT_15");
            if (oppositeAugment != null)
            {
                return oppositeAugment;
            }
        }
        else if (ChosenAugment.ID == "STAT_17")
        {
            oppositeAugment = augmentsSet.FirstOrDefault(a => a.ID == "STAT_18");
            if (oppositeAugment != null)
            {
                return oppositeAugment;
            }
        }
        else if (ChosenAugment.ID == "STAT_18")
        {
            oppositeAugment = augmentsSet.FirstOrDefault(a => a.ID == "STAT_17");
            if (oppositeAugment != null)
            {
                return oppositeAugment;
            }
        }

        return null;
    }
}