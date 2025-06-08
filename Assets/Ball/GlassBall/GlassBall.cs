using UnityEngine;

public class GlassBall : IBall
{
    private void OnEnable()
    {
        base.OnEnable();
        BallType = "GlassBall";
        BallBaseDamage = 1;
        GameManager.instance.SummonedBalls += 1;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (Random.value < 0.008f && collision.gameObject.CompareTag("Brick"))
        {
            BallDied();
        }
    }
}
