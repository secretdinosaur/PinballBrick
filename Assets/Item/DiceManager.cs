using Redcode.Pools;
using UnityEngine;
using System.Collections.Generic;
public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    private PoolManager poolManager;
    List<Dice> Dices = new List<Dice>();

    private void Start()
    {
        Instance = this;
        poolManager = GetComponent<PoolManager>();
    }
    public void RollDice(int NumOfDice)
    {
        for(int i = 0; i < NumOfDice; i++)
        {
            Dices.Add(poolManager.GetFromPool<Dice>());
        }
    }
}
