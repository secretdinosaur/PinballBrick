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
    public override string ID => "SPECIAL_1";
    public override string Name => "FireBall";
    public override string Description => "Bricks have a 30% chance to catch fire when struck by a ball";
    public override int Rarity => 4;
    public override string Type => "Fire";
    public override string AdditionalDescription => "Fire damage : <sprite=2> 50% per stack";
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<FireEffectStackBlast>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<TheFlamethrower>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<Pyromaniac>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<FireEffectStackBlast>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<TheFlamethrower>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<Pyromaniac>().FirstOrDefault());
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
    public override string ID => "SPECIAL_2";
    public override string Name => "Magnetic Ball";
    public override string Description => "Ball chases bricks within range";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class BouncePower: SpecialAugment
{
    public override string ID => "SPECIAL_3";
    public override string Name => "Thrust";
    public override string Description => "<sprite=0> +2%  for each wall bounce up to a maximum of 10%";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_4";
    public override string Name => "Afterimage";
    public override string Description => "Rapid three additional attacks follow after ball hits";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => "Additional attack : <sprite=0> 25%";
    public override bool isFollowUpAugment => false;

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.25f, 0.1f, 0.2f));
        }
    }
}

public class CriticalAdditionalAttack : SpecialAugment
{
    public override string ID => "SPECIAL_5";
    public override string Name => "Deadpoint";
    public override string Description => "Additional attack follows after critical attack";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => "Additional attack : <sprite=0> 25%";
    public override bool isFollowUpAugment => false;
    public override void Apply(){  }

    public override void Remove(){ }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf) //Critical Additional Attack
        {
            BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.25f));
        }
    }
}

public class Execution : SpecialAugment
{
    public override string ID => "SPECIAL_6";
    public override string Name => "Execution";
    public override string Description => "The ball executes bricks with less than 15% of max HP";
    public override int Rarity => 2;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
    public override void Apply(){ }

    public override void Remove(){ }
}

public class EffectChanceUp : SpecialAugment
{
    public override string ID => "SPECIAL_7";
    public override string Name => "Catalyst";
    public override string Description => "All effect hit chance +7%";
    public override int Rarity => 3;
    public override string Type => "Effect";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.effectChance.FireChance += 0.07f;
        AugmentManager.instance.effectChance.PoisonChance += 0.07f;
        AugmentManager.instance.effectChance.DecayChance += 0.07f;
        AugmentManager.instance.effectChance.CurseChance += 0.07f;
        AugmentManager.instance.effectChance.LightningChance += 0.07f;
    }

    public override void Remove()
    {
        AugmentManager.instance.effectChance.FireChance -= 0.07f;
        AugmentManager.instance.effectChance.PoisonChance -= 0.07f;
        AugmentManager.instance.effectChance.DecayChance -= 0.07f;
        AugmentManager.instance.effectChance.CurseChance -= 0.07f;
        AugmentManager.instance.effectChance.LightningChance -= 0.07f;
    }
}

public class FireEffectStackBlast : SpecialAugment
{
    public override string ID => "SPECIAL_8";
    public override string Name => "Fire Burst";
    public override string Description => "An explosion occurs and spreads fire when the brick reaches maximum fire stack";
    public override int Rarity => 3;
    public override string Type => "Fire";
    public override string AdditionalDescription => "Fire damage : <sprite=2> 50% per stack\n" +
                                    "Explosion damage: <sprite=2> 300%";
    public override bool isFollowUpAugment => true;
    public override void Apply()
    {
        
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<FireBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<FireEffectStackBlast>().FirstOrDefault());
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float BlastDamage = (StatManager.instance.Persistent_stat.EffectDamage + StatManager.instance.Temporary_stat.EffectDamage) 
            * StatManager.instance.statMultiplier.EffectDamageMultiplier 
            * 3f;
        if (Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack == Brick.BrickEffect.EffectStatusSet["Fire"].EffectStackLimit)
        {
            SoundManager.instance.ExplosionSound_Big();

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
    public override string ID => "SPECIAL_9";
    public override string Name => "Chain Lightning";
    public override string Description => "Lightning chains to 3 nearby bricks";
    public override int Rarity => 2;
    public override string Type => "Lightning";
    public override string AdditionalDescription => "Lightning damage: <sprite=2> 20% + 3% of brick's max HP";
    public override bool isFollowUpAugment => true;

    public override void Apply(){ }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<Lightning>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ChainLightning>().FirstOrDefault());
    }

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
        BrickManager.Instance.StartCoroutine(VisualEffectManager.Instance.SpawnChainLightning(OrderedTargetBricks));
    }
}

public class DeathExp : SpecialAugment
{
    public override string ID => "SPECIAL_10";
    public override string Name => "Death EXP";
    public override string Description => "Gain EXP when the ball drops";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect(IBall ball)
    {
        LevelManager.Instance.GetExp(15f);
    }
}

public class DeathGold : SpecialAugment
{
    public override string ID => "SPECIAL_11";
    public override string Name => "Death Gold";
    public override string Description => "Gain Gold when the ball drops";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect(IBall ball)
    {
        StatManager.instance.GetGold(20);
    }
}

public class BigBoss : SpecialAugment
{
    public override string ID => "SPECIAL_12";
    public override string Name => "Big Boss";
    public override string Description => "Summons a 'Big Boss' with extremely high HP\nDefeating it grants great rewards";
    public override int Rarity => 2;
    public override string Type => "Special";
    public override string AdditionalDescription => "Despite being called the Big Boss,\nit is not treated as an actual boss";
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        BrickManager.Instance.StartCoroutine(SettingBigBoss());
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
        BrickManager.Instance.StartCoroutine(BigBossAbility());
    }

    IEnumerator BigBossAbility()
    {
        yield return new WaitUntil(() => BrickManager.Instance.BigBossBrick != null);
        while (BrickManager.Instance.BigBossBrick != null)
        {
            yield return new WaitForSeconds(30f);
            BrickManager.Instance.TeleportBrickPosition(BrickManager.Instance.BigBossBrick);
        }
    }
}

public class ChangeBallSpawnPosition : SpecialAugment
{
    public override string ID => "SPECIAL_13";
    public override string Name => "Unexpected Strategy";
    public override string Description => "A ball is summoned from top";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        BallManager.StartPosition = new Vector3(0f, 6f, 35);
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
    public override string ID => "SPECIAL_14";
    public override string Name => "Lightning Attack";
    public override string Description => "An additional lightning strike occurs on attack";
    public override int Rarity => 4;
    public override string Type => "Lightning";
    public override string AdditionalDescription => "Lightning damage : <sprite=2> 20% + 3% of brick's max HP";
    public override bool isFollowUpAugment => false;

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<ThunderStrike>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<ChainLightning>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<LightningRod>().FirstOrDefault());
    }

    public override void Remove() 
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ThunderStrike>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ChainLightning>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<LightningRod>().FirstOrDefault());
    }

    public override void AfterAttackEffect(IBrick brick)
    {
        brick.DamageByLightningEffect(1f, 1);
    }
}

