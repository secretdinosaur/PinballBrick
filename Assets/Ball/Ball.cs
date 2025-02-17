using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Random = UnityEngine.Random;
using TMPro;
using TrailRenderer = UnityEngine.TrailRenderer;

public partial class Ball : MonoBehaviour
{
    public float CurrentSpeed;
    public float SpecialCriticalChance = 0f;
    public bool isPhoenix;
    public Flamethrower flamethrower;
    public bool isOverDeathLine;
    public Queue<Vector2> PositionSet = new Queue<Vector2>();
    public Vector2 StartPosition = new Vector2(0f, -5f);
    public float linearVelocityYValue = 10;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = StartPosition;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityY = linearVelocityYValue;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = Random.Range(-5f, 5f);
        linearVelocityYValue = 10;
        StartCoroutine(EnableCollider());
        spriteRenderer.color = new Color(1f, 1f, 1f);
        TrailRenderer = gameObject.GetComponent<TrailRenderer>();
        BounceTime = 1f;
        MaxBounce = 0;
        targets.Clear();
        currentTarget = null;
        isOverDeathLine = false;
        isPhoenix = false;
        flamethrower = GetComponentInChildren<Flamethrower>(true);
        if (BallManager.Instance.isOnFlamerower)
        {
            flamethrower.FlamethrowerOn();
        }
        if (BallManager.Instance.isAntiGravity)
        {
            rb.gravityScale = -1f;
        }
    }

    public void BallDied() 
    {
        if (isPhoenix)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {
            foreach (IAugment Augment in AugmentManager.instance.activeSpecialAugments)
            {
                Augment.AfterDeathEffect(this);
            }
            GrowthAttackDamage = 0f;
            BallManager.Instance.TakeBall(this);
        }
    }

    private IEnumerator EnableCollider()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.Find("LeftPin").GetComponent<Collider2D>(), true);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.Find("RightPin").GetComponent<Collider2D>(), true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.Find("LeftPin").GetComponent<Collider2D>(), false);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.Find("RightPin").GetComponent<Collider2D>(), false);
    }

    
    void Update()
    {
        DetectTargets();
        UpdateCurrentTarget();

        GrowthAttackDamageObserve = GrowthAttackDamage;
        TrailRenderer.startColor = spriteRenderer.color;
        CurrentSpeed = rb.linearVelocity.magnitude;
        if (isPhoenix)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, 0f);
        }
        
        
        if (CurrentSpeed > 8 && (AugmentManager.instance.determineAugmentID("SPECIAL_12") || AugmentManager.instance.determineAugmentID("SPECIAL_13"))) //Speed Augment
        {
            if (AugmentManager.instance.determineAugmentID("SPECIAL_13")) //Speed = Critical
            {
                SpecialCriticalChance = ((CurrentSpeed / 100f) * 1.5f);
            }
            else if(AugmentManager.instance.determineAugmentID("SPECIAL_12")) // Speed = Power
            {
                TransformationMultiple = 1f + ((CurrentSpeed / 100f) * 2.5f);
            }
            TrailRenderer.time = 1;
        }
        else
        {
            if (AugmentManager.instance.determineAugmentID("SPECIAL_13")) //Speed = Critical
            {
                SpecialCriticalChance = 0f;
            }
            else if(AugmentManager.instance.determineAugmentID("SPECIAL_12")) // Speed = Power
            {
                TransformationMultiple = 1f;
            }
            TrailRenderer.time -= TrailRenderer.time != 0 ? 0.005f : 0f;
        }
        if(transform.position.y <= -10 && !isOverDeathLine)
        {
            BallDied();
            isOverDeathLine = true;
        }

        if (rb.linearVelocity.magnitude > BallManager.Instance.maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * BallManager.Instance.maxSpeed;
        }

        if (AugmentManager.instance.determineAugmentID("SPECIAL_2")) //Homing
        {
            if (currentTarget != null)
            {
                Vector2 directionToTarget = (currentTarget.position - transform.position).normalized;
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
                rb.AddForce(directionToTarget * homingThrust);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_3")) //Bounce = Power
        {
            if (collision.gameObject.CompareTag("Wall") || (AugmentManager.instance.determineAugmentID("SPECIAL_21") && collision.gameObject.CompareTag("Ball")))
            {
                if (MaxBounce < 6)
                {
                    BounceTime += 0.05f;
                    MaxBounce++;
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g - 0.2f, spriteRenderer.color.b - 0.2f);
                }
            }
            else if (collision.gameObject.CompareTag("Brick"))
            {
                MaxBounce = 0;
                spriteRenderer.color = new Color(1f, 1f, 1f);
            }
        }
        if (AugmentManager.instance.determineAugmentID("SPECIAL_21") && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D TartgetRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 TargetDirection = (TartgetRigidBody.position - (Vector2)transform.position).normalized;
            rb.AddForce(RotateVector(TargetDirection, Random.Range(-30f, 30f)) * Random.Range(5f, 11f), ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("Brick") && gameObject.activeSelf)
        {
            collision.gameObject.GetComponent<IBrick>().BallSpeed = CurrentSpeed;
        }
    }

    Coroutine BallStuckCoroutine = null;
    bool IsStucking = false;
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Brick") && IsStucking && gameObject.activeSelf)
        {
            rb.AddForce(rb.linearVelocity.normalized / 2, ForceMode2D.Impulse);
            if(BallStuckCoroutine != null)
                StopCoroutine(BallStuckCoroutine);
            IsStucking = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Brick") && !IsStucking && gameObject.activeSelf)
        {
            BallStuckCoroutine = StartCoroutine(BallStuckDamage(collision.gameObject.GetComponent<IBrick>()));
        }
    }

    private IEnumerator BallStuckDamage(IBrick brick)
    {
        IsStucking = true;
        yield return new WaitForSeconds(5);
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            brick.TakeDamage(this);
        }
    }

    Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
}

