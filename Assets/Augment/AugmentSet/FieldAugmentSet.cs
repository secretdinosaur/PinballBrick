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
using System.Linq;

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
    public override string ID => "FIELD_1";
    public override string Name => "Holy Rain";
    public override string Description => "The holy rain damages all bricks";
    public override int Rarity => 3;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => "Damage: 5 + <sprite=2>";
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("HolyRain", StartAllDamage(Cooldown));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("HolyRain"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("HolyRain");
        }
    }

    private IEnumerator StartAllDamage(float period)
    {
        while (true)
        {
            for (int i = 0; i < BrickManager.Instance.CurrentBricks.Count; i++)
            {
                BrickManager.Instance.CurrentBricks[i].HolyRainAttack();
            }
            yield return new WaitForSeconds(period);
        }
    }
}

public class AngryBall : FieldAugment
{
    public override string ID => "FIELD_2";
    public override string Name => "Angry Ball";
    public override string Description => "Balls briefly become enraged and move faster";
    public override int Rarity => 3;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Buff";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown / 10 * 3;
    float Power = 10f;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("AngryBall", StartAngryBall(Cooldown, Power));
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<TargetMark>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<TargetMark>().FirstOrDefault());
        if (AugmentManager.instance.AugmentRoutines.hasKey("AngryBall"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("AngryBall");
        }
    }

    private IEnumerator StartAngryBall(float period, float Power)
    {
        while(true)
        {
            StatManager.instance.ChangeTemporaryStats_AttackDamage(1.4f);
            foreach (Ball ball in BallManager.Instance.InGameBall)
            {
                ball.StartCoroutine(AngryBallEffect(ball));
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * Power, ForceMode2D.Impulse);
            }
            foreach (Ball ball in BallManager.Instance.InGameSpecialBall)
            {
                ball.StartCoroutine(AngryBallEffect(ball));
                Rigidbody2D rb = ball.gameObject.GetComponent<Rigidbody2D>();
                Vector3 forceDirection = rb.linearVelocity.normalized;
                rb.AddForce(forceDirection * Power, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-1.4f);
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
    public override string ID => "FIELD_3";
    public override string Name => "Phoenix Ball";
    public override string Description => "The random ball become invincible for " + Duration.ToString("F1") + " seconds";
    public override int Rarity => 4;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Buff";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 2f;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("PhoenixBall", ChoosingPhoenixBall(Duration, Cooldown));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("PhoenixBall"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("PhoenixBall");
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
    public override string ID => "FIELD_4";
    public override string Name => "Focused Attack";
    public override string Description => "All balls target a random brick";
    public override int Rarity => 2;
    public override float CoolTime => 10;
    public override string Type => "Field";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    Coroutine Routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    GameObject Target;
    Material TargetMat;

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<AngryBall>().FirstOrDefault());
        Target = GameObject.Find("Target");
        TargetMat = Target.GetComponent<Renderer>().material;
        Target.SetActive(false);
        AugmentManager.instance.AugmentRoutines.Add("TargetMark", TargetMarking(Cooldown));
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<AngryBall>().FirstOrDefault());
        if (AugmentManager.instance.AugmentRoutines.hasKey("TargetMark"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("TargetMark");
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
                AugmentManager.instance.StartCoroutine(FindTarget());

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
    public override string ID => "FIELD_5";
    public override string Name => "Satellite Beam";
    public override string Description => "Generates a powerful satellite beam for " + Duration.ToString("F1") + "seconds";
    public override int Rarity => 1;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => "On hit damage: 10 <sprite=2>\nContinuous damage: <sprite=2>";
    public override bool isFollowUpAugment => false;

    Coroutine Routine;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 6;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;

    public override void Apply()
    {
        Routine = AugmentManager.instance.AugmentRoutines.Add("SatelliteBeam", FireSatelliteBeam(Cooldown, Duration));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("SatelliteBeam"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("SatelliteBeam");
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
    public override string ID => "FIELD_6";
    public override string Name => "Meditation";
    public override string Description => "Automatically gain 1% of the max EXP";
    public override int Rarity => 5;
    public override float CoolTime => 10;
    public override string Type => "Buff";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown + 5;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("AutoGetExp", GetExp(Cooldown));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("AutoGetExp"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("AutoGetExp");
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
    public override string ID => "FIELD_7";
    public override string Name => "Antigravity";
    public override string Description => "Reverses gravity for " + Duration.ToString("F1") + " seconds";
    public override int Rarity => 3;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    GameObject AntiGravityObj;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 5;

    public override void Apply()
    {
        AntiGravityObj = GameObject.Find("AntiGravity");
        AugmentManager.instance.AugmentRoutines.Add("AntiGravity", GetAntiGravity(Cooldown, Duration));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("AntiGravity"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("AntiGravity");
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
            
            AugmentManager.instance.StartCoroutine(AntiGravityEffect(false));
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

            AugmentManager.instance.StartCoroutine(AntiGravityEffect(true));
            yield return new WaitForSeconds(Duration);

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
    public override string ID => "FIELD_8";
    public override string Name => "Bounty Hunter";
    public override string Description => "Random brick is wanted for " + Duration.ToString("F1") + " seconds \nrewards are earned when breaking it";
    public override int Rarity => 4;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => "Kill wanted brick and get EXP and gold";
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;
    GameObject BountyHunterObj;
    Material BountyHunterMat;

    public override void Apply()
    {
        BountyHunterObj = GameObject.Find("BountyHunter");
        BountyHunterMat = BountyHunterObj.GetComponent<Renderer>().material;
        BountyHunterObj.SetActive(false);
        AugmentManager.instance.AugmentRoutines.Add("BountyHunter", GetBountyHunter(Cooldown, Duration));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("BountyHunter"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("BountyHunter");
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
                        
                    ItemManager.Instance.SpawnSpecificItem("ExpBead", Position, WantedBrick.IsBoss ? Random.Range(30, 50) : Random.Range(10, 15), 
                        (GainValue) => WantedBrick.ShowText("+" + GainValue.ToString("F1"), "Exp", false));
                    ItemManager.Instance.SpawnSpecificItem("Gold1", Position, WantedBrick.IsBoss ? Random.Range(10, 15) : Random.Range(3, 5),
                        (GainValue) => WantedBrick.ShowText("+" + GainValue.ToString("F1"), "Gold", false));
                    ItemManager.Instance.SpawnSpecificItem("Gold2", Position, WantedBrick.IsBoss ? Random.Range(10, 15) : Random.Range(3, 5),
                        (GainValue) => WantedBrick.ShowText("+" + GainValue.ToString("F1"), "Gold", false));
                    ItemManager.Instance.SpawnSpecificItem("Gold3", Position, WantedBrick.IsBoss ? Random.Range(10, 15) : Random.Range(3, 5),
                        (GainValue) => WantedBrick.ShowText("+" + GainValue.ToString("F1"), "Gold", false));
                    BountyHunterObj.SetActive(false);
                }
                else
                {
                    WantedBrick.HP += WantedBrick.HpTemp * 0.1f;
                    WantedBrick.HP = Mathf.Clamp(WantedBrick.HP, 1f, WantedBrick.HpTemp);
                }
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}

public class LightningRod: FieldAugment
{
    public override string ID => "FIELD_9";
    public override string Name => "Thunderstrike Zone";
    public override string Description => "Generates thunderstrike zone for " + Duration.ToString("F1") + " seconds";
    public override int Rarity => 2;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Lightning";
    public override string AdditionalDescription => "Lightning damage: 0.2 <sprite=2> + 3% of brick's max HP";
    public override bool isFollowUpAugment => true;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 20;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("LightningRod", GetLightningRod(Cooldown, Duration));
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<Lightning>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<LightningRod>().FirstOrDefault());
        if (AugmentManager.instance.AugmentRoutines.hasKey("LightningRod"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("LightningRod");
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
    public override string ID => "FIELD_10";
    public override string Name => "Last Breath";
    public override string Description => "<sprite=0> +30%,\nballs get die if they don't hit any bricks within " + Duration.ToString("F1") + " seconds.";
    public override int Rarity => 2;
    public override string Type => "Buff";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Duration = 8;

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.3f);
        AugmentManager.instance.AugmentRoutines.Add("AttackOrDie", GetAttackOrDie());
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.3f);
        if (AugmentManager.instance.AugmentRoutines.hasKey("AttackOrDie"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("AttackOrDie");
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
                        BallManager.Instance.InGameBall[i].AttackOrDieRoutine = AugmentManager.instance.StartCoroutine(GetDying(BallManager.Instance.InGameBall[i]));
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
    public override string ID => "FIELD_11";
    public override string Name => "Decay Zone";
    public override string Description => "Generate a decay zone for " + Duration.ToString("F1") + " seconds";
    public override int Rarity => 2;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Decay";
    public override string AdditionalDescription => "Decay Effect: Increases damage taken by bricks by 10% per stack";
    public override bool isFollowUpAugment => true;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 10;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("DecayZone", GetDecayZone(Cooldown));
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayZone>().FirstOrDefault());
        if (AugmentManager.instance.AugmentRoutines.hasKey("DecayZone"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("DecayZone");
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
    public override string ID => "FIELD_12";
    public override string Name => "Constant Disaster";
    public override string Description => "Random disaster occured for " + Duration.ToString("F1") + " seconds";
    public override int Rarity => 1;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => "Fire damage : 0.5 <sprite=2> per stack\n" +
            "Poison damage : 0.5 <sprite=2> + 3% of brick's current HP per stack\n" +
            "Decay Effect: Increases damage taken by bricks by 10% per stack \n" +
            "Curse damage : 1.5 <sprite=0>\n" +
            "Lightning damage : 0.2 <sprite=2> + 3% of brick's max HP";
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 3;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 10;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("WeatherChange", GetWeatherChange(Cooldown));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("WeatherChange"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("WeatherChange");
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
                    while (elapsedTime < Duration)
                    {
                        for (int j = 0; j < BrickManager.Instance.CurrentBricks.Count; j++)
                        {
                            if (Random.value < 0.3f)
                                BrickManager.Instance.CurrentBricks[j].HolyRainAttack();
                        }
                        elapsedTime += 1;
                        yield return new WaitForSeconds(1f);
                    }
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
    public override string ID => "FIELD_13";
    public override string Name => "Dice Gamble";
    public override string Description => "Roll the dice and get rewards based on the result";
    public override int Rarity => 2;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 6;
        

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("DiceGamble", GetDiceGamble(Cooldown));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("DiceGamble"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("DiceGamble");
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
                        brick.ExecutionCor = AugmentManager.instance.StartCoroutine(brick.Execution());
                    }
                    break;
                case 2:
                    ItemManager.Instance.SpawnSpecificItemInRandomPosition("ExpBead",200, 0.1f);
                    ItemManager.Instance.SpawnSpecificItemInRandomPosition("Gold1", 50, 0.1f);
                    ItemManager.Instance.SpawnSpecificItemInRandomPosition("Gold2", 50, 0.1f);
                    ItemManager.Instance.SpawnSpecificItemInRandomPosition("Gold3", 50, 0.1f);
                    break;
                case 3:
                    BallManager.Instance.CreateBombBall(10);
                    BallManager.Instance.CreateBall(50);
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
                        BallManager.Instance.CreateBall(DiceSum / 3);
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
                    BallManager.Instance.CreateBombBall(DiceSum / 3);
                    break;
                case 2:
                    BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
                    BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
                    BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
                break;
                case 3:
                        LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * DiceSum * 0.02f;
                        break;
                default:
                    break;
            }
        }
    }
}

public class Trampoline : FieldAugment
{
    public override string ID => "FIELD_14";
    public override string Name => "Trampoline";
    public override string Description => "Spawn rectangle trampoline for " + Duration.ToString("F1") + " seconds";
    public override int Rarity => 2;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Field";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 3;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 10;
    GameObject TrampolineObj;

    public override void Apply()
    {
        TrampolineObj = GameObject.Find("BangBang");
        TrampolineObj.transform.position = new Vector2(-0.75f, -0.55f);
        TrampolineObj.SetActive(false);

        AugmentManager.instance.AugmentRoutines.Add("Trampoline", StartTrampoline(Cooldown));
    }

    public override void Remove()
    {
        if (AugmentManager.instance.AugmentRoutines.hasKey("Trampoline"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("Trampoline");
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
    public override string ID => "FIELD_15";
    public override string Name => "Air Strike";
    public override string Description => "Summons 5 bombs that fall from above";
    public override int Rarity => 2;
    public override float CoolTime => (float)System.Math.Round(Cooldown, 2);
    public override string Type => "Bomb";
    public override string AdditionalDescription => "Bomb damage: 10 <sprite=0>";
    public override bool isFollowUpAugment => true;

    float Cooldown = AugmentManager.instance.FieldGlobalCooldown * 1.5f;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("Bombardment", StartAirStrike(Cooldown));
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<SpawnBombBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<Bombardment>().FirstOrDefault());
        if (AugmentManager.instance.AugmentRoutines.hasKey("Bombardment"))
        {
            AugmentManager.instance.AugmentRoutines.Stop("Bombardment");
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
        BombBall.linearVelocityYValue = -1f;
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