public class PoisonBall : SpecialAugment
{
    public override string ID => "SPECIAL_15";
    public override string Name => "Poison Ball";
    public override string Description => "30% chance to poison on hit";
    public override int Rarity => 4;
    public override string Type => "Poison";
    public override string AdditionalDescription => "Poison damage : <sprite=2> 50% + 3% of brick's current HP per stack";
    public override bool isFollowUpAugment => false;

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<DeadlyPoison>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<InfinitePoison>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<DecayingPoison>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<FlammablePoison>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DeadlyPoison>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<InfinitePoison>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayingPoison>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<FlammablePoison>().FirstOrDefault());
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
    public override string ID => "SPECIAL_16";
    public override string Name => "Infinite Poison";
    public override string Description => "Brick spreads poison to nearby bricks, on max stacks";
    public override int Rarity => 2;
    public override string Type => "Poison";
    public override string AdditionalDescription => "Poison damage : <sprite=2> 50% + 3% of brick's current HP per stack";
    public override bool isFollowUpAugment => true;
    Coroutine Routine = null;
    public override void Apply()
    {
        Routine = BrickManager.Instance.StartCoroutine(GetInfinitePoison());
    }

    public override void Remove() 
    {
        BrickManager.Instance.StopCoroutine(Routine);
        Routine = null;
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<PoisonBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<InfinitePoison>().FirstOrDefault());
    }

    IEnumerator GetInfinitePoison()
    {
        while (true)
        {
            if(AugmentManager.instance.PoisonBrickSet.Count > 0)
            {
                foreach (IBrick brick in AugmentManager.instance.PoisonBrickSet)
                {
                    if (brick.detectArea.NearbyBricks.Count > 0 && Random.value < 0.5f)
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
                                VisualEffectManager.Instance.SpawnParticle("InfinityPoison", brick.transform.position, 1);
                                SoundManager.instance.SpreadPoisonSound();
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
    public override string ID => "SPECIAL_17";
    public override string Name => "Deadly Poison";
    public override string Description => "+15% poison chance, make max poison stacks to 5.";
    public override int Rarity => 3;
    public override string Type => "Poison";
    public override string AdditionalDescription => "Max stack up 3 to 5";
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        AugmentManager.instance.PoisonStackLimit = 5;
        AugmentManager.instance.effectChance.PoisonChance += 0.15f;
    }

    public override void Remove()
    {
        AugmentManager.instance.PoisonStackLimit = 3;
        AugmentManager.instance.effectChance.PoisonChance -= 0.15f;
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<PoisonBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DeadlyPoison>().FirstOrDefault());
    }
}
public class Chaos : SpecialAugment
{
    public override string ID => "SPECIAL_18";
    public override string Name => "Chaos";
    public override string Description => "Random stat up or down\nThen, randomly teleport to a wave between the current wave +5 and +20";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
    GameObject Curtain;
    List<float> TotalChangedStatValue;

    public override void Apply()
    {
        Curtain = GameObject.Find("Canvas").transform.Find("Curtain").gameObject;
        Curtain.SetActive(false);
        TotalChangedStatValue = new List<float>
        {
            0, 0, 0, 0, 0
        };

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
    public override string ID => "SPECIAL_19";
    public override string Name => "Decay Ball";
    public override string Description => "Chance to decay on hit";
    public override int Rarity => 4;
    public override string Type => "Decay";
    public override string AdditionalDescription => "Decay Effect: Increases damage taken by bricks by 10% per stack";
    public override bool isFollowUpAugment => false;

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<PowerfulDecay>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<DecayExplosion>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<DecayZone>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<PowerfulDecay>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayExplosion>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayZone>().FirstOrDefault());
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
    public override string ID => "SPECIAL_20";
    public override string Name => "Powerful Decay";
    public override string Description => "Decay chance +15%, increase decay damage increase rate to 15%";
    public override int Rarity => 3;
    public override string Type => "Decay";
    public override string AdditionalDescription => "Decay Effect: Increases damage taken by bricks by 10% per stack";
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        AugmentManager.instance.DecayDamageMultiple = 1.15f;
        AugmentManager.instance.effectChance.DecayChance -= 0.15f;
    }

    public override void Remove()
    {
        AugmentManager.instance.DecayDamageMultiple = 1.1f;
        AugmentManager.instance.effectChance.DecayChance -= 0.15f;
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<PowerfulDecay>().FirstOrDefault());
    }
}
public class DecayExplosion : SpecialAugment
{
    public override string ID => "SPECIAL_21";
    public override string Name => "Decay Explosion";
    public override string Description => "Brick explodes when dying in decay";
    public override int Rarity => 2;
    public override string Type => "Decay";
    public override string AdditionalDescription => "Explosion damage: 5% of brick's max health";
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {

    }

    public override void Remove() 
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayExplosion>().FirstOrDefault());
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
    public override string ID => "SPECIAL_22";
    public override string Name => "Elasticity";
    public override string Description => "Balls bounce further when they collide with each other";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply() { }

    public override void Remove() { }
}

public class TheFlamethrower : SpecialAugment
{
    public override string ID => "SPECIAL_23";
    public override string Name => "Fire Aura";
    public override string Description => "Generates a fire aura around the balls that deals damage and may ignite bricks";
    public override int Rarity => 2;
    public override string Type => "Fire";
    public override string AdditionalDescription => "Aura damage : <sprite=2> 20% per stack\n" +
                                                    "Fire damage : <sprite=2> 50% per stack";
    public override bool isFollowUpAugment => true;

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

        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<FireBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<TheFlamethrower>().FirstOrDefault());
    }
}

public class LevelUpQuest : SpecialAugment
{
    public override string ID => "SPECIAL_24";
    public override string Name => "Slayer";
    public override string Description => "Receive 50% EXP and a quest. Complete the quest to level up";
    public override int Rarity => 5;
    public override string Type => "Quest";
    public override string AdditionalDescription => "Quest: Destroy 100 bricks";
    public override bool isFollowUpAugment => false;

    private int KilledEnemy = 0;
    private bool isCleared = false;
    private int ClearRequiredCount = 100;

    public override void Apply()
    {
        LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.5f;

        QuestAlarm.Instance.SetQuest("Slayer", "Destroyed", 0, ClearRequiredCount);
    }

