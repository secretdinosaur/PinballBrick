using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using TMPro;
using TrailRenderer = UnityEngine.TrailRenderer;

public partial class Ball : MonoBehaviour
{
    public float CurrentSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().enabled = false;
        transform.position = new Vector2(0, -5);
        gameObject.GetComponent<Rigidbody2D>().linearVelocityY = 10;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = Random.Range(-5f, 5f);
        StartCoroutine(EnableCollider());
        spriteRenderer.color = new Color(1f, 1f, 1f);
        TrailRenderer = gameObject.GetComponent<TrailRenderer>();
        BounceTime = 1f;
        MaxBounce = 0;
        targets.Clear();
        currentTarget = null;
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        TrailRenderer.startColor = spriteRenderer.color;
        CurrentSpeed = rb.linearVelocity.magnitude;
        
        if (CurrentSpeed > 8 && AugmentManager.instance.determineAugmentID("SPECIAL_12")) //Speed = Power
        {
            TransformationMultiple = 1f + ((CurrentSpeed / 100f) * 2.5f);
            TrailRenderer.time = 1;
        }
        else
        {
            TransformationMultiple = 1f;
            TrailRenderer.time -= TrailRenderer.time != 0 ? 0.005f : 0f;
        }
        
        if(transform.position.y <= -10)
        {
            foreach(IAugment Augment in AugmentManager.instance.activeSpecialAugments)
            {
                Augment.AfterDeathEffect();
            }
            BallManager.Instance.TakeBall(this);
        }

        if (rb.linearVelocity.magnitude > BallManager.Instance.maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * BallManager.Instance.maxSpeed;
        }
    }
    
    private void FixedUpdate()
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_2")) //Homing
        {
            DetectTargets();
            UpdateCurrentTarget();
            
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
            if (collision.gameObject.CompareTag("Wall"))
            {
                if (MaxBounce < 6)
                {
                    BounceTime += 0.1f;
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
}

public partial class Ball //Homing
{
    private float homingThrust = 80f;
    private float detectionRadius = 1.2f;
    private string targetLayerName = "Game";
    private float rotationSpeed = 20f;

    private Rigidbody2D rb;
    private LayerMask targetLayer = new LayerMask();
    [SerializeField]
    private List<Transform> targets = new List<Transform>();
    private Transform currentTarget = new RectTransform();
    private float lastDetectionTime;
    
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