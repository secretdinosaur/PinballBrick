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

public class Atk30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_1", "Super Ball II", "Attack damage +30%", 4);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.3f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class Atk50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_2", "Super Ball III", "Attack damage +30%", 2);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.5f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.5f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChance50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_3", "Sharpness III", "Critical chance +50%", 2);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class CritDamage30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_4", "Fatal Attack I", "Critical damage +30%", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalDamage += 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalDamage -= 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class CritDamage50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_5", "Fatal Attack II", "Critical damage +50%", 2);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalDamage += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalDamage -= 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class BossDamageUp : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_6", "Boss Killer", "+30% Additional damage to boss", 4);
    }

    public override void Apply(){  }

    public override void Remove(){  }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            Brick.TotalMultiple *= 1.3f;
        }
    }
}

public class AtkUpWhenCritical : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_7", "Lucky Streak", "When critical attack hits, attack damage +20%\nfor 3 seconds", 4);
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
            RunAttackDamageUpFewSec(3f, 0.2f);
        }
    }

    public void RunAttackDamageUpFewSec(float setTime, float AttackUpValue)
    {
        StartCoroutine(AttackDamageUpFewSec(setTime, AttackUpValue));
    }
    
    IEnumerator AttackDamageUpFewSec(float setTime, float AttackUpValue)
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier += AttackUpValue;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
        yield return new WaitForSeconds(setTime);
        StatManager.instance.statMultiplier.AttackDamageMultiplier -= AttackUpValue;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class EffectCritical : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_8", "Critical Effect", "Critical is applied to special effects", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.EffectCriticalDamage += 0.2f;
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.EffectCriticalDamage -= 0.2f;
    }
}

public class Atk10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_9", "Super Ball I", "Attack damage +10%", 5);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.1f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.1f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class CritChance10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_10", "Sharpness I", "Critical chance +10%", 5);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class CritChance20Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_11", "Sharpness II", "Critical chance +20%", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.CriticalChance += 0.2f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.2f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class GoldGain10Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_12", "Gold Rush I", "Gold Gain +10%", 5);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.GoldGain += 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.1f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class GoldGain30Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_13", "Gold Rush II", "Gold Gain +30%", 4);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.GoldGain += 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.3f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class GoldGain50Up : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_14", "Gold Rush III", "Gold Gain +50%", 3);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.GoldGain += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.CriticalChance -= 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class PowerButSlow : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_15", "Heavyweight Ball", "Attack damage +50%\nBall max speed -33%", 4);
    }

    public override void Apply()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 10f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 15f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class WeakButFast : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_16", "Lightweight Ball", "Attack damage -30%\nBall max speed +33%", 4);
    }

    public override void Apply()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 20f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 0.7f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        GameObject.Find("SpawnObject").transform.Find("BallSpawner").GetComponent<BallManager>().maxSpeed = 15f;
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 0.7f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class WeakButStrongEffect : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_17", "Magicion", "Attack damage -66%\nEffect damage +60%", 4);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 0.33f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier *= 1.6f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 0.33f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier /= 1.6f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class WeakButStrongAtk : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_18", "Fighter", "Attack damage +60%\nEffect damage -66%", 4);
    }

    public override void Apply()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.6f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier *= 0.33f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }

    public override void Remove()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 1.6f;
        StatManager.instance.statMultiplier.EffectDamageMultiplier /= 0.33f;
        StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);
    }
}

public class DecreaseFieldCooldown5 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_19", "Time Accelerator I", "Field effect cooltime -5%", 5);
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

public class DecreaseFieldCooldown30 : StatAugment
{
    private void OnEnable()
    {
        SetAugment("STAT_20", "Time Accelerator II", "Field effect cooltime -30%", 3);
    }

    public override void Apply()
    {
        AugmentManager.instance.FieldGlobalCooldown *= 0.7f;
    }

    public override void Remove()
    {
        AugmentManager.instance.FieldGlobalCooldown /= 0.7f;
    }
}

public class CritChanceToAttackDamage : StatAugment
{
    bool isGetAugment = false;
    private void OnEnable()
    {
        SetAugment("STAT_21", "Risk Management", "Converts 1% critical chance to 0.4 attack damage", 4);
    }

    public override void Apply()
    {
        isGetAugment = true;
    }

    public override void Remove()
    {
        isGetAugment = false;
    }

    public void Update()
    {
        if (isGetAugment && (StatManager.instance.Persistent_stat.CriticalChance > 0 || StatManager.instance.Temporary_stat.CriticalChance > 0))
        {
            float TotalCritChance = StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance;
            TransferToAttackDamage(TotalCritChance);
        }
    }

    private void TransferToAttackDamage(float CritChance)
    {
        StatManager.instance.Temporary_stat.AttackDamage += CritChance * 0.4f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
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
    }

    public override void Apply()
    {
        float attackDamageMin = -3f, attackDamageMax = 7f;
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
        StatManager.instance.Temporary_stat.AttackDamage += ChangeAttackDamage;
        StatManager.instance.Temporary_stat.EffectDamage += ChangeEffectDamage;
        StatManager.instance.Temporary_stat.CriticalDamage += ChangeCritDamage;
        StatManager.instance.Temporary_stat.CriticalChance += ChangeCritChance;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.AttackDamage -= ChangeAttackDamage;
        StatManager.instance.Temporary_stat.EffectDamage -= ChangeEffectDamage;
        StatManager.instance.Temporary_stat.CriticalDamage -= ChangeCritDamage;
        StatManager.instance.Temporary_stat.CriticalChance -= ChangeCritChance;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}