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
using Redcode.Pools;

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
        SetAugment("FIELD_1", "Holy Rain", "The holy rain damages all bricks", 3, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription("Damage: 5 + <sprite=2>");
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAllDamage(Cooldown, Damage));
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
            for (int i = 0; i < BrickManager.Instance.CurrentBricks.Count; i++)
            {
                IBrick brick = BrickManager.Instance.CurrentBricks[i];
                Damage = 5f + (StatManager.instance.Persistent_stat.EffectDamage + StatManager.instance.Temporary_stat.EffectDamage)
                    * (StatManager.instance.statMultiplier.EffectDamageMultiplier * StatManager.instance.Persistent_stat.EffectDamageMultiplier);
                StartCoroutine(SyncDamageAndEffect(Damage, brick));
            }
            yield return new WaitForSeconds(period);
        }
    }

    private IEnumerator SyncDamageAndEffect(float Damage, IBrick brick)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        VisualEffectManager.Instance.SpawnParticle("AllDamage", brick.transform.position, 1);

        yield return new WaitForSeconds(0.65f);
        brick.ArbitraryDamageAttack(Damage, false, Damage.ToString("F1"), "Lightning");
    }
}

public class AngryBall : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown / 10 * 3;
    float Power = 10f;

    private void OnEnable()
    {
        SetAugment("FIELD_2", "Angry Ball", "Balls briefly become enraged and move faster", 3, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription(null);
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
            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                StartCoroutine(AngryBallEffect(ball));
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * Power, ForceMode2D.Impulse);
            }
            foreach (Ball ball in BallManager.Instance.InGameSpecialBall)
            {
                StartCoroutine(AngryBallEffect(ball));
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * Power, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(period);
        }
    }

    IEnumerator AngryBallEffect(IBall RecentlyBall)
    {
        float elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            RecentlyBall.Renderer.material.color = new Color(Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 0, elapsedTime / 0.25f), Mathf.Lerp(1, 0, elapsedTime / 0.25f));
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            RecentlyBall.Renderer.material.color = new Color(Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(0f, 1, elapsedTime / 0.25f), Mathf.Lerp(0f, 1, elapsedTime / 0.25f));
            yield return null;
        }
    }
}

