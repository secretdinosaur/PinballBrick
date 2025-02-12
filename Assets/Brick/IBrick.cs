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
    private bool isDied = false;
    public TextMeshPro HPText;
    public string BrickType;
    public Transform BrickTransform;
    private Canvas canvas;
    private Camera camera;
    public float HpTemp;
    public float BallSpeed;

    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;

    public GameObject PoisonEffect;
    public float TotalMultiple;

    public Dictionary<string, EffectStatus> EffectStatusSet;

    public Coroutine Fire = null;
    public Coroutine Poison = null;
    public Coroutine Decay = null;
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
                else if(!isDied)
                {
                    isDied = true;
                    BrickDie();
                }
            }
            UpdateHPtext();
        }
    }

    public void BrickDie()
    {
        Fever.StartFever = true;
        Fever.Instance.FeverCount *= 2;
        Fever.Instance.FeverGauge = 100;
        ScoreManager.Instance.ContinuousScore += Point;
        LevelManager.Instance.Exp += 20 * StatManager.instance.Persistent_stat.BonusExp;
        foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.AfterBrickDieEffect(this);
        }
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
        EffectStatusSet = new Dictionary<string, EffectStatus>()
        {
            { "Fire", AugmentManager.instance.FireEffectStatus },
            { "Poison", AugmentManager.instance.PoisonEffectStatus },
            { "Decay", AugmentManager.instance.DecayEffectStatus }
        };
        FireEffectDuration = 0f;
        PoisonEffectDuration = 0f;
        DecayEffectDuration = 0f;
        EffectStatusSet["Fire"].IsActive = false;
        EffectStatusSet["Poison"].IsActive = false;
        EffectStatusSet["Decay"].IsActive = false;
        EffectStatusSet["Fire"].EffectStack = 0;
        EffectStatusSet["Poison"].EffectStack = 0;
        EffectStatusSet["Fire"].EffectStack = 0;
        GetComponents();
    }

    public void GetComponents()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        BrickTransform = GetComponent<Transform>();
        camera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        PoisonEffect.SetActive(false);
        if(spriteRenderer != null)
            UpdateOutline();
    }
    public void UpdateOutline()
    {
        Vector2 size = spriteRenderer.bounds.size;
        Vector3[] corners = new Vector3[4];
        float outlineThickness = 0.15f;
        lineRenderer.startWidth = outlineThickness;
        lineRenderer.endWidth = outlineThickness;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;

        corners[0] = new Vector3(-size.x / 2 - 0.15f, -size.y / 2 - 0.3f, 0);
        corners[1] = new Vector3(size.x / 2 + 0.15f, -size.y / 2 - 0.3f, 0);
        corners[2] = new Vector3(size.x / 2 + 0.15f, size.y / 2 + 0.3f, 0);
        corners[3] = new Vector3(-size.x / 2 - 0.15f, size.y / 2 + 0.3f, 0);

        lineRenderer.positionCount = 5;
        lineRenderer.SetPositions(new Vector3[]
        {
            corners[0],
            corners[1],
            corners[2],
            corners[3],
            corners[0]
        });
        lineRenderer.enabled = false;
    }

    private void OnDisable()
    {
        if (Fire != null)
        {
            StopCoroutine(Fire);
            Fire = null;
        }
        if (Poison != null)
        {
            StopCoroutine(Poison);
            Poison = null;
        }
        if(Decay != null)
        {
            StopCoroutine(Decay);
            Decay = null;
        }
    }

}


public partial class IBrick
{
    public Ball RecentlyBall;
    private float SpecialCriticalChance; // CriticalChance that Increased by Augment
    
