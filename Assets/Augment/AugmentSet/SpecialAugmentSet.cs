using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using Unity.VisualScripting;
using System.Linq;
using Random = UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using UnityEngine.U2D;
using UnityEngine.UI;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
using static UnityEngine.Rendering.DebugUI;

/*
public class Example : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_N", "Example", "Example", "Example", 3);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public virtual void BeforeAttackEffect(IBrick Brick) { }
    
    public virtual void AfterAttackEffect(IBrick Brick) { }

    public virtual void CriticalAttackEffect(IBrick Brick) { }
}
 */

public class FireBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_1", "FireBall", "Bricks have a 30% chance to catch fire when struck by a ball", "Effect", 4);
        SetAdditionalDescription("Fire damage : 0.5 <sprite=2> per stack" +
            "The max stack is 5");
    }

    public override void Apply()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<FireEffectStackBlast>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<TheFlamethrower>());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<FireEffectStackBlast>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<TheFlamethrower>());
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.FireChance)
        {
            Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.FireDuration); 
        }
    }
}

public class Homing : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_2", "Magnetic Ball", "Ball chases bricks within range", "Special", 1);
        SetAdditionalDescription(null);
    }
    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class BouncePower: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_3", "Thrust", "<sprite=0> +2%  for each wall bounce up to a maximum of 10%", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple *= Brick.RecentlyBall.BounceTime;
        Brick.RecentlyBall.BounceTime = 1;
    }
}

public class AdditionalThreeAttack: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_4", "Afterimage", "Rapid three additional attacks follow after ball hits", "Special", 1);
        SetAdditionalDescription(
            "Additional attack : 0.25 <sprite=0>\n");
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.25f, 0.1f, 0.2f));
        }
    }
}

public class CriticalAdditionalAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_5", "Deadpoint", "Additional attack follows after critical attack", "Special", 4);
        SetAdditionalDescription(
            "Additional attack : 0.25 <sprite=0> \n");
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf) //Critical Additional Attack
        {
            StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.25f));
        }
    }
}

public class Execution : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_6", "Execution", "THe ball executes bricks with less than 15% of max HP", "Special", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply(){ }

    public override void Remove(){ }
}

public class PercentDamage : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_7", "Rupture", "The ball deals 1% of brick's HP with each attack", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf)
        { 
            Brick.ArbitraryDamageAttack(Brick.HpTemp * 0.01f, false, "1%", null);
        }
    }
}

public class FireEffectStackBlast : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_8", "Fire Burst", "An explosion occurs and spreads fire when the brick reaches maximum fire stack", "Effect", 3);
        SetAdditionalDescription(
            "Fire damage : 0.5 <sprite=2> per stack\n" +
            "Explosion damage: 3 <sprite=2>");
        if(!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<FireBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        
    }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float BlastDamage = (StatManager.instance.Persistent_stat.EffectDamage + StatManager.instance.Temporary_stat.EffectDamage) 
            * StatManager.instance.statMultiplier.EffectDamageMultiplier 
            * 3f;
        if (Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack == Brick.BrickEffect.EffectStatusSet["Fire"].EffectStackLimit)
        {
            Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack = (int)(Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack * 0.3);
            foreach (IBrick nearbyBrick in Brick.detectArea.NearbyBricks)
            {
                nearbyBrick.ArbitraryDamageAttack(BlastDamage
                    ,
                    false,
                    BlastDamage.ToString("F1"),
                    "Explosion");
                nearbyBrick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.FireDuration);
            }
            VisualEffectManager.Instance.SpawnParticle("FireBlast", Brick.transform.position, 30);
        }
    }
}

public class ChainLightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_9", "Chain Lightning", "Lightning chains to 3 nearby bricks", "Effect", 2);
        SetAdditionalDescription("Lightning damage: 0.2 <sprite=2> + 3% of brick's max HP");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<Lightning>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        List<IBrick> OrderedTargetBricks = new List<IBrick>();
        IBrick targetBrick = Brick;
        List<IBrick> NearbyBricksOfTargetBrick = targetBrick.detectArea.NearbyBricks;

        if (Brick.detectArea.NearbyBricks.Count == 0)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            NearbyBricksOfTargetBrick = targetBrick.detectArea.NearbyBricks;
            NearbyBricksOfTargetBrick.RemoveAll(item => OrderedTargetBricks.Contains(item));

            OrderedTargetBricks.Add(targetBrick);

            if (NearbyBricksOfTargetBrick.Count == 0)
            {
                break;
            }
            int RandomNumber = Random.Range(0, NearbyBricksOfTargetBrick.Count);
            targetBrick = NearbyBricksOfTargetBrick[RandomNumber];
        }
        ChainLightningDamageProcess(OrderedTargetBricks);
    }

    public void ChainLightningDamageProcess(List<IBrick> OrderedTargetBricks)
    {
        for(int i = 0; i < OrderedTargetBricks.Count; i++)
        {
            OrderedTargetBricks[i].DamageByLightningEffect(1, 0);
        }
        StartCoroutine(VisualEffectManager.Instance.SpawnChainLightning(OrderedTargetBricks));
    }
}

public class DeathExp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_10", "Death EXP", "Gain EXP when the ball drops", "Special", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect(IBall ball)
    {
        LevelManager.Instance.GetExp(15f);
    }
}

public class DeathGold : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_11", "Death Gold", "Gain Gold when the ball drops", "Special", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect(IBall ball)
    {
        StatManager.instance.GetGold(20);
    }
}

public class BigBoss : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_12", "Big Boss", "Summons a 'Big Boss' with extremely high HP\nDefeating it grants great rewards", "Special", 2);
        SetAdditionalDescription("Despite being called the Big Boss,\nit is not treated as an actual boss");
    }

    public override void Apply()
    {
        StartCoroutine(SettingBigBoss());
    }

    public override void Remove()
    {

    }

    IEnumerator SettingBigBoss()
    {
        if (BrickManager.Instance.availablePositions.Count > 0)
        {
            BrickManager.Instance.SpawnBrick(BrickManager.Instance.SettingBigBoss());
        }
        else
        {
            yield return new WaitUntil(() => BrickManager.Instance.BrickQueue != null);
            BrickManager.Instance.BrickQueue.Enqueue(BrickManager.Instance.SettingBigBoss());
        }
        StartCoroutine(BigBossAbility());
    }

    IEnumerator BigBossAbility()
    {
        yield return new WaitUntil(() => BrickManager.Instance.BigBossBrick != null);
        while (BrickManager.Instance.BigBossBrick != null)
        {
            yield return new WaitForSeconds(30f);
            BrickManager.Instance.TeleportBrickPosition(BrickManager.Instance.BigBossBrick);
        }
        AugmentManager.instance.AddAugment(gameObject.GetComponent<LevelUpAgain>());
        AugmentManager.instance.RemoveOnAugmentsSet(gameObject.GetComponent<LevelUpAgain>());
    }
}

public class ChangeBallSpawnPosition : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_13", "Unexpected Strategy", "A ball is summoned from top", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        BallManager.StartPosition = new Vector3(0f, 5f, 35);
        BallManager.linearVelocityYValue = -10;
    }

    public override void Remove()
    {
        BallManager.StartPosition = new Vector3(0f, -5f, 35);
        BallManager.linearVelocityYValue = 10;
    }
}

public class Lightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_14", "Lightning Attack", "An additional lightning strike occurs on attack", "Effect", 4);
        SetAdditionalDescription("Lightning damage : 0.2 <sprite=2> + 3% of brick's max HP");
    }

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<ThunderStrike>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<ChainLightning>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<LightningRod>());
    }

    public override void Remove() 
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<ThunderStrike>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<ChainLightning>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<LightningRod>());
    }

    public override void AfterAttackEffect(IBrick brick)
    {
        brick.DamageByLightningEffect(1f, 1);
    }
}

public class PoisonBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_15", "Poison Ball", "30% chance to poison on hit", "Effect", 4);
        SetAdditionalDescription("Poison damage : 0.5 <sprite=2> + 3% of brick's current HP per stack");
    }

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<DeadlyPoison>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<InfinitePoison>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<DecayingPoison>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<FlammablePoison>());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<DeadlyPoison>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<InfinitePoison>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<DecayingPoison>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<FlammablePoison>());
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.PoisonChance)
        {
            Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.PoisonDuration); 
        }
    }
}

public class InfinitePoison : SpecialAugment
{
    bool isGetAugment = false;
    Coroutine Routine = null;
    private void OnEnable()
    {
        SetAugment("SPECIAL_16", "Infinite Poison", "Poison spreads to nearby bricks, on max stacks", "Effect", 2);
        SetAdditionalDescription("Poison damage : 0.5 <sprite=2> + 3% of brick's current HP per stack");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<PoisonBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }
    public override void Apply()
    {
        isGetAugment = true;
        Routine = StartCoroutine(GetInfinitePoison());
    }

