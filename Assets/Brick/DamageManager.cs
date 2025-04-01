using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        brick.RecentReceivedDamage = FinalDamage;
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

public static partial class DamageManager //Special Effect Attack
{
    public static IEnumerator AdditionalAttack(IBrick brick, float MaxAttackTime, float TimeCount, float DamageMultiflier, bool IsStrong = false) // AdditionalAttack Coroutine
    {
        yield return new WaitForSeconds(0.5f);

        while (MaxAttackTime > 0f)
        {
            if (!brick.gameObject.activeSelf)
            {
                yield break;
            }
            if (IsStrong)
                VisualEffectManager.Instance.SpawnParticle("StrongAdditionalAttack", brick.transform.position, 1);
            else
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
        if (AugmentManager.instance.determineAugmentID("SPECIAL_59"))
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
        VisualEffectManager.Instance.SpawnParticle("Curse", brick.transform.position, 5);
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
        if (brick.BrickType == "ShieldBrick" && !brick.GetComponent<ShieldBrick>().IsShieldDestroyed)
        {
            damage = 1;
            brick.HP -= damage;
            brick.ShowText("1", DamageType, isCritical);
        }
        else if(brick.BrickType == "WhiteBrick")
        {
            return;
        }
        else
        {
            brick.HP -= damage;
            brick.ShowText(damageText, DamageType, isCritical);
        }
    }

    public static float DecayDamageCalculate(IBrick brick, float Damage)
    {
        Damage *= Mathf.Pow(AugmentManager.instance.DecayDamageMultiple, brick.BrickEffect.EffectStatusSet["Decay"].EffectStack);
        return Damage;
    }
}

public class BrickEffectManager : MonoBehaviour
{
    private IBrick brick;
    public Dictionary<string, EffectStatus> EffectStatusSet;
    private float FireEffectDuration;
    private float PoisonEffectDuration;
    private float DecayEffectDuration;

    public BrickEffectManager(IBrick brick)
    {
        this.brick = brick;
        FireEffectDuration = 0f;
        PoisonEffectDuration = 0f;
        DecayEffectDuration = 0f;

        EffectStatusSet = new Dictionary<string, EffectStatus>()
        {
            { "Fire", new EffectStatus("Fire", false, 0, AugmentManager.instance.FireStackLimit) },
            { "Poison", new EffectStatus("Poison", false, 0, AugmentManager.instance.PoisonStackLimit) },
            { "Decay", new EffectStatus("Decay", false, 0, AugmentManager.instance.DecayStackLimit) }
        };
    }

