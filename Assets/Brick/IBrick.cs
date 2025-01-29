using UnityEngine;
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
    public string BrickType;
    public Transform BrickTransform;
    private Canvas canvas;
    private Camera camera;

    public float TotalMultiple;

    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;

            if (hp <= 0)
            {
                if (BrickType == "TwinBrick")
                {
                    hp = 0;
                    GetComponentInParent<TwinBrick>().CheckBothBrickDead();
                }
                else if (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed)
                {
                    GetComponent<ShieldBrick>().DestroyShield();
                }
                else if (BrickType == "BlackBrick")
                {
                    GetComponent<BlackBrick>().WhiteBrick.isBlackBrickDied();
                }
                else
                {
                    BrickDie();
                }
            }
            UpdateHPtext();
        }
    }

    public void BrickDie()
    {
        isEffectActive = false;
        Fever.StartFever = true;
        Fever.Instance.FeverCount *= 2;
        Fever.Instance.FeverGauge = 100;
        ScoreManager.Instance.ContinuousScore += Point;
        LevelManager.Instance.Exp += 20 * StatManager.instance.Persistent_stat.BonusExp;
        BrickManager.Instance.TakeBrick(gameObject.GetComponent<IBrick>());
    }

    private void ShowDamageText(float Damage, string DamageType, bool isCritical)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if(canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 damageTextPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera,
            out damageTextPosition
        );
     
        DamageTextManager.instance.CreateDamageText(damageTextPosition, Damage, DamageType, isCritical);
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
        GetComponents();
    }

    public void GetComponents()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        BrickTransform = GetComponent<Transform>();
        camera = Camera.main;
    }
}


public partial class IBrick
{
    public int EffectStack;
    public bool isEffectActive = false;
    public void TakeDamage(Ball ball)
    {
        TotalMultiple = StatManager.instance.statMultiplier.AttackDamageMultiplier;

        foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.BeforeAttackEffect(this);
        }

        HP -= FinalDamage();

        foreach(IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.AfterAttackEffect(this);
        }
        
        
    }
    private float FinalDamage()
    {
        bool CriticalHit = false;
        float TotalAttackStat = StatManager.instance.Persistent_stat.AttackDamage +
                                StatManager.instance.Temporary_stat.AttackDamage;

        if (IsCriticalHit())
        {
            CriticalHit = true;
            TotalMultiple *= CriticalDamage();

            foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
            {
                Augment.CriticalAttackEffect(this);
            }
        }

        float FinalDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) ? 1 : TotalAttackStat * TotalMultiple;
        if (BrickType == "TeleportBrick") BrickManager.Instance.TeleportBrickPosition(GetComponent<TeleportBrick>());
        ShowDamageText(FinalDamage, "Normal", CriticalHit);
        return FinalDamage;
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
    public IEnumerator DamageByEffect(float effectDuration)
    {
        if (isEffectActive) yield break;
        isEffectActive = true;

        float interval = 0.1f;
        while (effectDuration > 0 && EffectStack > 0)
        {
            float EffectAttackDamage = (StatManager.instance.Temporary_stat.EffectDamage *
                                        StatManager.instance.statMultiplier.EffectDamageMultiplier)
                                       * EffectStack;

            float FinalEffectAttackDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) ? 1 : EffectAttackDamage;
            HP -= FinalEffectAttackDamage;
            ShowDamageText(EffectAttackDamage, "Effect", false);
            EffectStack--;
            effectDuration -= interval;
            yield return new WaitForSeconds(interval);
        }
        isEffectActive = false;
    }
    
    public IEnumerator AdditionalAttack(float MaxAttackTime, float TimeCount) // AdditionalAttack Coroutine
    {
        yield return new WaitForSeconds(0.5f);
        
        while (MaxAttackTime > 0f)
        {
            bool CriticalHit = IsCriticalHit();
            float AdditionalAttackDamage = CriticalHit
                    ? (StatManager.instance.Persistent_stat.AttackDamage +
                       StatManager.instance.Temporary_stat.AttackDamage)
                      * StatManager.instance.statMultiplier.AttackDamageMultiplier * CriticalDamage()
                    : (StatManager.instance.Persistent_stat.AttackDamage +
                       StatManager.instance.Temporary_stat.AttackDamage)
                      * StatManager.instance.statMultiplier.AttackDamageMultiplier;

            float FinalAdditionalAttackDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) ? 1 : AdditionalAttackDamage;
            
            if(BrickType == "TeleportBrick") BrickManager.Instance.TeleportBrickPosition(GetComponent<TeleportBrick>());
            HP -= FinalAdditionalAttackDamage;
            ShowDamageText(FinalAdditionalAttackDamage, "Additional", CriticalHit);
            yield return new WaitForSeconds(TimeCount);
            MaxAttackTime -= TimeCount;
        }
    }
}

public partial class IBrick
{
    public bool isTouchedBlackHole = false;
    public GameObject TouchedObject;
    public void TouchedBlackHole(Ball ball)
    {
        isTouchedBlackHole = true;
        TouchedObject = ball.gameObject;
    }
}