    public override void Remove()
    {
        QuestAlarm.Instance.SetQuest("Slayer", "Destroyed", ClearRequiredCount, ClearRequiredCount);
    }

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
    public override string ID => "SPECIAL_25";
    public override string Name => "Grow Ball";
    public override string Description => "Accept a grow ball quest.\nComplete it to attack damage up";
    public override int Rarity => 5;
    public override string Type => "Quest";
    public override string AdditionalDescription => "Quest: Destroy 100 bricks\n" +
                                                    "<sprite=0> +0.02 per destroyed bricks\n" +
                                                    "<sprite=0> +0.04 per destroyed boss bricks";
    public override bool isFollowUpAugment => false;
    private int KilledEnemy;
    private float KeepAtk;
    private bool isCleared = false;
    int ClearRequiredCount = 100;

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
            QuestAlarm.Instance.SetQuest("Grow Ball", "Destroyed", ClearRequiredCount, ClearRequiredCount);
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
                BrickManager.Instance.StartCoroutine(GrowBall());
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
    public override string ID => "SPECIAL_26";
    public override string Name => "Infinite Grow Ball";
    public override string Description => "The ball's attack power increases after destroying bricks";
    public override int Rarity => 3;
    public override string Type => "Grow Ball";
    public override string AdditionalDescription => "Ball's <sprite=0> +0.1 per destroyed bricks\n" +
            "Ball's <sprite=0> +0.2 per destroyed boss bricks";
    public override bool isFollowUpAugment => false;

    public override void Apply() 
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<ShareBallAtk>().FirstOrDefault());
    }

    public override void Remove() 
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ShareBallAtk>().FirstOrDefault());
    }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if(Brick.RecentlyBall != null)
            Brick.RecentlyBall.Grow(Brick);
    }
}
public class SacrificeQuest1 : SpecialAugment
{
    public override string ID => "SPECIAL_27";
    public override string Name => "Sacrifice I";
    public override string Description => "Accept a sacrifice I quest.\nComplete it to Receive a weak reward";
    public override int Rarity => 5;
    public override string Type => "Quest";
    public override string AdditionalDescription => "Quest: Sacrifice 40 balls for <sprite=0> +1, <sprite=2> +1";
    public override bool isFollowUpAugment => false;
    private int SacrificeNumber = 0;
    int ClearRequiredCount = 40;
    private bool isCleared;

    public override void Apply() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice I", "Sacrificed", SacrificeNumber, ClearRequiredCount); 
    }

    public override void Remove() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice I", "Sacrificed", ClearRequiredCount, ClearRequiredCount);
        if (isCleared) 
        {
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest1>().FirstOrDefault());
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-1.5f);
            StatManager.instance.ChangeTemporaryStats_EffectDamage(-1.5f);
        }
        else
        {
            AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest1>().FirstOrDefault());
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

            AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest2>().FirstOrDefault());
            isCleared = true;
        }
    }
}
public class SacrificeQuest2 : SpecialAugment
{
    public override string ID => "SPECIAL_28";
    public override string Name => "Sacrifice II";
    public override string Description => "Accept a sacrifice II quest.\nComplete it to Receive a moderate reward";
    public override int Rarity => 3;
    public override string Type => "Quest";
    public override string AdditionalDescription => "Quest: sacrifice 70 balls for <sprite=0> +15%, <sprite=2> +15%, 500 Gold";
    public override bool isFollowUpAugment => true;
    private int SacrificeNumber = 0;
    private bool isCleared;
    int ClearRequiredCount = 70;

    public override void Apply() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice II", "Sacrificed", SacrificeNumber, ClearRequiredCount);
    }

    public override void Remove() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice II", "Sacrificed", ClearRequiredCount, ClearRequiredCount);
        if (isCleared)
        {
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest2>().FirstOrDefault());
            StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.17f);
            StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.17f);
        }
        else
        {
            AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest2>().FirstOrDefault());
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

            AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest3>().FirstOrDefault());
            isCleared = true;
        }
    }
}
public class SacrificeQuest3 : SpecialAugment
{
    public override string ID => "SPECIAL_29";
    public override string Name => "Sacrifice III";
    public override string Description => "Accept a sacrifice III quest.\nComplete it to Receive a powerful reward";
    public override int Rarity => 2;
    public override string Type => "Quest";
    public override string AdditionalDescription => "Quest: sacrifice 100 balls for secret reward";
    public override bool isFollowUpAugment => true;
    private int SacrificeNumber = 0;
    int ClearRequiredCount = 100;
    private bool isCleared;

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sacrifice III", "Sacrificed", SacrificeNumber, ClearRequiredCount); }

    public override void Remove() 
    {
        QuestAlarm.Instance.SetQuest("Sacrifice III", "Sacrificed", ClearRequiredCount, ClearRequiredCount);
        if (isCleared)
        {
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest3>().FirstOrDefault());
        }
        else
        {
            AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SacrificeQuest3>().FirstOrDefault());
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
    public override string ID => "SPECIAL_30";
    public override string Name => "Sincerity";
    public override string Description => "A ball is created every 100 hits";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
            if (attackNumber >= RequiredCount)
            {
                BallManager.Instance.CreateBall(1);
                attackNumber = 0;
            }
        }
    }

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sincerity", "Attack", AttackNumber, RequiredCount); }

    public override void Remove() 
    {
        QuestAlarm.Instance.SetQuest("Sincerity", "Sacrificed", RequiredCount, RequiredCount);
    }

    public override void AfterAttackEffect(IBrick Brick)
    {
        AttackNumber++;
        QuestAlarm.Instance.SetQuest("Sincerity", "Attack", AttackNumber, RequiredCount);
    }
}
public class ShareBallAtk : SpecialAugment
{
    public override string ID => "SPECIAL_31";
    public override string Name => "Inheritance";
    public override string Description => "Fallen ball's 70% attack power\ndivided among current balls";
    public override int Rarity => 5;
    public override string Type => "Grow Ball";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply() { }

    public override void Remove() 
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<InfiniteGrowBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ShareBallAtk>().FirstOrDefault());
    }

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
    public override string ID => "SPECIAL_32";
    public override string Name => "One in a thousand";
    public override string Description => "A powerful additional attack triggers at low chance";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => "Strong additional attack : <sprite=0>  1000%for 1% chance\n" +
            "It seems there are more powerful attacks...";
    public override bool isFollowUpAugment => false;

    float StrongAttackChance = 0.01f;
    float SuperStrongAttackChance = 0.0025f;
    float SuperUltraStrongAttackChance = 0.0001f;

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float RandomValue = Random.value;
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            if (RandomValue < SuperUltraStrongAttackChance)
                BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 100f, true));
            else if (RandomValue < SuperStrongAttackChance)
                BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 25f, true));
            else if (RandomValue < StrongAttackChance)
                BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 10f));
        }
    }
}

