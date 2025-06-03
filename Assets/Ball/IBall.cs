using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using Random = UnityEngine.Random;
using TMPro;
using TrailRenderer = UnityEngine.TrailRenderer;
using Unity.VisualScripting;

public partial class IBall : MonoBehaviour
{
    public string BallType;
    public float BallBaseDamage;
    public BallDetect detectArea;

    public float AttackDelayTime;
    public Coroutine AttackOrDieRoutine;

    public float CurrentSpeed;
    public float SpecialCriticalChance = 0f;
    public bool isPhoenix;
    public Flamethrower flamethrower;
    
    public bool isOverDeathLine;

    public Queue<Vector2> PositionSet = new Queue<Vector2>();
    public Vector3 StartPosition = new Vector3(0f, -5f, 35);
    public float linearVelocityYValue = 10;

    public Coroutine FusionMoveRoutine = null;

    public Renderer Renderer;
    public Color BrickColor;

    public int AttackNumber;

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    public void OnEnable()
    {
        foreach (IAugment Augment in AugmentManager.instance.activeAugments)
        {
            Augment.AfterBirthEffect(this);
        }
        gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = BallManager.StartPosition;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityY = linearVelocityYValue;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = Random.Range(-5f, 5f);
        spriteRenderer.color = new Color(1f, 1f, 1f);
        BounceTime = 1f;
        MaxBounce = 0;
        detectArea = GetComponentInChildren<BallDetect>();
        detectArea.NearbyBricks.Clear();
        Renderer.material.SetFloat("_Edge", -1);
        Renderer.material.SetFloat("_Alpha", -1);
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
        AttackDelayTime = 0f;
        StartCoroutine(CalAttackDelayTime());
        AttackNumber = 0;
        StartCoroutine(EnableCollider());
        BrickColor = spriteRenderer.color;

        if(!BallManager.Instance.InGameBall.Contains(this) && BallManager.Instance.BallList.Contains(BallType))
        {
            BallManager.Instance.InGameBall.Add(this);
            BallManager.Instance.inGameBallPosition.Add(gameObject.GetComponent<Transform>());
            BallManager.Instance.inGameBallCollider.Add(gameObject.GetComponent<Collider2D>());
        }
        else if (!BallManager.Instance.InGameSpecialBall.Contains(this) && BallManager.Instance.SpecialBallList.Contains(BallType))
        {
            BallManager.Instance.InGameSpecialBall.Add(this);
            BallManager.Instance.inGameSpecialBallPosition.Add(gameObject.GetComponent<Transform>());
            BallManager.Instance.inGameSpecialBallCollider.Add(gameObject.GetComponent<Collider2D>());
        }
    }

    public void BallDied()
    {
        if (BallManager.Instance.InGameBall.Contains(this))
        {
            BallManager.Instance.InGameBall.Remove(this);
            BallManager.Instance.inGameBallPosition.Remove(gameObject.GetComponent<Transform>());
            BallManager.Instance.inGameBallCollider.Remove(gameObject.GetComponent<Collider2D>());
        }
        else if (BallManager.Instance.InGameSpecialBall.Contains(this))
        {
            BallManager.Instance.InGameSpecialBall.Remove(this);
            BallManager.Instance.inGameSpecialBallPosition.Remove(gameObject.GetComponent<Transform>());
            BallManager.Instance.inGameSpecialBallCollider.Remove(gameObject.GetComponent<Collider2D>());
        }

        if (BallType != "GhostBall")
        {
            Renderer.material.color = Color.white;
            if (isPhoenix)
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
            else
            {
                for (int i = 0; i < AugmentManager.instance.AfterDeathActiveNumber; i++)
                {
                    foreach (IAugment Augment in AugmentManager.instance.activeAugments)
                    {
                        Augment.AfterDeathEffect(this);
                    }
                }
                GrowthAttackDamage = 0f;
                if (FusionMoveRoutine != null)
                {
                    StopCoroutine(FusionMoveRoutine);
                    FusionMoveRoutine = null;
                }
                GetComponent<Collider2D>().enabled = false;
                GameManager.instance.DiedBalls += 1;
                BallManager.Instance.TakeBall(gameObject.GetComponent<IBall>());
            }
        }
    }

