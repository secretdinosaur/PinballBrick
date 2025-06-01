using System;
using UnityEngine;

public class Ball : IBall
{
    void OnEnable()
    {
        base.OnEnable();
        BallType = "Ball";
        BallBaseDamage = 0;
        GameManager.instance.SummonedBalls += 1;
    }
}