    public IEnumerator DamageByFireEffect(float effectDuration)
    {
        EffectStatusSet["Fire"].IsActive = true;
        FireEffectDuration = effectDuration;
        float interval = 0.2f;

        if (EffectStatusSet["Fire"].EffectStack == EffectStatusSet["Fire"].EffectStackLimit)
        {
            brick.lineRenderer.material.SetColor("_OuterFireColor", new Color(255 / 255f, 8 / 255f, 0) * 10f);
            brick.lineRenderer.material.SetFloat("_FireHeight", 1f);
        }
        else
        {
            brick.lineRenderer.material.SetColor("_OuterFireColor", new Color(191 / 255f, 8 / 255f, 0) * 5f);
            brick.lineRenderer.material.SetFloat("_FireHeight", 2f);
        }
        brick.lineRenderer.enabled = true;

        while (FireEffectDuration > 0f)
        {
            bool isCritical = false;
            float EffectAttackDamage;
            if (EffectStatusSet["Fire"].EffectStack == EffectStatusSet["Fire"].EffectStackLimit)
            {
                brick.lineRenderer.material.SetColor("_OuterFireColor", new Color(255 / 255f, 8 / 255f, 0) * 10f);
                brick.lineRenderer.material.SetFloat("_FireHeight", 1f);
            }
            else
            {
                brick.lineRenderer.material.SetColor("_OuterFireColor", new Color(191 / 255f, 8 / 255f, 0) * 5f);
                brick.lineRenderer.material.SetFloat("_FireHeight", 2f);
            }
            if (AugmentManager.instance.determineAugmentID("SPECIAL_59"))
            {
                isCritical = DamageManager.IsCriticalHit(brick);
            }

            EffectAttackDamage = isCritical
                ? (StatManager.instance.Temporary_stat.EffectDamage *
                   StatManager.instance.statMultiplier.EffectDamageMultiplier)
                  * EffectStatusSet["Fire"].EffectStack * StatManager.instance.Temporary_stat.EffectCriticalDamage
                : (StatManager.instance.Temporary_stat.EffectDamage *
                   StatManager.instance.statMultiplier.EffectDamageMultiplier)
                  * EffectStatusSet["Fire"].EffectStack;
            float FinalEffectAttackDamage = (brick.BrickType == "ShieldBrick" && !brick.GetComponent<ShieldBrick>().IsShieldDestroyed)
                ? 1
                : brick.DecayDamageCalculate(EffectAttackDamage);
            brick.HP -= FinalEffectAttackDamage;
            brick.ShowText(FinalEffectAttackDamage.ToString("F1"), "Fire", isCritical);
            FireEffectDuration -= interval;
            if (!brick.gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
        brick.lineRenderer.enabled = false;
        brick.Fire = null;
        EffectStatusSet["Fire"].EffectStack = 0;
    }

    public float initialDuration;
    public IEnumerator DamageByPoisonEffect(float effectDuration)   
    {
        Material PoisonMaterial = brick.PoisonEffect.GetComponent<Renderer>().material;

        float elapsedTime = 0;
        brick.PoisonEffect.SetActive(true);
        EffectStatusSet["Poison"].IsActive = true;
        elapsedTime = 0f;

        if (EffectStatusSet["Poison"].EffectStack == EffectStatusSet["Poison"].EffectStackLimit)
        {
            PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
            PoisonMaterial.SetColor("_InnerColor", new Color(214 / 255f, 0 / 255f, 1));
            PoisonMaterial.SetFloat("_InnerPower", 1.75f);
        }
        else
        {
            PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
            PoisonMaterial.SetColor("_InnerColor", new Color(176 / 255f, 180 / 255f, 0));
            PoisonMaterial.SetFloat("_InnerPower", 1.5f);
        }

        while (elapsedTime < 1)
        {
            PoisonMaterial.SetFloat("_Alpha", Mathf.Lerp(1.1f, 0f, elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float interval = 1f;
        initialDuration = effectDuration;
        PoisonEffectDuration = effectDuration;
        
        while (PoisonEffectDuration > 0f)
        {
            if (EffectStatusSet["Poison"].EffectStack >= EffectStatusSet["Poison"].EffectStackLimit && !AugmentManager.instance.PoisonBrickSet.Contains(brick))
            {
                AugmentManager.instance.PoisonBrickSet.Add(brick);
            }
            if (EffectStatusSet["Poison"].EffectStack == EffectStatusSet["Poison"].EffectStackLimit)
            {
                PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
                PoisonMaterial.SetColor("_InnerColor", new Color(214 / 255f, 0 / 255f, 1));
                PoisonMaterial.SetFloat("_InnerPower", 1.75f);
            }
            else
            {
                PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
                PoisonMaterial.SetColor("_InnerColor", new Color(176 / 255f, 180 / 255f, 0));
                PoisonMaterial.SetFloat("_InnerPower", 1.5f);
            }
            bool isCritical = false;
            float EffectAttackDamage;
            if (AugmentManager.instance.determineAugmentID("SPECIAL_59"))
            {
                isCritical = DamageManager.IsCriticalHit(brick);
            }

            EffectAttackDamage = isCritical
                ? (brick.HP * (0.01f * StatManager.instance.statMultiplier.EffectDamageMultiplier * 0.8f
                         * EffectStatusSet["Poison"].EffectStack * StatManager.instance.Temporary_stat.EffectCriticalDamage))
                : (brick.HP * (0.01f * StatManager.instance.statMultiplier.EffectDamageMultiplier * 0.8f
                         * EffectStatusSet["Poison"].EffectStack));

            float FinalEffectAttackDamage = (brick.BrickType == "ShieldBrick" && !brick.GetComponent<ShieldBrick>().IsShieldDestroyed)
                ? 1
                : brick.DecayDamageCalculate(EffectAttackDamage);
            brick.HP -= FinalEffectAttackDamage;
            brick.ShowText(FinalEffectAttackDamage.ToString("0.###"), "Poison", isCritical);
            if (!brick.gameObject.activeInHierarchy)
            {
                yield break;
            }
            if (AugmentManager.instance.determineAugmentID("SPECIAL_16"))
            {
                if (Random.value < AugmentManager.instance.effectChance.PoisonChance)
                {
                    elapsedTime = 0;
                    while (elapsedTime < 0.25f)
                    {
                        PoisonMaterial.SetFloat("_InfinityPoison", Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    elapsedTime = 0;
                    while (elapsedTime < 0.25f)
                    {
                        PoisonMaterial.SetFloat("_InfinityPoison", Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    if (EffectStatusSet["Poison"].EffectStack < EffectStatusSet["Poison"].EffectStackLimit)
                    {
                        EffectStatusSet["Poison"].EffectStack++;
                    }
                    PoisonEffectDuration = initialDuration;
                }
                else
                {
                    PoisonEffectDuration -= interval;
                }
            }
            else
            {
                PoisonEffectDuration -= interval;
            }

            yield return new WaitForSeconds(interval);
        }
        elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            PoisonMaterial.SetFloat("_Alpha", Mathf.Lerp(0, 1.1f, elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        brick.Poison = null;
        EffectStatusSet["Poison"].IsActive = false;
        brick.PoisonEffect.SetActive(false);
        EffectStatusSet["Poison"].EffectStack = 0;
        
        brick.PoisonEffect.SetActive(false);
        if (AugmentManager.instance.PoisonBrickSet.Contains(brick))
        {
            AugmentManager.instance.PoisonBrickSet.Remove(brick);
        }
    }

    public IEnumerator DecayEffect(float effectDuration)
    {
        EffectStatusSet["Decay"].IsActive = true;
        DecayEffectDuration = effectDuration;
        int PreviousEffectStack = EffectStatusSet["Decay"].EffectStack;
        float interval = 0.1f;
        VisualEffectManager.Instance.SetDecayVisualEffect(brick);
        while (DecayEffectDuration > 0f)
        {
            DecayEffectDuration -= interval;
            if (!brick.gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
        brick.Decay = null;
        EffectStatusSet["Decay"].EffectStack = 0;
        VisualEffectManager.Instance.UnSetDecayVisualEffect(brick);
    }

    public void FireTimeUpdate(float effectDuration) { FireEffectDuration = effectDuration; }
    public void PoisonTimeUpdate(float effectDuration) { PoisonEffectDuration = effectDuration; }
    public void DecayTimeUpdate(float effectDuration) { DecayEffectDuration = effectDuration; }

    //public void StopFire() { if (brick.Fire != null) { StopCoroutine(brick.Fire); } }
    //public void StopPoison() { if (brick.Poison != null) { StopCoroutine(brick.Poison); } }
    //public void StopDecay() { if (brick.Decay != null) { StopCoroutine(brick.Decay); } }
}