using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;

public class DamageText : MonoBehaviour
{
    public RectTransform rectTransform;
    private Vector2 direction;
    private float speed = 8f;
    private float lifetime = 0.9f;

    private TMP_Text damageText;
    
    public void Awake()
    {
        damageText = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        do
        {
            direction = Random.insideUnitCircle.normalized;
        } while (direction.y < 0);
        StartCoroutine(ShowDamageText());
    }

    public void SetDamage(float damage, string damageType, bool isCritical)
    {
        damageText.color = (damageType == "Normal") ? Color.white
            : (damageType == "Additional") ? new Color(1f, 0.5f, 0f)
            : (damageType == "Effect") ? Color.red : Color.white;
        damageText.text = isCritical ? "!" + damage.ToString("F1") 
                                     : damage.ToString("F1");
    }
    
    IEnumerator ShowDamageText()
    {
        float timer = 0f;
        Vector2 StartPosition = rectTransform.anchoredPosition;

        while (timer < lifetime)
        {
            StartPosition = (direction * speed * timer);
            timer += Time.deltaTime;
            yield return null;
        }
        DamageTextManager.instance.TakeDamageText(this);
    }
}
