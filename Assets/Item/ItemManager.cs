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
            item = PoolManager.GetComponent<DiceItem>();
            item.transform.position = BlockPosition;
        }
        else
        {
            
        }
    }
}
