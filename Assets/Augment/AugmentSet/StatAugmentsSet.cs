using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
/*
public class Example : StatAugment
{
    private void OnEnable()
    {
        SetAugment("Stat_N", "Example", "Example", 3);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public virtual void BeforeAttackEffect(IBrick Brick) { }
    
    public virtual void AfterAttackEffect(IBrick Brick) { }

    public virtual void CriticalAttackEffect(IBrick Brick) { }
}
*/

public class AtkUp2 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_1", "Super Ball II", "Attack damage +1.3", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(1.3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-1.3f);
    }
}

public class AtkUp3 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_2", "Super Ball III", "Attack damage +2.5", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(2.5f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-2.5f);
    }
}

public class CritChance35Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_3", "Sharpness III", "Critical chance +15%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.15f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.15f);
    }
}

public class CritDamage15Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_4", "Fatal Attack I", "Critical damage +15%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(0.15f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-0.15f);
    }
}

public class CritDamage30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_5", "Fatal Attack II", "Critical damage +30%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(0.3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-0.3f);
    }
}

public class DecreaseFieldCooldown10 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_6", "Time Accelerator II", "Field effect cooltime -10%", 4);
        SetAdditionalDescription("Field effect cooltime :" + AugmentManager.instance.FieldGlobalCooldown.ToString() + "- 10% ="
            + AugmentManager.instance.FieldGlobalCooldown * 0.9f);
    }

    public override void Apply()
    {
        AugmentManager.instance.FieldGlobalCooldown *= 0.9f;
    }

    public override void Remove()
    {
        AugmentManager.instance.FieldGlobalCooldown /= 0.9f;
    }
}

public class EffectDuration10 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_7", "Sticky Ball I", "Effect duration +10%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDuration(0.1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-0.1f);
    }
}

public class ExpGain30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_8", "Researcher II", "Exp gain +15%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.15f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(-0.15f);
    }
}

public class AtkUp1 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_9", "Super Ball I", "Attack damage +1", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-1f);
    }
}

public class CritChance10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_10", "Sharpness I", "Critical chance +5%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.05f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.05f);
    }
}

public class CritChance20Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_11", "Sharpness II", "Critical chance +8%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.08f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.08f);
    }
}

public class GoldGain10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_12", "Gold Rush I", "Gold gain +20%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(0.2f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(-0.2f);
    }
}

public class GoldGain30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_13", "Gold Rush II", "Gold gain +30%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(0.3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(-0.3f);
    }
}

public class GoldGain50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_14", "Gold Rush III", "Gold gain +50%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(0.5f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(-0.5f);
    }
}

public class AttackAndEffectUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_15", "Supreme Ball", "Attack, Effect damage +15%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.15f);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.15f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.15f);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.15f);
    }
}

public class EffectDamageUp1 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_16", "Psychic Ball I", "Effect damage +1", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-1f);
    }
}

public class WeakButStrongEffect : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_17", "Magician", "Attack damage -12%\nEffect damage +12%", 3);
        SetAdditionalDescription(null);
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<WeakButStrongAtk>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<WeakButStrongAtk>());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.88f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.12f);
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<WeakButStrongAtk>());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.88f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.12f);
    }
}

public class WeakButStrongAtk : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_18", "Fighter", "Attack damage +12%\nEffect damage -12%", 3);
        SetAdditionalDescription(null);
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<WeakButStrongEffect>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<WeakButStrongEffect>());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.12f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(0.88f);
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<WeakButStrongEffect>());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.12f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-0.88f);
    }
}

public class DecreaseFieldCooldown5 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_19", "Time Accelerator I", "Field effect cooltime -5%", 5);
        SetAdditionalDescription("Field effect cooltime :" + AugmentManager.instance.FieldGlobalCooldown.ToString("F1") + "- 5% =" 
            + AugmentManager.instance.FieldGlobalCooldown * 0.95f);
    }

    public override void Apply()
    {
        AugmentManager.instance.FieldGlobalCooldown *= 0.95f;
    }

    public override void Remove()
    {
        AugmentManager.instance.FieldGlobalCooldown /= 0.95f;
    }
}