public class PhoenixBall : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 2f;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;
    private void OnEnable()
    {
        SetAugment("FIELD_3", "Phoenix Ball", "The random ball become invincible for " + Duration.ToString("F1") + " seconds", 4, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(ChoosingPhoenixBall(Duration, Cooldown));
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

            if (BallManager.Instance.InGameBall.Count > 0 && BallManager.Instance.InGameBall != null && !BallManager.Instance.thereISPhoenix)
            {
                IBall ball = BallManager.Instance.InGameBall[Random.Range(0, BallManager.Instance.InGameBall.Count)];
                ball.isPhoenix = true;
                BallManager.Instance.thereISPhoenix = true;
                ball.Renderer.material.color = new Color(2, 0, 0);
                yield return new WaitForSeconds(period);
                ball.isPhoenix = false;
                ball.Renderer.material.color = new Color(1, 1, 1);
                BallManager.Instance.thereISPhoenix = false;
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class TargetMark : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    GameObject Target;
    Material TargetMat;
    private void OnEnable()
    {
        SetAugment("FIELD_4", "Focused Attack", "All balls target a random brick", 2, 10);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        Target = GameObject.Find("Target");
        TargetMat = Target.GetComponent<Renderer>().material;
        Target.SetActive(false);
        Routine = StartCoroutine(TargetMarking(Cooldown));
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
            if (BallManager.Instance.InGameBall.Count > 0 && BallManager.Instance.InGameBall != null && BrickManager.Instance.CurrentBricks.Count > 0)
            {
                List<Rigidbody2D> BallRigidBody2D = new List<Rigidbody2D>();
                foreach(IBall ball in BallManager.Instance.InGameBall)
                {
                    CircleCollider2D collider = ball.GetComponent<CircleCollider2D>();
                    Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                    BallRigidBody2D.Add(rb);
                    collider.enabled = false;
                    rb.gravityScale = Random.Range(0.01f, 0.03f);
                    rb.linearVelocity = Vector2.zero;
                }
                foreach (IBall ball in BallManager.Instance.InGameSpecialBall)
                {
                    CircleCollider2D collider = ball.GetComponent<CircleCollider2D>();
                    Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
                    BallRigidBody2D.Add(rb);
                    collider.enabled = false;
                    rb.gravityScale = Random.Range(0.01f, 0.03f);
                    rb.linearVelocity = Vector2.zero;
                }

                IBrick TargetBrick = BrickManager.Instance.CurrentBricks[Random.Range(0, BrickManager.Instance.CurrentBricks.Count)];
                Vector3 TargetPosition = TargetBrick.transform.position;
                Target.transform.position = TargetPosition;
                Target.SetActive(true);
                StartCoroutine(FindTarget());

                float elapsed = 0f;
                while(elapsed < 1)
                {
                    if(!TargetBrick.gameObject.activeSelf)
                    {
                        TargetBrick = BrickManager.Instance.CurrentBricks[Random.Range(0, BrickManager.Instance.CurrentBricks.Count)];
                        TargetPosition = TargetBrick.transform.position;
                        Target.transform.position = TargetPosition;
                        Target.SetActive(true);
                        elapsed = 0;
                    }
                    yield return null;
                    elapsed += Time.deltaTime;
                    
                }

                Target.SetActive(false);
                foreach (Rigidbody2D ball in BallRigidBody2D)
                {
                    CircleCollider2D collider = ball.gameObject.GetComponent<CircleCollider2D>();
                    collider.enabled = true;
                    Vector2 TargetDirection = (TargetPosition - ball.transform.position).normalized;
                    ball.AddForce(TargetDirection * 50f, ForceMode2D.Impulse);
                    yield return null;
                    ball.gravityScale = 0.5f;
                }
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }

    private IEnumerator FindTarget()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            TargetMat.SetFloat("_Alpha", Mathf.Lerp(1, 0, elapsedTime / 1f));
            TargetMat.SetFloat("_Color", Mathf.Lerp(0, 1, elapsedTime / 1f));
            yield return null;
        }
    }
}

public class SatelliteBeam : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 6;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;
private void OnEnable()
    {
        SetAugment("FIELD_5", "Satellite Beam", "Generates a powerful satellite beam for " + Duration.ToString("F1") + "seconds", 1, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription("On hit damage: 10 <sprite=2>\nContinuous damage: <sprite=2>");
    }

    public override void Apply()
    {
        Routine = StartCoroutine(FireSatelliteBeam(Cooldown, Duration));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator FireSatelliteBeam(float Cooldown, float Duration)
    {
        while (true)
        {
            SpecialObjManager.Instance.CreateSpecialObj("SatelliteBeamObj", Duration);
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class AutoGetExp : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    private void OnEnable()
    {
        SetAugment("FIELD_6", "Meditation", "Automatically gain 1% of the max EXP", 5, 10);
        SetAdditionalDescription(null);
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
            LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.01f;
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class AntiGravity : FieldAugment
{
    GameObject AntiGravityObj;
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 5;
    private void OnEnable()
    {
        SetAugment("FIELD_7", "Antigravity", "Reverses gravity for " + Duration.ToString("F1") + " seconds", 3, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetAntiGravity(Cooldown, Duration));
        AntiGravityObj = GameObject.Find("AntiGravity");
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
        BallManager.Instance.isAntiGravity = false;
        foreach (Ball ball in BallManager.Instance.InGameBall)
        {
            Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.5f;
        }
        foreach (Ball ball in BallManager.Instance.InGameSpecialBall)
        {
            Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.5f;
        }
    }

    private IEnumerator GetAntiGravity(float Cooldown, float Duration)
    {
        while (true)
        {
            BallManager.Instance.isAntiGravity = true;

            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = -1f;
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * 1f, ForceMode2D.Impulse);
            }
            foreach (Ball ball in BallManager.Instance.InGameSpecialBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = -1f;
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * 1f, ForceMode2D.Impulse);
            }

            StartCoroutine(AntiGravityEffect(true));
            yield return new WaitForSeconds(Duration);
            StartCoroutine(AntiGravityEffect(false));
            BallManager.Instance.isAntiGravity = false;

            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0.5f;
            }
            foreach (Ball ball in BallManager.Instance.InGameSpecialBall)
            {
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0.5f;
            }

            yield return new WaitForSeconds(Cooldown);
        }
    }

    private IEnumerator AntiGravityEffect(bool IsUp)
    {
        float rotation = IsUp ? 0 : 180;
        AntiGravityObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        float elapsedTime = 0f;

        if (IsUp)
            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                AntiGravityObj.transform.position = Vector3.Lerp(new Vector3(0, -8, 0), new Vector3(0, 8, 0), elapsedTime / 0.25f);
                yield return null;
            }
        else
            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                AntiGravityObj.transform.position = Vector3.Lerp(new Vector3(0, 8, 0), new Vector3(0, -8, 0), elapsedTime / 0.25f);
                yield return null;
            }
    }
}

