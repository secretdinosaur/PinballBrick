using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;

/*
public class Example : FieldAugment
{
    private void OnEnable()
    {
        SetAugment("Field_N", "Example", "Example", 3);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public virtual void BeforeAttackEffect(IBrick Brick) { }
    
    public virtual void AfterAttackEffect(IBrick Brick) { }

    public virtual void CriticalAttackEffect(IBrick Brick) { }
}
 */

public class AllDamage : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_1", "All Damage", "All Damage", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAllDamage(5f, 10f));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator StartAllDamage(float period, float Damage)
    {
        while(true)
        {
            yield return new WaitForSeconds(period);

            foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
            {
                brick.HP -= 10 * StatManager.instance.statMultiplier.AttackDamageMultiplier;
            }
        }
    }
}

public class AngryBall : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_2", "AngryBall", "AngryBall", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAngryBall(3f, 50f));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator StartAngryBall(float period, float Power)
    {
        while(true)
        {
            yield return new WaitForSeconds(period);

            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * Power, ForceMode2D.Impulse);
            }
        }
    }
}

public class PhoenixBall : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_3", "PhoenixBall", "PhoenixBall", 4);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(ChoosingPhoenixBall(20f, 30f));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator ChoosingPhoenixBall(float period, float Cooldown)
    {
        while(true)
        {
            yield return new WaitForSeconds(Cooldown);
            if (BallManager.Instance.InGameBall.Count > 0 && BallManager.Instance.InGameBall != null && !BallManager.Instance.thereISPhoenix)
            {
                Ball ball = BallManager.Instance.InGameBall[Random.Range(0, BallManager.Instance.InGameBall.Count)];
                ball.isPhoenix = true;
                BallManager.Instance.thereISPhoenix = true;
                yield return new WaitForSeconds(period);
                ball.isPhoenix = false;
                BallManager.Instance.thereISPhoenix = false;
            }
        }
    }
}

public class TargetMark : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_4", "TargetMark", "TargetMark", 1);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(TargetMarking(8f));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator TargetMarking(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            if (BallManager.Instance.InGameBall.Count > 0 && BallManager.Instance.InGameBall != null)
            {
                List<Rigidbody2D> BallRigidBody2D = new List<Rigidbody2D>();
                foreach(Ball ball in BallManager.Instance.InGameBall)
                {
                    Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                    BallRigidBody2D.Add(rb);
                    rb.gravityScale = Random.Range(0.01f, 0.03f);
                    rb.linearVelocity = Vector2.zero;
                }
                yield return new WaitForSeconds(1.5f);
                IBrick brick = BrickManager.Instance.CurrentBricks[Random.Range(0, BrickManager.Instance.CurrentBricks.Count)];
                foreach (Rigidbody2D ball in BallRigidBody2D)
                {
                    Vector2 TargetDirection = (brick.transform.position - ball.transform.position).normalized;
                    ball.AddForce(TargetDirection * 50f, ForceMode2D.Impulse);
                    yield return null;
                    ball.gravityScale = 0.5f;
                }
            }
        }
    }
}