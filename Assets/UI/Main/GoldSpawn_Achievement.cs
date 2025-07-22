using UnityEngine;
using Redcode.Pools;
using System;
using System.Collections;
public class GoldSpawn_Achievement : MonoBehaviour
{
    public PoolManager PoolManager;
    public static GoldSpawn_Achievement Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void SpawnGold(int amount, Vector2 position)
    {
        int[] goldValues = { 100, 10, 1 };
        string[] goldNames = { "Gold3", "Gold2", "Gold1" };

        if (amount >= 1000)
        {
            for(int i = 0; i <  goldValues.Length; i++)
            {
                 goldValues[i] *= (amount / 100);
            }
        }

        for (int i = 0; i < goldValues.Length; i++)
        {
            int goldValue = goldValues[i];
            string goldName = goldNames[i];
            int count = amount / goldValue;
            amount %= goldValue;

            for (int j = 0; j < count; j++)
            {
                SpawnGoldByType(goldName, position, goldValue);
            }
        }
        if(amount >= 1)
        {
            StatManager.instance.GetGold(amount);
        }
    }

    private void SpawnGoldByType(string itemName, Vector2 position, int amount)
    {
        Type type = Type.GetType(itemName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(type);
        Item item = (Item)genericMethod.Invoke(PoolManager, null);
        item.GetComponent<SpriteRenderer>().enabled = true;
        item.transform.position = position;
        item.ItemName = itemName;

        if (itemName == "Gold1") item.GetComponent<Gold1>().GoldAmount = amount;
        if (itemName == "Gold2") item.GetComponent<Gold2>().GoldAmount = amount;
        if (itemName == "Gold3") item.GetComponent<Gold3>().GoldAmount = amount;

        item.MoveItem_Main();
    }

    public void TakeItem(Item item)
    {
        StartCoroutine(TakeItemCo(item));
    }

    public IEnumerator TakeItemCo(Item item)
    {
        if (item.ItemName == "Gold1" || item.ItemName == "Gold2" || item.ItemName == "Gold3") SoundManager.instance.GetGoldSOund(item.GetComponent<AudioSource>());
        item.GetComponent<SpriteRenderer>().enabled = false;
        UpgradeManager.Instance.UpdateGoldUI();

        yield return new WaitForSeconds(1);
        Type itemType = Type.GetType(item.ItemName);
        var method = typeof(PoolManager).GetMethod("TakeToPool");
        var genericMethod = method.MakeGenericMethod(itemType);
        genericMethod.Invoke(PoolManager, new object[] { item.GetComponent<Item>() });
    }
}
