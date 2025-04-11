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
    private float speed = 200f;
    private float lifetime = 0.5f;

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
        switch(TextType)
        {
            case "Normal":
                Routine = StartCoroutine(ShowDamageTextRandom());
                damageText.color = Color.white;
                break;
            case "Additional":
                Routine = StartCoroutine(ShowDamageTextRandom());
                damageText.color = new Color(1f, 0.5f, 0f);
                break;
            case "Fire":
                Routine = StartCoroutine(ShowDamageTextFire());
                damageText.color = Color.red;
                break;
            case "Lightning":
                Routine = StartCoroutine(ShowDamageTextLightning());
                damageText.color = Color.cyan;
                break;
            case "Poison":
                Routine = StartCoroutine(ShowDamageTextPoison());
                damageText.color = Color.green;
                break;
            case "Curse":
                Routine = StartCoroutine(ShowDamageTextPoison());
                damageText.color = new Color(110 / 255f, 0, 2 / 255f);
                break;
            case "Heal":
                Routine = StartCoroutine(ShowDamageTextPoison());
                damageText.color = Color.magenta;
                break;
            default:
                Routine = StartCoroutine(ShowDamageTextRandom());
                damageText.color = Color.white;
                break;

        }
        damageText.text = isCritical ? "!" + Text : Text;
    }
    
    IEnumerator ShowDamageTextRandom()
    {
        float timer = 0f;
        Vector2 StartPosition = rectTransform.anchoredPosition;
        while (timer < lifetime)
        {
            rectTransform.anchoredPosition = StartPosition + (direction * speed * timer - direction * speed * 0.5f * timer * timer);
            timer += Time.deltaTime;
            yield return null;
        }
        DamageTextManager.instance.TakeDamageText(this);
    }

    IEnumerator ShowDamageTextPoison()
    {
        float timer = 0f;
        Vector2 StartPosition = rectTransform.anchoredPosition;
        while (timer < lifetime * 2)
        {
            rectTransform.anchoredPosition = StartPosition + (direction * speed * timer - direction * speed * 0.5f * timer * timer) / 2;
            timer += Time.deltaTime;
            yield return null;
        }
        DamageTextManager.instance.TakeDamageText(this);
    }

    IEnumerator ShowDamageTextFire()
    {
        float timer = 0f;
        Vector2 StartPosition = rectTransform.anchoredPosition;
        while (timer < lifetime / 2)
        {
            rectTransform.anchoredPosition = StartPosition + (new Vector2(0,1f) * speed * timer - new Vector2(0,-1f) * speed * 0.5f * timer * timer);
            timer += Time.deltaTime;
            yield return null;
        }
        DamageTextManager.instance.TakeDamageText(this);
    }

    IEnumerator ShowDamageTextLightning()
    {
        float timer = 0f;
        Vector2 StartPosition = rectTransform.anchoredPosition;
        while (timer < lifetime / 2)
        {
            rectTransform.anchoredPosition = StartPosition + (direction * speed * timer * 2.5f);
            timer += Time.deltaTime;
            yield return null;
        }
        DamageTextManager.instance.TakeDamageText(this);
    }
}
