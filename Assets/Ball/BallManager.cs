using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    public float maxSpeed = 15f;
    private PoolManager PoolManager;
    void Awake()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void CreateBall(int Num)
    {
        for(int i = 0; i < Num; i++)
        {
            Ball ball = PoolManager.GetFromPool<Ball>();
        }
    }

    public void TakeBall(Ball ball)
    {
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        ball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        ball.transform.position = new Vector2(0, -5);
        PoolManager.TakeToPool<Ball>(ball);
    }
    
    public List<Ball> InGameBall = new List<Ball>();
    public Vector2 boxSize = new Vector2(5f, 10f);

    void FixedUpdate()
    {
        CollectNearbyBricks();
        RemoveInactiveBricks();
    }

    void CollectNearbyBricks()
    {
        LayerMask detectionLayer = LayerMask.GetMask("Ball");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0, detectionLayer);
        foreach (Collider2D collider in colliders)
        {
            Ball obj = collider.gameObject.GetComponent<Ball>();
            if (obj != null && collider.gameObject.activeInHierarchy && !InGameBall.Contains(obj) && obj != this)
            {
                InGameBall.Add(obj);
            }
        }
    }

    void RemoveInactiveBricks()
    {
        InGameBall.RemoveAll(ball => ball == null || !ball.gameObject.activeInHierarchy);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
