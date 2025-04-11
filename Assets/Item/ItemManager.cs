using Redcode.Pools;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public PoolManager PoolManager;

    public ItemInfo ItemInfo;

    public bool IsPowerUp = false;
    private List<PowerUpItem> PowerUpItemQueue = new List<PowerUpItem>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(Vector2 BlockPosition)
    {
        float RandomNumber = UnityEngine.Random.value;
        Item item;
        float ChanceSum = 0;
        for (int i = 0; i < ItemInfo.Items.Length; i++)
        {
            ChanceSum += ItemInfo.ItemSpawnChance[i];
            if (RandomNumber < ChanceSum)
            {
                Type brickType = Type.GetType(ItemInfo.ItemName[i]);
                var method = typeof(PoolManager).GetMethod("GetFromPool");
                var genericMethod = method.MakeGenericMethod(brickType);
                item = (Item)genericMethod.Invoke(PoolManager, null);
                item.transform.position = BlockPosition;
                break;
            }
        }
    }

    public void TakeItem(Item item)
    {
        Type ItemName = Type.GetType(item.ItemName);
        var method = typeof(PoolManager).GetMethod("TakeToPool");
        var genericMethod = method.MakeGenericMethod(ItemName);
        genericMethod.Invoke(PoolManager, new object[] { item.GetComponent<Item>() });
    }

    public void CreateBangBang()
    {
        BangBangParent bangbang = PoolManager.GetFromPool<BangBangParent>();
        StartCoroutine(DisappearAfter10s(bangbang));
    }
    private IEnumerator DisappearAfter10s(BangBangParent bangbang)
    {
        yield return new WaitForSeconds(10f);
        PoolManager.TakeToPool<BangBangParent>(bangbang);
    }

    public IEnumerator UsePowerUpItem(PowerUpItem Item)
    {
        PowerUpItemQueue.Add(Item);

        if (IsPowerUp)
            yield break;


        IsPowerUp = true;
        while (PowerUpItemQueue.Count > 0)
        {
            IsPowerUp = true;
            StatManager.instance.statMultiplier.AttackDamageMultiplier *= 2;
            StatManager.instance.statMultiplier.EffectDamageMultiplier *= 2;
            PowerUpItemQueue[0].GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(10f);

            PowerUpItemQueue[0].GetComponent<SpriteRenderer>().enabled = true;
            IsPowerUp = false;
            StatManager.instance.statMultiplier.AttackDamageMultiplier /= 2;
            StatManager.instance.statMultiplier.EffectDamageMultiplier /= 2;
            TakeItem(PowerUpItemQueue[0]);
            PowerUpItemQueue.RemoveAt(0);
        }
        IsPowerUp = false;
    }
}
