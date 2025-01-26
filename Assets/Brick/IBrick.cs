using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Random = UnityEngine.Random;

public partial class IBrick : MonoBehaviour
{
    public int Point;
    public float BasicHP;
    private float hp;
    public bool IsBoss = false;
    public TextMeshPro HPText;
    public string Type;

    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;

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
                ;
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
    private void OnEnable()
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
        }
        
        if (AugmentManager.instance.determineAugmentID("STAT_6")) //Boss Damage Up
        {
            if (IsBoss)
            {
                TotalMultiple *= 1.3f;
            }
        }
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
}