    public override void Remove() 
    {
        isGetAugment = false;
        StopCoroutine(Routine);
        Routine = null;
    }

    IEnumerator GetInfinitePoison()
    {
        while (true)
        {
            if(AugmentManager.instance.PoisonBrickSet.Count > 0)
            {
                foreach (IBrick brick in AugmentManager.instance.PoisonBrickSet)
                {
                    if (brick.detectArea.NearbyBricks.Count > 0 && Random.value < 0.25f)
                    {
                        List<IBrick> ChosenBricks = new List<IBrick>(brick.detectArea.NearbyBricks);
                        int infectionLimit = Random.Range(0, ChosenBricks.Count > 2 ? 2 : ChosenBricks.Count);
                        for (int i = 0; i < ChosenBricks.Count; i++)
                        {
                            int swapIndex = Random.Range(i, ChosenBricks.Count);
                            (ChosenBricks[i], ChosenBricks[swapIndex]) = (ChosenBricks[swapIndex], ChosenBricks[i]);
                        }

                        ChosenBricks = ChosenBricks.GetRange(0, infectionLimit);

                        foreach (IBrick Chosen in ChosenBricks)
                        {
                            if (Chosen.gameObject.activeInHierarchy && Chosen.Poison == null)
                            {
                                VisualEffectManager.Instance.SpawnParticle("InfinityPoison", transform.position, 1);
                                Chosen.ShowText("Infection!", null, false);
                                Chosen.DamageByPoisonEffect(AugmentManager.instance.effectDuration.PoisonDuration * StatManager.instance.Temporary_stat.EffectDuration);
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}

public class DeadlyPoison : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_17", "Deadly Poison", "+15% poison chance, make max poison stacks to 5.", "Effect", 3);
        SetAdditionalDescription(
            "Max stack up 3 to 5\n");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<PoisonBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.PoisonStackLimit = 5;
        AugmentManager.instance.effectChance.PoisonChance += 0.15f;
    }

    public override void Remove()
    {
        AugmentManager.instance.PoisonStackLimit = 3;
        AugmentManager.instance.effectChance.PoisonChance -= 0.15f;
    }
}
public class Chaos : SpecialAugment
{
    GameObject Curtain;
    List<float> TotalChangedStatValue;
    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "Chaos", "Random stat up or down\nThen, randomly teleport to a wave between the current wave +5 and +20", "Special", 1);
        SetAdditionalDescription(null);
        Curtain = GameObject.Find("Canvas").transform.Find("Curtain").gameObject;
        Curtain.SetActive(false);
        TotalChangedStatValue = new List<float>
        {
            0, 0, 0, 0, 0
        };
    }

    public override void Apply()
    {
        int RandomNumber = Random.Range(3, 5);
        for(int i = 0; i < RandomNumber; i++)
        {
            int RandomStat = Random.Range(1, 5);
            switch (RandomStat)
            {
                case 1:
                    float RandomAttack = (float)System.Math.Round(Random.Range(-1f, 4f), 2);
                    StatManager.instance.ChangeTemporaryStats_AttackDamage(RandomAttack);
                    TotalChangedStatValue[0] += RandomAttack;
                    break;
                case 2:
                    float RandomCritDamage = (float)System.Math.Round(Random.Range(-2f, 5f) / 100, 2) ;
                    StatManager.instance.ChangeTemporaryStats_CriticalDamage(RandomCritDamage);
                    TotalChangedStatValue[1] += RandomCritDamage;
                    break;
                case 3:
                    float RandomCritChance = (float)System.Math.Round(Random.Range(-1f, 8f) / 100, 2);
                    StatManager.instance.ChangeTemporaryStats_CriticalChance(RandomCritChance);
                    TotalChangedStatValue[2] += RandomCritChance;
                    break;
                case 4:
                    float RandomEffect = (float)System.Math.Round(Random.Range(-1f, 4f), 2);
                    StatManager.instance.ChangeTemporaryStats_EffectDamage(RandomEffect);
                    TotalChangedStatValue[3] += RandomEffect;
                    break;
                case 5:
                    float RandomEffectDuration = (float)System.Math.Round(Random.Range(-5f, 12f) / 100, 2);
                    StatManager.instance.ChangeTemporaryStats_EffectDuration(RandomEffectDuration);
                    TotalChangedStatValue[4] += RandomEffectDuration;
                    break;
                default:
                    break;
            }
        }
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => {
            Curtain.SetActive(true);
            GameManager.instance.OnChaos = true;
        });
        seq.Append(Curtain.GetComponent<Image>().DOFade(1f, 0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(Curtain.GetComponent<Image>().DOFade(0.4f, 0.4f));
        seq.AppendCallback(() => {
            BrickManager.Instance.WaveSkip(Random.Range(-5, 10));
        });
        seq.Append(Curtain.GetComponent<Image>().DOFade(0f, 0.6f));
        seq.AppendCallback(() => {
            Curtain.SetActive(false);
            GameManager.instance.OnChaos = false;
            BallManager.Instance.CreateBall(3);
        });
        
    }

    public override void Remove()
    {
        for(int i = 0; i < TotalChangedStatValue.Count; i++)
        {
            switch (i)
            {
                case 0:
                    StatManager.instance.ChangeTemporaryStats_AttackDamage(TotalChangedStatValue[0]);
                    break;
                case 1:
                    StatManager.instance.ChangeTemporaryStats_CriticalDamage(TotalChangedStatValue[1]);
                    break;
                case 2:
                    StatManager.instance.ChangeTemporaryStats_CriticalChance(TotalChangedStatValue[2]);
                    break;
                case 3:
                    StatManager.instance.ChangeTemporaryStats_EffectDamage(TotalChangedStatValue[3]);
                    break;
                case 4:
                    StatManager.instance.ChangeTemporaryStats_EffectDuration(TotalChangedStatValue[4]);
                    break;
                default:
                    break;
            }
        }
        TotalChangedStatValue = new List<float>
        {
            0, 0, 0, 0, 0
        };
    }
}
public class DecayBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "Decay Ball", "hance to decay on hit", "Effect", 4);
        SetAdditionalDescription("Decay Effect: Increases damage taken by bricks by 10% per stack");
    }

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<PowerfulDecay>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<DecayExplosion>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<DecayZone>());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<PowerfulDecay>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<DecayExplosion>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<DecayZone>());
    }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.DecayChance)
        {
            Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration * AugmentManager.instance.effectDuration.DecayDuration);
        }
    }
}
public class PowerfulDecay : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_19", "Powerful Decay", "Decay chance +15%, increase decay damage increase rate to 15%", "Effect", 3);
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<DecayBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.DecayDamageMultiple = 1.15f;
        AugmentManager.instance.effectChance.DecayChance -= 0.15f;
    }

    public override void Remove()
    {
        AugmentManager.instance.DecayDamageMultiple = 1.1f;
        AugmentManager.instance.effectChance.DecayChance -= 0.15f;
    }
}
public class DecayExplosion : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_20", "Decay Explosion", "Brick explodes when die with max decay stack", "Effect", 2);
        SetAdditionalDescription(
            "Explosion damage: 5% of brick's max health");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<DecayBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {

    }

    public override void Remove() 
    {

    }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if (Brick.Decay != null)
        {
            VisualEffectManager.Instance.DecayExplosionPS.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Brick.GetComponent<Renderer>().material.color);
            VisualEffectManager.Instance.SpawnParticle("DecayExplosion", Brick.transform.position, 1);
            float explosionRadius = 3f;
            LayerMask explosionLayer = LayerMask.GetMask("Ball");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Brick.transform.position, explosionRadius, explosionLayer);

            foreach (IBrick brick in Brick.detectArea.NearbyBricks)
            {
                brick.ArbitraryDamageAttack(Brick.HpTemp * 0.05f, false, (Brick.HpTemp * 0.05f).ToString("F1"), "Explosion");
            }
            foreach (Collider2D ball in colliders)
            {
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 explosionDirection = (rb.position - (Vector2)Brick.transform.position).normalized;
                    float distance = Vector2.Distance(rb.position, Brick.transform.position);
                    float forceMultiplier = Mathf.Clamp01(1 - (distance / explosionRadius));

                    rb.AddForce(explosionDirection * 15f * forceMultiplier, ForceMode2D.Impulse);
                }
            }
        }
    }
}

public class ExplosionWithBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_21", "Elasticity", "Balls bounce further when they collide with each other", "Special", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply() { }

    public override void Remove() { }
}