public class RandomEffect : SpecialAugment
{
    public override string ID => "SPECIAL_33";
    public override string Name => "Random Effect Attack";
    public override string Description => "A random effect triggers on attack\nit may be positive or negative";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    private Dictionary<string, float> ProbabilitySet = new Dictionary<string, float>()
    {
        { "WeakAdditionalDamage", 0.5f },
        { "Nothing", 0.3f },
        { "AdditionalDamage",  0.1f},
        { "RandomEffect", 0.04f },
        { "HealBrick", 0.02f },
        { "BrickSpawn", 0.02f },
        //{ "BossSpawn", 0.01f },
        { "Execution", 0.01f },
    };

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
                BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.5f));
                return;
            }

            accumulatedProbability += ProbabilitySet["AdditionalDamage"];
            if (RandomValue < accumulatedProbability)
            {
                BrickManager.Instance.StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.8f));
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
                    Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration);
                }
                else if (Number == 2)
                {
                    Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration);
                }
                else if (Number == 3)
                {
                    Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration);
                }
                return;
            }

            accumulatedProbability += ProbabilitySet["HealBrick"];
            if (RandomValue < accumulatedProbability)
            {
                Brick.HP += Brick.HpTemp * 0.1f;
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
    public override string ID => "SPECIAL_34";
    public override string Name => "Arc Link";
    public override string Description => "All balls are linked by electricity for " + Duration + "seconds";
    public override int Rarity => 1;
    public override string Type => "Lightning";
    public override string AdditionalDescription => "Lightning damage : <sprite=2> 20% + 3% of brick's max HP";
    public override bool isFollowUpAugment => true;
    public override float CoolTime => AugmentManager.instance.FieldGlobalCooldown / 2;

    Coroutine routine = null;
    float Duration = StatManager.instance.Temporary_stat.EffectDuration * 3;

    public override void Apply() 
    {
        routine = BrickManager.Instance.StartCoroutine(MakeBallThunder());
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<Lightning>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ThunderStrike>().FirstOrDefault());
        BrickManager.Instance.StopCoroutine(routine);
    }

    private IEnumerator MakeBallThunder()
    {
        Vector2 start;
        Vector2 end;

        RaycastHit2D[] hits;

        AudioSource Sound = null;
        if (Sound == null)
        {
            Sound = SoundManager.instance.ArcLinkSound(Duration);
        }
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

                    if(!Sound.isPlaying)
                        Sound.Play();
                    
                    for (int i = 0; i < Balls.Count - 1; i++)
                    {
                        start = Balls[i].transform.position;
                        end = Balls[i + 1].transform.position;

                        hits = Physics2D.LinecastAll(start, end);
                        for (int j = 0; j < hits.Length; j++)
                            if (hits[j].collider.CompareTag("Brick"))
                            {
                                IBrick brick = hits[j].collider.GetComponent<IBrick>();

                                brick.DamageByLightningEffect(10, 0, "ArcLink");
                            }
                    }
                    start = Balls[Balls.Count - 1].transform.position;
                    end = Balls[0].transform.position;

                    hits = Physics2D.LinecastAll(start, end);
                    for (int j = 0; j < hits.Length; j++)
                        if (hits[j].collider.CompareTag("Brick"))
                        {
                            IBrick brick = hits[j].collider.GetComponent<IBrick>();

                            brick.DamageByLightningEffect(10, 0, "ArcLink");
                        }
                }
                else
                {
                    Sound.Pause();
                }
                yield return new WaitForSeconds(0.1f);
                elapsed += 0.1f;
            }
            Sound.Stop();
            yield return new WaitForSeconds(CoolTime);
        }
    }
}
public class RebirthBall : SpecialAugment
{
    public override string ID => "SPECIAL_35";
    public override string Name => "Immortality";
    public override string Description => "Adds a 5% chance to revive after the ball died";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => "Excluding certain balls";
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_36";
    public override string Name => "Uprising";
    public override string Description => "Summons a special brick when the boss is defeated";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_37";
    public override string Name => "2x!";
    public override string Description => "All summoned balls are doubled";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
    public override void Apply() 
    {
        AugmentManager.instance.SpawnBallAmount = 2;

        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SpawnBallAmountMultiple2>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.SpawnBallAmount = 1;

        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SpawnBallAmountMultiple2>().FirstOrDefault());
    }
}

public class SpawnBallAmountMultiple2 : SpecialAugment
{
    public override string ID => "SPECIAL_38";
    public override string Name => "3x!!";
    public override string Description => "All summoned balls are tripled";
    public override int Rarity => 0;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        AugmentManager.instance.SpawnBallAmount = 3;
    }

    public override void Remove()
    {
        AugmentManager.instance.SpawnBallAmount = 1;
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<SpawnBallAmountMultiple1>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SpawnBallAmountMultiple2>().FirstOrDefault());
    }
}
public class BallSwarm : SpecialAugment
{
    public override string ID => "SPECIAL_39";
    public override string Name => "Ball Swarm";
    public override string Description => "<sprite=0> +2 \nwhen 15 or more balls are summoned";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => "Extra balls are stored\n" +
        "and released one by one when the count falls below 15";
    public override bool isFollowUpAugment => false;

    bool Upgrade = false;
    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("BallSwarm", GetBallSwarm());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<LonelyBall>().FirstOrDefault());
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
        AugmentManager.instance.AugmentRoutines.Stop("BallSwarm");
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<LonelyBall>().FirstOrDefault());
        if (Upgrade)
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }

    IEnumerator GetBallSwarm()
    {
        while (true)
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
            }
            if (BallManager.Instance.InGameBall.Count < 15 && BallManager.Instance.LeftBallCount == 0 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
            }
            yield return null;
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
        ball.StartPosition = BallManager.StartPosition;
        ball.rb.linearVelocityY = BallManager.linearVelocityYValue;
        return ball;
    }
}
public class LonelyBall : SpecialAugment
{
    public override string ID => "SPECIAL_40";
    public override string Name => "Lonely Ball";
    public override string Description => "<sprite=0> +2 \nwhen 5 or less balls are summoned";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => "Extra balls are stored\n" +
            "and released one by one when the count falls below 5";
    public override bool isFollowUpAugment => false;

