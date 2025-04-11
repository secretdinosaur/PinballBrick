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

    public bool IsPowerUp = false;
    private List<PowerUpItem> PowerUpItemQueue = new List<PowerUpItem>();

    public float BaseItemSpawnChance;

    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
        BaseItemSpawnChance = 0.1f;
    }

    public void SpawnItem(Vector2 BlockPosition)
    {
        if(Random.value < BaseItemSpawnChance)
        {
            int RandomNumber = Random.Range(0, ItemInfo.Items.Length); 
            Item item;
            Type brickType = Type.GetType(ItemInfo.ItemName[RandomNumber]);
            var method = typeof(PoolManager).GetMethod("GetFromPool");
            var genericMethod = method.MakeGenericMethod(brickType);
            item = (Item)genericMethod.Invoke(PoolManager, null);
            item.transform.position = BlockPosition;
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
