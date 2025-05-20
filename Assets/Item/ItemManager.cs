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

    public void SpawnItem(Vector2 BrickPosiotion)
    {
        float ChanceSum = 0;
        float RandomNumber = Random.value;

        for (int i = 0; i < ItemInfo.ItemSpawnChance.Length; i++)
        {
            ChanceSum += ItemInfo.ItemSpawnChance[i];
            if (RandomNumber < ChanceSum)
            {
                Type Type = Type.GetType(ItemInfo.ItemName[i]);
                var method = typeof(PoolManager).GetMethod("GetFromPool");
                var genericMethod = method.MakeGenericMethod(Type);
                Item Item = (Item)genericMethod.Invoke(PoolManager, null);
                Item.transform.position = BrickPosiotion;
                Item.ItemName = ItemInfo.ItemName[i];
                Item.MoveItem();
                break;
            }
        }
    }

    public void TakeItem(Item item)
    {
        StartCoroutine(TakeItemCo(item));
    }

    public IEnumerator TakeItemCo(Item item)
    {
        yield return new WaitForSeconds(1);
        Type itemType = Type.GetType(item.ItemName);
        var method = typeof(PoolManager).GetMethod("TakeToPool");
        var genericMethod = method.MakeGenericMethod(itemType);
        genericMethod.Invoke(PoolManager, new object[] { item.GetComponent<Item>() });
    }
}
