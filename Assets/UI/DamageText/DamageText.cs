using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DamageText : MonoBehaviour
{
    private Vector2 direction;
    private float radius = 1.0f;
    private float speed = 2.0f;
    private float lifetime = 1.0f;

    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;

    public void OnEnable()
    {
        do
        {
            direction = Random.insideUnitCircle.normalized;
        } while (direction.y < 0);

        StartCoroutine(ShowDamageText());
    }

    public void SetDamage(int damage)
    {
        textMeshPro.text = damage.ToString();
    }

    IEnumerator ShowDamageText()
    {
        float timer = 0f;
        Vector3 startPosition = transform.position;

        while (timer < lifetime)
        {
            transform.position = startPosition + (Vector3)(direction * speed * timer);
            timer += Time.deltaTime;
            yield return null;
        }

        DamageTextManager.instance.TakeDamageText(this);
    }
}
