using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IgnoreBrick : IBrick
{
    public bool Ignoring = false;
    public float IgnoreTime;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "IgnoreBrick";
        Ignoring = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if(!Ignoring)
            {
                TakeDamage(collision.gameObject.GetComponent<IBall>());
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(IgnoreDamage());
                }
            }
            else
            {
                StartCoroutine(IgnoreDamageEffect());
            }
        }
    }

    public IEnumerator IgnoreDamage()
    {
        Ignoring = true;
        spriteRenderer.material.SetColor("_Color", new Color(130 / 255f, 130 / 255f, 130 / 255f));
        yield return new WaitForSeconds(IgnoreTime);
        Ignoring = false;
        spriteRenderer.material.SetColor("_Color", new Color(222 / 255f, 222 / 255f, 222 / 255f));
    }

    public IEnumerator IgnoreDamageEffect()
    {
        spriteRenderer.material.SetColor("_Color", new Color(150 / 255f, 150 / 255f, 150 / 255f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(130 / 255f, 130 / 255f, 130 / 255f));

    }
}