public partial class Ball //Homing
{
    private float homingThrust = 80f;
    private float detectionRadius = 1.4f;
    private string targetLayerName = "Game";
    private float rotationSpeed = 20f;

    private Rigidbody2D rb;
    private LayerMask targetLayer = new LayerMask();
    [SerializeField]
    private List<Transform> targets = new List<Transform>();
    private Transform currentTarget = new RectTransform();
    private float lastDetectionTime;

    public List<IBrick> NearbyBricks = new List<IBrick>();
    
    private void DetectTargets()
    {
        if (Time.time - lastDetectionTime < 1f) return;
        lastDetectionTime = Time.time;

        LayerMask targetLayer = LayerMask.GetMask(targetLayerName);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);

        targets.Clear();
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject.activeInHierarchy && collider.transform != this.transform)
            {
                targets.Add(collider.transform);
                NearbyBricks.Add(collider.GetComponent<IBrick>());
            }
        }
    }

    private void UpdateCurrentTarget()
    {
        targets.RemoveAll(target => target == null 
                                    || !target.gameObject.activeInHierarchy 
                                    || Vector2.Distance(transform.position, target.position) > detectionRadius);

        if (targets.Count > 0)
        {
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (Transform target in targets)
            {
                float distance = Vector2.Distance(transform.position, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            currentTarget = closestTarget;
        }
        else
        {
            currentTarget = null;
        }
    }
    
    private void OnDrawGizmos()
    {
        // 검은색으로 범위 표시 (원)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

public partial class Ball //Bounce = Power
{
    public float BounceTime { get; set; } = 1f;
    private int MaxBounce = 0;
    private SpriteRenderer spriteRenderer;
}

public partial class Ball //Speed = Power
{
    private TrailRenderer TrailRenderer;
    public float TransformationMultiple = 1f;
}

public partial class Ball //GrowBall
{
    [SerializeField]
    public float GrowthAttackDamage { get; set; } = 0;
    public float GrowthAttackDamageObserve;
    public void Grow(IBrick brick)
    {
        if (brick.IsBoss)
        {
            GrowthAttackDamage += 0.2f;
        }
        else
        {
            GrowthAttackDamage += 0.04f;
        }
    }
}