public class BountyHunter: FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;
    GameObject BountyHunterObj;
    Material BountyHunterMat;
    private void OnEnable()
    {
        SetAugment("FIELD_8", "Bounty Hunter", "Random brick is wanted for " + Duration.ToString("F1") + " seconds \nrewards are earned when breaking it", 4, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription("Kill wanted brick and get EXP and gold");
    }

    public override void Apply()
    {
        BountyHunterObj = GameObject.Find("BountyHunter");
        BountyHunterMat = BountyHunterObj.GetComponent<Renderer>().material;
        BountyHunterObj.SetActive(false);
        Routine = StartCoroutine(GetBountyHunter(Cooldown, Duration));
}

public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetBountyHunter(float Cooldown, float Duration)
    {
        while (true)
        {
            if(BrickManager.Instance.CurrentBricks.Count >= 1)
            {
                IBrick WantedBrick = BrickManager.Instance.CurrentBricks[Random.Range(0, BrickManager.Instance.CurrentBricks.Count)];
                BountyHunterObj.SetActive(true);
                Vector2 Position = WantedBrick.transform.position;
                BountyHunterObj.transform.position = Position;
                
                bool isDestroy = false;
                float timer = Duration;
                while (timer > 0)
                {
                    BountyHunterMat.SetFloat("_Alpha", Mathf.Lerp(1, 0, timer / 20f));
                    BountyHunterMat.SetFloat("_Color", Mathf.Lerp(0, 1, timer / 20f));
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
                        
                    ItemManager.Instance.SpawnSpecificItem("ExpBead", Position, WantedBrick.IsBoss ? Random.Range(30, 50) : Random.Range(10, 15));
                    ItemManager.Instance.SpawnSpecificItem("Gold1", Position, WantedBrick.IsBoss ? Random.Range(10, 15) : Random.Range(3, 5));
                    ItemManager.Instance.SpawnSpecificItem("Gold2", Position, WantedBrick.IsBoss ? Random.Range(10, 15) : Random.Range(3, 5));
                    ItemManager.Instance.SpawnSpecificItem("Gold3", Position, WantedBrick.IsBoss ? Random.Range(10, 15) : Random.Range(3, 5));
                    BountyHunterObj.SetActive(false);
                }
                else
                {
                    WantedBrick.HP += WantedBrick.HpTemp * 0.1f;
                }
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class LightningRod: FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;

    private void OnEnable()
    {
        SetAugment("FIELD_9", "Thunderstrike Zone", "Generates thunderstrike zone for " + Duration.ToString("F1") + " seconds", 2, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription("Lightning damage: 0.2 <sprite=2> + 3% of brick's max HP");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<Lightning>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetLightningRod(Cooldown, Duration));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator GetLightningRod(float Cooldown, float Duration)
    {
        while (true)
        {
            SpecialObjManager.Instance.CreateSpecialObj("LightningRodObj", Duration);
            yield return new WaitForSeconds(Cooldown + Duration);
        }
    }
}

public class AttackOrDie : FieldAugment
{
    Coroutine Routine = null;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 10;

    private void OnEnable()
    {
        SetAugment("FIELD_11", "Last Breath", "<sprite=0> +40%,\nballs get die if they don't hit any bricks within " + Duration.ToString("F1") +" seconds.", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.4f);
        Routine = StartCoroutine(GetAttackOrDie());
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.4f);
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
                for (int i = 0; i < BallManager.Instance.InGameBall.Count; i++)
                {
                    if(BallManager.Instance.InGameBall[i].AttackOrDieRoutine == null)
                    {
                    BallManager.Instance.InGameBall[i].AttackOrDieRoutine = StartCoroutine(GetDying(BallManager.Instance.InGameBall[i]));
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

        while (ball.AttackDelayTime < Duration)
        {
            if (ball.AttackDelayTime >= 5f)
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
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 10;
    private void OnEnable()
    {
        SetAugment("FIELD_12", "Decay Zone", "Generate a decay zone for " + Duration.ToString("F1") + " seconds", 2, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription("Decay Effect: Increases damage taken by bricks by 10% per stack");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<DecayBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetDecayZone(Cooldown));
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
            SpecialObjManager.Instance.CreateSpecialObj("DecayZoneObj", Duration);
            yield return new WaitForSeconds(Cooldown + Duration);
        }
    }
}

public class WeatherChange : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 3;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 10;

private void OnEnable()
    {
        SetAugment("FIELD_13", "Constant Disaster", "Random disaster occured for " + Duration.ToString("F1") + " seconds", 1, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription(
            "Fire damage : 0.5 <sprite=2> per stack\n" +
            "Poison damage : 0.5 <sprite=2> + 3% of brick's current HP per stack\n" +
            "Decay Effect: Increases damage taken by bricks by 10% per stack \n" +
            "Curse damage : 1.5 <sprite=0>\n" +
            "Lightning damage : 0.2 <sprite=2> + 3% of brick's max HP");
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetWeatherChange(Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    float FireChance = 0.5f;
    float PoisonChance = 0.5f;
    float DecayChance = 0.5f;
    float CurseChance = 0.5f;
    float LightningChance = 0.5f;

    private IEnumerator GetWeatherChange(float Cooldown)
    {
        while (true)
        {
            float elapsedTime = 0;
            switch (Random.Range(1, 6))
            {
                case 1:
                    AugmentManager.instance.effectChance.FireChance += 0.15f;
                    while (elapsedTime < Duration)
                    {
                        for(int j = 0; j < BrickManager.Instance.CurrentBricks.Count; j++)
                        {
                            if(Random.value < FireChance)
                                BrickManager.Instance.CurrentBricks[j].
                                    DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.FireDuration);
                        }
                        elapsedTime += 1;
                        yield return new WaitForSeconds(1f);
                    }
                    AugmentManager.instance.effectChance.FireChance -= 0.15f;
                    break;
                case 2:
                    AugmentManager.instance.effectChance.PoisonChance += 0.15f;
                    while (elapsedTime < Duration)
                    {
                        for (int j = 0; j < BrickManager.Instance.CurrentBricks.Count; j++)
                        {
                            if (Random.value < PoisonChance)
                                BrickManager.Instance.CurrentBricks[j].
                                    DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.PoisonDuration);
                        }
                        elapsedTime += 1;
                        yield return new WaitForSeconds(1f);
                    }
                    AugmentManager.instance.effectChance.PoisonChance -= 0.15f;
                    break;
                case 3:
                    //acid rain
                    AugmentManager.instance.effectChance.DecayChance += 0.15f;
                    while (elapsedTime < Duration)
                    {
                        for (int j = 0; j < BrickManager.Instance.CurrentBricks.Count; j++)
                        {
                            if (Random.value < DecayChance)
                                BrickManager.Instance.CurrentBricks[j].
                                    DecayEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.DecayDuration);
                        }
                        elapsedTime += 1;
                        yield return new WaitForSeconds(1f);
                    }
                    AugmentManager.instance.effectChance.DecayChance -= 0.15f;
                    break;
                case 4:
                    AugmentManager.instance.effectChance.CurseChance += 0.15f;
                    while (elapsedTime < Duration)
                    {
                        for (int j = 0; j < BrickManager.Instance.CurrentBricks.Count; j++)
                        {
                            if (Random.value < CurseChance)
                                BrickManager.Instance.CurrentBricks[j].DamageByCurse();
                        }
                        elapsedTime += 1;
                        yield return new WaitForSeconds(1f);
                    }
                    AugmentManager.instance.effectChance.CurseChance -= 0.15f;
                    break;
                case 5:
                    while (elapsedTime < Duration)
                    {
                        for (int j = 0; j < BrickManager.Instance.CurrentBricks.Count; j++)
                        {
                            if (Random.value < LightningChance)
                                BrickManager.Instance.CurrentBricks[j].DamageByLightningEffect(1, 1);
                        }
                        elapsedTime += 1;
                        yield return new WaitForSeconds(1f);
                    }
                    break;
                default:
                    break;

            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class DiceGamble : FieldAugment
{
    Coroutine Routine = null;
    Coroutine Routine2 = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 6;
        
    private void OnEnable()
    {
        SetAugment("FIELD_14", "Dice Gamble", "Roll the dice and get rewards based on the result", 2, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(GetDiceGamble(Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            StopCoroutine(Routine2);
            Routine = null;
            Routine2 = null;
    }
}
    private List<int[]> rolledDiceSet = new List<int[]>();
    private IEnumerator GetDiceGamble(float Cooldown)
    {
        while (true)
        {
            rolledDiceSet = new List<int[]>();
            DiceManager.Instance.StartCoroutine(DiceManager.Instance.RollDicesforDiceGamble(3, AddDiceSet));
            yield return new WaitUntil(() => rolledDiceSet.Count == 1);
            CalDiceSet();
            yield return new WaitForSeconds(Cooldown);
        }
    }

    private void AddDiceSet(int[] dice)
    {
        rolledDiceSet.Add(dice);
    }

    IEnumerator SpawnLuckyItem()
    {
        int SpawnCount = 0;
        while(SpawnCount < 30)
        {
            for(int i = 0; i < BrickManager.Instance.CurrentBricks.Count; i++)
            {
                IBrick brick = BrickManager.Instance.CurrentBricks[i];
                if(brick.gameObject.activeSelf)
                {
                    ItemManager.Instance.SpawnSpecificItem("ExpBead", brick.transform.position, 3);
                    ItemManager.Instance.SpawnSpecificItem("Gold1", brick.transform.position, 1);
                    ItemManager.Instance.SpawnSpecificItem("Gold2", brick.transform.position, 1);
                    ItemManager.Instance.SpawnSpecificItem("Gold3", brick.transform.position, 1);
                    SpawnCount += 1;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CalDiceSet()
    {
        rolledDiceSet.Sort();
        int DiceSum = rolledDiceSet[0][0] + rolledDiceSet[0][1] + rolledDiceSet[0][2];
        if (rolledDiceSet[0][0] == rolledDiceSet[0][1] && rolledDiceSet[0][1] == rolledDiceSet[0][2])
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
                    {
                        StartCoroutine(brick.Execution());
                    }
                    break;
                case 2:
                    Routine2 = StartCoroutine(SpawnLuckyItem());
                break;
                case 3:
                    foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
                    {
                        brick.ArbitraryDamageAttack(brick.HpTemp * 0.77f, false, "77%", "Normal");
                        brick.
                            DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.FireDuration);
                        brick.
                            DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.PoisonDuration);
                        brick.
                            DecayEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.DecayDuration);
                    }
                    break;
                default:
                    break;
            }
        }
        else if (rolledDiceSet[0][0] + 1 == rolledDiceSet[0][1] && rolledDiceSet[0][1] + 1 == rolledDiceSet[0][2])
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    for(int i = 0; i < DiceSum / 2; i++)
                    {
                        BallManager.Instance.CreateBombBall(1);
                    }
                break;
                case 2:
                    foreach (Brick brick in BrickManager.Instance.CurrentBricks)
                    {
                        brick.ArbitraryDamageAttack(brick.HpTemp * 0.077f, false, "7.7%", "Normal");
                        brick.ArbitraryDamageAttack(brick.HpTemp * 0.077f, false, "7.7%", "Normal");
                        brick.ArbitraryDamageAttack(brick.HpTemp * 0.077f, false, "7.7%", "Normal");
                    }
                break;
                case 3:
                    LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.5f * DiceSum;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    BallManager.Instance.CreateBall(DiceSum / 2);
                    break;
                case 2:
                    BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
                    BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
                    BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
                break;
                case 3:
                        LevelManager.Instance.Exp += DiceSum * 0.02f;
                        break;
                default:
                    break;
            }
        }
    }
}

public class Trampoline : FieldAugment
{
    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 30;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 5;
    GameObject TrampolineObj;

private void OnEnable()
    {
        SetAugment("FIELD_15", "Trampoline", "Spawn rectangle trampoline for " + Duration.ToString("F1") + " seconds", 2, (float)System.Math.Round(Cooldown, 2));
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        TrampolineObj = GameObject.Find("BangBang");
        TrampolineObj.transform.position = new Vector2(-0.75f, -0.55f);
        TrampolineObj.SetActive(false);

        Routine = StartCoroutine(StartTrampoline(Cooldown));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator StartTrampoline(float Cooldown)
    {
        while (true)
        {
            TrampolineObj.SetActive(true);
            yield return new WaitForSeconds(Duration);
            TrampolineObj.SetActive(false);
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class Bombardment : FieldAugment
{
Coroutine Routine = null;
float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 1.5f;

private void OnEnable()
{
    SetAugment("FIELD_16", "Air Strike", "Summons 5 bombs that fall from above", 2, (float)System.Math.Round(Cooldown, 2));
    SetAdditionalDescription("Bomb damage: 10 <sprite=0>");
}

public override void Apply()
{
    Routine = StartCoroutine(StartAirStrike(Cooldown));
}

public override void Remove()
{
    if (Routine != null)
    {
        StopCoroutine(Routine);
        Routine = null;
    }
}

private IEnumerator StartAirStrike(float Cooldown)
{
    while (true)
    {      
        CreateAirStrikeBombBall(new Vector3(-1.8f, 4.725f, 0));
        yield return new WaitForSeconds(0.3f);
        CreateAirStrikeBombBall(new Vector3(-0.9f, 4.725f, 0));
        yield return new WaitForSeconds(0.3f);
        CreateAirStrikeBombBall(new Vector3(0f, 4.725f, 0));
        yield return new WaitForSeconds(0.3f);
        CreateAirStrikeBombBall(new Vector3(0.9f, 4.725f, 0));
        yield return new WaitForSeconds(0.3f);
        CreateAirStrikeBombBall(new Vector3(1.8f, 4.725f, 0));

        yield return new WaitForSeconds(Cooldown);
    }
}

void CreateAirStrikeBombBall(Vector3 position)
{
    IBall BombBall = GetSpecificBall("BombBall");
    BombBall.gameObject.SetActive(false);
    BombBall.StartPosition.x = Random.Range(position.x - 0.2f, position.x + 0.2f);
    BombBall.StartPosition.y = position.y;
    BombBall.linearVelocityYValue -= 1f;
    BombBall.gameObject.SetActive(true);
}

private IBall GetSpecificBall(string ballTypeName)
{
    IBall ball = null;

    Type ballType = System.Type.GetType(ballTypeName);
    var method = typeof(PoolManager).GetMethod("GetFromPool");
    var genericMethod = method.MakeGenericMethod(ballType);
    ball = (IBall)genericMethod.Invoke(BallManager.Instance.PoolManager, null);
    return ball;
}
}