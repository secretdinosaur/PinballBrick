using UnityEngine;

public class GlassBall : IBall
{
    private void OnEnable()
    {
        base.OnEnable();
        BallType = "GlassBall";
        BallBaseDamage = 4;
        GameManager.instance.SummonedBalls += 1;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (Random.value < 0.01f && collision.gameObject.CompareTag("Brick"))
        {
            BallDied();
        }
    }
}
