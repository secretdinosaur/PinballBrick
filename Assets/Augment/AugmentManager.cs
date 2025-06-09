using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;
using System.Text.RegularExpressions;

public partial class AugmentManager //Utility
{
    public void printAugments()
    {
        Dictionary<int, string> spriteDescriptions = new Dictionary<int, string>
        {
            { 0, "Attack Damage" },
            { 1, "Critical Damage" },
            { 2, "Effect Damage" },
            { 3, "Critical Chance" },
            { 6, "Effect Duration" },
            { 8, "Gold Gain" },
            { 10, "EXP Gain" },
            { 11, "Cooltime Reduce" }
        };

        string ReplaceSpriteTags(string input)
        {
            return Regex.Replace(input, @"<sprite=(\d+)>", match =>
            {
                int index = int.Parse(match.Groups[1].Value);
                return spriteDescriptions.ContainsKey(index) ? spriteDescriptions[index] : match.Value;
            });
        }

        string str = "";
        for(int i = 0; i < augmentsSet.Count; i++)
        {
            str += augmentsSet[i].Type + "\t" + augmentsSet[i].Rarity + "\t" + augmentsSet[i].Name + "\t" + augmentsSet[i].Description.Replace("\r\n", "").Replace("\n", "") + "\n";
        }
        print(ReplaceSpriteTags(str));
    }

    private void Start()
    {
        printAugments();
     }
    public bool determineSpecialAugmentType(string Type)
    {
        return activeAugments.Any(a => a.Type == Type);
    }
    public bool determineAugmentID(string ID)
    {
        return activeAugments.Any(a => a.ID == ID);
    }
    
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
        activeAugments.Add(augment);
        augment.Apply();
        ActivatedAugmentNumber++;
    }

    public void RemoveAugment(IAugment augment)
    {
        LockAugment(augment);
        activeAugments.Remove(augment);
        augment.Remove();
        ActivatedAugmentNumber--;
    }

    public void RemoveAugmentOnType(string augmentType)
    {
        var activeAugmentsCopy = activeAugments.ToList();
        foreach (IAugment augment in activeAugmentsCopy)
        {
            if(augment.Type == augmentType)
            {
                RemoveAugment(augment);
            }
        }
    }


    public void RemoveOnAugmentsSet(IAugment augment)
    {
        if (activeAugments.Any(a => a.Name == augment.Name && augment.ID != "SPECIAL_73"))
        {
            augmentsSet.Remove(augment);
        }
    }
    public void ClearAllAugments()
    {
        for(int i = 0; i < activeAugments.Count; i++)
        {
            activeAugments[i].Remove();
        }
        activeAugments.Clear();
    }

    public void UnlockAugment(IAugment augment)
    {
        if(exceptionAugments.Contains(augment))
            exceptionAugments.Remove(augment);
    }

    public void LockAugment(IAugment augment)
    {
        if (!exceptionAugments.Contains(augment))
            exceptionAugments.Add(augment);
    }
}