public class DecreaseFieldCooldown20 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_20", "Time Accelerator III", "Field effect cooltime -20%", 3);
        SetAdditionalDescription("Field effect cooltime :" + AugmentManager.instance.FieldGlobalCooldown.ToString() + "- 20% ="
            + AugmentManager.instance.FieldGlobalCooldown * 0.8f);
    }

    public override void Apply()
    {
        AugmentManager.instance.FieldGlobalCooldown *= 0.8f;
    }

    public override void Remove()
    {
        AugmentManager.instance.FieldGlobalCooldown /= 0.8f;
    }
}

public class CritChanceToAttackDamage : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_21", "Risk Management", "Critical chance is fixed at 0%\nConverts 1% critical chance to 0.04 attack damage", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0);
    }

    public override void Remove()
    {

    }
}

public class RandomStatUpOrDown : StatAugment
{
    public float ChangeAttackDamage;
    public float ChangeEffectDamage;
    public float ChangeCritDamage;
    public float ChangeCritChance;
    private void OnEnable()
    {
        SetAugment("STAT_22", "Stat Roulette", "Damage, critical stats randomly increase or decrease", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        float attackDamageMin = -3f, attackDamageMax = 4f;
        float effectDamageMin = -0.1f, effectDamageMax = 0.2f;
        float critDamageMin = -0.1f, critDamageMax = 0.3f;
        float critChanceMin = -0.05f, critChanceMax = 0.1f;

        ChangeAttackDamage = Mathf.Round(Random.Range(attackDamageMin, attackDamageMax) * 100f) / 100f;
        ChangeEffectDamage = Mathf.Round(Random.Range(effectDamageMin, effectDamageMax) * 100f) / 100f;
        ChangeCritDamage = Mathf.Round(Random.Range(critDamageMin, critDamageMax) * 100f) / 100f;
        ChangeCritChance = Mathf.Round(Random.Range(critChanceMin, critChanceMax) * 100f) / 100f;

        bool hasPositiveValue = (ChangeAttackDamage > 0) || (ChangeEffectDamage > 0) || (ChangeCritDamage > 0) || (ChangeCritChance > 0);

        if (!hasPositiveValue)
        {
            int index = Random.Range(0, 4);

            switch (index)
            {
                case 0:
                    ChangeAttackDamage = Random.Range(0.1f, attackDamageMax);
                    break;
                case 1:
                    ChangeEffectDamage = Random.Range(0.1f, effectDamageMax);
                    break;
                case 2:
                    ChangeCritDamage = Random.Range(0.1f, critDamageMax);
                    break;
                case 3:
                    ChangeCritChance = Random.Range(0.01f, critChanceMax);
                    break;
            }
        }
        StatManager.instance.ChangeTemporaryStats_AttackDamage(ChangeAttackDamage);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(ChangeEffectDamage);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(ChangeCritDamage);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(ChangeCritChance);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-ChangeAttackDamage);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-ChangeEffectDamage);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-ChangeCritDamage);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-ChangeCritChance);
    }
}

public class Trinity : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_23", "Trinity", "Attack Damage, Effect Damage, Critical Chance 33.3% up and Exp gain 20%", 0);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.333f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.333f);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.333f);
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.2f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.333f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.333f);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.333f);
        StatManager.instance.ChangeTemporaryStats_ExpGain(-0.2f);
    }
}

public class Perfectionism : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_24", "Perfectionism", "Critical chance becomes 100%,\n Attack damage -50%", 2);
        SetAdditionalDescription(null);
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<MassivelyCriticalDamageUp>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<MassivelyCriticalDamageUp>());
        StatManager.instance.ChangeTemporaryStats_CriticalChance(1 - StatManager.instance.Persistent_stat.CriticalChance);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.6f);
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<MassivelyCriticalDamageUp>());
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-StatManager.instance.Temporary_stat.CriticalChance);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.6f);
    }
}

public class MassivelyCriticalDamageUp : StatAugment
{
    bool isGetAugment;
    private void OnEnable()
    {
        SetAugment("STAT_25", "Waiter", "Max critical chance becomes 10%,\n Critical damage upgrade values are doubled after level up", 2);
        SetAdditionalDescription(null);
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<Perfectionism>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        isGetAugment = true;
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<Perfectionism>());
        LevelManager.Instance.StatUpValue[1][0] *= 2;
        LevelManager.Instance.StatUpValue[1][1] *= 2;
    }

    public override void Remove()
    {
        isGetAugment = false;
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<Perfectionism>());
        LevelManager.Instance.StatUpValue[1][0] /= 2;
        LevelManager.Instance.StatUpValue[1][1] /= 2;
    }
}

