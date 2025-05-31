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
    public List<IAugment> exceptionAugments = new List<IAugment>();
    private List<IAugment> alreadyChosenAugments = new List<IAugment>();
    public IAugment GetRandomAugment()
    {
        if (augmentsSet == null || augmentsSet.Count == 0) return null;
        var availableAugments = augmentsSet
            .Where(a => !alreadyChosenAugments.Contains(a) && !exceptionAugments.Contains(a) && a.Rarity == GetTargetRarity())
            .ToList();
        if (availableAugments.Count == 0) return null;

        int index = Random.Range(0, availableAugments.Count);
        IAugment chosenAugment = availableAugments[index];
        alreadyChosenAugments.Add(chosenAugment);
        return chosenAugment;
    }

    public IAugment GetRandomAugmentByRarity(int targetRarity)
    {
        if (augmentsSet == null || augmentsSet.Count == 0) return null;

        var availableAugments = augmentsSet
            .Where(a => !alreadyChosenAugments.Contains(a) && !exceptionAugments.Contains(a) && a.Rarity == targetRarity)
            .ToList();
        if (availableAugments.Count == 0) return null;

        int index = Random.Range(0, availableAugments.Count);
        IAugment chosenAugment = availableAugments[index];
        alreadyChosenAugments.Add(chosenAugment);
        return chosenAugment;
    }

    public int GetTargetRarity()
    {
        float chance = Random.value;
        if (chance < rarityWeights[0])
            return 0;
        chance -= rarityWeights[0];
        if (chance < rarityWeights[1])
            return 1;
        chance -= rarityWeights[1];
        if (chance < rarityWeights[2])
            return 2;
        chance -= rarityWeights[2];
        if (chance < rarityWeights[3])
            return 3;
        chance -= rarityWeights[3];
        if (chance < rarityWeights[4])
            return 4;
        return 5;
    }
}

public partial class AugmentManager
{
    public void AddAugment(IAugment augment)
    {
        alreadyChosenAugments.Clear();
        exceptionAugments.Add(augment);
        if (augment is StatAugment statAugment)
        {
            activeStatAugments.Add(statAugment);
            augment.Apply();
        }
        else if (augment is FieldAugment fieldAugment)
        {
            activeFieldAugments.Add(fieldAugment);
            augment.Apply();
        }
        else if (augment is SpecialAugment specialAugment)
        {
            if(specialAugment.ID != "SPECIAL_73")
                activeSpecialAugments.Add(specialAugment);
            augment.Apply();
        }
        IAugment OppositeAugment = GetOppositeAugment(augment);
        if (OppositeAugment != null)
        {
            exceptionAugments.Add(OppositeAugment);
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
            if (activeSpecialAugments.Any(a => a.Name == augment.Name && specialAugment.ID != "SPECIAL_73"))
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

    public void RemoveOnExceptAugmentsList(IAugment augment)
    {
        if(exceptionAugments.Contains(augment))
            exceptionAugments.Remove(augment);
    }
    
    private IAugment GetOppositeAugment(IAugment ChosenAugment)
    {
        /*
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
        */
        return null;
    }
}