public class TheFlamethrower : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_22", "Fire Aura", "Generates a fire aura around the balls that deals damage and may ignite bricks", "Effect", 2);
        SetAdditionalDescription("Aura damage : 0.2 <sprite=2> per stack\n" +
            "Fire damage : 0.5 <sprite=2> per stack");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<FireBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }
    

    public override void Apply() 
    {
        foreach (IBall ball in BallManager.Instance.InGameBall)
        {
            ball.flamethrower.FlamethrowerOn();
        }
        foreach (IBall ball in BallManager.Instance.InGameSpecialBall)
        {
            ball.flamethrower.FlamethrowerOn();
        }

        BallManager.Instance.isOnFlamerower = true;
    }

    public override void Remove() 
    {
        foreach (IBall ball in BallManager.Instance.InGameBall)
        {
            ball.flamethrower.FlamethrowerOff();
        }
        foreach (IBall ball in BallManager.Instance.InGameSpecialBall)
        {
            ball.flamethrower.FlamethrowerOff();
        }

        BallManager.Instance.isOnFlamerower = false;
    }
}

public class LevelUpQuest : SpecialAugment
{
    private int KilledEnemy = 0;
    private bool isCleared = false;
    private int ClearRequiredCount = 100;
    private void OnEnable()
    {
        SetAugment("SPECIAL_23", "Slayer", "Receive 50% EXP and a quest. Complete the quest to level up", "Quest", 5);
        SetAdditionalDescription("Quest: Destroy 50 bricks");
    }

    public override void Apply()
    {
        LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.5f;

        QuestAlarm.Instance.SetQuest("Slayer", "Destroyed", 0, ClearRequiredCount);
    }

    public override void Remove(){ }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        KilledEnemy++;
        if(KilledEnemy <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Slayer", "Destroyed", KilledEnemy, ClearRequiredCount);

        if (KilledEnemy >= ClearRequiredCount && !isCleared)
        {
            isCleared = true;
            LevelManager.Instance.LevelUp();
        }
    }
}
public class GrowBallQuest : SpecialAugment
{
    private int KilledEnemy;
    private float KeepAtk;
    private bool isCleared = false;
    int ClearRequiredCount = 100;
    private void OnEnable()
    {
        SetAugment("SPECIAL_24", "Grow Ball", "Accept a grow ball quest.\nComplete it to attack damage up", "Quest", 5);
        SetAdditionalDescription("Quest: Destroy 100 bricks\n" +
            "<sprite=0> +0.02 per destroyed bricks\n" +
            "<sprite=0> +0.04 per destroyed boss bricks");
    }

    public override void Apply()
    {
        QuestAlarm.Instance.SetQuest("Grow Ball", "Destroyed", KilledEnemy, ClearRequiredCount);
    }

    public override void Remove() 
    {
        if (isCleared)
        {
            isCleared = false;
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-(KeepAtk + 1));
        }
    }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        KilledEnemy++;
        if (KilledEnemy <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Grow Ball", "Destroyed", KilledEnemy, ClearRequiredCount);
        if (Brick.RecentlyBall != null && !isCleared)
        {
            QuestProgressing(Brick);
            if(KilledEnemy >= ClearRequiredCount)
            {
                StatManager.instance.ChangeTemporaryStats_AttackDamage(KeepAtk + 1);
                isCleared = true;
                StartCoroutine(GrowBall());
            }
        }
    }

    IEnumerator GrowBall()
    {
        float elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 0, elapsedTime / 0.25f));
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(0, 1, elapsedTime / 0.25f));
            yield return null;
        }
    }

    public void QuestProgressing(IBrick brick)
    {
        if (brick.IsBoss)
        {
            KeepAtk += 0.2f;
        }
        else
        {
            KeepAtk += 0.1f;
        }
    }
}

public class InfiniteGrowBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_25", "Infinite Grow Ball", "The ball's attack power increases after destroying bricks", "Special", 3);
        SetAdditionalDescription("When a ball dies, its increased stats are reset" +
            "<sprite=0> +0.1 per destroyed bricks\n" +
            "<sprite=0> +0.2 per destroyed boss bricks");
    }

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<ShareBallAtk>());
    }

    public override void Remove() 
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<ShareBallAtk>());
    }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if(Brick.RecentlyBall != null)
            Brick.RecentlyBall.Grow(Brick);
    }
}
public class SacrificeQuest1 : SpecialAugment
{
    private int SacrificeNumber = 0;
    int ClearRequiredCount = 40;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_26", "Sacrifice I", "Accept a sacrifice I quest.\nComplete it to Receive a weak reward", "Quest", 5);
        SetAdditionalDescription("Quest: Sacrifice 40 balls for <sprite=0> +1, <sprite=2> +1");
    }

    public override void Apply() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice I", "Sacrificed", SacrificeNumber, ClearRequiredCount); 
    }

    public override void Remove() 
    {    
        if (isCleared) 
        {
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-1.5f);
            StatManager.instance.ChangeTemporaryStats_EffectDamage(-1.5f);
        }
        else
        {
            AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<SacrificeQuest2>());
        }
    }

    public override void AfterDeathEffect(IBall ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Sacrifice I", "Sacrificed", SacrificeNumber, ClearRequiredCount);
        if (SacrificeNumber >= ClearRequiredCount && !isCleared)
        {
            StatManager.instance.ChangeTemporaryStats_AttackDamage(1.5f);
            StatManager.instance.ChangeTemporaryStats_EffectDamage(1.5f);

            AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<SacrificeQuest2>());
            isCleared = true;
        }
    }
}
public class SacrificeQuest2 : SpecialAugment
{
    private int SacrificeNumber = 0;
    private bool isCleared;
    int ClearRequiredCount = 70;
    private void OnEnable()
    {
        SetAugment("SPECIAL_27", "Sacrifice II", "Accept a sacrifice II quest.\nComplete it to Receive a moderate reward", "Quest", 3);
        SetAdditionalDescription("Quest: sacrifice 70 balls for <sprite=0> +15%, <sprite=2> +15%, 500 Gold");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<SacrificeQuest1>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice II", "Sacrificed", SacrificeNumber, ClearRequiredCount);
    }

    public override void Remove() 
    {
        if (isCleared)
        {
            StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.17f);
            StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.17f);
        }
    }

    public override void AfterDeathEffect(IBall ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Sacrifice II", "Sacrificed", SacrificeNumber, ClearRequiredCount);
        if (SacrificeNumber >= ClearRequiredCount && !isCleared)
        {
            StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.17f);
            StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.17f);
            StatManager.instance.GetGold(500);

            AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<SacrificeQuest3>());
            isCleared = true;
        }
    }
}
public class SacrificeQuest3 : SpecialAugment
{
    private int SacrificeNumber = 0;
    int ClearRequiredCount = 100;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_28", "Sacrifice III", "Accept a sacrifice III quest.\nComplete it to Receive a powerful reward", "Quest", 2);
        SetAdditionalDescription("Quest: sacrifice 100 balls for secret reward");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<SacrificeQuest2>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sacrifice III", "Sacrificed", SacrificeNumber, ClearRequiredCount); }

    public override void Remove() 
    {
        if (!isCleared)
        {
            AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<SacrificeQuest3>());
        }
    }

    public override void AfterDeathEffect(IBall ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Sacrifice III", "Sacrificed", SacrificeNumber, ClearRequiredCount);
        if (SacrificeNumber >= 100 && !isCleared)
        {
            AugmentManager.instance.isSacrificeQuest3Cleared = true;
            isCleared = true;
        }
    }
}
public class AttackAfterAdditionalBall : SpecialAugment
{
    private int attackNumber = 0;
    private int RequiredCount = 100;
    private int AttackNumber
    {
        get
        {
            return attackNumber;
        }
        set
        {
            attackNumber = value;
            if(attackNumber >= RequiredCount)
            {
                BallManager.Instance.CreateBall(1);
                attackNumber = 0;
            }
        }
    }
    private void OnEnable()
    {
        SetAugment("SPECIAL_29", "Sincerity", "A ball is created every 100 hits", "Special", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sincerity", "Attack", AttackNumber, RequiredCount); }

    public override void Remove() { }

    public override void AfterAttackEffect(IBrick Brick)
    {
        AttackNumber++;
        QuestAlarm.Instance.SetQuest("Sincerity", "Attack", AttackNumber, RequiredCount);
    }
}
public class ShareBallAtk : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_30", "Inheritance", "Fallen ball's 70% attack power\ndivided among current balls", "Special", 5);
        SetAdditionalDescription(null);
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<InfiniteGrowBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(IBall ball)
    {
        float ShareAtk = ball.GrowthAttackDamage *= 0.7f;
        foreach (IBall inGameBall in BallManager.Instance.InGameBall)
        {
            inGameBall.GrowthAttackDamage += ShareAtk / BallManager.Instance.InGameBall.Count;
        }
    }
}
public class StrongAdditionalAttack : SpecialAugment
{
    float StrongAttackChance = 0.01f;
    float SuperStrongAttackChance = 0.001f;
    float SuperUltraStrongAttackChance = 0.0001f;
    private void OnEnable()
    {
        SetAugment("SPECIAL_31", "One in a thousand", "A powerful additional attack triggers at low chance", "Special", 3);
        SetAdditionalDescription("Strong additional attack : 10 <sprite=0> for 1% chance\n" +
            "It seems there are more powerful attacks...");
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float RandomValue = Random.value;
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            if (RandomValue < SuperUltraStrongAttackChance)
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 500f, true));
            else if (RandomValue < SuperStrongAttackChance)
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 50f, true));
            else if (RandomValue < StrongAttackChance)
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 10f));
        }
    }
}

