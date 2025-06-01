using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;

public partial class IBrick : MonoBehaviour
{
    public int Point;
    public float Exp;
    public float BasicHP;
    private float hp;
    public bool IsBoss = false;
    public bool IsSpecialBrick = false;
    public TextMeshPro HPText;
    public string BrickType;
    public Transform BrickTransform;
    private Canvas canvas;
    private Camera camera;
    public float HpTemp;
    public float BallSpeed;

    public BrickDetect detectArea;

    public SpriteRenderer spriteRenderer;
    public LineRenderer lineRenderer;

    public GameObject PoisonEffect;
    public float TotalMultiple;

    public Coroutine Fire = null;
    public Coroutine Poison = null;
    public Coroutine Decay = null;
    public Coroutine Curse = null;
    public Coroutine ExecutionCor = null;
    public Coroutine BrickHealCoroutine = null;

    public GameObject ExecutionObject1;
    public GameObject ExecutionObject2;

    private Collider2D Collider;

    public bool isGoldBrick;
    public float HP
    {
        get { return hp; }
        set
        {

            hp = value;
            if (hp > HpTemp && HpTemp > 0) hp = HpTemp;

            if (AugmentManager.instance.determineAugmentID("SPECIAL_6") && (BrickType != "ShieldBrick" 
                || (BrickType == "ShieldBrick" && GetComponent<ShieldBrick>().IsShieldDestroyed)) && BrickType != "TwinBrick")
            {
                if (hp <= HpTemp * 0.15 && hp > 0 && HpTemp != 9999999)
                {
                    ExecutionCor = StartCoroutine(Execution());
                }
            }
            if ((int)hp <= 0)
            {
                hp = 0;
                HPText.enabled = false;
                if (BrickType == "ShieldBrick" && !GetComponent<ShieldBrick>().IsShieldDestroyed)
                {
                    GetComponent<ShieldBrick>().DestroyShield();
                }
                else if(BrickType == "TwinBrick" && !GetComponent<TwinBrick>().IsHpZero)
                {
                    StartCoroutine(GetComponent<TwinBrick>().StartRebirth());
                    HPText.text = "10";
                }
                else if(BrickType == "TwinBrick")
                {

                }
                else
                {
                    if (BrickType == "HealBrick") StopCoroutine(BrickHealCoroutine);
                    if (IsSpecialBrick)
                    {
                        if (BrickType == "ExpBrick")
                        {
                            LevelManager.Instance.GetExp(100f + GameManager.instance.Wave / 2);
                        }
                        else if(BrickType == "GoldBrick")
                        {
                            StatManager.instance.GetGold(100 + GameManager.instance.Wave / 2);
                        }
                        else if(BrickType == "BallBrick")
                        {
                            BallManager.Instance.CreateBall(2);
                        }
                    }
                    BrickDie();
                }
            }
            UpdateHPtext();
        }
    }

    public void BrickDie()
    {
        foreach(IBrick brick in detectArea.NearbyBricks)
        {
            if (brick.BrickType == "AbsorbBrick")
                brick.GetComponent<AbsorbBrick>().absorbHP(this);
        }
        if(BrickType == "TwinBrick") GetComponent<TwinBrick>().Twin = null;
        if(BrickType == "HealBrick") spriteRenderer.material.SetColor("_Color", GetComponent<HealBrick>().startColor);
        GameManager.instance.DestroyedBricks++;
        Fever.StartFever = true;
        Fever.Instance.CalFeverCount();
        Fever.Instance.FeverGauge = 100;
        ScoreManager.Instance.ContinuousScore += Point;
        LevelManager.Instance.GetExp(Exp);
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

        if ((int)hp <= 0 && BrickType == "TwinBrick")
        {
            return;
        }

        if (HPText != null)
        {
            HPText.text = ((int)hp).ToString();
            HPText.color = IsBoss ? Color.red : Color.white;

        }
        else
        {
            HPText = GetComponentInChildren<TextMeshPro>();
            HPText.text = ((int)hp).ToString();
            HPText.color = IsBoss ? Color.red : Color.white;

        }

        if ((int)hp >= 100000)
        {
            HPText.fontSize = 2.7f;
        }
        else if ((int)hp >= 10000)
        {
            HPText.fontSize = 3.2f;
        }
        else if ((int)hp >= 1000)
        {
            HPText.fontSize = 4f;
        }
        else
        {
            HPText.fontSize = 5f;
        }
    }
    public void Init(bool IsBoss, bool IsSpecialBrick, int Point, float Exp)
    {
        this.IsBoss = IsBoss;
        this.IsSpecialBrick = IsSpecialBrick;
        this.Point = Point;
        this.Exp = Exp;

        if (IsBoss)
            spriteRenderer.material.SetFloat("_IsBoss", 1);
        else
            spriteRenderer.material.SetFloat("_IsBoss", 0);

        HPText.color = IsBoss ? Color.red : Color.white;
    }
    public void Start()
    {
        HPText = GetComponentInChildren<TextMeshPro>();
        HPText.enabled = true;

    }
    public void OnEnable()
    {
        HpTemp = 9999999;
        //To sync Twin's HP
        if(BrickType == "TwinBrick")
            HP = BasicHP * BrickManager.Instance.HPPool[1];
        else
            HP = BasicHP * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)];
        HpTemp = HP;

        GetComponents();
        spriteRenderer.enabled = true;
        HPText.enabled = true;
        ExecutionObject1.transform.position = transform.position;
        ExecutionObject2.transform.position = transform.position;
        isGoldBrick = false;
        spriteRenderer.material.SetFloat("_Decay", 0);
        if (detectArea.NearbyBricks.Contains(this))
            detectArea.NearbyBricks.Remove(this);
    }

    public void GetComponents()
    {
        Collider = GetComponent<Collider2D>();
        detectArea = GetComponentInChildren<BrickDetect>();
        detectArea.NearbyBricks.Clear();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        BrickTransform = GetComponent<Transform>();
        camera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        PoisonEffect.SetActive(false);
        if (spriteRenderer != null)
            UpdateOutline();
        BrickEffect = new BrickEffectManager(this);
        spriteRenderer.enabled = true;
        Collider.enabled = true;
        ExecutionObject1.SetActive(false);
        ExecutionObject2.SetActive(false);
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
                DecayEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.DecayDuration);
            }
            else if (AugmentManager.instance.determineAugmentID("SPECIAL_56"))
            {
                DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.FireDuration);
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
        if(AugmentManager.instance.determineAugmentID("SPECIAL_75"))
            if(Curse == null)
            {
                Curse = StartCoroutine(BrickEffect.DamageByCurse2());
            }
        else
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

public partial class IBrick
{  
    private IEnumerator Execution()
    {
        ExecutionObject1.SetActive(true);
        ExecutionObject2.SetActive(true);

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

        ExecutionObject1.GetComponent<SpriteRenderer>().material.SetColor("_Color", spriteRenderer.material.color);
        ExecutionObject2.GetComponent<SpriteRenderer>().material.SetColor("_Color", spriteRenderer.material.color);

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