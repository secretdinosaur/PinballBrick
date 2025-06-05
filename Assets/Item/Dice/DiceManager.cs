using Redcode.Pools;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    private PoolManager poolManager;
    public bool IsRolling = false;
    private void Awake()
    {
        Instance = this;
        poolManager = GetComponent<PoolManager>();
    }

    public IEnumerator RollDices(int NumOfDice, System.Action<int> Sum)
    {
        List<Dice> Dices = new List<Dice>();
        List<Dice> StoppedDices = new List<Dice>();
        int DiceSum = 0;
        for (int i = 0; i < NumOfDice; i++)
        {
            Dices.Add(poolManager.GetFromPool<Dice>());
        }

        while (!Dices.ToHashSet().SetEquals(StoppedDices))
        {
            foreach (Dice dice in Dices)
            {
                if (dice.Rigidbody.IsSleeping() && !StoppedDices.Contains(dice))
                {
                    StoppedDices.Add(dice);
                    DiceSum += dice.Die;
                }
            }
            yield return null;
        }

        foreach (Dice dice in Dices)
        {
            poolManager.TakeToPool<Dice>(dice);
        }
        Sum(DiceSum);
    }

    public IEnumerator RollDicesforDiceGamble(int NumOfDice, System.Action<int[]> Sum)
    {
        List<Dice> Dices = new List<Dice>();
        List<Dice> StoppedDices = new List<Dice>();
        int[] Dice = new int[3];
        int index = 0;
        for (int i = 0; i < NumOfDice; i++)
        {
            Dices.Add(poolManager.GetFromPool<Dice>());
        }

        while (!Dices.ToHashSet().SetEquals(StoppedDices))
        {
            foreach (Dice dice in Dices)
            {
                if (dice.Rigidbody.IsSleeping() && !StoppedDices.Contains(dice))
                {
                    StoppedDices.Add(dice);
                    Dice[index] =  dice.Die;
                    index += 1;
                }
            }
            yield return null;
        }

        foreach (Dice dice in Dices)
        {
            poolManager.TakeToPool<Dice>(dice);
        }
        Sum(Dice);
    }
}