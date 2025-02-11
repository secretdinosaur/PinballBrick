using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flamethrower : MonoBehaviour
{
    public float FlameDamage = 0.3f;
    private float timer = 0f;
    private float interval = 0.2f;
    private float damage;

    public void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick") && collision.gameObject.activeInHierarchy)
        {
            if (timer >= interval)
            {
                damage = (FlameDamage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier;
                collision.gameObject.GetComponent<IBrick>().ArbitraryDamageAttack(damage, false, damage.ToString("F1"), "Fire");
                if (Random.value < 0.1f)
                {
                    collision.gameObject.GetComponent<IBrick>().StartCoroutine(collision.gameObject.GetComponent<IBrick>().DamageByFireEffect(1f));
                }
                timer = 0f;
            }
        }
    }

    public void FlamethrowerOn()
    {
        gameObject.SetActive(true);
    }
}
