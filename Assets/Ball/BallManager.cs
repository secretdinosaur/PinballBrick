using UnityEngine;
using Redcode.Pools;
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
}
