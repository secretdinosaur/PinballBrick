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

    public void CreateDamageText(Vector2 position, float damage, string damageType, bool isCritical)
    {
        DamageText damageText;
        damageText = PoolManager.GetFromPool<DamageText>();
        damageText.rectTransform.anchoredPosition = position;
        damageText.SetDamage(damage, damageType, isCritical);
    }
    
    public void TakeDamageText(DamageText damageText)
    {
        PoolManager.TakeToPool<DamageText>(damageText);
    }
}