    public void TakeDamage(Ball ball)
    {
        RecentlyBall = ball;
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
                                StatManager.instance.Temporary_stat.AttackDamage +
                                RecentlyBall.GetGrowthAttackDamage();

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

    public float FireEffectDuration;
    public float PoisonEffectDuration;
    public float DecayEffectDuration;
    public IEnumerator DamageByFireEffect(float effectDuration)
    {
        Debug.Log("Activate");
        if (Fire != null) 
        {
            FireEffectDuration = effectDuration;
            yield break; 
        }
        EffectStatusSet["Fire"].IsActive = true;
        lineRenderer.enabled = true;
        FireEffectDuration = effectDuration;
        float interval = 0.2f;
        while (FireEffectDuration > 0)
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
            ShowText(FinalEffectAttackDamage.ToString("F1"), "Fire", isCritical);
            FireEffectDuration -= interval;
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
        Fire = null;
        lineRenderer.enabled = false;
        EffectStatusSet["Fire"].EffectStack = 0;
    }

    float initialDuration;
    public IEnumerator DamageByPoisonEffect(float effectDuration)
    {
        if (Poison != null)
        { 
            yield break;
        }
        EffectStatusSet["Poison"].IsActive = true;
        PoisonEffect.SetActive(true);

        Color PoisonColor = PoisonEffect.GetComponent<SpriteRenderer>().material.color;
        Renderer renderer = PoisonEffect.GetComponent<SpriteRenderer>();
        renderer.material.SetFloat("_AlphaSubtract", 0);
        
        float interval = 0.5f;
        initialDuration = effectDuration;
        PoisonEffectDuration = effectDuration;

        while (PoisonEffectDuration > 0)
        {
            //Deadly Poison Effect
            if (EffectStatusSet["Poison"].EffectStack > 3)
            {
                PoisonEffect.GetComponent<Renderer>().material.SetColor("_Color", new Color(13 / 255, 103 / 255, 0));
                PoisonEffect.GetComponent<Renderer>().material.SetColor("_InnerColor", new Color(130 / 255, 0, 7 / 255));
            }
            else
            {
                PoisonEffect.GetComponent<Renderer>().material.SetColor("_Color", new Color(45 / 255, 255 / 255, 62 / 255));
                PoisonEffect.GetComponent<Renderer>().material.SetColor("_InnerColor", new Color(255 / 255, 33 / 255, 0));
            }

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
            ShowText(FinalEffectAttackDamage.ToString("0.###"), "Poison", isCritical);
            if (AugmentManager.instance.determineAugmentID("SPECIAL_16"))
            {
                if (EffectStatusSet["Poison"].EffectStack == EffectStatusSet["Poison"].EffectStackLimit && NearbyBricks.Count > 0 && Random.value < 0.25f)
                {
                    List<IBrick> ChosenBricks = new List<IBrick>(this.NearbyBricks);
                    int infectionLimit = Random.Range(0, ChosenBricks.Count > 2 ? 2 : ChosenBricks.Count);
                    for (int i = 0; i < ChosenBricks.Count; i++)
                    {
                        int swapIndex = Random.Range(i, ChosenBricks.Count);
                        (ChosenBricks[i], ChosenBricks[swapIndex]) = (ChosenBricks[swapIndex], ChosenBricks[i]);
                    }

                    ChosenBricks = ChosenBricks.GetRange(0, infectionLimit);
                    foreach (IBrick brick in ChosenBricks)
                    {
                        if (brick.gameObject.activeInHierarchy && brick.Poison == null)
                        {
                            VisualEffectManager.Instance.SpawnParticle("InfinityPoison", transform.position, 1);
                            brick.EffectStatusSet["Poison"].EffectStack +=
                                (brick.EffectStatusSet["Poison"].EffectStack != EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
                            brick.Poison = brick.StartCoroutine(brick.DamageByPoisonEffect(initialDuration));
                        }
                    }
                }
                else
                {
                    if (Random.value < AugmentManager.instance.effectChance.PoisonChance - 0.1f)
                    {
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
            }
            else
            {
                PoisonEffectDuration -= interval;
            }
            float alpha = Mathf.Lerp(0.5f, 0f, (initialDuration - PoisonEffectDuration) / initialDuration);
            renderer.material.SetFloat("_AlphaSubtract", alpha);
            yield return null;

            yield return new WaitForSeconds(interval);
            if (!gameObject.activeInHierarchy)
            {
                yield break;
            }
        }
        EffectStatusSet["Poison"].IsActive = false;


        float startAlpha = renderer.material.color.a;
        float elapsedTime = 0f;
        float StartAlpha = renderer.material.GetFloat("_AlphaSubtract");
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1, elapsedTime / 1);
            renderer.material.SetFloat("_AlphaSubtract", alpha);
        }

        PoisonEffect.SetActive(false);
        EffectStatusSet["Poison"].EffectStack = 0;
        Poison = null;
    }
    public IEnumerator DecayEffect(float effectDuration)
    {
        if (Decay != null)
        {
            DecayEffectDuration = effectDuration;
            yield break;
        }
        EffectStatusSet["Decay"].IsActive = true;
        DecayEffectDuration = effectDuration;
        int PreviousEffectStack = EffectStatusSet["Decay"].EffectStack;
        float interval = 0.1f;
        while (effectDuration > 0)
        {
            DecayEffectDuration -= interval;
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            yield return new WaitForSeconds(interval);
        }
        Decay = null;
        EffectStatusSet["Decay"].EffectStack = 0;
    }

    public float DecayDamageCalculate(float Damage)
    {
        Damage *= Mathf.Pow(AugmentManager.instance.DecayDamageMultiple, EffectStatusSet["Decay"].EffectStack);
        return Damage;
    }
    
    public IEnumerator AdditionalAttack(float MaxAttackTime, float TimeCount) // AdditionalAttack Coroutine
    {
        yield return new WaitForSeconds(0.5f);
        
        while (MaxAttackTime > 0f)
        {
            if (!gameObject.activeSelf)
            {
                yield break;
            }
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

    void Update()
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