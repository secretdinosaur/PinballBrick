using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public partial class Ball : MonoBehaviour
{
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
        if(transform.position.y <= -10)
        {
            BallManager.Instance.TakeBall(this);
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
                if (MaxBounce < 3)
                {
                    BounceTime += 0.2f;
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
    public static float BounceTime { get; set; } = 1f;
    private int MaxBounce = 0;
    private SpriteRenderer spriteRenderer;
}
