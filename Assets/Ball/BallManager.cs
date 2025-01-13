using UnityEngine;
using Redcode.Pools;
public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    public PoolManager PoolManager;
    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void CreateBall()
    {
        Ball ball = PoolManager.GetFromPool<Ball>();
        ball.transform.position = new Vector2(0, -5);
        ball.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = 10;
        ball.gameObject.GetComponent<Rigidbody2D>().linearVelocityX = Random.Range(-5f, 5f);
    }

    public void TakeBall(Ball ball)
    {
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        PoolManager.TakeToPool<Ball>(ball);
    }
}
