using System;
using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    public float maxSpeed = 15f;
    public bool isOnFlamerower;
    public bool isAntiGravity;
    
    public PoolManager PoolManager;
    public string NormalBall;
    
    public BallInfo BallInfo;
    
    private Canvas canvas;
    private Camera camera;
    void Awake()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
        NormalBall = "Ball";
        
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        camera = Camera.main;
    }

    public void CreateBall(int Num)
    {
        for(int i = 0; i < Num; i++)
        {
            IBall ball = GetSpecificBall(NormalBall);
            if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 0.1f)
            {
                IBall BombBall = GetSpecificBall("BombBall");
            }
        }
    }

    public void CreateSpecificBall(int Num, string BallType)
    {
        for (int i = 0; i < Num; i++)
        {
            IBall ball = GetSpecificBall(BallType);
            if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 1f)
            {
                IBall BombBall = GetSpecificBall("BombBall");
            }
        }
    }
    
    private IBall GetSpecificBall(string ballTypeName)
    {
        IBall ball = null;
        
        Type ballType = Type.GetType(ballTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(ballType);
        ball = (IBall)genericMethod.Invoke(PoolManager, null);
        return ball;
    }

    public void TakeBall(IBall ball)
    {
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        ball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        ball.transform.position = new Vector2(0, -5);
        
        Type ballType = Type.GetType(ball.BallType);
        var method = typeof(PoolManager).GetMethod("TakeToPool");
        var genericMethod = method.MakeGenericMethod(ballType);
        genericMethod.Invoke(PoolManager, new object[] { ball.GetComponent<IBall>() });
    }
    
    public void BallShowText(IBall ball, string Text, string DamageType, bool isCritical)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(ball.gameObject.transform.position);
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
    
    public List<IBall> InGameBall = new List<IBall>();
    public List<Transform> inGameBallPosition = new List<Transform>();
    public List<Collider2D> inGameBallCollider = new List<Collider2D>();
    public bool thereISPhoenix = false;
    public Vector2 boxSize = new Vector2(5f, 10f);

    void FixedUpdate()
    {
        CollectNearbyBalls();
        RemoveInactiveBalls();
    }

    void CollectNearbyBalls()
    {
        LayerMask detectionLayer = LayerMask.GetMask("Ball");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0, detectionLayer);
        foreach (Collider2D collider in colliders)
        {
            IBall obj = collider.gameObject.GetComponent<IBall>();
            if (obj != null && collider.gameObject.activeInHierarchy && !InGameBall.Contains(obj) && obj != this)
            {
                InGameBall.Add(obj);
                inGameBallPosition.Add(obj.gameObject.GetComponent<Transform>());
                inGameBallCollider.Add(obj.gameObject.GetComponent<Collider2D>());
            }
        }
    }

    void RemoveInactiveBalls()
    {
        InGameBall.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
        inGameBallPosition.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
        inGameBallCollider.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
