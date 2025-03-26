using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static partial class DamageManager //Normal Attack
{
    public static void TakeDamage(IBrick brick, IBall ball)
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_51"))
        {
            BrickManager.Instance.MainBrick.RecentlyBall = ball;
            BrickManager.Instance.MainBrick.TotalMultiple = StatManager.instance.statMultiplier.AttackDamageMultiplier;
            BrickManager.Instance.MainBrick.SpecialCriticalChance = ball.SpecialCriticalChance;
            DamageProcess(BrickManager.Instance.MainBrick);
        }
        else
        {
            brick.RecentlyBall = ball;
            brick.TotalMultiple = StatManager.instance.statMultiplier.AttackDamageMultiplier;
            brick.SpecialCriticalChance = ball.SpecialCriticalChance;
            DamageProcess(brick);
        }
    }

    private static void DamageProcess(IBrick brick)
    {
        foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.BeforeAttackEffect(brick);
        }

        brick.HP -= FinalDamage(brick);

        foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.AfterMainAttackEffect(brick);
            Augment.AfterAttackEffect(brick);
        }
    }

    private static float FinalDamage(IBrick brick)
    {
        bool CriticalHit = false;
        float TotalAttackStat = StatManager.instance.Persistent_stat.AttackDamage +
                                StatManager.instance.Temporary_stat.AttackDamage +
                                brick.RecentlyBall.GrowthAttackDamage +
                                brick.RecentlyBall.BallBaseDamage;

        if (IsCriticalHit(brick))
        {
            CriticalHit = true;
            brick.TotalMultiple *= CriticalDamage();
            foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
            {
                Augment.CriticalAttackEffect(brick);
            }
        }

        float FinalDamage = (brick.BrickType == "ShieldBrick" && !brick.GetComponent<ShieldBrick>().IsShieldDestroyed)
            ? 1
            : brick.DecayDamageCalculate(TotalAttackStat * brick.TotalMultiple);
        if (brick.BrickType == "TeleportBrick") BrickManager.Instance.TeleportBrickPosition(brick.GetComponent<TeleportBrick>());
        brick.ShowText(FinalDamage.ToString("F1"), "Normal", CriticalHit);
        return FinalDamage;
    }
    public static bool IsCriticalHit(IBrick brick)
    {
        return (Random.value < (StatManager.instance.Persistent_stat.CriticalChance != 0.0f
            ? StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance + brick.SpecialCriticalChance
            : StatManager.instance.Temporary_stat.CriticalChance + brick.SpecialCriticalChance));
    }
    public static float CriticalDamage()
    {
        return (StatManager.instance.Persistent_stat.CriticalDamage + StatManager.instance.Temporary_stat.CriticalDamage);
    }
}

public static partial class DamageManager //Normal Effect Attack
{
    public static float DecayDamageCalculate(IBrick brick, float Damage)
    {
        Damage *= Mathf.Pow(AugmentManager.instance.DecayDamageMultiple, brick.EffectStatusSet["Decay"].EffectStack);
        return Damage;
    }
}

public static partial class DamageManager //Special Effect Attack
{
    public static IEnumerator AdditionalAttack(IBrick brick, float MaxAttackTime, float TimeCount, float DamageMultiflier) // AdditionalAttack Coroutine
    {
        yield return new WaitForSeconds(0.5f);

        while (MaxAttackTime > 0f)
        {
            if (!brick.gameObject.activeSelf)
            {
                yield break;
            }
            VisualEffectManager.Instance.SpawnParticle("AdditionalAttack", brick.transform.position, 1);
            bool CriticalHit = IsCriticalHit(brick);
            float AdditionalAttackDamage = CriticalHit
                    ? (StatManager.instance.Persistent_stat.AttackDamage +
                       StatManager.instance.Temporary_stat.AttackDamage)
                      * StatManager.instance.statMultiplier.AttackDamageMultiplier * CriticalDamage() * DamageMultiflier
                    : (StatManager.instance.Persistent_stat.AttackDamage +
                       StatManager.instance.Temporary_stat.AttackDamage)
                      * StatManager.instance.statMultiplier.AttackDamageMultiplier * DamageMultiflier;

            float FinalAdditionalAttackDamage = (brick.BrickType == "ShieldBrick" && !brick.GetComponent<ShieldBrick>().IsShieldDestroyed)
                ? 1
                : DecayDamageCalculate(brick, AdditionalAttackDamage);

            if (brick.BrickType == "TeleportBrick") BrickManager.Instance.TeleportBrickPosition(brick.GetComponent<TeleportBrick>());
            brick.HP -= FinalAdditionalAttackDamage;
            brick.ShowText(FinalAdditionalAttackDamage.ToString("F1"), "Additional", CriticalHit);
            foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
            {
                Augment.AfterAttackEffect(brick);
            }
            yield return new WaitForSeconds(TimeCount);
            MaxAttackTime -= TimeCount;
        }
    }
    public static void DamageByLightningEffect(IBrick brick, float DamageAmount, int ParticleCount)
    {
        bool isCritical = true;
        if (AugmentManager.instance.determineAugmentID("SPECIAL_7"))
        {
            isCritical = IsCriticalHit(brick);
        }
        float Percent = isCritical ? Random.Range(0.01f, 0.02f) : Random.Range(0.015f, 0.03f);
        float plusDamage = brick.HpTemp * Percent;

        float FinalDamage = brick.DecayDamageCalculate(plusDamage + DamageAmount);
        brick.HP -= FinalDamage;

        VisualEffectManager.Instance.SpawnParticle("Lightning", brick.transform.position, isCritical ? ParticleCount + 4 : ParticleCount);
        brick.ShowText(FinalDamage.ToString("F1"), "Lightning", isCritical);
    }

    public static void DamageByCurse(IBrick brick)
    {
        float baseDamage = (StatManager.instance.Persistent_stat.AttackDamage
            + StatManager.instance.Temporary_stat.AttackDamage
            + brick.RecentlyBall.GrowthAttackDamage)
            * StatManager.instance.statMultiplier.AttackDamageMultiplier;

        float damage = Random.Range(baseDamage * 0.5f, baseDamage * 1.3f);
        float FinalDamage = brick.DecayDamageCalculate(damage);
        brick.HP -= FinalDamage;
        brick.ShowText(FinalDamage.ToString("F1"), "Curse", false);
    }

    public static void ArbitraryDamageAttack(IBrick brick, float damage, bool isCritical, string damageText, string DamageType)
    {
        damage = DecayDamageCalculate(brick, damage);
        brick.HP -= damage;
        brick.ShowText(damageText, DamageType, isCritical);
    }
}