    private IEnumerator EnableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
        while (transform.position.y < -2)
        {
            yield return null;
        }
        GetComponent<Collider2D>().enabled = true;
    }
    
    private IEnumerator CalAttackDelayTime()
    {
        while(true)
        {
            AttackDelayTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Update()
    {
        UpdateCurrentTarget();

        GrowthAttackDamageObserve = GrowthAttackDamage;
        CurrentSpeed = rb.linearVelocity.magnitude;

        
        if(transform.position.y <= -10 && !isOverDeathLine)
        {
            BallDied();
            isOverDeathLine = true;
        }

        if (rb.linearVelocity.magnitude > BallManager.Instance.maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * BallManager.Instance.maxSpeed;
        }
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_3")) //Bounce = Power
        {
            if (collision.gameObject.CompareTag("Wall") || (AugmentManager.instance.determineAugmentID("SPECIAL_21") && collision.gameObject.CompareTag("Ball")))
            {
                if (MaxBounce < 6)
                {
                    BounceTime += 0.02f;
                    MaxBounce++;
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g - 0.2f, spriteRenderer.color.b - 0.2f);
                }
            }
            else if (collision.gameObject.CompareTag("Brick"))
            {
                MaxBounce = 0;
                spriteRenderer.color = BrickColor;
            }
        }
        if (AugmentManager.instance.determineAugmentID("SPECIAL_21") && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D TartgetRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 TargetDirection = (TartgetRigidBody.position - (Vector2)transform.position).normalized;
            rb.AddForce(RotateVector(TargetDirection, Random.Range(-30f, 30f)) * Random.Range(5f, 11f), ForceMode2D.Impulse);

            if(collision.gameObject.GetInstanceID() < GetInstanceID())
            {
                Vector2 ContactPos = collision.contacts[0].point;
                Vector2 ContactVec = collision.contacts[0].normal;

                VisualEffectManager.Instance.SpawnExplosionWithBallEffect(ContactVec, ContactPos);
            }
        }
        if (collision.gameObject.CompareTag("Brick") && gameObject.activeSelf)
        {
            collision.gameObject.GetComponent<IBrick>().BallSpeed = CurrentSpeed;
            currentTarget = null;
            if (detectArea.NearbyBricks.Contains(collision.gameObject.GetComponent<IBrick>()))
            {
                detectArea.NearbyBricks.Remove(collision.gameObject.GetComponent<IBrick>());
            }
        }
        if (!collision.gameObject.CompareTag("Ball") && gameObject.activeSelf)
        {
            SoundManager.instance.BallAttackSound(gameObject.GetComponent<AudioSource>());
        }
    }

    Coroutine BallStuckCoroutine = null;
    bool IsStucking = false;
    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Brick") && IsStucking && gameObject.activeSelf)
        {
            rb.AddForce(rb.linearVelocity.normalized / 2, ForceMode2D.Impulse);
            if(BallStuckCoroutine != null)
                StopCoroutine(BallStuckCoroutine);
            IsStucking = false;
        }
    }

    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Brick") && !IsStucking && gameObject.activeSelf)
        {
            if (collision.gameObject.GetComponent<IBrick>().IsBoss)
                rb.AddForce(new Vector2(Random.value * 3, Random.value * 5));
            else
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

public partial class IBall //Homing
{
    private float homingThrust = 150f;
    private float detectionRadius = 1.6f;
    private float rotationSpeed = 40f;

    public Rigidbody2D rb;
    private Transform currentTarget = new RectTransform();

    public void FixedUpdate()
    {
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

    private void UpdateCurrentTarget()
    {
        if (detectArea.NearbyBricks.Count > 0)
        {
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (IBrick target in detectArea.NearbyBricks)
            {
                float distance = Vector2.Distance(transform.position, target.gameObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.gameObject.transform;
                }
            }

            currentTarget = closestTarget;
        }
        else
        {
            currentTarget = null;
        }
    }
}

public partial class IBall //Bounce = Power
{
    public float BounceTime { get; set; } = 1f;
    private int MaxBounce = 0;
    protected SpriteRenderer spriteRenderer;
}

public partial class IBall //GrowBall
{
    [SerializeField]
    public float GrowthAttackDamage { get; set; } = 0;
    public float GrowthAttackDamageObserve;
    public void Grow(IBrick brick)
    {
        StartCoroutine(GrowBall());
        if (brick.IsBoss)
        {
            GrowthAttackDamage += 0.2f;
        }
        else
        {
            GrowthAttackDamage += 0.04f;
        }

        IEnumerator GrowBall()
        {
            float elapsedTime = 0;
            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                Renderer.sharedMaterial.color = new Color(Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 0, elapsedTime / 0.25f));
                yield return null;
            }

            elapsedTime = 0;
            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                Renderer.sharedMaterial.color = new Color(Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(0, 1, elapsedTime / 0.25f));
                yield return null;
            }
        }
    }

    
}