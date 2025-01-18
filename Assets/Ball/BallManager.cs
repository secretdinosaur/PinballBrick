using UnityEngine;
using Redcode.Pools;
public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    private PoolManager PoolManager;
    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    public void CreateBall()
    {
        DiceManager.Instance.RollDice(1);
        
    }

    public void TakeBall(Ball ball)
    {
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        ball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        ball.transform.position = new Vector2(0, -5);
        PoolManager.TakeToPool<Ball>(ball);
    }
}