    bool Upgrade = false;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("LonelyBall", GetLonelyBall());

        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<BallSwarm>().FirstOrDefault());
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
        AugmentManager.instance.AugmentRoutines.Stop("LonelyBall");

        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<BallSwarm>().FirstOrDefault());
        if (Upgrade)
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }

    IEnumerator GetLonelyBall()
    {
        while (true)
        {
            if (BallManager.Instance.InGameBall.Count <= 5)
            {
                int ballsToAdd = Mathf.Min(5 - BallManager.Instance.InGameBall.Count, BallManager.Instance.LeftBallCount);
                CreateBall(ballsToAdd);
                BallManager.Instance.LeftBallCount -= ballsToAdd;
            }

            if (BallManager.Instance.InGameBall.Count <= 5 && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(2f);
                //BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(2, 2, 2);
            }
            if (BallManager.Instance.InGameBall.Count > 5 && BallManager.Instance.LeftBallCount == 0 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
                //BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(1, 1, 1);
            }
            yield return null;
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
    public override string ID => "SPECIAL_41";
    public override string Name => "Potential Bomb";
    public override string Description => "A low chance for the ball to explode after an attack";
    public override int Rarity => 1;
    public override string Type => "Bomb";
    public override string AdditionalDescription => "Explosion damage: 30 + <sprite=2> 300%";
    public override bool isFollowUpAugment => true;

    public override void Apply() { }

    public override void Remove() 
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<SpawnBombBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SuicideBoomBall>().FirstOrDefault());
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        IBall ball = Brick.RecentlyBall;
        if(Random.value < 0.02f && !BallManager.Instance.InGameSpecialBall.Contains(ball))
        {
            SoundManager.instance.ExplosionSound_Small();
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
    public override string ID => "SPECIAL_42";
    public override string Name => "Diffraction";
    public override string Description => "Balls Have a 1% chance to split after an attack";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
            AugmentManager.instance.StartCoroutine(SplitBallEffect(RecentlyBall));
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
                AugmentManager.instance.StartCoroutine(SplitBallEffect(ball));
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
    public override string ID => "SPECIAL_43";
    public override string Name => "Curse Ball";
    public override string Description => "When the ball falls, \n30% chance of curse damage to a random block";
    public override int Rarity => 4;
    public override string Type => "Curse";
    public override string AdditionalDescription => "Curse damage : <sprite=0> 150%";
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SummonGhostBall>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<EnhanceCurseAttack>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<EnhanceCurseAttack>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SummonGhostBall>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<EnhanceCurseAttack>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<EnhanceCurseAttack>().FirstOrDefault());
    }

    public override void AfterDeathEffect(IBall ball)
    {
        if (Random.value < AugmentManager.instance.effectChance.CurseChance && BrickManager.Instance.CurrentBricks.Count > 0)
        {
            IBrick brick = BrickManager.Instance.GetRandomBrick();
            if(brick != null) brick.DamageByCurse();
        }
    }
}

public class SummonGhostBall : SpecialAugment
{
    public override string ID => "SPECIAL_44";
    public override string Name => "Revenant";
    public override string Description => "When the ball falls, \nSummon ghost ball at low chance";
    public override int Rarity => 3;
    public override string Type => "Curse";
    public override string AdditionalDescription => "Summoned ghost balls do not collide with each balls \n" +
            "and disappear after 8 seconds\n" +
            "When a Ghost Ball is disappear, Curse Attack activates with a 100% chance";
    public override bool isFollowUpAugment => true;

    public override void Apply() 
    {
        
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<CurseBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SummonGhostBall>().FirstOrDefault());

    }

    public override void AfterDeathEffect(IBall ball)
    {
        if(Random.value < 0.15f)
            BallManager.Instance.CreateGhostBall(1, ball);
    }
}
public class EnhanceCurseAttack : SpecialAugment
{
    public override string ID => "SPECIAL_45";
    public override string Name => "Endless Revenge";
    public override string Description => "Curse damage increases by 1.5% whenever the ball dies";
    public override int Rarity => 2;
    public override string Type => "Curse";
    public override string AdditionalDescription => "30% of the balls that already died will be counted";
    public override bool isFollowUpAugment => true;

    public override void Apply() 
    {
        AugmentManager.instance.CurseDamageValue += (GameManager.instance.RecordStatistics.DiedBalls * 0.3f) * 0.015f;
    }

    public override void Remove() 
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<CurseBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<EnhanceCurseAttack>().FirstOrDefault());
        AugmentManager.instance.CurseDamageValue = 1f;
    }

    public override void AfterDeathEffect(IBall ball)
    {
        AugmentManager.instance.CurseDamageValue += 0.015f;
    }
}
public class DeathEffectTwice : SpecialAugment
{
    public override string ID => "SPECIAL_46";
    public override string Name => "Die Again?";
    public override string Description => "All actions performed after the ball's death are doubled";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_47";
    public override string Name => "Bomb Maniac";
    public override string Description => "Low chance of bomb ball on summon";
    public override int Rarity => 3;
    public override string Type => "Bomb";
    public override string AdditionalDescription => "The summoned bomb ball explodes after 3 seconds\n" +
            "Explosion damage: <sprite=2> 1000%";
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<RebornBombBall>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<SuicideBoomBall>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<Bombardment>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<InstantBombBall>().FirstOrDefault());
    }

    public override void Remove()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<RebornBombBall>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<SuicideBoomBall>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<Bombardment>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<InstantBombBall>().FirstOrDefault());
    }
}
public class ChangeNormalToGlass : SpecialAugment
{
    public override string ID => "SPECIAL_48";
    public override string Name => "Glassification";
    public override string Description => "The basic ball becomes a glass ball";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => "<sprite=0> +4\nballs have a 1% chance to break on hit";
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_49";
    public override string Name => "Bomb Modification";
    public override string Description => "Bomb ball revive at a low chance after explode";
    public override int Rarity => 4;
    public override string Type => "Bomb";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply() { }

    public override void Remove() 
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<SpawnBombBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<RebornBombBall>().FirstOrDefault());
    }
}
public class OneMoreChance : SpecialAugment
{
    public override string ID => "SPECIAL_50";
    public override string Name => "One More Chance";
    public override string Description => "If no balls exist,\nsummon 5 balls at once";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply() 
    {
        AugmentManager.instance.AugmentRoutines.Add("OneMoreChance", GetOneMoreChance());
    }

    public override void Remove() 
    {
        AugmentManager.instance.AugmentRoutines.Stop("OneMoreChance");
    }

    IEnumerator GetOneMoreChance()
    {
        while (true)
        {
            if (BallManager.Instance.InGameBall.Count <= 0)
            {
                BallManager.Instance.CreateBall(5);
                yield break;
            }
            yield return null;
        }
    }
}

