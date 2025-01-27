using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Random = UnityEngine.Random;

public partial class IBrick : MonoBehaviour
{
    public int Point;
    public float BasicHP;
    private float hp;
    public bool IsBoss = false;
    public TextMeshPro HPText;
    public string BrickType;

    public float HP
    {
        get { return hp; }
        set
        {
            if (BrickType == "Shield" && !GetComponent<ShieldBrick>().IsShieldDestroyed)
            {
                if (GetComponent<ShieldBrick>().FirstShieldSpawn)
                {
                    hp = value;
                    GetComponent<ShieldBrick>().FirstShieldSpawn = false;
                }
                else
                {
                    hp -= 1;
                }

                if(hp <= 0)
                {
                    GetComponent<ShieldBrick>().DestroyShield();
                }
            }
            else
            {
                hp = value;
            }

            UpdateHPtext();

            if (hp <= 0)
            {
                isEffectActive = false;
                Fever.StartFever = true;
                Fever.Instance.FeverCount *= 2;
                Fever.Instance.FeverGauge = 100;
                ScoreManager.Instance.ContinuousScore += Point;
                LevelManager.Instance.Exp += 10 * StatManager.instance.Persistent_stat.BonusExp;
                BrickManager.Instance.TakeBrick(gameObject.GetComponent<IBrick>());
            }
        }
    }
    public void UpdateHPtext()
    {
        if (HPText != null)
        {
            HPText.text = ((int)hp).ToString();
        }
        else
        {
            HPText = GetComponentInChildren<TextMeshPro>();
            HPText.text = ((int)hp).ToString();
        }
    }
    public void Start()
    {
        HPText = GetComponentInChildren<TextMeshPro>();
    }
    public void OnEnable()
    {
        HP = BasicHP * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)];
    }
}

public partial class IBrick
{
    private int EffectStack;
    private bool isEffectActive = false;
    protected void TakeDamage(Ball ball)
    {
        HP -= FinalDamage();

        if (AugmentManager.instance.determineSpecialAugmentType("Effect") && gameObject.activeSelf)
        {
            EffectStack += (EffectStack != 5) ? 1 : 0;
            StartCoroutine(DamageByEffect(StatManager.instance.Temporary_stat.EffectDuration));
        }

        if (AugmentManager.instance.determineAugmentID("SPECIAL_4") && gameObject.activeSelf)
        {
            RunAdditionalAttack(0.3f, 0.1f);
        }
    }
    private float FinalDamage()
    {
        float TotalAttackStat = StatManager.instance.Persistent_stat.AttackDamage +
                                StatManager.instance.Temporary_stat.AttackDamage;
        float TotalMultiple = StatManager.instance.statMultiplier.AttackDamageMultiplier;
        if (AugmentManager.instance.determineAugmentID("SPECIAL_3")) //Bounce = Power
        {
            TotalMultiple *= Ball.BounceTime;
            Ball.BounceTime = 1;
        }

        if (IsCriticalHit())
        {
            TotalMultiple *= CriticalDamage();
            if (AugmentManager.instance.determineAugmentID("SPECIAL_5") && gameObject.activeSelf) //Critical Additional Attack
            {
                RunAdditionalAttack(0.5f, 0.5f);
            }

            if (AugmentManager.instance.determineAugmentID("STAT_7") && gameObject.activeSelf) //AtkUpWhenCriticalHit
            {
                RunAttackDamageUpFewSec(3f, 0.2f);
            }
        }
        
        if (AugmentManager.instance.determineAugmentID("STAT_6")) //Boss Damage Up
        {
            if (IsBoss)
            {
                TotalMultiple *= 1.3f;
            }
        }
        DamageTextManager.instance.CreateDamageText()
        return TotalAttackStat * TotalMultiple;
    }
    private bool IsCriticalHit()
    {
        return (Random.value < (StatManager.instance.Persistent_stat.CriticalChance != 0.0f
            ? StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance
            : StatManager.instance.Temporary_stat.CriticalChance));
    }
    private float CriticalDamage()
    {
        return (StatManager.instance.Persistent_stat.CriticalDamage + StatManager.instance.Temporary_stat.CriticalDamage);
    }
    IEnumerator DamageByEffect(float effectDuration)
    {
        if (isEffectActive) yield break;
        isEffectActive = true;

        float interval = 0.1f;
        while (effectDuration > 0 && EffectStack > 0)
        {
            HP -= (StatManager.instance.Temporary_stat.EffectDamage * StatManager.instance.statMultiplier.EffectDamageMultiplier)
                  * EffectStack;
            EffectStack--;
            effectDuration -= interval;
            yield return new WaitForSeconds(interval);
        }
        isEffectActive = false;
    }

    private void RunAdditionalAttack(float MaxAttackTime, float TimeCount)
    {
        StartCoroutine(AdditionalAttack(MaxAttackTime, TimeCount));
    }
    
    IEnumerator AdditionalAttack(float MaxAttackTime, float TimeCount) // AdditionalAttack Coroutine
    {
        yield return new WaitForSeconds(0.5f);
        
        while (MaxAttackTime > 0f)
        {
            HP -= IsCriticalHit() ? (StatManager.instance.Persistent_stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage)
                  * StatManager.instance.statMultiplier.AttackDamageMultiplier * CriticalDamage() :
                (StatManager.instance.Persistent_stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage)
                * StatManager.instance.statMultiplier.AttackDamageMultiplier;
            yield return new WaitForSeconds(TimeCount);
            MaxAttackTime -= TimeCount;
        }
    }

    private void RunAttackDamageUpFewSec(float setTime, float AttackUpValue)
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