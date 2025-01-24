using Redcode.Pools;
using UnityEngine;
using System.Collections;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public PoolManager PoolManager;

    private float DiceItemProbability = 10;
    private float BangBangItemProbability = 10;








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
        float RandomNumber = Random.Range(0, 100);
        Item item;
        if (RandomNumber < DiceItemProbability)
        {
            item = PoolManager.GetFromPool<DiceItem>();
            item.transform.position = BlockPosition;
        }
        
        else if (RandomNumber < DiceItemProbability + BangBangItemProbability)
        {
            item = PoolManager.GetFromPool<BangBangItem>();
            item.transform.position = BlockPosition;
        }
    }

    public void TakeItem(Item item)
    {
        PoolManager.TakeToPool<Item>(item);
    }

    public void TakeBangBangItem(BangBangItem item)
    {
        PoolManager.TakeToPool<BangBangItem>(item);
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
