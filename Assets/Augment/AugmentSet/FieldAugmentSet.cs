using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Damage = 10f;

    private void OnEnable()
    {
        SetAugment("FIELD_1", "All Damage", "All Damage", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAllDamage(10f + Cooldown, Damage));
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
    float Cooldown = 3f;
    float Power = 10f;

    private void OnEnable()
    {
        SetAugment("FIELD_2", "AngryBall", "AngryBall", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAngryBall(Cooldown, Power));
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
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 2f;

    private void OnEnable()
    {
        SetAugment("FIELD_3", "PhoenixBall", "PhoenixBall", 4);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(ChoosingPhoenixBall(20f, 20f + Cooldown));
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
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    private void OnEnable()
    {
        SetAugment("FIELD_4", "TargetMark", "TargetMark", 1);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(TargetMarking(5f + Cooldown));
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

public class SatelliteBeam : FieldAugment
{
    Coroutine Routine = null;
    GameObject SatelliteBeamObj;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 10;

    private void OnEnable()
    {
        SetAugment("FIELD_5", "SatelliteBeam", "SatelliteBeam", 1);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(FireSatelliteBeam(20f + Cooldown));
        SatelliteBeamObj = Instantiate(AugmentManager.instance.SatelliteBeamObj, new Vector2(1000, 1000), Quaternion.identity);
    }

    public override void Remove()
    {
        Destroy(SatelliteBeamObj);
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator FireSatelliteBeam(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            Vector2 StartLocation = StartCoordinateSetting();
            Vector2 FinishLocation = FinishCoordinateSetting(StartLocation);
            SatelliteBeamObj.transform.position = StartLocation;
            yield return StartCoroutine(MoveSatelliteBeam(StartLocation, FinishLocation));
        }
    }

    private IEnumerator MoveSatelliteBeam(Vector2 StartLocation, Vector2 FinishLocation)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 12.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 12.5f;
            t = Mathf.SmoothStep(0, 1, t);
            SatelliteBeamObj.transform.position = Vector2.Lerp(StartLocation, FinishLocation, t);
            yield return null;
        }

        SatelliteBeamObj.transform.position = FinishLocation;
    }

    private Vector2 StartCoordinateSetting()
    {
        float X, Y;
        
        X = Random.value < 0.5f ? 5f : -5f;
        Y = Random.Range(-1.5f, 3.5f);

        return new Vector2(X, Y);
    }

    private Vector2 FinishCoordinateSetting(Vector2 StartCoordinate)
    {
        float X, Y;

        X = StartCoordinate.x == 5f ? -5f : 5f;
        Y = Random.Range(-1f, 4f);

        return new Vector2(X, Y);
    }
}

public class AutoGetExp : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_6", "AutoGetExp", "AutoGetExp", 5);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetExp(5f + Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetExp(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.01f;
        }
    }
}

public class AntiGravity : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_7", "AntiGravity", "AntiGravity", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetAntiGravity(12f + Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetAntiGravity(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            BallManager.Instance.isAntiGravity = true;
            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = -1f;
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * 1f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(5f);
            BallManager.Instance.isAntiGravity = false;
            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0.5f;
            }
        }
    }
}

public class BountyHunter: FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_7", "BountyHunter", "BountyHunter", 4);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetBountyHunter(Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetBountyHunter(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            if(BrickManager.Instance.CurrentBricks.Count >= 1)
            {
                IBrick WantedBrick = BrickManager.Instance.CurrentBricks[Random.Range(0, BrickManager.Instance.CurrentBricks.Count)];
                bool isDestroy = false;
                float timer = 20f;
                while (timer > 0)
                {
                    if (WantedBrick == null || !WantedBrick.gameObject.activeInHierarchy)
                    {
                        isDestroy = true;
                        break;
                    }
                    timer -= Time.deltaTime;
                    yield return null;
                }
                if (isDestroy)
                {
                    StatManager.instance.GetGold(WantedBrick.IsBoss ? Random.Range(20, 101) : Random.Range(20, 61));
                    LevelManager.Instance.GetExp(WantedBrick.IsBoss ?
                        Random.Range(LevelManager.Instance.LevelBar.maxValue * 0.1f, LevelManager.Instance.LevelBar.maxValue * 0.2f)
                        : Random.Range(LevelManager.Instance.LevelBar.maxValue * 0.05f, LevelManager.Instance.LevelBar.maxValue * 0.1f));
                }
                else
                {
                    WantedBrick.HP += WantedBrick.HpTemp * 0.1f;
                }
            }
        }
    }
}