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
        SetAugment("STAT_2", "Super Ball III", "Attack damage +3.1", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(3.1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-3.1f);
    }
}

public class CritChance35Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_3", "Sharpness III", "Critical chance +35%", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.35f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.35f);
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
        SetAugment("STAT_5", "Fatal Attack II", "Critical damage +30%", 2);
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

public class BossDamageUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_6", "Boss Killer", "+10% Additional damage to boss", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply(){  }

    public override void Remove(){  }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            Brick.TotalMultiple *= 1.1f;
        }
    }
}

public class AtkUpWhenCritical : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_7", "Lucky Streak", "When critical attack hits, attack damage +1.0\nfor 3 seconds", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    { 

    }

    public override void Remove()
    { 

    }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf)
        {
            RunAttackDamageUpFewSec(3f, 1.2f);
        }
    }

    public void RunAttackDamageUpFewSec(float setTime, float AttackUpValue)
    {
        StartCoroutine(AttackDamageUpFewSec(setTime, AttackUpValue));
    }
    
    IEnumerator AttackDamageUpFewSec(float setTime, float AttackUpValue)
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(1.0f);
        yield return new WaitForSeconds(setTime);
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-1.0f);
    }
}

public class ExpGain30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_8", "Senior", "Exp gain +30%", 4);
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

public class AtkUp1 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_9", "Super Ball I", "Attack damage +0.8", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(0.8f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-0.8f);
    }
}

public class CritChance10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_10", "Sharpness I", "Critical chance +10%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.1f);
    }
}

public class CritChance20Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_11", "Sharpness II", "Critical chance +20%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.2f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.2f);
    }
}

public class GoldGain10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_12", "Gold Rush I", "Gold gain +10%", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(0.1f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_GoldGain(-0.1f);
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

public class PowerButSlow : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_15", "Heavyweight Ball", "Attack damage +15%\nBall max speed -33%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 10f;
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.15f);
    }

    public override void Remove()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 15f;
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.15f);
    }
}

public class WeakButFast : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_16", "Lightweight Ball", "Attack damage -15%\nBall max speed +33%", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 20f;
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.85f);
    }

    public override void Remove()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 15f;
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.85f);
    }
}

public class WeakButStrongEffect : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_17", "Magician", "Attack damage -12%\nEffect damage +12%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.88f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.12f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.88f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.12f);
    }
}

public class WeakButStrongAtk : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_18", "Fighter", "Attack damage +12%\nEffect damage -12%", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.12f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(0.88f);
    }

    public override void Remove()
    {
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
        SetAugment("STAT_20", "Time Accelerator II", "Field effect cooltime -20%", 3);
        SetAdditionalDescription("Field effect cooltime :" + AugmentManager.instance.FieldGlobalCooldown.ToString("F1") + "- 20% ="
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
        SetAugment("STAT_23", "Trinity", "Attack Damage, Effect Damage, CriticalDamage 33% up", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.33f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.33f);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(0.33f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.33f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.33f);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-0.33f);
    }
}

public class Perfectionism : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_24", "Perfectionism", "Critical chance becomes 100%,\n Attack damage -40%", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_CriticalChance(1 - StatManager.instance.Persistent_stat.CriticalChance);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.6f);
    }

    public override void Remove()
    {
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
    }

    public override void Apply()
    {
        isGetAugment = true;
        LevelManager.Instance.StatUpValue[1][0] *= 2;
        LevelManager.Instance.StatUpValue[1][1] *= 2;
    }

    public override void Remove()
    {
        isGetAugment = false;
        LevelManager.Instance.StatUpValue[1][0] /= 2;
        LevelManager.Instance.StatUpValue[1][1] /= 2;
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if(StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance > 0.1f)
            {
                float excess = (StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance) - 0.1f;
                StatManager.instance.ChangeTemporaryStats_CriticalChance(-excess);
            }
        }
    }
}

public class ExpGain10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_26", "Freshman", "Exp gain +10%", 5);
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
        SetAugment("STAT_27", "Graduate", "Exp gain +50%", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.5f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_ExpGain(-0.5f);
    }
}

public class LevelUpAttackDamageValueUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_25", "Perseverant", "Attack damage upgrade values are doubled after level up", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<LevelUpEffectDamageValueUp>());
        LevelManager.Instance.StatUpValue[0][0] *= 2;
        LevelManager.Instance.StatUpValue[0][1] *= 2;
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<LevelUpEffectDamageValueUp>());
        LevelManager.Instance.StatUpValue[0][0] /= 2;
        LevelManager.Instance.StatUpValue[0][1] /= 2;
    }
}

public class LevelUpEffectDamageValueUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_25", "Awake", "Effect damage upgrade values are doubled after level up", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<LevelUpAttackDamageValueUp>());
        LevelManager.Instance.StatUpValue[3][0] *= 2;
        LevelManager.Instance.StatUpValue[3][1] *= 2;
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<LevelUpAttackDamageValueUp>());
        LevelManager.Instance.StatUpValue[3][0] /= 2;
        LevelManager.Instance.StatUpValue[3][1] /= 2;
    }
}