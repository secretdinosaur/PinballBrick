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
}
