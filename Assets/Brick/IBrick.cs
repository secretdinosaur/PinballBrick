using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class IBrick : MonoBehaviour
{
    public int Point;
    public float BasicHP;
    private float hp;

    public TextMeshPro HPText;
    public string Type;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;

            UpdateHPtext();

            if (hp <= 0)
            {

                Fever.StartFever = true;
                Fever.Instance.FeverCount *= 2;
                Fever.Instance.FeverGauge = 100;
                ScoreManager.Instance.ContinuousScore += Point;
                LevelManager.Instance.Exp += 10 * StatManager.instance.Persistent_stat.BonusExp;
                BrickManager.Instance.TakeBrick(gameObject.GetComponent<IBrick>()); ;
            }
        }
    }

    public void Start()
    {
        HPText = GetComponentInChildren<TextMeshPro>();
    }

    private void OnEnable()
    {
        HP = BasicHP * BrickManager.Instance.BrickStage;
    }

    public void TakeDamage(Ball ball)
    {
        if (IsCriticalHit())
        {
            HP -= FinalDamage() * CriticalDamage();
        }
        else
        {
            HP -= FinalDamage();
        }

        if (AugmentManager.instance.determineSpecialAugmentType("Effect"))
        {
            Debug.Log("TLqkf");
            StartCoroutine(DamageByEffect(StatManager.instance.Temporary_stat.EffectDuration));
        }
    }

    IEnumerator DamageByEffect(float EffectDuration)
    {
        while (EffectDuration > 0)
        {
            Debug.Log(EffectDuration + " seconds left! Hp is " + HP);
            EffectDuration -= 0.02f;
            HP -= StatManager.instance.Temporary_stat.EffectDamage;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private float FinalDamage()
    {
        return StatManager.instance.Persistent_stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage;
    }

    private bool IsCriticalHit()
    {
        return (Random.value < (StatManager.instance.Persistent_stat.CriticalChance != 0.0f
            ? StatManager.instance.Persistent_stat.CriticalChance
            : StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance));
    }

    private float CriticalDamage()
    {
        return (StatManager.instance.Persistent_stat.CriticalDamage + StatManager.instance.Temporary_stat.CriticalDamage);
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
}