public class Concentrate : SpecialAugment
{
    public override string ID => "SPECIAL_51";
    public override string Name => "Concentrate";
    public override string Description => "Stats increase massively, \nbut 10 ball fall, it's game over";
    public override int Rarity => 0;
    public override string Type => "Special";
    public override string AdditionalDescription => "Attack, Effect Damage +300%\n" +
            "Set current balls to 10\n" +
            "Balls destroyed will converted to <sprite=0> +0.2, <sprite=2> + 0.2";
    public override bool isFollowUpAugment => false;

    int MaxCount = 10;
    int Count = 10;
    int DestroyedBall = 0;

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
        QuestAlarm.Instance.SetQuest("Concentrate", "GameOver In", 0, 0);
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
    public override string ID => "SPECIAL_52";
    public override string Name => "Group Violence";
    public override string Description => "After attack, transfers 10% of Attack damage to the brick with the highest HP";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => "Applies only to normal attacks and specific effects attacks";
    public override bool isFollowUpAugment => false;


    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("GroupViolence", GetGroupViolence());
    }

    public override void Remove() 
    {
        AugmentManager.instance.AugmentRoutines.Stop("GroupViolence");
    }

    IEnumerator GetGroupViolence()
    {
        while (true)
        {
            if (BrickManager.Instance.CurrentBricks.Count > 0)
            {
                float maxHp = BrickManager.Instance.CurrentBricks.Max(b => b.HP);
                BrickManager.Instance.MainBrick = BrickManager.Instance.CurrentBricks.FirstOrDefault(b => b.HP == maxHp && b.BrickType != "IgnoreBrick");
            }
            yield return null;
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
    public override string ID => "SPECIAL_53";
    public override string Name => "Oneness";
    public override string Description => "All balls merge into one,\nthe ball becomes stronger as it merges";
    public override int Rarity => 2;
    public override string Type => "Special";
    public override string AdditionalDescription => "<sprite=0> +30% of merged ball's <sprite=0>";
    public override bool isFollowUpAugment => false;

    IBall MainBall = null;

    IEnumerator GetOneness()
    {
        while (true)
        {
            if (MainBall == null)
                AugmentManager.instance.StartCoroutine(WaitforMainBall());
            else
            {
                for (int i = 0; i < BallManager.Instance.InGameBall.Count; i++)
                {
                    if (BallManager.Instance.InGameBall[i] != MainBall && BallManager.Instance.InGameBall[i].FusionMoveRoutine == null)
                        BallManager.Instance.InGameBall[i].FusionMoveRoutine = AugmentManager.instance.StartCoroutine(MoveToPosition(BallManager.Instance.InGameBall[i], MainBall, 5f, 5f));
                }
            }
            yield return null;
        }
    }

    public override void Apply() 
    {
        IBall MainBall = null;
        AugmentManager.instance.AugmentRoutines.Add("Oneness_1", GetOneness());
        AugmentManager.instance.AugmentRoutines.Add("Oneness_2", WaitforMainBall());
    }

    public override void Remove()
    {
        IBall MainBall = null;
        AugmentManager.instance.AugmentRoutines.Stop("Oneness_1");
        AugmentManager.instance.AugmentRoutines.Stop("Oneness_2");
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
        AugmentManager.instance.StartCoroutine(OnenessBallEffect(MainBall));
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
    public override string ID => "SPECIAL_54";
    public override string Name => "Happy Birth Day";
    public override string Description => "<sprite=0> +5 when ball is summoned for " + Duration + " seconds";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    float Duration = StatManager.instance.Temporary_stat.EffectDuration;

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
    public override string ID => "SPECIAL_55";
    public override string Name => "Midas Hand";
    public override string Description => "Attacked bricks have a low chance to\ntransform into golden blocks";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_56";
    public override string Name => "Mass Sacrifice";
    public override string Description => "Sacrifice half of the balls,\nand increse corresponding attack damage";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => "<sprite=0> +0.2 for each sacrificed ball";
    public override bool isFollowUpAugment => false;

    float AtkUpValue = 0;

    public override void Apply() 
    {
        if(BallManager.Instance.InGameBall.Count > 1)
        {
            for(int i = 0; i < BallManager.Instance.InGameBall.Count / 2; i++)
            {
                AugmentManager.instance.StartCoroutine(BallRemoveEffect(BallManager.Instance.InGameBall[i]));
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
    public override string ID => "SPECIAL_57";
    public override string Name => "Chronobreak";
    public override string Description => "lose half of augment, Attack and Effect Damage\nThen, return to Wave 1";
    public override int Rarity => 0;
    public override string Type => "Special";
    public override string AdditionalDescription => "All bricks and balls are destroyed, then 10 balls are summoned \n" +
            "Certain bricks and balls are excluded";
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        BrickManager.Instance.WaveSkip(-(GameManager.instance.Wave) + 1);
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
    public override string ID => "SPECIAL_58";
    public override string Name => "Decaying Poison";
    public override string Description => "When poisoned, decay is also applied";
    public override int Rarity => 3;
    public override string Type => "Poison";
    public override string AdditionalDescription => "Increases damage taken by bricks by 10% per stack";
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<FlammablePoison>().FirstOrDefault());
        AugmentManager.instance.effectDuration.FireDuration += 1f;
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<FlammablePoison>().FirstOrDefault());
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<PoisonBall>().FirstOrDefault()) ||
            AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<FlammablePoison>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayingPoison>().FirstOrDefault());
        AugmentManager.instance.effectDuration.FireDuration -= 1f;
    }
}

public class FlammablePoison: SpecialAugment
{
    public override string ID => "SPECIAL_59";
    public override string Name => "Flammable Poison";
    public override string Description => "When poisoned, fire is also applied";
    public override int Rarity => 3;
    public override string Type => "Poison";
    public override string AdditionalDescription => "<sprite=2> 50% per stack";
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayingPoison>().FirstOrDefault());
        AugmentManager.instance.effectDuration.DecayDuration += 2f;
    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<DecayingPoison>().FirstOrDefault());
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<PoisonBall>().FirstOrDefault()) ||
            AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<DecayingPoison>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<FlammablePoison>().FirstOrDefault());
        AugmentManager.instance.effectDuration.DecayDuration -= 2f;
    }
}

