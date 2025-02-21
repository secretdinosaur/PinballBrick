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

    private SpriteRenderer spriteRenderer;
    public LineRenderer lineRenderer;

    public GameObject PoisonEffect;
    public float TotalMultiple;

    public Dictionary<string, EffectStatus> EffectStatusSet;

    public Coroutine Fire = null;
    public Coroutine Poison = null;
    public Coroutine Decay = null;
    public Coroutine ExecutionCor = null;

    public GameObject ExecutionObject1;
    public GameObject ExecutionObject2;

    private Collider2D Collider;

    public bool IsNeedDelayedDIeForEffect = false;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (AugmentManager.instance.determineAugmentID("SPECIAL_6"))
            {
                if (hp <= HpTemp * 0.15 && hp > 0)
                {
                    ExecutionCor = StartCoroutine(Execution());
                }
            }
            if (hp <= 0)
            {
                hp = 0;
                if (BrickType == "TwinBrick")
                {
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
        Fever.StartFever = true;
        Fever.Instance.FeverCount *= 2;
        Fever.Instance.FeverGauge = 100;
        ScoreManager.Instance.ContinuousScore += Point;
        LevelManager.Instance.Exp += 20 * StatManager.instance.Persistent_stat.BonusExp;
        foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
        {
            Augment.AfterBrickDieEffect(this);
        }
        StopCoroutineAfterDie();
        BrickManager.Instance.TakeBrick(gameObject.GetComponent<IBrick>());
    }

    private void StopCoroutineAfterDie()
    {
        if(Fire != null) { StopCoroutine(Fire); }
        if(Poison !=  null) { StopCoroutine(Poison); }
        if(Decay != null) { StopCoroutine(Decay); }
    }

    public void ShowText(string Text, string DamageType, bool isCritical)
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
        
        GetComponents();
        spriteRenderer.enabled = true;
        HPText.enabled = true;
        ExecutionObject1.transform.position = transform.position;
        ExecutionObject2.transform.position = transform.position;
    }

    public void GetComponents()
    {
        Collider = GetComponent<Collider2D>();
        boxSize = new Vector2(2f, 2f);
        EffectStatusSet = new Dictionary<string, EffectStatus>()
        {
            { "Fire", new EffectStatus("Fire", false, 0, AugmentManager.instance.FireStackLimit) },
            { "Poison", new EffectStatus("Poison", false, 0, AugmentManager.instance.PoisonStackLimit) },
            { "Decay", new EffectStatus("Decay", false, 0, AugmentManager.instance.DecayStackLimit) }
        };
        FireEffectDuration = 0f;
        PoisonEffectDuration = 0f;
        DecayEffectDuration = 0f;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        BrickTransform = GetComponent<Transform>();
        camera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        PoisonEffect.SetActive(false);
        if (spriteRenderer != null)
            UpdateOutline();
    }

    public void UpdateOutline()
    {
        Vector2 size = spriteRenderer.bounds.size;
        Vector3[] corners = new Vector3[5];
        float outlineThickness = 0.15f;
        lineRenderer.startWidth = outlineThickness;
        lineRenderer.endWidth = outlineThickness;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;

        corners[0] = new Vector3(-size.x / 2 - 0.125f, -size.y / 2 - 0.3f, 0);
        corners[1] = new Vector3(size.x / 2 + 0.15f, -size.y / 2 - 0.3f, 0);
        corners[2] = new Vector3(size.x / 2 + 0.15f, size.y / 2 + 0.3f, 0);
        corners[3] = new Vector3(-size.x / 2 - 0.15f, size.y / 2 + 0.3f, 0);
        corners[4] = new Vector3(-size.x / 2 - 0.15f, -size.y / 2 - 0.3f, 0);
        lineRenderer.positionCount = 5;
        lineRenderer.SetPositions(new Vector3[]
        {
            corners[0],
            corners[1],
            corners[2],
            corners[3],
            corners[4]
        });
        lineRenderer.enabled = false;
    }

    private void OnDisable()
    {
        
    }

}


public partial class IBrick
{
    public IBall RecentlyBall;
    private float SpecialCriticalChance; // CriticalChance that Increased by Augment
    
