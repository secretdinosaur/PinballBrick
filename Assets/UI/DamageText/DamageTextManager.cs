using UnityEngine;
using Redcode.Pools;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;
    private PoolManager PoolManager;

    void Start()
    {
        instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void CreateDamageText(Vector2 position, int damage)
    {
        DamageText damageText = PoolManager.GetFromPool<DamageText>();
        damageText.transform.position = position;
        damageText.SetDamage(damage);
    }

    public void TakeDamageText(DamageText damageText)
    {
        PoolManager.TakeToPool<DamageText>(damageText);
    }
}
