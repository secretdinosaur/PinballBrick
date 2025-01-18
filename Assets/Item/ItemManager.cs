using Redcode.Pools;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private PoolManager PoolManager;
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
        int RandomNumber = Random.Range(0, 100);
        Item item;
        if (RandomNumber < 10)
        {
            item = PoolManager.GetFromPool<DiceItem>();
            item.transform.position = BlockPosition;
        }
        else
        {
            
        }
    }

    public void TakeItem(Item item)
    {
        PoolManager.TakeToPool<Item>(item);
    }
}
