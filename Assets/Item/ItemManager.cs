using Redcode.Pools;
using UnityEngine;
using System.Collections;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public PoolManager PoolManager;

    private float DiceItemChance = 0.1f;
    private float BangBangItemChance = 0.05f;
    private float HPOneItemChance = 0.05f;
    private float PowerUpItemChance = 0.2f;



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
        float RandomNumber = Random.value;
        Item item;
        if (RandomNumber < DiceItemChance)
        {
            item = PoolManager.GetFromPool<DiceItem>();
            item.transform.position = BlockPosition;
        }
        
        else if (RandomNumber < DiceItemChance + BangBangItemChance)
        {
            item = PoolManager.GetFromPool<BangBangItem>();
            item.transform.position = BlockPosition;
        }

        else if (RandomNumber < DiceItemChance + BangBangItemChance + HPOneItemChance)
        {
            item = PoolManager.GetFromPool<HPOneItem>();
            item.transform.position = BlockPosition;
        }

        else if (RandomNumber < DiceItemChance + BangBangItemChance + HPOneItemChance + PowerUpItemChance)
        {
            item = PoolManager.GetFromPool<PowerUpItem>();
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

    public void TakeHPOneItem(HPOneItem item)
    {
        PoolManager.TakeToPool<HPOneItem>(item);
    }
    public void TakePowerUpItem(PowerUpItem item)
    {
        PoolManager.TakeToPool<PowerUpItem>(item);
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
