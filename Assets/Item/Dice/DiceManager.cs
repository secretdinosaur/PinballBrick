using Redcode.Pools;
using UnityEngine;
using System.Collections.Generic;
public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    private PoolManager poolManager;
    public List<Dice> Dices = new List<Dice>();
    public bool IsRolling = false;
    private void Start()
    {
        Instance = this;
        poolManager = GetComponent<PoolManager>();
    }

    private void Update()
    {
        bool IsAllDiceSleeping = true;
        foreach (Dice dice in Dices)
        {
            if (!dice.Rigidbody.IsSleeping())
            {
                IsAllDiceSleeping = false;
            }
        }
        if(Dices.Count == 0)
        {
            IsAllDiceSleeping = false;
        }
        if(IsAllDiceSleeping && IsRolling)
        {
            CheckDice();
            IsRolling = false;
        }
    }
    public void RollDice(int NumOfDice)
    {
        IsRolling = true;
        
        for (int i = 0; i < NumOfDice; i++)
        {
            Dices.Add(poolManager.GetFromPool<Dice>());
        }
    }

    private void CheckDice()
    {
        int DiceSum = 0;
        List<Dice> RemoveDices = new List<Dice>();
        foreach (Dice dice in Dices)
        {
            DiceSum += dice.Die;
            RemoveDices.Add(dice);
            poolManager.TakeToPool<Dice>(dice);
        }
        foreach (Dice RemoveDice in RemoveDices)
        {
            Dices.Remove(RemoveDice);
        }
        BallManager.Instance.CreateBall(DiceSum);
    }
}
