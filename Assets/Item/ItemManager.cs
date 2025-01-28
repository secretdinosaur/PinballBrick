using Redcode.Pools;
using UnityEngine;
using System.Collections;
using System;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public PoolManager PoolManager;

    public ItemInfo ItemInfo;


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
        BangBang bangbang = PoolManager.GetFromPool<BangBang>();
        bangbang.transform.position = new Vector2(0, -2.9f);
        StartCoroutine(DisappearAfter5s(bangbang));
    }
    private IEnumerator DisappearAfter5s(BangBang bangbang)
    {
        yield return new WaitForSeconds(5f);
        PoolManager.TakeToPool<BangBang>(bangbang);
    }
}