public class RandomEffect : SpecialAugment
{
    private Dictionary<string, float> ProbabilitySet = new Dictionary<string, float>()
    {
        { "WeakAdditionalDamage", 0.5f },
        { "Nothing", 0.3f },
        { "AdditionalDamage",  0.1f},
        { "RandomEffect", 0.04f },
        { "FullHealth", 0.02f },
        { "BrickSpawn", 0.02f },
        //{ "BossSpawn", 0.01f },
        { "Execution", 0.01f },
    };


    private void OnEnable()
    {
        SetAugment("SPECIAL_32", "Random Effect Attack", "A random effect triggers on attack\nit may be positive or negative", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {

    float RandomValue = Random.value;
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            float accumulatedProbability = 0;

            accumulatedProbability += ProbabilitySet["WeakAdditionalDamage"];
            if (RandomValue < accumulatedProbability)
            {
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.5f));
                return;
            }

            accumulatedProbability += ProbabilitySet["AdditionalDamage"];
            if (RandomValue < accumulatedProbability)
            {
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.8f));
                return;
            }

            accumulatedProbability += ProbabilitySet["Nothing"];
            if (RandomValue < accumulatedProbability)
            {
                return;
            }

            accumulatedProbability += ProbabilitySet["RandomEffect"];
            if (RandomValue < accumulatedProbability)
            {
                int Number = Random.Range(1, 4);
                if (Number == 1)
                {
                    Brick.BrickEffect.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration);
                }
                else if (Number == 2)
                {
                    Brick.BrickEffect.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration);
                }
                else if (Number == 3)
                {
                    Brick.BrickEffect.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration);
                }
                return;
            }

            accumulatedProbability += ProbabilitySet["FullHealth"];
            if (RandomValue < accumulatedProbability)
            {
                Brick.HP = Brick.HpTemp;
                return;
            }

            accumulatedProbability += ProbabilitySet["BrickSpawn"];
            if (RandomValue < accumulatedProbability)
            {
                BrickManager.Instance.BrickQueue.Enqueue(BrickManager.Instance.SettingBrick());
                return;
            }

            accumulatedProbability += ProbabilitySet["BossSpawn"];
            if (RandomValue < accumulatedProbability)
            {
                BrickManager.Instance.BrickQueue.Enqueue(BrickManager.Instance.SettingBoss());
                return;
            }

            accumulatedProbability += ProbabilitySet["Execution"];
            if (RandomValue < accumulatedProbability)
            {
                Brick.HP -= 999999 + Brick.HpTemp;
                Brick.ShowText("EXECUTION", null, true);
            }
        }
    }
}

public class ThunderStrike : SpecialAugment
{
    Coroutine routine = null;
    float Cooldown = AugmentManager.instance.FieldGlobalCooldown / 2;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 3;
    private void OnEnable()
    {
        SetAugment("SPECIAL_33", "Arc Link", "All balls are linked by electricity for " + Duration + "seconds", "Special", 1, Cooldown);
        SetAdditionalDescription("Lightning damage : 0.2 <sprite=2> + 3% of brick's max HP");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<Lightning>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() 
    {
        routine = StartCoroutine(MakeBallThunder());
    }

    public override void Remove()
    {
        StopCoroutine(routine);
    }

    private IEnumerator MakeBallThunder()
    {
        Vector2 start;
        Vector2 end;

        RaycastHit2D[] hits;

        while (true)
        {
            float elapsed = 0;
            while(elapsed < Duration)
            {
                List<IBall> Balls = new List<IBall>();

                foreach (IBall ball in BallManager.Instance.InGameBall)
                {
                    if (!Balls.Contains(ball))
                        Balls.Add(ball);
                }
                foreach (IBall ball in BallManager.Instance.InGameSpecialBall)
                {
                    if (!Balls.Contains(ball))
                        Balls.Add(ball);
                }

                if (Balls.Count >= 2)
                {
                    VisualEffectManager.Instance.SpawnThunderStrike(Balls);

                    for (int i = 0; i < Balls.Count - 1; i++)
                    {
                        start = Balls[i].transform.position;
                        end = Balls[i + 1].transform.position;

                        hits = Physics2D.LinecastAll(start, end);
                        for (int j = 0; j < hits.Length; j++)
                            if (hits[j].collider.CompareTag("Brick"))
                            {
                                IBrick brick = hits[j].collider.GetComponent<IBrick>();

                                brick.DamageByLightningEffect(10, 0);
                            }
                    }
                    start = Balls[Balls.Count - 1].transform.position;
                    end = Balls[0].transform.position;

                    hits = Physics2D.LinecastAll(start, end);
                    for (int j = 0; j < hits.Length; j++)
                        if (hits[j].collider.CompareTag("Brick"))
                        {
                            IBrick brick = hits[j].collider.GetComponent<IBrick>();

                            brick.DamageByLightningEffect(10, 0);
                        }
                }
                yield return new WaitForSeconds(0.1f);
                elapsed += 0.1f;
            }
            yield return new WaitForSeconds(Cooldown);
        }
    }
}
public class RebirthBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_34", "Immortality", "Adds a 5% chance to revive after the ball died", "Special", 5);
        SetAdditionalDescription("Excluding certain balls");
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(IBall ball)
    {
        if (Random.value < 0.05f)
        {
            BallManager.Instance.CreateSpecificBall(1, ball.BallType, true);
        }
    }

    
}

public class BossKillBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_35", "Uprising", "Summons a special brick when the boss is defeated", "Special", 5);
        SetAdditionalDescription(null);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            BrickManager.Instance.OnSpecialBrickSpawnButtonClick();
        }
    }
}

public class SpawnBallAmountMultiple1 : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_36", "2x!", "All summoned balls are doubled", "Special", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply() 
    {
        AugmentManager.instance.SpawnBallAmount = 2;
    }

    public override void Remove()
    {
        AugmentManager.instance.SpawnBallAmount = 1;
    }
}

public class SpawnBallAmountMultiple2 : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_37", "3x!!", "All summoned balls are tripled", "Special", 0);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        AugmentManager.instance.SpawnBallAmount = 3;
    }

    public override void Remove()
    {
        AugmentManager.instance.SpawnBallAmount = 1;
    }
}
public class BallSwarm : SpecialAugment
{
    bool isGetAugment = false;
    bool Upgrade = false;

