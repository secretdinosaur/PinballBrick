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
    public override string ID => "STAT_1";
    public override string Name => "Super Ball II";
    public override string Description => "<sprite=0> +1.3";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_2";
    public override string Name => "Super Ball III";
    public override string Description => "<sprite=0> +2.5";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_3";
    public override string Name => "Sharpness III";
    public override string Description => "<sprite=3> +15%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_4";
    public override string Name => "Fatal Attack I";
    public override string Description => "<sprite=1> +15%";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_5";
    public override string Name => "Fatal Attack II";
    public override string Description => "<sprite=1> +30%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_6";
    public override string Name => "Time Accelerator II";
    public override string Description => "<sprite=10> -10%";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_7";
    public override string Name => "Sticky Ball I";
    public override string Description => "<sprite=6> +10%";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_8";
    public override string Name => "Researcher II";
    public override string Description => "<sprite=9> +15%";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_9";
    public override string Name => "Super Ball I";
    public override string Description => "<sprite=0> +1";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_10";
    public override string Name => "Sharpness I";
    public override string Description => "<sprite=3> +5%";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_11";
    public override string Name => "Sharpness II";
    public override string Description => "<sprite=3> +8%";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_12";
    public override string Name => "Gold Rush I";
    public override string Description => "<sprite=8> +20%";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_13";
    public override string Name => "Gold Rush II";
    public override string Description => "<sprite=8> +30%";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_14";
    public override string Name => "Gold Rush III";
    public override string Description => "<sprite=8> +50%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_15";
    public override string Name => "Supreme Ball";
    public override string Description => "<sprite=0>, <sprite=2> +15%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_16";
    public override string Name => "Psychic Ball I";
    public override string Description => "<sprite=2> +1";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_17";
    public override string Name => "Magician";
    public override string Description => "<sprite=0> -12%\n<sprite=2> +12%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<WeakButStrongAtk>().FirstOrDefault());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.88f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.12f);
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<WeakButStrongAtk>().FirstOrDefault());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.88f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.12f);
    }
}

public class WeakButStrongAtk : StatAugment
{
    public override string ID => "STAT_18";
    public override string Name => "Fighter";
    public override string Description => "<sprite=0> +12%\n<sprite=2> -12%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<WeakButStrongEffect>().FirstOrDefault());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.12f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(0.88f);
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<WeakButStrongEffect>().FirstOrDefault());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.12f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-0.88f);
    }
}

public class DecreaseFieldCooldown5 : StatAugment
{
    public override string ID => "STAT_19";
    public override string Name => "Time Accelerator I";
    public override string Description => "<sprite=10> +5%";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_20";
    public override string Name => "Time Accelerator III";
    public override string Description => "<sprite=10> +20%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
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
    public override string ID => "STAT_21";
    public override string Name => "Risk Management";
    public override string Description => "<sprite=3> is fixed at 0%\nConverts 1% <sprite=3> to <sprite=0> +0.04";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_22";
    public override string Name => "Stat Roulette";
    public override string Description => "<sprite=0>, <sprite=1>, <sprite=2>, <sprite=3> randomly increase or decrease";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public float ChangeAttackDamage;
    public float ChangeEffectDamage;
    public float ChangeCritDamage;
    public float ChangeCritChance;

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
    public override string ID => "STAT_23";
    public override string Name => "Trinity";
    public override string Description => "<sprite=0>, <sprite=2>, <sprite=3> 33.3% up and Exp gain 20%";
    public override int Rarity => 0;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_24";
    public override string Name => "Perfectionism";
    public override string Description => "<sprite=3> becomes 100%,\n <sprite=0> -50%";
    public override int Rarity => 2;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<MassivelyCriticalDamageUp>().FirstOrDefault());
        StatManager.instance.ChangeTemporaryStats_CriticalChance(1 - StatManager.instance.Persistent_stat.CriticalChance);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.6f);
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<MassivelyCriticalDamageUp>().FirstOrDefault());
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-StatManager.instance.Temporary_stat.CriticalChance);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.6f);
    }
}

public class MassivelyCriticalDamageUp : StatAugment
{
    public override string ID => "STAT_25";
    public override string Name => "Waiter";
    public override string Description => "Max <sprite=3> becomes 10%,\n Critical damage upgrade values are doubled after level up";
    public override int Rarity => 2;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;


    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<Perfectionism>().FirstOrDefault());
        LevelManager.Instance.StatUpValue[1][0] *= 2;
        LevelManager.Instance.StatUpValue[1][1] *= 2;
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<Perfectionism>().FirstOrDefault());
        LevelManager.Instance.StatUpValue[1][0] /= 2;
        LevelManager.Instance.StatUpValue[1][1] /= 2;
    }
}

public class ExpGain10Up : StatAugment
{
    public override string ID => "STAT_26";
    public override string Name => "Researcher I";
    public override string Description => "<sprite=9> +10%";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_27";
    public override string Name => "Researcher III";
    public override string Description => "<sprite=9> +30%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_28";
    public override string Name => "Greedy";
    public override string Description => "<sprite=0>, <sprite=2> stats share 20% of each other's increase";
    public override int Rarity => 1;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float UpgradeAttackStat;
    float UpgradeEffectStat;

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
    public override string ID => "STAT_29";
    public override string Name => "Awake";
    public override string Description => "All stat up";
    public override int Rarity => 1;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(2 * LevelManager.Instance.StatUpValue[0][1]);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(2f * LevelManager.Instance.StatUpValue[1][1]);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(2 * LevelManager.Instance.StatUpValue[2][1]);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(2 * LevelManager.Instance.StatUpValue[3][1]);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(2f * LevelManager.Instance.StatUpValue[4][1]);
        StatManager.instance.ChangeTemporaryStats_ExpGain(2f * LevelManager.Instance.StatUpValue[6][1]);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-(2 * LevelManager.Instance.StatUpValue[0][1]));
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-(2f * LevelManager.Instance.StatUpValue[1][1]));
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-(2 * LevelManager.Instance.StatUpValue[2][1]));
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-(2 * LevelManager.Instance.StatUpValue[3][1]));
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-(2f * LevelManager.Instance.StatUpValue[4][1]));
        StatManager.instance.ChangeTemporaryStats_ExpGain(-(2f * LevelManager.Instance.StatUpValue[6][1]));
    }
}

public class EffectDamageUp2 : StatAugment
{
    public override string ID => "STAT_30";
    public override string Name => "Psychic Ball II";
    public override string Description => "<sprite=2> +1.3";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_31";
    public override string Name => "Psychic Ball III";
    public override string Description => "<sprite=2> +2.5";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_32";
    public override string Name => "Level Up Mania";
    public override string Description => "After level up, <sprite=9> +2%";
    public override int Rarity => 5;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float TotalUpgradedValue;

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
    public override string ID => "STAT_33";
    public override string Name => "Sticky Ball II";
    public override string Description => "<sprite=6> +15%";
    public override int Rarity => 4;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_34";
    public override string Name => "Sticky Ball III";
    public override string Description => "<sprite=6> +30%";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "STAT_35";
    public override string Name => "Late Bloomer";
    public override string Description => "<sprite=0>, <sprite=2> -50% and Gain +3% <sprite=0>, <sprite=2> each wave is cleared (up to 75%)";
    public override int Rarity => 3;
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public int PassedWave = 0;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("LateBloomer", GetLateBloomer());
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.5f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(0.5f);
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("LateBloomer"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("LateBloomer");
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



    IEnumerator GetLateBloomer()
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
        AugmentManager.instance.AugmentRoutines.Stop("LateBloomer");
    }
}