public class ExpGain10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_26", "Researcher I", "Exp gain +10%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(-0.1f);
    }
}

public class ExpGain50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_27", "Researcher III", "Exp gain +30%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(-0.3f);
    }
}

public class AttackEffectBothUp : StatAugment
{
    float UpgradeAttackStat;
    float UpgradeEffectStat;
    private void OnEnable()
    {
        SetAugment("STAT_28", "Greedy", "Attack, Effect Damage stats share 20% of each other's increase", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        UpgradeAttackStat = ((StatManager.instance.Persistent_stat.EffectDamage + StatManager.instance.Temporary_stat.EffectDamage)
            * StatManager.instance.statMultiplier.EffectDamageMultiplier) * 0.2f;
        
        UpgradeEffectStat = ((StatManager.instance.Persistent_stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage)
            * StatManager.instance.statMultiplier.AttackDamageMultiplier) * 0.2f;

        StatManager.instance.ChangeTemporaryStats_AttackDamage(UpgradeAttackStat);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(UpgradeEffectStat);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-UpgradeAttackStat);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-UpgradeEffectStat);
    }
}

public class AllStatUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_29", "Awake", "All stat up", 1);
        SetAdditionalDescription("Except gold gain stat");
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(2 * LevelManager.Instance.StatUpValue[0][1]);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(1.2f * LevelManager.Instance.StatUpValue[1][1]);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(2 * LevelManager.Instance.StatUpValue[2][1]);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(2 * LevelManager.Instance.StatUpValue[3][1]);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(1.2f * LevelManager.Instance.StatUpValue[4][1]);
        StatManager.instance.ChangeTemporaryStats_ExpGain(1.2f * LevelManager.Instance.StatUpValue[6][1]);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-(2 * LevelManager.Instance.StatUpValue[0][1]));
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-(1.2f * LevelManager.Instance.StatUpValue[1][1]));
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-(2 * LevelManager.Instance.StatUpValue[2][1]));
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-(2 * LevelManager.Instance.StatUpValue[3][1]));
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-(1.2f * LevelManager.Instance.StatUpValue[4][1]));
        StatManager.instance.ChangeTemporaryStats_ExpGain(-(1.2f * LevelManager.Instance.StatUpValue[6][1]));
    }
}

public class EffectDamageUp2 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_30", "Psychic Ball II", "Effect damage +1.3", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(1.3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-1.3f);
    }
}

public class EffectDamageUp3 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_31", "Psychic Ball III", "Effect damage +2.5", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(2.5f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-2.5f);
    }
}

public class LevelUpMania : StatAugment
{
    float TotalUpgradedValue;
    private void OnEnable()
    {
        SetAugment("STAT_32", "Level Up Mania", "After level up, Exp gain +2%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        TotalUpgradedValue = 0;
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(-TotalUpgradedValue);
    }

    public override void AfterLevelUpEffect()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.02f);
        TotalUpgradedValue += 0.025f;
    }
}

public class EffectDuration15 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_33", "Sticky Ball II", "Effect duration +15%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDuration(0.15f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-0.15f);
    }
}

public class EffectDuration30 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_34", "Sticky Ball III", "Effect duration +30%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDuration(0.3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-0.3f);
    }
}

public class LateBloomer : StatAugment
{
    public int PassedWave = 0;
    Coroutine Routine;
    private void OnEnable()
    {
        SetAugment("STAT_35", "Late Bloomer", "Attack, Effect Damage -50% and Gain +3% Attack, Effect Damage each time Wave 1 is cleared (up to 75%)", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StatUpRoutine());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.5f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(0.5f);
    }

    public override void Remove()
    {
        if(Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.5f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-0.5f);
        if(PassedWave >= 1)
        {
            for (int i = 0; i < PassedWave; i++)
            {
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.0374f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.0374f);
            }
        }
    }

    IEnumerator StatUpRoutine()
    {
        int PreviousWave = GameManager.instance.Wave;
        while (PassedWave <= 25)
        {
            yield return new WaitUntil(() => GameManager.instance.Wave != PreviousWave);
            if (GameManager.instance.Wave > PreviousWave)
            {
                for (int i = 0; i < GameManager.instance.Wave - PreviousWave; i++)
                {
                    StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.0374f);
                    StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.0374f);
                    PassedWave++;
                }
            }
            PreviousWave = GameManager.instance.Wave;
        }
    }
}