    private void OnEnable()
    {
        SetAugment("SPECIAL_38", "Ball Swarm", "<sprite=0> +2 \nwhen 15 or more balls are summoned", "Special", 4);
        SetAdditionalDescription("Extra balls are stored and released one by one when the count falls below 15");
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<LonelyBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        isGetAugment = true;

        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<LonelyBall>());
        if (BallManager.Instance.InGameBall.Count > 15)
        {
            for (int i = 0; i < BallManager.Instance.InGameBall.Count - 15; i++)
            {
                BallManager.Instance.LeftBallCount++;
                BallManager.Instance.TakeBall(BallManager.Instance.InGameBall[i]);
            }
        }
    }

    public override void Remove()
    {
        isGetAugment = false;

        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<LonelyBall>());
        if (Upgrade)
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if (BallManager.Instance.InGameBall.Count <= 15)
            {
                int ballsToAdd = Mathf.Min(15 - BallManager.Instance.InGameBall.Count, BallManager.Instance.LeftBallCount);
                CreateBall(ballsToAdd);
                BallManager.Instance.LeftBallCount -= ballsToAdd;
            }

            if (BallManager.Instance.InGameBall.Count >= 15 && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(2f);
                StatManager.instance.ShowChangedStatRoutine = StartCoroutine(StatManager.instance.ShowChangedStat());
                StatManager.instance.isChangedStat = false;
            }
            if (BallManager.Instance.InGameBall.Count < 15 && BallManager.Instance.LeftBallCount == 0 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
                StatManager.instance.ShowChangedStatRoutine = StartCoroutine(StatManager.instance.ShowChangedStat());
                StatManager.instance.isChangedStat = false;
            }
        }
    }

    public void CreateBall(int num)
    {
        for (int i = 0; i < num; i++)
        {
            IBall ball = GetSpecificBall(BallManager.Instance.NormalBall);
            if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 0.1f)
            {
                IBall BombBall = GetSpecificBall("BombBall");
            }
        }
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
public class LonelyBall : SpecialAugment
{
    bool isGetAugment = false;
    bool Upgrade = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_39", "Lonely Ball", "<sprite=0> +2 \nwhen 5 or less balls are summoned", "Special", 4);
        SetAdditionalDescription("Extra balls are stored\n" +
            "and released one by one when the count falls below 5");
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<BallSwarm>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        isGetAugment = true;

        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<BallSwarm>());
        if (BallManager.Instance.InGameBall.Count > 5)
        {
            for (int i = 0; i < BallManager.Instance.InGameBall.Count - 5; i++)
            {
                BallManager.Instance.LeftBallCount++;
                BallManager.Instance.TakeBall(BallManager.Instance.InGameBall[i]);
            }
        }
    }

    public override void Remove()
    {
        isGetAugment = false;

        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<BallSwarm>());
        if (Upgrade)
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if(BallManager.Instance.InGameBall.Count <= 5)
            {
                int ballsToAdd = Mathf.Min(5 - BallManager.Instance.InGameBall.Count, BallManager.Instance.LeftBallCount);
                CreateBall(ballsToAdd);
                BallManager.Instance.LeftBallCount -= ballsToAdd;
            }

            if (BallManager.Instance.InGameBall.Count <= 5 && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(2f);
                StatManager.instance.ShowChangedStatRoutine = StartCoroutine(StatManager.instance.ShowChangedStat());
                StatManager.instance.isChangedStat = false;
                //BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(2, 2, 2);
            }
            if (BallManager.Instance.InGameBall.Count > 5 && BallManager.Instance.LeftBallCount == 0 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
                StatManager.instance.ShowChangedStatRoutine = StartCoroutine(StatManager.instance.ShowChangedStat());
                StatManager.instance.isChangedStat = false;
                //BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(1, 1, 1);
            }
        }
    }

    public void CreateBall(int num)
    {
        for(int i = 0; i < num; i++)
        {
            IBall ball = GetSpecificBall(BallManager.Instance.NormalBall);
            if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 0.1f)
            {
                IBall BombBall = GetSpecificBall("BombBall");
            }
        }
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
public class SuicideBoomBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_40", "Potential Bomb", "A low chance for the ball to explode after an attack", "Special", 1);
        SetAdditionalDescription("Explosion damage: 3 <sprite=2>\n" +
            "After explosion, the ball dies but has a high chance to revive");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<SpawnBombBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        IBall ball = Brick.RecentlyBall;
        if(Random.value < 0.02f && !BallManager.Instance.InGameSpecialBall.Contains(ball))
        {
            Brick.ExplosionAttack(3f);
            VisualEffectManager.Instance.SpawnParticle("SuicideBoomBall", Brick.transform.position, 1);
            foreach (IBrick brick in Brick.detectArea.NearbyBricks)
            {
                brick.ExplosionAttack(3f);
            }
        }
    }
}
public class SplitBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_41", "Diffraction", "A low chance for the ball to split after an attack", "Special", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if(Random.value < 0.04f)
        {
            IBall RecentlyBall = Brick.RecentlyBall;
            StartCoroutine(SplitBallEffect(RecentlyBall));
            CreateBall(AugmentManager.instance.SpawnBallAmount, RecentlyBall);
        }
    }

    IEnumerator SplitBallEffect(IBall RecentlyBall)
    {
        float elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            RecentlyBall.Renderer.material.color = new Color(Mathf.Lerp(1, 0, elapsedTime / 0.25f), Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f), Mathf.Lerp(1, 1.5f, elapsedTime / 0.25f));
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            RecentlyBall.Renderer.material.color = new Color(Mathf.Lerp(0, 1, elapsedTime / 0.25f), Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f), Mathf.Lerp(1.5f, 1, elapsedTime / 0.25f));
            yield return null;
        }
    }

    public void CreateBall(int Num, IBall RecentlyBall)
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_50"))
        {
            StatManager.instance.ChangeTemporaryStats_AttackDamage(Num * 0.2f);
            StatManager.instance.ChangeTemporaryStats_EffectDamage(Num * 0.2f);
        }
        else if (AugmentManager.instance.determineAugmentID("SPECIAL_38"))
        {
            BallManager.Instance.LeftBallCount += Num;
        }
        else if (AugmentManager.instance.determineAugmentID("SPECIAL_39"))
        {
            BallManager.Instance.LeftBallCount += Num;
        }
        else
        {
            for (int i = 0; i < Num; i++)
            {
                IBall ball = GetSpecificBall(RecentlyBall.BallType);
                if (AugmentManager.instance.determineAugmentID("SPECIAL_46") && Random.value < 0.1f)
                {
                    IBall BombBall = GetSpecificBall("BombBall");
                }
                ball.gameObject.SetActive(false);
                ball.StartPosition = RecentlyBall.gameObject.transform.position;
                ball.linearVelocityYValue = Random.Range(-5f, 5f);
                if (ball.linearVelocityYValue >= 0)
                {
                    ball.linearVelocityYValue += 3f;
                }
                else
                {
                    ball.linearVelocityYValue -= 3f;
                }
                ball.gameObject.SetActive(true);
                StartCoroutine(SplitBallEffect(ball));
            }
        }     
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
public class CurseBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_42", "Curse Ball", "When the ball falls, \n30% chance of curse damage to a random block", "Effect", 4);
        SetAdditionalDescription("Curse damage : 1.5 <sprite=0>");
    }

    public override void Apply()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<SummonGhostBall>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<EnhanceCurseAttack>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<ContinuingCurseAttack>());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<SummonGhostBall>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<EnhanceCurseAttack>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<ContinuingCurseAttack>());
    }

    public override void AfterDeathEffect(IBall ball)
    {
        if (Random.value < AugmentManager.instance.effectChance.CurseChance && BrickManager.Instance.CurrentBricks.Count > 0)
        {
            BrickManager.Instance.GetRandomBrick().DamageByCurse();
        }
    }
}
public class SummonGhostBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_43", "Revenant", "When the ball falls, \nSummon ghost ball at low chance", "Effect", 3);
        SetAdditionalDescription("Summoned ghost balls do not collide with each balls \n" +
            "and disappear after 8 seconds\n" +
            "When a Ghost Ball is disappear, Curse Attack activates with a 100% chance");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<CurseBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() 
    {
        
    }

    public override void Remove()
    {
        
    }

    public override void AfterDeathEffect(IBall ball)
    {
        if(Random.value < 0.15f)
            BallManager.Instance.CreateGhostBall(1, ball);
    }
}
public class EnhanceCurseAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_44", "Endless Revenge", "Curse damage increases by 1.5% whenever the ball dies", "Effect", 2);
        SetAdditionalDescription("30% of the balls that already died will be counted");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<CurseBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() 
    {
        AugmentManager.instance.CurseDamageValue += (GameManager.instance.DiedBalls * 0.3f) * 0.01f;
    }

    public override void Remove() 
    {
        AugmentManager.instance.CurseDamageValue = 1f;
    }

    public override void AfterDeathEffect(IBall ball)
    {
        AugmentManager.instance.CurseDamageValue += 0.01f;
    }
}
public class DeathEffectTwice : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_45", "Die Again?", "All actions performed after the ball's death are doubled", "Special", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        AugmentManager.instance.AfterDeathActiveNumber = 2;
    }

    public override void Remove()
    {
        AugmentManager.instance.AfterDeathActiveNumber = 1;
    }
}

public class SpawnBombBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_46", "Bomb Maniac", "Low chance of bomb ball on summon", "Special", 3);
        SetAdditionalDescription("The summoned bomb ball explodes after 3 seconds\n" +
            "Explosion damage: 10 <sprite=2>");
    }

    public override void Apply()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<RebornBombBall>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<SuicideBoomBall>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<Bombardment>());
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<InstantBombBall>());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<RebornBombBall>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<SuicideBoomBall>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<Bombardment>());
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<InstantBombBall>());
    }
}
public class ChangeNormalToGlass : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_47", "Glassification", "The basic ball becomes a glass ball", "Special", 4);
        SetAdditionalDescription("<sprite=0> +2");
    }

    public override void Apply()
    {
        BallManager.Instance.NormalBall = "GlassBall";
    }

    public override void Remove()
    {
        BallManager.Instance.NormalBall = "Ball";
    }
}
public class RebornBombBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_48", "Bomb Modification", "Bomb ball revive a low chance after explode", "Special", 4);
        SetAdditionalDescription(null);
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<SpawnBombBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply() { }

    public override void Remove() { }
}
public class OneMoreChance : SpecialAugment
{
    bool isRevived = true;
    private void OnEnable()
    {
        SetAugment("SPECIAL_49", "One More Chance", "If no balls exist,\nsummon 5 balls at once", "Special", 4);
    }

