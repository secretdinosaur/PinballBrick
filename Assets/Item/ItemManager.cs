using Redcode.Pools;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public PoolManager PoolManager;

    public ItemInfo ItemInfo;
    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void SpawnItem(Vector2 BlockPosition)
    {
        float ChanceSum = 0;
        float RandomNumber = Random.value;

        for (int i = 0; i < ItemInfo.ItemSpawnChance.Length; i++)
        {
            ChanceSum += ItemInfo.ItemSpawnChance[i];
            if (RandomNumber < ChanceSum)
            {
                //Item
                break;
            }
        }
    }
}
