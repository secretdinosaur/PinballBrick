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
                if (Item.ItemName == "ExpBead") Item.GetComponent<ExpBead>().ExpAmount = (10 + GameManager.instance.Wave / 3 + LevelManager.Instance.LevelBar.maxValue / 200) / 2;
                if (Item.ItemName == "Gold1")Item.GetComponent<Gold1>().GoldAmount = (5 + GameManager.instance.Wave / 2) / 2;
                if (Item.ItemName == "Gold2")Item.GetComponent<Gold2>().GoldAmount = (25 + GameManager.instance.Wave / 2) / 2;
                if (Item.ItemName == "Gold3")Item.GetComponent<Gold3>().GoldAmount = (50 + GameManager.instance.Wave / 2) / 2;
                Item.MoveItem();
                break;
            }
        }
    }

    public void SpawnSpecificItem(string Item, Vector2 BrickPosiotion, int num)
    {
        int index = 0;
        for (int i = 0; i < ItemInfo.ItemName.Length; i++)
        {
            if (Item == ItemInfo.ItemName[i])
                index = i;
        }

        for(int i = 0; i < num; i++)
        {
            Type Type = Type.GetType(ItemInfo.ItemName[index]);
            var method = typeof(PoolManager).GetMethod("GetFromPool");
            var genericMethod = method.MakeGenericMethod(Type);
            Item item = (Item)genericMethod.Invoke(PoolManager, null);
            item.transform.position = BrickPosiotion;
            item.ItemName = ItemInfo.ItemName[index];
            if (item.ItemName == "ExpBead") item.GetComponent<ExpBead>().ExpAmount = (10 + GameManager.instance.Wave / 3 + LevelManager.Instance.LevelBar.maxValue / 200) / 2;
            if (item.ItemName == "Gold1") item.GetComponent<Gold1>().GoldAmount = (5 + GameManager.instance.Wave / 2) / 2;
            if (item.ItemName == "Gold2") item.GetComponent<Gold2>().GoldAmount = (25 + GameManager.instance.Wave / 2) / 2;
            if (item.ItemName == "Gold3") item.GetComponent<Gold3>().GoldAmount = (50 + GameManager.instance.Wave / 2) / 2;
            item.MoveItem();
        }
    }

    public IEnumerator SpawnSpecificItemInRandomPosition(string Item, int num, float interval)
    {
        int index = 0;
        for (int i = 0; i < ItemInfo.ItemName.Length; i++)
        {
            if (Item == ItemInfo.ItemName[i])
                index = i;
        }

        for (int i = 0; i < num; i++)
        {
            Vector2 RandomPosition = new Vector2(Random.Range(-2.2f, 2.2f), Random.Range(-2.7f, 4.25f));

            Type Type = Type.GetType(ItemInfo.ItemName[index]);
            var method = typeof(PoolManager).GetMethod("GetFromPool");
            var genericMethod = method.MakeGenericMethod(Type);
            Item item = (Item)genericMethod.Invoke(PoolManager, null);
            item.transform.position = RandomPosition;
            item.ItemName = ItemInfo.ItemName[index];
            if (item.ItemName == "ExpBead") item.GetComponent<ExpBead>().ExpAmount = (10 + GameManager.instance.Wave / 3 + LevelManager.Instance.LevelBar.maxValue / 200) / 2;
            if (item.ItemName == "Gold1") item.GetComponent<Gold1>().GoldAmount = (5 + GameManager.instance.Wave / 2) / 2;
            if (item.ItemName == "Gold2") item.GetComponent<Gold2>().GoldAmount = (25 + GameManager.instance.Wave / 2) / 2;
            if (item.ItemName == "Gold3") item.GetComponent<Gold3>().GoldAmount = (50 + GameManager.instance.Wave / 2) / 2;
            item.MoveItem();
            yield return new WaitForSeconds(interval);
        }
    }

    public void TakeItem(Item item)
    {
        StartCoroutine(TakeItemCo(item));
    }

    public IEnumerator TakeItemCo(Item item)
    {
        if (item.ItemName == "Gold1" || item.ItemName == "Gold2" || item.ItemName == "Gold3") SoundManager.instance.GetGoldSOund(item.GetComponent<AudioSource>());
        if (item.ItemName == "ExpBead")
        {
            print(1);
            SoundManager.instance.GetExpSound(item.GetComponent<AudioSource>());
        }
        yield return new WaitForSeconds(1);
        Type itemType = Type.GetType(item.ItemName);
        var method = typeof(PoolManager).GetMethod("TakeToPool");
        var genericMethod = method.MakeGenericMethod(itemType);
        genericMethod.Invoke(PoolManager, new object[] { item.GetComponent<Item>() });
    }
}