    public override void Apply() 
    {
        isRevived = false;
    }

    public override void Remove() 
    {
        isRevived = true;
    }

    public void Update()
    {
        if (!isRevived && BallManager.Instance.InGameBall.Count <= 0)
        {
            BallManager.Instance.CreateBall(5);
            isRevived = true;
        }
    }
}

public class Concentrate : SpecialAugment
{
    int MaxCount = 10;
    int Count = 10;
    int DestroyedBall = 0;
    private void OnEnable()
    {
        SetAugment("SPECIAL_50", "Concentrate", "Stats increase massively, \nbut 10 ball fall, it's game over", "Special", 0);
        SetAdditionalDescription("Attack, Effect Damage +300%\n" +
            "Balls are limited to 10\n" +
            "Excess balls are destroyed\n" +
            "If fewer than 10 balls, generate until 10\n" +
            "Balls generated from now on convert to 0.2 Attack, Effect Damage");
    }

    public override void Apply()
    {
        QuestAlarm.Instance.SetQuest("Concentrate", "GameOver In",  Count, 0);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(3f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(3f);
        DestroyedBall = BallManager.Instance.InGameBall.Count - 10 <= 0 ? 0 : BallManager.Instance.InGameBall.Count - 10;
        BallManager.Instance.DestroyBallNum(DestroyedBall, true);
        CreateBall(10 - BallManager.Instance.InGameBall.Count <= 0 ? 0 : 10 - BallManager.Instance.InGameBall.Count);
        StatManager.instance.ChangeTemporaryStats_AttackDamage(DestroyedBall * 0.2f);
        StatManager.instance.ChangeTemporaryStats_EffectDamage(DestroyedBall * 0.2f);
    }

    public override void Remove() 
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-3f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-3f);

        StatManager.instance.ChangeTemporaryStats_AttackDamage(-(DestroyedBall * 0.2f));
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-(DestroyedBall * 0.2f));
        DestroyedBall = 0;
        MaxCount = 10;
        Count = 10;
    }

    public override void AfterDeathEffect(IBall ball)
    {
        Count--;
        QuestAlarm.Instance.SetQuest("Concentrate", "GameOver In",  Count, 0);

        if (Count <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void CreateBall(int num)
    {
        for (int i = 0; i < num; i++)
        {
            IBall ball = GetSpecificBall(BallManager.Instance.NormalBall);
        }
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
public class GroupViolence: SpecialAugment
{
    bool isGetAugment = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_51", "Group Violence", "After attack, transfers 10% of Attack damage to the brick with the highest HP", "Special", 3);
        SetAdditionalDescription("Applies only to normal attacks and specific effects attacks");
    }

    public override void Apply()
    {
        isGetAugment = true;
    }

    public override void Remove() 
    {
        isGetAugment = false;
    }

    public void Update()
    {
        if (isGetAugment && BrickManager.Instance.CurrentBricks.Count > 0)
        {
            float maxHp = BrickManager.Instance.CurrentBricks.Max(b => b.HP);
            BrickManager.Instance.MainBrick = BrickManager.Instance.CurrentBricks.FirstOrDefault(b => b.HP == maxHp && b.BrickType != "IgnoreBrick");
        }
    }

    public override void AfterAttackEffect(IBrick Brick)
    {
        if(Brick != BrickManager.Instance.MainBrick)
            DamageManager.ArbitraryDamageAttack
                (BrickManager.Instance.MainBrick, StatManager.AttackDamage * 0.1f, false, (StatManager.AttackDamage * 0.1f).ToString("F1"), "Additional");
    }
}
public class Oneness: SpecialAugment
{
    IBall MainBall = null;
    bool isGetAugment = false;
    Coroutine routine;
    private void OnEnable()
    {
        SetAugment("SPECIAL_52", "Oneness", "All balls merge into one,\nthe ball becomes stronger as it merges", "Special", 2);
        SetAdditionalDescription(null);
    }

    private void Update()
    {
        if(isGetAugment == true && MainBall == null)
            StartCoroutine(WaitforMainBall());
        else if(isGetAugment)
        {
            for (int i = 0; i < BallManager.Instance.InGameBall.Count; i++)
            {
                if (BallManager.Instance.InGameBall[i] != MainBall && BallManager.Instance.InGameBall[i].FusionMoveRoutine == null)
                    BallManager.Instance.InGameBall[i].FusionMoveRoutine = StartCoroutine(MoveToPosition(BallManager.Instance.InGameBall[i], MainBall, 5f, 5f));
            }
        }
    }

    public override void Apply() 
    {
        IBall MainBall = null;
        isGetAugment = true;
        routine = StartCoroutine(WaitforMainBall());
    }

    public override void Remove()
    {
        IBall MainBall = null;
        isGetAugment = false;
        StopCoroutine(routine);
    }

    IEnumerator OnenessBallEffect(IBall RecentlyBall)
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

    IEnumerator WaitforMainBall()
    {
        while(MainBall == null)
        {
            for (int i = 0; i < BallManager.Instance.InGameBall.Count; i++)
            {
                IBall ball = BallManager.Instance.InGameBall[i];
                if (ball.transform.position.y > -1 && ball.rb.linearVelocityY > 0)
                {
                    MainBall = ball;
                }
            }
            yield return null;
        }
    }

    IEnumerator MoveToPosition(IBall obj, IBall target, float duration, float smoothTime)
    {
        yield return new WaitForSeconds(0.5f);
        obj.gameObject.GetComponent<Collider2D>().enabled = false;
        while (Vector3.Distance(obj.transform.position, target.transform.position) > 0.35f)
        {
            if (target != null)
            {
                Vector3 direction = (target.transform.position - obj.transform.position).normalized;
                obj.gameObject.GetComponent<Rigidbody2D>().linearVelocity = direction * 7.5f;
            }
            else
            {
                break;
            }
            yield return null;
        }
        StartCoroutine(OnenessBallEffect(MainBall));
        MainBall.GrowthAttackDamage += ((StatManager.instance.Persistent_stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage) 
            * StatManager.instance.statMultiplier.AttackDamageMultiplier)
            * 0.3f;
        MainBall.transform.localScale *= 1.001f;
        if (obj.AttackOrDieRoutine != null)
        {
            obj.FusionMoveRoutine = null;
        }
        if (obj.gameObject.activeInHierarchy)
        {
            obj.BallDied();
        }
        yield break;
    }
}
public class HappyBirthday : SpecialAugment
{
    float Duration = StatManager.instance.Temporary_stat.EffectDuration;
    private void OnEnable()
    {
        SetAugment("SPECIAL_53", "Happy Birth Day", "Gain 5 attack damage when ball is summoned for " + Duration + " seconds", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply() {    }

    public override void Remove() {    }

    public override void AfterBirthEffect(IBall ball)
    {
        ball.StartCoroutine(AtkUp(ball));
    }
    IEnumerator AtkUp(IBall ball)
    {
        yield return null;
        ball.BallBaseDamage += 5;
        ball.Renderer.material.color = Color.white * 2;
        yield return new WaitForSeconds(Duration);
        ball.Renderer.material.color = Color.white;
        ball.BallBaseDamage -= 5;
    }
}

public class MidasHand : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_54", "Midas Hand", "Attacked bricks have a low chance to\ntransform into golden blocks", "Special", 4);
        SetAdditionalDescription(null);
    }
    Coroutine Routine = null;
    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if(Random.value < 0.5f 
            && Brick.BrickType != "TwinBrick" && Brick.BrickType != "ShieldBrick" 
            && Brick.BrickType != "TeleportBrick" && Brick.BrickType != "BlackBrick"
            && Brick.BrickType != "IgnoreBrick" && Routine == null)
        {
            Routine = Brick.StartCoroutine(ChangeToGoldBrick(Brick));
        }
    }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if(Brick.isGoldBrick)
        {
            StatManager.instance.GetGold(Random.Range(1, 10));
        }
    }

    IEnumerator ChangeToGoldBrick(IBrick Brick)
    {
        SpriteRenderer BrickRenderer = Brick.gameObject.GetComponent<SpriteRenderer>();
        Color PreviousColor = BrickRenderer.color;

        yield return null;
        Brick.isGoldBrick = true;
        BrickRenderer.color = new Color(1f, 0.843f, 0f);

        float elapsedTime = 0f;
        while(elapsedTime < 0.25f)
        {
            BrickRenderer.color = new Color(Mathf.Lerp(PreviousColor.r, 2, elapsedTime / 0.25f), Mathf.Lerp(PreviousColor.g, 0.843f * 2, elapsedTime / 0.25f), Mathf.Lerp(PreviousColor.b, 0, elapsedTime / 0.25f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(4.75f);
        elapsedTime = 0;
        while (elapsedTime < 0.25f)
        {
            BrickRenderer.color = new Color(Mathf.Lerp(2, PreviousColor.r, elapsedTime / 0.25f), Mathf.Lerp(0.843f * 2, PreviousColor.g, elapsedTime / 0.25f), Mathf.Lerp(0, PreviousColor.b, elapsedTime / 0.25f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Brick.isGoldBrick = false;
        BrickRenderer.color = PreviousColor;
        Routine = null;
    }
}

public class HalfBallSacrifice : SpecialAugment
{
    float AtkUpValue = 0;
    private void OnEnable()
    {
        SetAugment("SPECIAL_55", "Mass Sacrifice", "Sacrifice half of the balls,\nand increse corresponding attack damage", "Special", 4);
        SetAdditionalDescription("Increases by 0.1 for each ball");
    }

    public override void Apply() 
    {
        if(BallManager.Instance.InGameBall.Count > 1)
        {
            for(int i = 0; i < BallManager.Instance.InGameBall.Count / 2; i++)
            {
                StartCoroutine(BallRemoveEffect(BallManager.Instance.InGameBall[i]));
                AtkUpValue += 0.2f;
            }
            StatManager.instance.ChangeTemporaryStats_AttackDamage(AtkUpValue);
        }
    }

    private IEnumerator BallRemoveEffect(IBall ball)
    {
        VisualEffectManager.Instance.SpawnParticle("BallSacrifice", ball.transform.position, 30);
        yield return null;
        ball.BallDied();
    }

    public override void Remove() 
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-AtkUpValue);
    }
}

public class Chronobreak : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_54", "Chronobreak", "lose half of augment, Attack and Effect Damage\nThen, return to Wave 1", "Special", 0);
        SetAdditionalDescription("The minimum wave to return to is Wave 1\n" +
            "All bricks are destroyed\n" +
            "All balls are destroyed, then 10 balls are summoned \n" +
            "Certain bricks and balls are excluded");
    }

    public override void Apply()
    {
        BrickManager.Instance.WaveSkip(-(GameManager.instance.Wave + 2));
        for (int i = 0; i < AugmentManager.instance.activeAugments.Count / 2; i++)
        {
            int RandomIndex = Random.Range(0, AugmentManager.instance.activeAugments.Count);
            AugmentManager.instance.RemoveAugment(AugmentManager.instance.activeAugments[RandomIndex]);

        }
        BallManager.Instance.DestroyBallNum(BallManager.Instance.InGameBall.Count);
        BrickManager.Instance.DestroyBricksNum(BrickManager.Instance.CurrentBricks.Count);
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-(StatManager.AttackDamage / 2));
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-(StatManager.EffectDamage / 2));
        BallManager.Instance.CreateBall(10);
    }

    public override void Remove()
    {

    }
}