public class CritAfterThreeAttack : SpecialAugment
{
    public override string ID => "SPECIAL_60";
    public override string Name => "Weakness Detection";
    public override string Description => "The fourth attack always triggers a critical hit";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_61";
    public override string Name => "Miraculous Technique";
    public override string Description => "Ball attacks all nearby bricks,\nIt count as direct hits by the ball";
    public override int Rarity => 0;
    public override string Type => "Special";
    public override string AdditionalDescription => "Applies all effects triggered by direct hits";
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_62";
    public override string Name => "Effect Amplification";
    public override string Description => "Effect attacks can deal critical hits";
    public override int Rarity => 3;
    public override string Type => "Effect";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class RollDiceBall : SpecialAugment
{
    public override string ID => "SPECIAL_63";
    public override string Name => "Roll the Dice!";
    public override string Description => "Roll dice instead of summoning balls.\nsummon as many balls as the dice show.";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class PortalBall : SpecialAugment
{
    public override string ID => "SPECIAL_64";
    public override string Name => "Portal Ball";
    public override string Description => "Hits may cause the ball to \nteleport to the random brick's position";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_65";
    public override string Name => "Rapidfire";
    public override string Description => "<sprite=2> +1.8, <sprite=6> +15%,\nAll waves appear as transformed rapid waves";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_66";
    public override string Name => "Legion";
    public override string Description => "Every Bricks appear 50% more frequently, \nbut their health is reduced by 33%.";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => "Special bricks are excluded";
    public override bool isFollowUpAugment => false;
    
    public override void Apply()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1.5f;
        BrickManager.Instance.BaseBrickHealthValue = 0.66f;
        BrickManager.Instance.BaseBossHealthValue = 0.66f;
        BrickManager.Instance.BaseWaitingTimeValue = 6;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 0.5f).ToList();
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<LessAndStrongBricks>().FirstOrDefault());
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1f;
        BrickManager.Instance.BaseBrickHealthValue = 1f;
        BrickManager.Instance.BaseBossHealthValue = 1f;
        BrickManager.Instance.BaseWaitingTimeValue = 5;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 2f).ToList();
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<LessAndStrongBricks>().FirstOrDefault());
    }
}

public class LessAndStrongBricks : SpecialAugment
{
    public override string ID => "SPECIAL_67";
    public override string Name => "Elite Group";
    public override string Description => "Every Bricks appear 50% less frequently, \nbut their health is increased by 33%.";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => "Special bricks are excluded";
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 0.5f;
        BrickManager.Instance.BaseBrickHealthValue = 1.33f;
        BrickManager.Instance.BaseBossHealthValue = 1.33f;
        BrickManager.Instance.BaseWaitingTimeValue = 4;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 2f).ToList();
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<MoreAndWeakBricks>().FirstOrDefault());
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1f;
        BrickManager.Instance.BaseBrickHealthValue = 1f;
        BrickManager.Instance.BaseBossHealthValue = 1f;
        BrickManager.Instance.BaseWaitingTimeValue = 5;
        BrickManager.Instance.BrickExpValue = BrickManager.Instance.BrickExpValue.Select(x => x * 0.5f).ToList();
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<MoreAndWeakBricks>().FirstOrDefault());
    }
}

public class DoubleBoss : SpecialAugment
{
    public override string ID => "SPECIAL_68";
    public override string Name => "Twins!";
    public override string Description => "The boss splits into two, \neach having 66% the original health.";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_69";
    public override string Name => "Only One";
    public override string Description => "Waves are replaced by one brick with combined health.";
    public override int Rarity => 2;
    public override string Type => "Special";
    public override string AdditionalDescription => "It is treated as a boss";
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_70";
    public override string Name => "Promotion Gift";
    public override string Description => "After level up, roll a dice \nand gain that many balls.";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_71";
    public override string Name => "Lucky Day";
    public override string Description => "Special bricks spawn more frequently.";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_72";
    public override string Name => "Stick to the Basics";
    public override string Description => "Gain 24% more stats when selecting after level up.";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;
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
    public override string ID => "SPECIAL_73";
    public override string Name => "Milestone";
    public override string Description => "Receive rewards upon reaching certain waves.";
    public override int Rarity => 3;
    public override string Type => "Special";
    public override string AdditionalDescription => "Wave 5: Small exp\n" +
            "Wave 10: 15 Balls\n" +
            "Wave 30: <sprite=0> +5%, <sprite=3> +5%\n" +
            "Wave 50: Moderate exp\n" +
            "Wave 70: 25 Balls, <sprite=0> +5%, <sprite=3> +5%\n" +
            "Wave 100: Huge exp, <sprite=0> +10%, <sprite=3> +10%\n";
    public override bool isFollowUpAugment => false;

    bool[] Goals;
    int NextGoalWave = 5;

    public override void Apply()
    {
        AugmentManager.instance.AugmentRoutines.Add("Milestone", GetMilestone());
        Goals = new bool[6];

        if (GameManager.instance.Wave < 5)
            NextGoalWave = 5;
        else if (GameManager.instance.Wave < 10)
            NextGoalWave = 10;
        else if (GameManager.instance.Wave < 30)
            NextGoalWave = 30;
        else if (GameManager.instance.Wave < 50)
            NextGoalWave = 50;
        else if (GameManager.instance.Wave < 70)
            NextGoalWave = 70;
        else if (GameManager.instance.Wave < 100)
            NextGoalWave = 100;
    }

    public override void Remove()
    {
        AugmentManager.instance.AugmentRoutines.Stop("Milestone");
        for (int i = 0; i < Goals.Length; i++)
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

    IEnumerator GetMilestone()
    {
        while (true)
        {
            if (NextGoalWave == 999)
            {
                yield break;
            }
            else
                QuestAlarm.Instance.SetQuest("Milestone", "Wave", GameManager.instance.Wave, NextGoalWave);
            if (GameManager.instance.Wave == 5 && !Goals[0])
            {
                Goals[0] = true;
                ItemManager.Instance.SpawnSpecificItemInRandomPosition("ExpBead", 10, 0.1f);
                NextGoalWave = 10;
            }
            else if (GameManager.instance.Wave == 10 && !Goals[1])
            {
                Goals[1] = true;
                for (int i = 0; i < 3; i++)
                {
                    BallManager.Instance.CreateBall(5);
                }
                NextGoalWave = 30;
            }
            else if (GameManager.instance.Wave == 30 && !Goals[2])
            {
                Goals[2] = true;
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.05f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.05f);
                NextGoalWave = 50;
            }
            else if (GameManager.instance.Wave == 50 && !Goals[3])
            {
                Goals[3] = true;
                AugmentManager.instance.StartCoroutine(ItemManager.Instance.SpawnSpecificItemInRandomPosition("ExpBead", 40, 0.1f));
                NextGoalWave = 70;
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
                NextGoalWave = 100;
            }
            else if (GameManager.instance.Wave == 100 && !Goals[5])
            {
                Goals[5] = true;
                ItemManager.Instance.SpawnSpecificItemInRandomPosition("ExpBead", 100, 0.1f);

                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.1f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.1f);
                QuestAlarm.Instance.SetQuest("Milestone", "Wave", 100, 100);
                NextGoalWave = 999;
            }
            yield return null;
        }
    }
}

