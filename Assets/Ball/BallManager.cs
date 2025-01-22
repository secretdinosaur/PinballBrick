using UnityEngine;
using Redcode.Pools;
public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    [SerializeField]
    public Persistent_Stat Persistent_Stat;
    private PoolManager PoolManager;
    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
        Persistent_Stat = StatManager.instance.LoadPersistentStats();
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