public class DecayingPoison: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_55", "Decaying Poison", "When poisoned, decay is also applied", "Effect", 3);
        SetAdditionalDescription("Decay effect : increases all damage taken by stacks \n" +
            "Poison damage : deals damage scaling with brick's current health, <sprite=2>, and stacks");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<PoisonBall>()) ||
            !AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<FlammablePoison>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<FlammablePoison>());
        AugmentManager.instance.effectDuration.FireDuration += 1f;
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<FlammablePoison>());
        AugmentManager.instance.effectDuration.FireDuration -= 1f;
    }
}

public class FlammablePoison: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_56", "Flammable Poison", "When poisoned, fire is also applied", "Effect", 3);
        SetAdditionalDescription("Fire damage : deals damage scaling with <sprite=2> and stacks\n" +
            "Poison damage : deals damage scaling with brick's current health, <sprite=2>, and stacks");
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<PoisonBall>()) ||
            !AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<DecayingPoison>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<DecayingPoison>());
        AugmentManager.instance.effectDuration.DecayDuration += 2f;
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<DecayingPoison>());
        AugmentManager.instance.effectDuration.DecayDuration -= 2f;
    }
}

public class CritAfterThreeAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_57", "Weakness Detection", "The fourth attack always triggers a critical hit", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        
    }

    public override void Remove()
    {
        
    }


    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.RecentlyBall.AttackNumber++;
    }
}

public class DamageInfection : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_58", "Miraculous Technique", "Attacks nearby bricks,\nIt count as direct hits by the ball", "Special", 0);
        SetAdditionalDescription("Applies all effects triggered by direct hits");
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if(Brick.detectArea.NearbyBricks.Count > 0)
        {
            List<IBrick> ChosenBricks = new List<IBrick>(Brick.detectArea.NearbyBricks);
            int infectionLimit = Random.Range(1, ChosenBricks.Count > 5 ? 5 : ChosenBricks.Count);
            for (int i = 0; i < ChosenBricks.Count; i++)
            {
                int swapIndex = Random.Range(i, ChosenBricks.Count);
                (ChosenBricks[i], ChosenBricks[swapIndex]) = (ChosenBricks[swapIndex], ChosenBricks[i]);
            }

            ChosenBricks = ChosenBricks.GetRange(0, infectionLimit);

            foreach (IBrick brick in ChosenBricks)
            {
                DamageManager.TakeDamageWithoutMainAttack(brick, Brick.RecentlyBall);
            }
        }
    }
}

public class CriticalOnEffect : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_59", "Effect Catalyst", "Effect attacks can deal critical hits", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class RollDiceBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_60", "Roll the Dice!", "Roll dice instead of summoning balls.\nsummon as many balls as the dice show.", "Special", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class PortalBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_61", "Portal Ball", "Hits may cause the ball to \nteleport to the random brick's position", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {
    }

    public override void AfterAttackEffect(IBrick Brick)
    {
        if (Brick.RecentlyBall != null && Brick.RecentlyBall.gameObject.activeSelf && BrickManager.Instance.CurrentBricks.Count > 1
            && 0.05f > Random.value)
        {
            IBrick RandomBrick = BrickManager.Instance.CurrentBricks[Random.Range(0, BrickManager.Instance.CurrentBricks.Count)];
            if (Brick != RandomBrick)
                Brick.RecentlyBall.transform.position = RandomBrick.transform.position;
        }
    }
}

public class InfiniteRapid : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_62", "Rapidfire", "Effect damage +1.8, Effect duration +15%,\nAll waves appear as transformed rapid waves", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(1.8f);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(0.15f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_EffectDamage(-1.8f);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-0.15f);
    }
}

public class MoreAndWeakBricks : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_63", "Legion", "Every Bricks appear 50% more frequently, \nbut their health is reduced by 33%", "Special", 3);
        SetAdditionalDescription("Special bricks are excluded");
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<LessAndStrongBricks>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }
    
    public override void Apply()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1.5f;
        BrickManager.Instance.BaseBrickHealthValue = 0.66f;
        BrickManager.Instance.BaseBossHealthValue = 0.66f;
        BrickManager.Instance.BaseWaitingTimeValue = 6;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 0.5f).ToList();
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<LessAndStrongBricks>());
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1f;
        BrickManager.Instance.BaseBrickHealthValue = 1f;
        BrickManager.Instance.BaseBossHealthValue = 1f;
        BrickManager.Instance.BaseWaitingTimeValue = 5;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 2f).ToList();
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<LessAndStrongBricks>());
    }
}

public class LessAndStrongBricks : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_63", "Elite Group", "Every Bricks appear 50% less frequently, \nbut their health is increased by 33%.", "Special", 3);
        SetAdditionalDescription("Special bricks are excluded");
        if (AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<MoreAndWeakBricks>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 0.5f;
        BrickManager.Instance.BaseBrickHealthValue = 1.33f;
        BrickManager.Instance.BaseBossHealthValue = 1.33f;
        BrickManager.Instance.BaseWaitingTimeValue = 4;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 2f).ToList();
        AugmentManager.instance.UnlockAugment(AugmentManager.AugmentSet.GetComponent<LessAndStrongBricks>());
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1f;
        BrickManager.Instance.BaseBrickHealthValue = 1f;
        BrickManager.Instance.BaseBossHealthValue = 1f;
        BrickManager.Instance.BaseWaitingTimeValue = 5;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 0.5f).ToList();
        AugmentManager.instance.LockAugment(AugmentManager.AugmentSet.GetComponent<LessAndStrongBricks>());
    }
}

public class DoubleBoss : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_64", "Twins!", "The boss splits into two, \neach having 66% the original health.", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        BrickManager.Instance.BaseBossSpawnValue = 2;
        BrickManager.Instance.BaseBossHealthValue = 0.66f;
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBossSpawnValue = 1;
        BrickManager.Instance.BaseBossHealthValue = 1f;
    }
}

