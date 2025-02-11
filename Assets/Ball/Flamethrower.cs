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
    private void Start()
    {
        damage = (FlameDamage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick") && collision.gameObject.activeInHierarchy)
        {
            if (timer >= interval)
            {
                collision.gameObject.GetComponent<IBrick>().ArbitraryDamageAttack(damage, false, damage.ToString("F1"), "Fire");
                timer = 0f;
            }
        }
    }


    private IEnumerator FlametrowerEffect(IBrick brick)
    {
        while (true)
        {
            if(brick == null || !brick.gameObject.activeInHierarchy)
            {
                yield break;
            }
            brick.ArbitraryDamageAttack(FlameDamage, false, FlameDamage.ToString("F1"), "Fire");

            if (Random.value < 0.1f)
            {
                brick.StartCoroutine(brick.DamageByFireEffect(1f));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FlamethrowerOn()
    {
        gameObject.SetActive(true);
    }
}
