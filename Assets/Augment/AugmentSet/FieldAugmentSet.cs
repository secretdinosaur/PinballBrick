using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using Color = UnityEngine.Color;

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
    float Damage;

    private void OnEnable()
    {
        SetAugment("FIELD_1", "All Damage", "All Damage", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAllDamage(0.5f, Damage));
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
        while (true)
        {
            yield return new WaitForSeconds(period);

            for (int i = 0; i < BrickManager.Instance.CurrentBricks.Count; i++)
            {
                IBrick brick = BrickManager.Instance.CurrentBricks[i];
                Damage = 10 * StatManager.instance.statMultiplier.AttackDamageMultiplier * StatManager.instance.Persistent_stat.AttackDamageMultiplier;
                brick.ArbitraryDamageAttack(Damage, false, Damage.ToString("F1"), "Lightning");
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
                IBall ball = BallManager.Instance.InGameBall[Random.Range(0, BallManager.Instance.InGameBall.Count)];
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
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 10;

    private void OnEnable()
    {
        SetAugment("FIELD_5", "SatelliteBeam", "SatelliteBeam", 1);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(FireSatelliteBeam(20f + Cooldown));
    }

    public override void Remove()
    {
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
            SpecialObjManager.Instance.CreateSpecialObj("SatelliteBeamObj");
        }
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
        //12f + Cooldown
        Routine = StartCoroutine(GetAntiGravity(7f));
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
        SetAugment("FIELD_8", "BountyHunter", "BountyHunter", 4);
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

public class LightningRod: FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_9", "LightningRod", "LightningRod", 2);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetLightningRod(8f + Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetLightningRod(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            SpecialObjManager.Instance.CreateSpecialObj("LightningRodObj");
        }
    }
}

public class Laser : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_10", "Laser", "Laser", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetLaser(3f + Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetLaser(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(Cooldown - 4f, Cooldown + 4f));
            if (BallManager.Instance.InGameBall.Count > 0)
            {
                for(int i = 0; i < Random.Range(1, BallManager.Instance.InGameBall.Count); i++)
                {
                    SpecialObjManager.Instance.CreateSpecialObj("LaserObj");
                }
            }
        }
    }
}

public class AttackOrDie : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_11", "AttackOrDie", "AttackOrDie", 2);
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.AttackDamage *= 1.4f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
        Routine = StartCoroutine(GetAttackOrDie());
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.AttackDamage /= 1.4f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetAttackOrDie()
    {
        while (true)
        {
            if (BallManager.Instance.InGameBall.Count > 0)
            {
                foreach (IBall ball in BallManager.Instance.InGameBall)
                {
                    if(ball.AttackDelayTime >= 2f && ball.AttackOrDieRoutine == null)
                    {
                        ball.AttackOrDieRoutine = StartCoroutine(GetDying(ball));
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator GetDying(IBall ball)
    {
        float elapsedTime = 0f;
        Color color = ball.GetComponent<SpriteRenderer>().color;

        while (ball.AttackDelayTime < 7f)
        {
            if (ball.AttackDelayTime >= 2f)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / 5f);
                ball.GetComponent<SpriteRenderer>().color = color;
            }
            else
            {
                color.a = 1f;
                ball.GetComponent<SpriteRenderer>().color = color;
                elapsedTime = 0f;
            }
            yield return null;
        }

        color.a = 0f;
        ball.GetComponent<SpriteRenderer>().color = color;
        if (ball.AttackOrDieRoutine != null)
        {
            ball.AttackOrDieRoutine = null;
        }
        if (ball.gameObject.activeInHierarchy)
        {
            ball.BallDied();
        }
    }
}

public class DecayZone : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_12", "DecayZone", "DecayZone", 2);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetDecayZone(7f + Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetDecayZone(float Cooldown)
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);
            SpecialObjManager.Instance.CreateSpecialObj("DecayZoneObj");
        }
    }
}

public class WeatherChange : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_13", "WeatherChange", "WeatherChange", 1);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetWeatherChange(30f));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetWeatherChange(float Cooldown)
    {
        while (true)
        {
            int i = Random.Range(1, 6);
            switch (i)
            {
                case 1:
                    //heat wave
                    Debug.Log("heat wave");
                    AugmentManager.instance.effectChance.FireChance += 0.15f;
                    yield return new WaitForSeconds(Cooldown);
                    AugmentManager.instance.effectChance.FireChance -= 0.15f;
                    break;
                case 2:
                    //toxic fog
                    Debug.Log("toxic fog");
                    AugmentManager.instance.effectChance.PoisonChance += 0.15f;
                    yield return new WaitForSeconds(Cooldown);
                    AugmentManager.instance.effectChance.PoisonChance -= 0.15f;
                    break;
                case 3:
                    //acid rain
                    Debug.Log("acid rain");
                    AugmentManager.instance.effectChance.DecayChance += 0.15f;
                    yield return new WaitForSeconds(Cooldown);
                    AugmentManager.instance.effectChance.DecayChance -= 0.15f;
                    break;
                case 4:
                    //solar eclipse
                    Debug.Log("solar eclipse");
                    AugmentManager.instance.effectChance.CurseChance += 0.15f;
                    yield return new WaitForSeconds(Cooldown);
                    AugmentManager.instance.effectChance.CurseChance -= 0.15f;
                    break;
                default:
                    //nothing
                    Debug.Log("nothing");
                    yield return new WaitForSeconds(Cooldown);
                    break;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}