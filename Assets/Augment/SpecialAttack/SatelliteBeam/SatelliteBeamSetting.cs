using System.Collections.Generic;
using UnityEngine;

public class SatelliteBeamSetting : MonoBehaviour
{
    private float TouchDamage;
    private float EffectDamage;
    private float timer;

    private List<IBrick> bricksInRange = new List<IBrick>();

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.15f)
        {
            foreach (var brick in bricksInRange)
            {
                if (brick != null)
                {
                    EffectDamage = (2f + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier;
                    brick.ArbitraryDamageAttack(EffectDamage, false, EffectDamage.ToString("F1"), "Fire");
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
            if (brick == null) return;

            if (!bricksInRange.Contains(brick))
            {
                bricksInRange.Add(brick);
            }

            TouchDamage = (500 + StatManager.instance.Temporary_stat.AttackDamage) * StatManager.instance.statMultiplier.AttackDamageMultiplier;
            brick.ArbitraryDamageAttack(TouchDamage, false, TouchDamage.ToString("F1"), "Normal");
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
}
