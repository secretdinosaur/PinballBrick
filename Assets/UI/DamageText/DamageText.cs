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
    private float speed = 500f;
    private float lifetime = 0.9f;

    private TMP_Text damageText;

    public Coroutine Routine = null;
    
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
    }

    public void SetText(string Text, string TextType, bool isCritical)
    {
        Routine = StartCoroutine(ShowDamageText());

        damageText.color = (TextType == "Normal") ? Color.white
            : (TextType == "Additional") ? new Color(1f, 0.5f, 0f)
            : (TextType == "Fire") ? Color.red 
            : (TextType == "Poison") ? Color.green 
            : (TextType == "Lightning") ? Color.cyan 
            : (TextType == "Curse") ? Color.blue
            : (TextType == "Heal") ? new Color(1f, 0f, 1f) : Color.magenta;
        damageText.text = isCritical ? "!" + Text : Text;
    }
    
    IEnumerator ShowDamageText()
    {
        float timer = 0f;
        Vector2 StartPosition = rectTransform.anchoredPosition;
        while (timer < lifetime)
        {
            rectTransform.anchoredPosition = StartPosition + (direction * speed * timer);
            timer += Time.deltaTime;
            yield return null;
        }
        DamageTextManager.instance.TakeDamageText(DamageTextManager.instance.DamageTextQueue.Dequeue());
    }
}
