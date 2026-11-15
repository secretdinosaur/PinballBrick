using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;
using static UnityEngine.UI.Image;

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
            case "Explosion":
                Routine = StartCoroutine(ShowDamageTextRandom());
                damageText.color = Color.red;
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
                Color.RGBToHSV(damageText.color, out float h, out float s, out float v);
                v = Mathf.Clamp01(v + 0.3f);
                Color brighter = Color.HSVToRGB(h, s, v);

                break;
            case "Heal":
                Routine = StartCoroutine(ShowDamageTextPoison());
                damageText.color = Color.magenta;
                break;
            case "Gold":
                Routine = StartCoroutine(ShowDamageTextRandom());
                damageText.color = new Color(1, 1, 160f / 255f);
                break;
            case "Exp":
                Routine = StartCoroutine(ShowDamageTextRandom());
                damageText.color = new Color(161f / 255f, 1, 1);
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
            rectTransform.anchoredPosition = StartPosition + (direction * speed * timer - direction * speed * 1f * timer * timer);
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
