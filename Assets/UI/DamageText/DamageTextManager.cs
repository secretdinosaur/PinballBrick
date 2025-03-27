using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Unity.VisualScripting;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;
    private PoolManager PoolManager;
    public Queue<DamageText> DamageTextQueue;

    void Start()
    {
        instance = this;
        PoolManager = GetComponent<PoolManager>();
        DamageTextQueue = new Queue<DamageText>();
    }

    public void CreateDamageText(Vector2 position, string Text, string damageType, bool isCritical)
    {
        if (DamageTextQueue.Count >= 50)
        {
            TakeDamageText(DamageTextQueue.Dequeue());
        }
        DamageText damageText;
        damageText = PoolManager.GetFromPool<DamageText>();
        DamageTextQueue.Enqueue(damageText);
        damageText.rectTransform.anchoredPosition = position;
        damageText.SetText(Text, damageType, isCritical);
    }
    
    public void TakeDamageText(DamageText damageText)
    {
        if(instance.DamageTextQueue.Count >= 1)
        {
            instance.DamageTextQueue.Dequeue();
        }
        damageText.GetComponent<DamageText>().StopCoroutine(damageText.Routine);
        damageText.Routine = null;
        PoolManager.TakeToPool<DamageText>(damageText);
    }
}
