using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flamethrower : MonoBehaviour
{
    public float FlameDamage = 0.3f;
    private float timer = 0f;
    private float interval = 0.2f;

    // 트리거 영역 내의 Brick들을 저장할 리스트
    private List<IBrick> bricksInRange = new List<IBrick>();

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            for (int i = bricksInRange.Count - 1; i >= 0; i--)
            {
                IBrick brick = bricksInRange[i];

                if (brick == null || !((MonoBehaviour)brick).gameObject.activeInHierarchy)
                {
                    bricksInRange.RemoveAt(i);
                    continue;
                }

                float damage = (FlameDamage + StatManager.instance.Temporary_stat.EffectDamage) * 
                    StatManager.instance.statMultiplier.EffectDamageMultiplier;
                brick.ArbitraryDamageAttack(damage, false, damage.ToString("F1"), "Fire");

                if (Random.value < 0.1f)
                {
                    brick.DamageByFireEffect(1f);
                }
            }
            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick") && collision.gameObject.activeInHierarchy)
        {
            IBrick brick = collision.gameObject.GetComponent<IBrick>();
            if (brick != null && !bricksInRange.Contains(brick))
            {
                bricksInRange.Add(brick);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            IBrick brick = collision.gameObject.GetComponent<IBrick>();
            if (brick != null && bricksInRange.Contains(brick))
            {
                bricksInRange.Remove(brick);
            }
        }
    }

    public void FlamethrowerOn()
    {
        gameObject.SetActive(true);
    }
}
