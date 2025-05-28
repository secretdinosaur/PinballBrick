using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class HealBrick : IBrick
{
    public Color startColor;
    private int HealCount = 1;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "HealBrick";
        startColor = spriteRenderer.material.GetColor("_Color");
        BrickHealCoroutine = StartCoroutine(BrickHeal());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    public IEnumerator BrickHeal()
    {
        Color startColor = spriteRenderer.material.GetColor("_Color");
        Color targetColor = startColor * 2;


        while (true)
        {
            yield return new WaitForSeconds(10f - HealCount / 5);

            if (HealCount <= 25)
                HealCount += 1;
            if (HP + HpTemp / 10 > HpTemp && HP != HpTemp)
            {
                HP = HpTemp;
                ShowText("+" + (HpTemp - HP).ToString(), "Heal", false);

                float duration = 0.25f;
                float elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    spriteRenderer.material.SetColor("_Color", Color.Lerp(startColor, targetColor, elapsed / duration));
                    yield return null;
                }

                elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    spriteRenderer.material.SetColor("_Color", Color.Lerp(targetColor, startColor, elapsed / duration));
                    yield return null;
                }

                spriteRenderer.material.SetColor("_Color", startColor);
            }
            else if (HP != HpTemp)
            {
                HP += (int)(HpTemp / 10);
                ShowText("+" + ((int)(HpTemp / 10)).ToString(), "Heal", false);

                float duration = 0.25f;
                float elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    spriteRenderer.material.SetColor("_Color", Color.Lerp(startColor, targetColor, elapsed / duration));
                    yield return null;
                }

                elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    spriteRenderer.material.SetColor("_Color", Color.Lerp(targetColor, startColor, elapsed / duration));
                    yield return null;
                }

                spriteRenderer.material.SetColor("_Color", startColor);
            }
            else // for sync heal cooltime
            {
                yield return new WaitForSeconds(0.5f);
            }

        }
    }
}