    public void TakeDamage(IBall ball)
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
                                RecentlyBall.GrowthAttackDamage +
                                RecentlyBall.BallBaseDamage;

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
        if (EffectStatusSet["Fire"].EffectStack == EffectStatusSet["Fire"].EffectStackLimit)
        {
            lineRenderer.material.SetColor("_OuterFireColor", new Color(255 / 255f, 8 / 255f, 0) * 10f);
            lineRenderer.material.SetFloat("_FireHeight", 1f);
        }
        else
        {
            lineRenderer.material.SetColor("_OuterFireColor", new Color(191 / 255f, 8 / 255f, 0) * 5f);
            lineRenderer.material.SetFloat("_FireHeight", 2f);
        }
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
            if (EffectStatusSet["Fire"].EffectStack == EffectStatusSet["Fire"].EffectStackLimit)
            {
                lineRenderer.material.SetColor("_OuterFireColor", new Color(255 / 255f, 8 / 255f, 0) * 10f);
                lineRenderer.material.SetFloat("_FireHeight", 1f);
            }
            else
            {
                lineRenderer.material.SetColor("_OuterFireColor", new Color(191 / 255f, 8 / 255f, 0) * 5f);
                lineRenderer.material.SetFloat("_FireHeight", 2f);
            }
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

    public float initialDuration;
    public IEnumerator DamageByPoisonEffect(float effectDuration)
    {
        if (Poison != null)
        {
            yield break;
        }
        Material PoisonMaterial = PoisonEffect.GetComponent<Renderer>().material;
        if (EffectStatusSet["Poison"].EffectStack == EffectStatusSet["Poison"].EffectStackLimit)
        {
            PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
            PoisonMaterial.SetColor("_InnerColor", new Color(214 / 255f, 0 / 255f, 1));
            PoisonMaterial.SetFloat("_InnerPower", 1.75f);
        }
        else
        {
            PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
            PoisonMaterial.SetColor("_InnerColor", new Color(176 / 255f, 180 / 255f, 0));
            PoisonMaterial.SetFloat("_InnerPower", 1.5f);
        }
        float elapsedTime = 0f;

        PoisonEffect.SetActive(true);
        EffectStatusSet["Poison"].IsActive = true;

        while (elapsedTime < 1)
        {
            PoisonMaterial.SetFloat("_Alpha", Mathf.Lerp(1.1f, 0, elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float interval = 0.5f;
        initialDuration = effectDuration;
        PoisonEffectDuration = effectDuration;

        while (PoisonEffectDuration > 0)
        {
            if (EffectStatusSet["Poison"].EffectStack >= EffectStatusSet["Poison"].EffectStackLimit && !AugmentManager.instance.PoisonBrickSet.Contains(this))
            {
                AugmentManager.instance.PoisonBrickSet.Add(this);
            }
            //Deadly Poison Effect
            if (EffectStatusSet["Poison"].EffectStack == EffectStatusSet["Poison"].EffectStackLimit)
            {
                PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
                PoisonMaterial.SetColor("_InnerColor", new Color(214 / 255f, 0 / 255f, 1));
                PoisonMaterial.SetFloat("_InnerPower", 1.75f);
            }
            else
            {
                PoisonMaterial.SetColor("_Color", new Color(103 / 255f, 180 / 255f, 0) * 1.5f);
                PoisonMaterial.SetColor("_InnerColor", new Color(176 / 255f, 180 / 255f, 0));
                PoisonMaterial.SetFloat("_InnerPower", 1.5f);
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
            if (!gameObject.activeInHierarchy)
            {
                yield break;
            }
            if (AugmentManager.instance.determineAugmentID("SPECIAL_16"))
            {
                if (Random.value < AugmentManager.instance.effectChance.PoisonChance)
                {
                    elapsedTime = 0;
                    while (elapsedTime < 0.25f)
                    {
                        PoisonMaterial.SetFloat("_InfinityPoison", Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    elapsedTime = 0;
                    while (elapsedTime < 0.25f)
                    {
                        PoisonMaterial.SetFloat("_InfinityPoison", Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
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
            else
            {
                PoisonEffectDuration -= interval;
            }

            yield return new WaitForSeconds(1f);
        }

        elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            PoisonMaterial.SetFloat("_Alpha", Mathf.Lerp(0, 1.1f, elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Poison = null;
        EffectStatusSet["Poison"].IsActive = false;
        PoisonEffect.SetActive(false);
        Poison = null;
        EffectStatusSet["Poison"].EffectStack = 0;
        if (AugmentManager.instance.PoisonBrickSet.Contains(this))
        {
            AugmentManager.instance.PoisonBrickSet.Remove(this);
        }
    }
    public IEnumerator DecayEffect(float effectDuration)
    {
        if (Decay != null)
        {
            DecayEffectDuration = effectDuration;
            yield break;
        }
        VisualEffectManager.Instance.SetDecayVisualEffect(this);
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
        VisualEffectManager.Instance.UnSetDecayVisualEffect(this);
    }
    
    public void DamageByLightningEffect(float DamageAmount, int ParticleCount)
    {
        bool isCritical = true;
        if (AugmentManager.instance.determineAugmentID("SPECIAL_7"))
        {
            isCritical = IsCriticalHit();
        }
        float Percent = isCritical ? Random.Range(0.01f, 0.02f) : Random.Range(0.015f, 0.03f);
        float plusDamage = HpTemp * Percent;
        
        float FinalDamage = DecayDamageCalculate(plusDamage + DamageAmount);
        HP -= FinalDamage;
        
        VisualEffectManager.Instance.SpawnParticle("Lightning", this.transform.position, isCritical ? ParticleCount + 4 : ParticleCount);
        ShowText(FinalDamage.ToString("F1"), "Lightning", isCritical);
    }

    public void DamageByCurse()
    {
        float baseDamage = (StatManager.instance.Persistent_stat.AttackDamage
            + StatManager.instance.Temporary_stat.AttackDamage
            + RecentlyBall.GrowthAttackDamage)
            * StatManager.instance.statMultiplier.AttackDamageMultiplier;

        float damage = Random.Range(baseDamage * 0.5f, baseDamage * 1.3f);
        float FinalDamage = DecayDamageCalculate(damage);
        HP -= FinalDamage;
        ShowText(FinalDamage.ToString("F1"), "Curse", false);
    }

    public float DecayDamageCalculate(float Damage)
    {
        Damage *= Mathf.Pow(AugmentManager.instance.DecayDamageMultiple, EffectStatusSet["Decay"].EffectStack);
        return Damage;
    }
    
    public IEnumerator AdditionalAttack(float MaxAttackTime, float TimeCount, float DamageMultiflier) // AdditionalAttack Coroutine
    {
        yield return new WaitForSeconds(0.5f);
        
        while (MaxAttackTime > 0f)
        {
            if (!gameObject.activeSelf)
            {
                yield break;
            }
            VisualEffectManager.Instance.SpawnParticle("AdditionalAttack", transform.position, 1);
            bool CriticalHit = IsCriticalHit();
            float AdditionalAttackDamage = CriticalHit
                    ? (StatManager.instance.Persistent_stat.AttackDamage +
                       StatManager.instance.Temporary_stat.AttackDamage)
                      * StatManager.instance.statMultiplier.AttackDamageMultiplier * CriticalDamage() * DamageMultiflier
                    : (StatManager.instance.Persistent_stat.AttackDamage +
                       StatManager.instance.Temporary_stat.AttackDamage)
                      * StatManager.instance.statMultiplier.AttackDamageMultiplier * DamageMultiflier;

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

    private IEnumerator Execution()
    {
        spriteRenderer.enabled = false;
        HPText.enabled = false;
        Collider.enabled = false;
        float ElapsedTime = 0f;

        float RandomRadian = Random.Range(0f, Mathf.PI);

        float Execute1DirRad;
        float Execute2DirRad;

        Vector3 Execute1Dir;
        Vector3 Execute2Dir;

        ExecutionObject1.GetComponent<SpriteRenderer>().material.SetFloat("_Rotation", RandomRadian);
        ExecutionObject2.GetComponent<SpriteRenderer>().material.SetFloat("_Rotation", RandomRadian);

        float Speed = 0.006f;
        if(Random.value < 0.5f)
        {
            Execute1DirRad = RandomRadian + Mathf.PI / 4 + Mathf.PI / 2;
            Execute2DirRad = RandomRadian + Mathf.PI / 4 - Mathf.PI / 2;
        }
        else
        {
            Execute1DirRad = RandomRadian + Mathf.PI / 4 - Mathf.PI / 2;
            Execute2DirRad = RandomRadian + Mathf.PI / 4 + Mathf.PI / 2;
        }
        Execute1Dir = new Vector3(Mathf.Cos(Execute1DirRad), Mathf.Sin(Execute1DirRad), 0);
        Execute2Dir = new Vector3(Mathf.Cos(Execute2DirRad), Mathf.Sin(Execute2DirRad), 0);
        while (ElapsedTime < 0.1f)
        {
            ExecutionObject1.transform.position += Execute1Dir * Speed;
            ExecutionObject2.transform.position += Execute2Dir * Speed;
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        hp -= 999999 + HpTemp;
        ShowText("EXECUTION", null, true);
        Collider.enabled = true;
        BrickDie();
    }
}

public partial class IBrick //BlackHole Brick
{
    public bool isTouchedBlackHole = false;
    public GameObject TouchedObject;
    public void TouchedBlackHole(IBall ball)
    {
        isTouchedBlackHole = true;
        TouchedObject = ball.gameObject;
    }
}