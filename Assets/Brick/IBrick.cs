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

    public SpriteRenderer spriteRenderer;
    public LineRenderer lineRenderer;

    public GameObject PoisonEffect;
    public float TotalMultiple;

    public Coroutine Fire = null;
    public Coroutine Poison = null;
    public Coroutine Decay = null;
    public Coroutine ExecutionCor = null;
    public Coroutine BrickHealCoroutine = null;

    public GameObject ExecutionObject1;
    public GameObject ExecutionObject2;

    private Collider2D Collider;

    public bool IsNeedDelayedDIeForEffect = false;

    public bool isGoldBrick;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp > HpTemp && HpTemp > 0) hp = HpTemp;

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
                HPText.enabled = false;
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
                    if (BrickType == "HealBrick") StopCoroutine(BrickHealCoroutine);
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
        //StopCoroutineAfterDie();
        BrickManager.Instance.TakeBrick(gameObject.GetComponent<IBrick>());
    }

    /*
    private void StopCoroutineAfterDie()
    {
        if(Fire != null) { BrickEffect.StopFire(); }
        if(Poison !=  null) { BrickEffect.StopPoison(); }
        if(Decay != null) { BrickEffect.StopDecay(); }
    }
    */

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
        HpTemp = 9999999;
        HP = BasicHP * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)];
        HpTemp = HP;
        
        GetComponents();
        spriteRenderer.enabled = true;
        HPText.enabled = true;
        ExecutionObject1.transform.position = transform.position;
        ExecutionObject2.transform.position = transform.position;
        isGoldBrick = false;
        spriteRenderer.material.SetFloat("_Decay", 0);
    }

    public void GetComponents()
    {
        Collider = GetComponent<Collider2D>();
        boxSize = new Vector2(2f, 2f);
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        BrickTransform = GetComponent<Transform>();
        camera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        PoisonEffect.SetActive(false);
        if (spriteRenderer != null)
            UpdateOutline();
        BrickEffect = new BrickEffectManager(this);
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
    public float SpecialCriticalChance; // CriticalChance that Increased by Augment
    public BrickEffectManager BrickEffect;
    public float RecentReceivedDamage;

    public void TakeDamage(IBall ball)
    {
        RecentlyBall = ball;
        float PreviousCritChance = RecentlyBall.SpecialCriticalChance;
        RecentlyBall.AttackDelayTime = 0f;
        if(RecentlyBall.AttackNumber % 3 == 0 && RecentlyBall.AttackNumber != 0)
        {
            RecentlyBall.SpecialCriticalChance = 1f;
        }
        DamageManager.TakeDamage(this, RecentlyBall);
        RecentlyBall.SpecialCriticalChance = PreviousCritChance;
    }

    public void DamageByFireEffect(float effectDuration)
    {
        BrickEffect.EffectStatusSet["Fire"].EffectStack +=
                (BrickEffect.EffectStatusSet["Fire"].EffectStack != BrickEffect.EffectStatusSet["Fire"].EffectStackLimit) ? 1 : 0;
        if (Fire == null)
        {
            Fire = StartCoroutine(BrickEffect.DamageByFireEffect(effectDuration));
        }
        else
        {
            BrickEffect.FireTimeUpdate(effectDuration);
        }
    }
    public void DamageByPoisonEffect(float effectDuration)
    {
        BrickEffect.EffectStatusSet["Poison"].EffectStack +=
                (BrickEffect.EffectStatusSet["Poison"].EffectStack != BrickEffect.EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
        if (Poison == null)
        {
            Poison = StartCoroutine(BrickEffect.DamageByPoisonEffect(effectDuration));
            if (AugmentManager.instance.determineAugmentID("SPECIAL_55"))
            {
                DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.DecayDuration);
            }
            else if (AugmentManager.instance.determineAugmentID("SPECIAL_56"))
            {
                DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.FireDuration);
            }
        }
        else
        {
            BrickEffect.PoisonTimeUpdate(effectDuration);
        }
    }
    public void DecayEffect(float effectDuration)
    {
        BrickEffect.EffectStatusSet["Decay"].EffectStack +=
                (BrickEffect.EffectStatusSet["Decay"].EffectStack != BrickEffect.EffectStatusSet["Decay"].EffectStackLimit) ? 1 : 0;
        if (Decay == null)
        {
            Decay = StartCoroutine(BrickEffect.DecayEffect(effectDuration));
        }
        else
        {
            BrickEffect.DecayTimeUpdate(effectDuration);
        }
    }
    
    public void DamageByLightningEffect(float DamageAmount, int ParticleCount)
    {
        DamageManager.DamageByLightningEffect(this, DamageAmount, ParticleCount);
    }

    public void DamageByCurse()
    {
        DamageManager.DamageByCurse(this);
    }

    public float DecayDamageCalculate(float Damage)
    {
        return DamageManager.DecayDamageCalculate(this, Damage);
    }
    
    public void AdditionalAttack(float MaxAttackTime, float TimeCount, float DamageMultiflier)
    {
        DamageManager.AdditionalAttack(this, MaxAttackTime, TimeCount, DamageMultiflier);
    }

    public void ArbitraryDamageAttack(float damage, bool isCritical, string damageText, string DamageType)
    {
        DamageManager.ArbitraryDamageAttack(this, damage, isCritical, damageText, DamageType);
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