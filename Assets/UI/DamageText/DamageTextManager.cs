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

    public void CreateDamageText(Vector2 position, string Text, string damageType, bool isCritical)
    {
        DamageText damageText;
        damageText = PoolManager.GetFromPool<DamageText>();
        damageText.rectTransform.anchoredPosition = position;
        damageText.SetText(Text, damageType, isCritical);
    }
    
    public void TakeDamageText(DamageText damageText)
    {
        PoolManager.TakeToPool<DamageText>(damageText);
    }
}