public class ReducesExp10: SpecialAugment
{
    public override string ID => "SPECIAL_74";
    public override string Name => "Brilliant Idea I";
    public override string Description => "Reduces exp required to level up by 10%";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_75";
    public override string Name => "Brilliant Idea II";
    public override string Description => "Reduces exp required to level up by 15%";
    public override int Rarity => 2;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_76";
    public override string Name => "Curse of the Number 4";
    public override string Description => "All stats 16% Up\nEvery 4 waves, a Rapid Wave activates and a enhanced boss appears";
    public override int Rarity => 1;
    public override string Type => "Special";
    public override string AdditionalDescription => "Except gold gain stat";
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_77";
    public override string Name => "Reroll";
    public override string Description => "Refreshes the currently available augments";
    public override int Rarity => 5;
    public override string Type => "Special";
    public override string AdditionalDescription => "This augmentation may appear again after being selected\n" +
            "and it won't appear again right away after a refresh";
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_78";
    public override string Name => "Boss Killer";
    public override string Description => "+10% Additional damage to boss";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

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
    public override string ID => "SPECIAL_79";
    public override string Name => "Possession";
    public override string Description => "The curse can activate multiple times based on a specific chance";
    public override int Rarity => 2;
    public override string Type => "Curse";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {

    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<CurseBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<ContinuingCurseAttack>().FirstOrDefault());
    }
}

public class LevelUpAgain : SpecialAugment
{
    public override string ID => "SPECIAL_80";
    public override string Name => "Blessing";
    public override string Description => "Level up once more after level up and holy attack on all bricks";
    public override int Rarity => 6;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {

    }

    public override void Remove()
    {
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<LevelUpAgain>().FirstOrDefault());
    }

    public override void AfterLevelUpEffect()
    {
        var CurrentBricksCopy = BrickManager.Instance.CurrentBricks.ToList();
        foreach (IBrick brick in CurrentBricksCopy)
        {
            brick.HolyRainAttack();
        }
        if (!AugmentManager.instance.isLevelUpOnceAgain)
        {
            AugmentManager.instance.isLevelUpOnceAgain = true;
            LevelManager.Instance.LevelUp();
        }
    }
}

public class AtkUpWhenCritical : SpecialAugment
{
    public override string ID => "SPECIAL_81";
    public override string Name => "Lucky Streak";
    public override string Description => "When critical attack hits, <sprite=0> +1.8\nfor 3 seconds";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    int setTime = 3;

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (!AugmentManager.instance.AugmentRoutines.hasKey("LuckyStreak"))
        {
            AugmentManager.instance.AugmentRoutines.Add("LuckyStreak", AttackDamageUpFewSec(setTime));
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
        AugmentManager.instance.AugmentRoutines.Stop("LuckyStreak");
    }
}

public class InstantBombBall : SpecialAugment
{
    public override string ID => "SPECIAL_82";
    public override string Name => "Instant Bomb";
    public override string Description => "Bomb ball is changed to explode immediately after attacking the brick";
    public override int Rarity => 4;
    public override string Type => "Bomb";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<SpawnBombBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<InstantBombBall>().FirstOrDefault());
    }
}

public class Pyromaniac : SpecialAugment
{
    public override string ID => "SPECIAL_83";
    public override string Name => "Pyromaniac";
    public override string Description => "<sprite=2> +2, Bricks always catch fire when hit\nNo other effects will apply";
    public override int Rarity => 4;
    public override string Type => "Fire";
    public override string AdditionalDescription => "Lose all other effect augments";
    public override bool isFollowUpAugment => true;

    public override void Apply()
    {
        AugmentManager.instance.RemoveAugmentOnType("Poison");
        AugmentManager.instance.RemoveAugmentOnType("Decay");
        AugmentManager.instance.RemoveAugmentOnType("Lightning");
        AugmentManager.instance.RemoveAugmentOnType("Curse");

        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<PoisonBall>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<DecayBall>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<CurseBall>().FirstOrDefault());
        AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<Lightning>().FirstOrDefault());


        StatManager.instance.ChangeTemporaryStats_EffectDamage(2f);
        AugmentManager.instance.effectChance.FireChance += 1f;
        AugmentManager.instance.effectChance.PoisonChance += -99f;
        AugmentManager.instance.effectChance.DecayChance += -99f;
        AugmentManager.instance.effectChance.CurseChance += -99f;
        AugmentManager.instance.effectChance.LightningChance += -99f;
    }

    public override void Remove()
    {
        if (!AugmentManager.instance.activeAugments.Contains(AugmentManager.instance.augmentsSet.OfType<FireBall>().FirstOrDefault()))
            AugmentManager.instance.LockAugment(AugmentManager.instance.augmentsSet.OfType<Pyromaniac>().FirstOrDefault());

        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<PoisonBall>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<DecayBall>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<CurseBall>().FirstOrDefault());
        AugmentManager.instance.UnlockAugment(AugmentManager.instance.augmentsSet.OfType<Lightning>().FirstOrDefault());


        StatManager.instance.ChangeTemporaryStats_EffectDamage(-2f);
        AugmentManager.instance.effectChance.FireChance += -1f;
        AugmentManager.instance.effectChance.PoisonChance += 99f;
        AugmentManager.instance.effectChance.DecayChance += 99f;
        AugmentManager.instance.effectChance.CurseChance += 99f;
        AugmentManager.instance.effectChance.LightningChance += 99f;
    }
}

public class AdditionalEffectDamage : SpecialAugment
{
    public override string ID => "SPECIAL_84";
    public override string Name => "Dual Wield";
    public override string Description => "Deals additional damage equal to 10% of the <sprite=2> after the attack";
    public override int Rarity => 4;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        
    }

    public override void Remove()
    {
        
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float damage = StatManager.EffectDamage * 0.1f;
        Brick.ArbitraryDamageAttack(damage, false, damage.ToString("F1"), "Additional");
    }
}

public class PenetratingBall: SpecialAugment
{
    public override string ID => "SPECIAL_85";
    public override string Name => "Penetrating Ball";
    public override string Description => "The ball passes through the brick. <sprite=0> -75%, <sprite=2> -75%";
    public override int Rarity => 0;
    public override string Type => "Special";
    public override string AdditionalDescription => null;
    public override bool isFollowUpAugment => false;

    public override void Apply()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(0.25f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(0.25f);

        for(int i = 0; i < BrickManager.Instance.CurrentBricks.Count; i++)
        {
            BrickManager.Instance.CurrentBricks[i].GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public override void Remove()
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-0.25f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-0.25f);

        for (int i = 0; i < BrickManager.Instance.CurrentBricks.Count; i++)
        {
            BrickManager.Instance.CurrentBricks[i].GetComponent<Collider2D>().isTrigger = false;
        }
    }
}