public class FusionBricks : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_65", "Only One", "Waves are replaced by one brick with combined health.", "Special", 2);
        SetAdditionalDescription("It is treated as a boss");
    }

    public override void Apply()
    {
        BrickManager.Instance.BaseSpawnTimeValue = 0.4f;

        BrickManager.Instance.SpecialBrickSpawnChance = 0;
        BrickManager.Instance.SpecialBrickSpawnChanceValue = 0f;

        BrickManager.Instance.RapidStageChance = 0;
        BrickManager.Instance.RapidStageChanceValue = 0f;
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseSpawnTimeValue = 1f;
        BrickManager.Instance.SpecialBrickSpawnChanceValue = 0.1f;
        BrickManager.Instance.RapidStageChanceValue = 0.09f;
    }
}

public class GainBallAfterLevelUp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_66", "Promotion Gift", "After level up, roll a dice \nand gain that many balls", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void AfterLevelUpEffect()
    {
        DiceManager.Instance.StartCoroutine(DiceManager.Instance.RollDices(AugmentManager.instance.SpawnBallAmount, BallManager.Instance.OnCreateBall));
    }
}

public class SpecialBrickChanceUp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_67", "Lucky Day", "Special bricks spawn more frequently.", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        BrickManager.Instance.SpecialBrickSpawnChanceValue = 1.75f;
    }

    public override void Remove()
    {
        BrickManager.Instance.SpecialBrickSpawnChanceValue = 1f;
    }
}

public class IncreseStatUpValue : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_68", "Stick to the Basics", "Gain 24% More When Selecting Stats After Level Up.", "Special", 1);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        LevelManager.Instance.BaseStatUpValue = 1.24f;
    }

    public override void Remove()
    {
        LevelManager.Instance.BaseStatUpValue = 1f;
    }
}

public class GoalWaveBonus : SpecialAugment
{
    bool isGetAugment;
    bool[] Goals;
    private void OnEnable()
    {
        SetAugment("SPECIAL_69", "Milestone", "Receive Rewards Upon Reaching Certain Waves.", "Special", 3);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        isGetAugment = true;
        Goals = new bool[6];
    }

    public override void Remove()
    {
        isGetAugment = false;
        for(int i = 0; i < Goals.Length; i++)
        {
            if (Goals[i])
            {
                switch (i)
                {
                    case 2:
                        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.05f);
                        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.05f);
                        break;
                    case 4:
                        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.05f);
                        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.05f);
                        break;
                    case 5:
                        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.1f);
                        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.1f);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if(GameManager.instance.Wave == 5 && !Goals[0])
            {
                Goals[0] = true;
                for (int i = 0; i < 3; i++)
                {
                    LevelManager.Instance.GetExp(20);
                }
            }
            else if(GameManager.instance.Wave == 10 && !Goals[1])
            {
                Goals[1] = true;
                for (int i = 0; i < 5; i++)
                {
                    BallManager.Instance.CreateBall(5);
                }
            }
            else if (GameManager.instance.Wave == 30 && !Goals[2])
            {
                Goals[2] = true;
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.05f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.05f);
            }
            else if (GameManager.instance.Wave == 50 && !Goals[3])
            {
                Goals[3] = true;
                for (int i = 0; i < 5; i++)
                {
                    LevelManager.Instance.GetExp(50);
                }
            }
            else if (GameManager.instance.Wave == 70 && !Goals[4])
            {
                Goals[4] = true;
                for (int i = 0; i < 5; i++)
                {
                    BallManager.Instance.CreateBall(5);
                }
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.05f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.05f);
            }
            else if (GameManager.instance.Wave == 100 && !Goals[5])
            {
                Goals[5] = true;
                for (int i = 0; i < 20; i++)
                {
                    LevelManager.Instance.GetExp(20);
                }
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.1f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.1f);
            }
        }
    }
}

public class ReducesExp10: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_70", "Brilliant Idea I", "Reduces exp required to level up by 10%", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        LevelManager.Instance.BaseMaxExpValue *= 0.9f;
        LevelManager.Instance.LevelBarMaxValueSet(LevelManager.Instance.Level);
    }

    public override void Remove()
    {
        LevelManager.Instance.BaseMaxExpValue /= 0.9f;
        LevelManager.Instance.LevelBarMaxValueSet(LevelManager.Instance.Level);
    }
}

public class ReducesExp15 : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_71", "Brilliant Idea II", "Reduces exp required to level up by 15%", "Special", 2);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        LevelManager.Instance.BaseMaxExpValue *= 0.85f;
        LevelManager.Instance.LevelBarMaxValueSet(LevelManager.Instance.Level);
    }

    public override void Remove()
    {
        LevelManager.Instance.BaseMaxExpValue /= 0.85f;
        LevelManager.Instance.LevelBarMaxValueSet(LevelManager.Instance.Level);
    }
}

public class WaveFourBoss : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_72", "Curse of the Number 4", "All stats 16% Up\nEvery 4 waves, a Rapid Wave activates and a enhanced boss appears", "Special", 1);
        SetAdditionalDescription("Except gold gain stat");
    }

    public override void Apply()
    {
        BrickManager.Instance.BossSpawnWaveNumber = 4;
        BrickManager.Instance.NextBossSpawnWave = GameManager.instance.Wave + BrickManager.Instance.BossSpawnWaveNumber;
        BrickManager.Instance.BaseBossHealthValue *= 1.44f;

        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.16f);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(0.16f);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(0.16f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.16f);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(0.16f);
        StatManager.instance.ChangeTemporaryStats_ExpGain(0.16f);
    }

    public override void Remove()
    {
        BrickManager.Instance.BossSpawnWaveNumber = 10;
        BrickManager.Instance.NextBossSpawnWave = ((BrickManager.Instance.NextBossSpawnWave / 10) + 1) * 10;
        BrickManager.Instance.BaseBossHealthValue /= 1.44f;

        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.16f);
        StatManager.instance.ChangeTemporaryStats_CriticalDamage(-0.16f);
        StatManager.instance.ChangeTemporaryStats_CriticalChance(-0.16f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.16f);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-0.16f);
        StatManager.instance.ChangeTemporaryStats_ExpGain(-0.16f);
    }
}

public class RerollAugments : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_73", "Reroll", "Refreshes the currently available augments", "Special", 5);
        SetAdditionalDescription("This augment may appear again after being selected\n" +
            "and it won't appear again right away after a refresh");
    }

    public override void Apply()
    {
        LevelManager.Instance.isThereRerolled = true;
        LevelManager.Instance.RerollAugment = this;
    }

    public override void Remove()
    {
        
    }
}

public class BossDamageUp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_74", "Boss Killer", "+10% Additional damage to boss", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            Brick.TotalMultiple *= 1.1f;
        }
    }
}

public class ContinuingCurseAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_75", "Possession", "Transforms Curse Damage into a damage-over-time attack", "Special", 2);
        SetAdditionalDescription(null);
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<CurseBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class LevelUpAgain : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_76", "Blessing", "Level up once more after level up", "Special", 6);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Add(gameObject.AddComponent<LevelUpAgain>());
    }

    public override void AfterLevelUpEffect()
    {
        if (!AugmentManager.instance.isLevelUpOnceAgain)
        {
            AugmentManager.instance.isLevelUpOnceAgain = true;
            LevelManager.Instance.LevelUp();
        }
    }
}

public class AtkUpWhenCritical : SpecialAugment
{
    Coroutine Routine;
    int setTime = 3;

    private void OnEnable()
    {
        SetAugment("SPECIAL_77", "Lucky Streak", "When critical attack hits, attack damage +1.8\nfor 3 seconds", "Special", 4);
        SetAdditionalDescription(null);
    }

    public override void Apply()
    {
        Routine = null;
    }

    public override void Remove()
    {

    }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Routine == null)
        {
            Routine = StartCoroutine(AttackDamageUpFewSec(1.8f));
        }
        else
        {
            setTime = 3;
        }
    }

    IEnumerator AttackDamageUpFewSec(float AttackUpValue)
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(AttackUpValue);
        while (setTime > 0)
        {
            yield return new WaitForSeconds(1);
            setTime--;
        }
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-AttackUpValue);
        setTime = 3;
        Routine = null;
    }
}

public class InstantBombBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_78", "Instant Bomb", "Bomb ball is changed to explode immediately after attacking the brick", "Special", 4);
        SetAdditionalDescription(null);
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.AugmentSet.GetComponent<SpawnBombBall>()))
            AugmentManager.instance.exceptionAugments.Add(this);
    }

    public override void Apply()
    {
        
    }

    public override void Remove()
    {

    }
}