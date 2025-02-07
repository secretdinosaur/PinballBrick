using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float HpTemp;
    public float BallSpeed;

    public float TotalMultiple;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (AugmentManager.instance.determineAugmentID("SPECIAL_6"))
            {
                if (hp <= HpTemp * 0.15)
                { 
                    hp -= 999999 + HpTemp;
                    ShowText("EXECUTION", null , true);
                }
            }
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
        if (AugmentManager.instance.determineAugmentID("SPECIAL_20"))
        {
            if (EffectStatusSet["Decay"].EffectStack == EffectStatusSet["Decay"].EffectStackLimit)
            {
                DecayExplosion();
            }
        }
        Fever.StartFever = true;
        Fever.Instance.FeverCount *= 2;
        Fever.Instance.FeverGauge = 100;
        ScoreManager.Instance.ContinuousScore += Point;
        LevelManager.Instance.Exp += 20 * StatManager.instance.Persistent_stat.BonusExp;
        BrickManager.Instance.TakeBrick(gameObject.GetComponent<IBrick>());
    }

    private void ShowText(string Text, string DamageType, bool isCritical)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if(canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 TextPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera,
            out TextPosition
        );
     
        DamageTextManager.instance.CreateDamageText(TextPosition, Text, DamageType, isCritical);
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
        HpTemp = HP;
        boxSize = new Vector2(2f, 2f);
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
    public Ball Ball;
    private float SpecialCriticalChance; // CriticalChance that Increased by Augment
    
    public Dictionary<string, EffectStatus> EffectStatusSet = new Dictionary<string, EffectStatus>()
    {
        { "Fire", new EffectStatus("Fire", false, 0, 5) },
        { "Poison", new EffectStatus("Poison", false, 0, 3) },
        { "Decay", new EffectStatus("Decay", false, 0, 3)}
    };
    public void TakeDamage(Ball ball)
    {
        Ball = ball;
        TotalMultiple = StatManager.instance.statMultiplier.AttackDamageMultiplier;
        SpecialCriticalChance = ball.SpecialCriticalChance;

        foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.BeforeAttackEffect(this);
        }

        HP -= FinalDamage();

        foreach(IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.AfterMainAttackEffect(this);
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

        float FinalDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) 
            ? 1 
            : DecayDamageCalculate(TotalAttackStat * TotalMultiple);
        if (BrickType == "TeleportBrick") BrickManager.Instance.TeleportBrickPosition(GetComponent<TeleportBrick>());
        ShowText(FinalDamage.ToString("F1"), "Normal", CriticalHit);
        return FinalDamage;
    }
    private bool IsCriticalHit()
    {
        return (Random.value < (StatManager.instance.Persistent_stat.CriticalChance != 0.0f
            ? StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance + SpecialCriticalChance
            : StatManager.instance.Temporary_stat.CriticalChance + SpecialCriticalChance));
    }
    private float CriticalDamage()
    {
        return (StatManager.instance.Persistent_stat.CriticalDamage + StatManager.instance.Temporary_stat.CriticalDamage);
    }
    public IEnumerator DamageByFireEffect(float effectDuration)
    {
        if (EffectStatusSet["Fire"].IsActive) yield break;
        EffectStatusSet["Fire"].IsActive = true;
        float interval = 0.1f;
        while (effectDuration > 0)
        {
            bool isCritical = false;
            float EffectAttackDamage;
            if (AugmentManager.instance.determineAugmentID("SPECIAL_7"))
            {
                isCritical = IsCriticalHit();
            }
            
            EffectAttackDamage = isCritical
                ? (StatManager.instance.Temporary_stat.EffectDamage *
                   StatManager.instance.statMultiplier.EffectDamageMultiplier)
                  * EffectStatusSet["Fire"].EffectStack * StatManager.instance.Temporary_stat.EffectCriticalDamage
                : (StatManager.instance.Temporary_stat.EffectDamage *
                   StatManager.instance.statMultiplier.EffectDamageMultiplier)
                  * EffectStatusSet["Fire"].EffectStack;

            float FinalEffectAttackDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) 
                ? 1 
                : DecayDamageCalculate(EffectAttackDamage);
            HP -= FinalEffectAttackDamage;
            ShowText(EffectAttackDamage.ToString("F1"), "Fire", isCritical);
            effectDuration -= interval;
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
        EffectStatusSet["Fire"].EffectStack = 0;
        EffectStatusSet["Fire"].IsActive = false;
    }

    public IEnumerator DamageByPoisonEffect(float effectDuration)
    {
        if (EffectStatusSet["Poison"].IsActive) yield break;
        EffectStatusSet["Poison"].IsActive = true;
        float effectDurationTemp = effectDuration;
        effectDuration = 0.8f * (effectDuration * 0.5f);
        float interval = 0.7f;
        while (effectDuration > 0)
        {
            bool isCritical = false;
            float EffectAttackDamage;
            if (AugmentManager.instance.determineAugmentID("SPECIAL_7"))
            {
                isCritical = IsCriticalHit();
            }

            EffectAttackDamage = isCritical
                ? (HP * (0.01f * StatManager.instance.statMultiplier.EffectDamageMultiplier * 0.8f
                         * EffectStatusSet["Poison"].EffectStack * StatManager.instance.Temporary_stat.EffectCriticalDamage))
                : (HP * (0.01f * StatManager.instance.statMultiplier.EffectDamageMultiplier * 0.8f
                         * EffectStatusSet["Poison"].EffectStack));

            float FinalEffectAttackDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) 
                ? 1 
                : DecayDamageCalculate(EffectAttackDamage);
            HP -= FinalEffectAttackDamage;
            ShowText(((FinalEffectAttackDamage / (HP + FinalEffectAttackDamage)) * 100f).ToString("0.###")+ "%", "Poison", isCritical);
            if (AugmentManager.instance.determineAugmentID("SPECIAL_16")) // Infinite Poison
            {
                if (EffectStatusSet["Poison"].EffectStack == EffectStatusSet["Poison"].EffectStackLimit)
                {
                    foreach (IBrick brick in NearbyBricks)
                    {
                        brick.EffectStatusSet["Poison"].EffectStack += 
                            (brick.EffectStatusSet["Poison"].EffectStack != EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
                        if (!brick.EffectStatusSet["Poison"].IsActive)
                        {
                            brick.StartCoroutine(brick.DamageByPoisonEffect(effectDurationTemp));
                        }
                    }
                }
                else
                {
                    if (Random.value < AugmentManager.instance.effectChance.PoisonChance)
                    {
                        EffectStatusSet["Poison"].EffectStack++;
                        effectDuration = effectDurationTemp;
                    }
                    else
                    {
                        effectDuration -= interval;
                    }
                }
            }
            else
            {
                effectDuration -= interval;
            }
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
        EffectStatusSet["Poison"].EffectStack = 0;
        EffectStatusSet["Poison"].IsActive = false;
    }
    public IEnumerator DecayEffect(float effectDuration)
    {
        if (EffectStatusSet["Decay"].IsActive) yield break;
        EffectStatusSet["Decay"].IsActive = true;
        float interval = 0.1f;
        while (effectDuration > 0)
        {
            effectDuration -= interval;
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
        EffectStatusSet["Decay"].EffectStack = 0;
        EffectStatusSet["Decay"].IsActive = false;
    }

    public float DecayDamageMultiple = 1.1f;
    public float DecayDamageCalculate(float Damage)
    {
        Damage *= Mathf.Pow(DecayDamageMultiple, EffectStatusSet["Decay"].EffectStack);
        return Damage;
    }
    
    public void DecayExplosion()
    {
        float explosionRadius = 3f;
        LayerMask explosionLayer = LayerMask.GetMask("Ball");
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayer);
        
        foreach (Collider2D ball in colliders)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 explosionDirection = (rb.position - (Vector2)transform.position).normalized;
                float distance = Vector2.Distance(rb.position, transform.position);
                float forceMultiplier = Mathf.Clamp01(1 - (distance / explosionRadius));
                
                rb.AddForce(explosionDirection * 5f * forceMultiplier, ForceMode2D.Impulse);
            }
        }
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

            float FinalAdditionalAttackDamage = (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed) 
                ? 1 
                : DecayDamageCalculate(AdditionalAttackDamage);
            
            if(BrickType == "TeleportBrick") BrickManager.Instance.TeleportBrickPosition(GetComponent<TeleportBrick>());
            HP -= FinalAdditionalAttackDamage;
            ShowText(FinalAdditionalAttackDamage.ToString("F1"), "Additional", CriticalHit);
            foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
            {
                Augment.AfterAttackEffect(this);
            }
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(TimeCount);
            MaxAttackTime -= TimeCount;
        }
    }

    public void ArbitraryDamageAttack(float damage, bool isCritical, string damageText, string DamageType)
    {
        damage = DecayDamageCalculate(damage);
        HP -= damage;
        ShowText(damageText, DamageType, isCritical);
    }
}

public partial class IBrick //Detect Nearby Bricks
{
    public List<IBrick> NearbyBricks = new List<IBrick>();
    public Vector2 boxSize;

    void FixedUpdate()
    {
        CollectNearbyBricks();
        RemoveInactiveBricks();
    }

    void CollectNearbyBricks()
    {
        LayerMask detectionLayer = LayerMask.GetMask("Game");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0, detectionLayer);
        foreach (Collider2D collider in colliders)
        {
            IBrick obj = collider.gameObject.GetComponent<IBrick>();
            if (obj != null && collider.gameObject.activeInHierarchy && !NearbyBricks.Contains(obj) && obj != this)
            {
                NearbyBricks.Add(obj);
            }
        }
    }

    void RemoveInactiveBricks()
    {
        NearbyBricks.RemoveAll(brick => brick == null || !brick.gameObject.activeInHierarchy);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}

public partial class IBrick //BlackHole Brick
{
    public bool isTouchedBlackHole = false;
    public GameObject TouchedObject;
    public void TouchedBlackHole(Ball ball)
    {
        isTouchedBlackHole = true;
        TouchedObject = ball.gameObject;
    }
}