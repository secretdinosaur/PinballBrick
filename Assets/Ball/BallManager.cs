using System;
using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using Random = UnityEngine.Random;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;

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

    public static Vector3 StartPosition = new Vector3(0f, -5f, 35);
    public static float linearVelocityYValue = 10;

    public int LeftBallCount;
    void Awake()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
        NormalBall = "Ball";
        
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        camera = Camera.main;

        LeftBallCount = 0;
    }

    public void CreateBall(int Num)
    {
        Num *= AugmentManager.instance.SpawnBallAmount;
        if (AugmentManager.instance.determineAugmentID("SPECIAL_60"))
        {
            DiceManager.Instance.StartCoroutine(DiceManager.Instance.RollDices(Num, OnCreateBall));
        }
        else
        {
            Debug.Log("create ball");
            OnCreateBall(Num);
        }
    }

    public void OnCreateBall(int diceSum)
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_38"))
        {
            LeftBallCount += diceSum;
        }
        else if (AugmentManager.instance.determineAugmentID("SPECIAL_39"))
        {
            LeftBallCount += diceSum;
        }
        else
        {
            for (int i = 0; i < diceSum; i++)
            {
                IBall ball = GetSpecificBall(NormalBall);
                if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 0.1f)
                {
                    CreateBombBall(1);
                }
            }
        }
    }

    public void CreateSpecificBall(int Num, string BallType, bool IsRebirth = false)
    {
        Num *= AugmentManager.instance.SpawnBallAmount;

        if (AugmentManager.instance.determineAugmentID("SPECIAL_38"))
        {
            LeftBallCount += Num;
        }
        else if (AugmentManager.instance.determineAugmentID("SPECIAL_39"))
        {
            LeftBallCount += Num;
        }
        else
        {
            for (int i = 0; i < Num; i++)
            {
                IBall ball = GetSpecificBall(BallType, IsRebirth);
                if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 0.05f)
                {
                    CreateBombBall(1);
                }
            }
        }
    }

    public void CreateGhostBall(int Num, IBall DeadBall, bool IsRebirth = false)
    {
        Num *= AugmentManager.instance.SpawnBallAmount;
        for (int i = 0; i < Num; i++)
        {
            IBall ball = GetSpecificBall("GhostBall", IsRebirth);
            ball.GetComponent<GhostBall>().Init(DeadBall.GetComponent<SpriteRenderer>().sprite, DeadBall.BallBaseDamage);
        }
    }

    public void CreateBombBall(int Num, bool IsRebirth = false)
    {
        Num *= AugmentManager.instance.SpawnBallAmount;
        for (int i = 0; i < Num; i++)
        {
            IBall ball = GetSpecificBall("BombBall", IsRebirth);
        }
    }

    private IBall GetSpecificBall(string ballTypeName, bool IsRebirth = false)
    {
        IBall ball = null;
        
        Type ballType = Type.GetType(ballTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(ballType);
        ball = (IBall)genericMethod.Invoke(PoolManager, null);
        ball.StartPosition = StartPosition;
        ball.linearVelocityYValue = linearVelocityYValue;
        if (IsRebirth)
        {
            StartCoroutine(RebirthBallEffect(ball));
        }
        ball.transform.position = ball.StartPosition;
        return ball;

        IEnumerator RebirthBallEffect(IBall RecentlyBall)
        {
            float elapsedTime = 0;
            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                RecentlyBall.Renderer.material.color = new Color(Mathf.Lerp(1, 0, elapsedTime / 0.25f), Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f));
                yield return null;
            }

            elapsedTime = 0;
            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                RecentlyBall.Renderer.material.color = new Color(Mathf.Lerp(0, 1, elapsedTime / 0.25f), Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f));
                yield return null;
            }
        }
    }

    public void DestroyBallNum(int Num, bool isBallDied = false)
    {
        var ballsCopy = InGameBall.ToList();
        if (isBallDied)
        {
            for (int i = 0; i < Mathf.Min(Num, ballsCopy.Count); i++)
            {
                StartCoroutine(BallRemoveEffect(InGameBall[i]));
                InGameBall[i].BallDied();
            }
        }
        else
        {
            for (int i = 0; i < Mathf.Min(Num, ballsCopy.Count); i++)
            {
                StartCoroutine(BallRemoveEffect(InGameBall[i]));
                TakeBall(InGameBall[i]);
            }
        }  
    }

    private IEnumerator BallRemoveEffect(IBall ball)
    {
        VisualEffectManager.Instance.SpawnParticle("BallSacrifice", ball.transform.position, 30);
        yield return null;
    }

    public void TakeBall(IBall ball)
    {
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        ball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        ball.transform.position = new Vector3(0, -5, 35);
        
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
    public List<IBall> InGameSpecialBall = new List<IBall>();

    public List<Transform> inGameBallPosition = new List<Transform>();
    public List<Transform> inGameSpecialBallPosition = new List<Transform>();

    public List<Collider2D> inGameBallCollider = new List<Collider2D>();
    public List<Collider2D> inGameSpecialBallCollider = new List<Collider2D>();

    public bool thereISPhoenix = false;
    public Vector2 boxSize = new Vector2(5f, 10f);

    public List<string> SpecialBallList = new List<string>
    {
        "GhostBall", "BombBall"
    };

    public List<string> BallList = new List<string>
    {
        "GlassBall", "Ball"
    };

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
            if (obj != null && collider.gameObject.activeInHierarchy && !InGameBall.Contains(obj) && obj != this && BallList.Contains(obj.BallType))
            {
                InGameBall.Add(obj);
                inGameBallPosition.Add(obj.gameObject.GetComponent<Transform>());
                inGameBallCollider.Add(obj.gameObject.GetComponent<Collider2D>());
            }
            else if(obj != null && collider.gameObject.activeInHierarchy && !InGameSpecialBall.Contains(obj) && obj != this && SpecialBallList.Contains(obj.BallType))
            {
                InGameSpecialBall.Add(obj);
                inGameSpecialBallPosition.Add(obj.gameObject.GetComponent<Transform>());
                inGameSpecialBallCollider.Add(obj.gameObject.GetComponent<Collider2D>());
            }
        }
    }

    void RemoveInactiveBalls()
    {
        InGameBall.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
        inGameBallPosition.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
        inGameBallCollider.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);

        InGameSpecialBall.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
        inGameSpecialBallPosition.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
        inGameSpecialBallCollider.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
