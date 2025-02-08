using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using Unity.VisualScripting;

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

/**
 * 확률로 블럭에 불 이펙트 효과 적용, 해당 블럭의 불 이펙트 스택이 최대치가 아니라면 +1
 */
public class FireBall : SpecialAugment
{
    private float FireDamage; //EffectDamage
    private float FireDuration; //EffectDuration

    private void OnEnable()
    {
        SetAugment("SPECIAL_1", "FireBall", "Time to Cast Magic", "Effect", 4);
        FireDamage = 0.3f;
        FireDuration = 0.5f;
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.EffectDamage += FireDamage;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.EffectDamage -= FireDamage;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.FireChance)
        {
            Brick.EffectStatusSet["Fire"].EffectStack += (Brick.EffectStatusSet["Fire"].EffectStack != 5) ? 1 : 0;
            StartCoroutine(Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + FireDuration));
        }
    }
}

/**
 * 공이 주변 블럭을 찾아서 유도
 */
public class Homing : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_2", "Homing Ball", "Lock On!", "Homing", 1);
    }
    public override void Apply()
    { 
        
    }

    public override void Remove(){  }
}

/**
 * 벽에 튕길때마다 바운스 횟수 증가(데미지 증가)
 */
public class BouncePower: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_3", "Bounce = Power", "Get Stronger with Bounces", "Bounce", 3);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.RecentlyBall.BounceTime * StatManager.instance.statMultiplier.AttackDamageMultiplier;
        Brick.RecentlyBall.BounceTime = 1;
    }
}

/**
 * 공격 이후에 3회 추가타발생
 */
public class AdditionalThreeAttack: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_4", "Ninja", "Blink of an Eye", "AdditionalAttack", 1);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            StartCoroutine(Brick.AdditionalAttack(0.3f, 0.1f));
        }
    }
}


/**
 * 크리티컬 공격 이후에 추가타 1회 발생
 */
public class CriticalAdditionalAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_5", "CriticalAdditionalAttack", "CriticalAdditionalAttack", "AdditionalAttack", 4);
    }

    public override void Apply(){  }

    public override void Remove(){ }

    public override void CriticalAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf) //Critical Additional Attack
        {
            Brick.AdditionalAttack(0.5f, 0.5f);
        }
    }
}

/**
 * 일정 체력 이하 블럭 처형
 */

public class Execution : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_6", "Execution", "Execution", "Special", 1);
    }

    public override void Apply(){ }

    public override void Remove(){ }
}

/**
 * 블럭 최대체력 퍼센트 데미지
 */
public class PercentDamage : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_7", "Percent Damage", "Percent Damage", "Special", 3);
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

/**
 * 블럭의 불 이펙트 스택이 최대라면 폭발하며 이후 주변 블럭에 (불 최대스택 * 3) 만큼의 스택을 전이시키고 불 이펙트 효과 발동
 */
public class FireEffectStackBlast : SpecialAugment
{
    private float FireDuration;
    private void OnEnable()
    {
        SetAugment("SPECIAL_8", "FireEffectStackBlast", "FireEffectStackBlast", "Effect", 3);
    }

    public override void Apply()
    {
        FireDuration = 0.5f;
    }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.EffectStatusSet["Fire"].EffectStack == Brick.EffectStatusSet["Fire"].EffectStackLimit)
        {
            Brick.EffectStatusSet["Fire"].EffectStack = (int)(Brick.EffectStatusSet["Fire"].EffectStack * 0.3);
            foreach (IBrick nearbyBrick in Brick.NearbyBricks)
            {
                nearbyBrick.EffectStatusSet["Fire"].EffectStack += 
                    (nearbyBrick.EffectStatusSet["Fire"].EffectStack != Brick.EffectStatusSet["Fire"].EffectStackLimit) ? 2 : 0;
                nearbyBrick.ArbitraryDamageAttack(
                    StatManager.instance.Temporary_stat.EffectDamage,
                    false, 
                    StatManager.instance.Temporary_stat.EffectDamage.ToString("F1"), 
                    "Fire");
                StartCoroutine(nearbyBrick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + FireDuration));
            }
        }
    }
}

/**
 * 공격받은 블럭을 제외한 주변 4개의 블럭으로 전이되는 번개
 */
public class ChainLightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_9", "ChainLightning", "ChainLightning", "AdditionalAttack", 3);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        List<IBrick> OrderedTargetBricks = new List<IBrick>();

        IBrick targetBrick = Brick;
        if (Brick.NearbyBricks.Count == 0)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            List<IBrick> BrickToBrick = targetBrick.NearbyBricks;
            OrderedTargetBricks.Add(targetBrick);

            if (BrickToBrick.Count == 0)
            {
                break;
            }
            int RandomNumber = Random.Range(0, BrickToBrick.Count);
            BrickToBrick[RandomNumber].ArbitraryDamageAttack(1 * StatManager.instance.statMultiplier.EffectDamageMultiplier, 
                false, (1 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"), null);
            BrickToBrick[RandomNumber].NearbyBricks.Remove(targetBrick);

            targetBrick = BrickToBrick[RandomNumber];

        }
        VisualEffectManager.Instance.SpawnChainLightning(OrderedTargetBricks);
    }
}

/**
 * 공이 죽으면 경험치 획득
 */
public class DeathExp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_10", "Death = Exp", "Death = Exp", "Death", 5);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect()
    {
        LevelManager.Instance.Exp += 30 * StatManager.instance.Persistent_stat.BonusExp;
    }
}

/**
 * 공이 죽으면 골드 획득
 */
public class DeathGold : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_11", "Death = Gold", "Death = Gold", "Death", 5);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect()
    {
        StatManager.instance.Persistent_stat.Gold += 18 + (18 * StatManager.instance.Temporary_stat.GoldGain);
    }
}

/**
 * 속도가 빠를 수록 공격력 증가
 */
public class SpeedPower : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_12", "Speed = Power", "Speed = Power", "Speed", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.RecentlyBall.TransformationMultiple * StatManager.instance.statMultiplier.AttackDamageMultiplier;
    }
}

/**
 * 속도가 빠를 수록 크리티컬 확률 증가
 */
public class SpeedCritical : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_13", "Speed = Critical Chance", "Speed = Critical Chance", "Speed", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.RecentlyBall.TransformationMultiple * StatManager.instance.statMultiplier.AttackDamageMultiplier;

    }
}

/**
 * 타격시 번개 생성
 */

public class Lightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_14", "Lightning", "Lightning", "Lightning", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterAttackEffect(IBrick brick)
    {
        brick.ArbitraryDamageAttack(1 * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                false, (1 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"), null);
        VisualEffectManager.Instance.SpawnParticle("Lightning", brick.transform.position, 1);

    }
}

/*
 * N초동안 M초 주기로 블럭 현재체력 비례 데미지
 */
public class PoisonBall : SpecialAugment
{
    private float PoisonDuration;

    private void OnEnable()
    {
        SetAugment("SPECIAL_15", "PoisonBall", "PoisonBall", "Effect", 4);
        PoisonDuration = 1f;
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.PoisonChance)
        {
            Brick.EffectStatusSet["Poison"].EffectStack += (Brick.EffectStatusSet["Poison"].EffectStack != Brick.EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
            StartCoroutine(Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration + PoisonDuration));
        }
    }
}

/*
 * When poisoned, there is a certain chance to increase the Poison Effect stack by 1 and refresh its duration.
 * If the stack reaches 3, the poison spreads to nearby blocks.
 */
public class InfinitePoison : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_16", "InfinitePoison", "InfinitePoison", "Effect", 2);
    }

    public override void Apply()
    {
        AugmentManager.instance.effectChance.PoisonChance += 0.1f;
    }

    public override void Remove() 
    {
        AugmentManager.instance.effectChance.PoisonChance -= 0.1f;
    }
}

public class DeadlyPoison : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_17", "DeadlyPoison", "DeadlyPoison", "Effect", 3);
    }

    public override void Apply()
    {
        BrickInfo brickInfo = BrickManager.Instance.BrickInfo;
        foreach (IBrick brick in brickInfo.Bricks)
        {
            brick.EffectStatusSet["Poison"].EffectStackLimit = 5;
        }

        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            brick.EffectStatusSet["Poison"].EffectStackLimit = 5;
        }
        AugmentManager.instance.effectChance.PoisonChance += 0.1f;
    }

    public override void Remove()
    {
        BrickInfo brickInfo = BrickManager.Instance.BrickInfo;
        foreach (IBrick brick in brickInfo.Bricks)
        {
            brick.EffectStatusSet["Poison"].EffectStackLimit = 3;
        }
        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            brick.EffectStatusSet["Poison"].EffectStackLimit = 3;
        }
        AugmentManager.instance.effectChance.PoisonChance -= 0.1f;
    }
}


public class Chaos : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "Chaos", "Chaos", "Special", 1);
    }

    public override void Apply()
    {
        AugmentManager.instance.rarityWeights[1] = 20f;
        AugmentManager.instance.rarityWeights[2] = 20f;
        AugmentManager.instance.rarityWeights[3] = 20f;
        AugmentManager.instance.rarityWeights[4] = 20f;
        AugmentManager.instance.rarityWeights[5] = 20f;
    }

    public override void Remove()
    {
        AugmentManager.instance.rarityWeights[1] = 0.5f;
        AugmentManager.instance.rarityWeights[2] = 3f;
        AugmentManager.instance.rarityWeights[3] = 12.5f;
        AugmentManager.instance.rarityWeights[4] = 32f;
        AugmentManager.instance.rarityWeights[5] = 52f;
    }
}

/*
 * 블럭이 받는 데미지 (1.1^부식 스택) 만큼 증가
 */
public class DecayBall : SpecialAugment
{
    private float DecayDuration;

    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "DecayBall", "DecayBall", "Effect", 4);
        DecayDuration = 1f;
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.DecayChance)
        {
            Brick.EffectStatusSet["Decay"].EffectStack += (Brick.EffectStatusSet["Decay"].EffectStack != Brick.EffectStatusSet["Decay"].EffectStackLimit) ? 1 : 0; 
            StartCoroutine(Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + DecayDuration));
        }
    }
}

public class PowerfulDecay : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_19", "PowerfulDecay", "PowerfulDecay", "Effect", 3);
    }

    public override void Apply()
    {
        BrickInfo brickInfo = BrickManager.Instance.BrickInfo;
        foreach (IBrick brick in brickInfo.Bricks)
        {
            brick.DecayDamageMultiple = 1.15f;
        }
        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            brick.DecayDamageMultiple = 1.15f;
        }
        AugmentManager.instance.effectChance.DecayChance += 0.1f;
    }

    public override void Remove()
    {
        BrickInfo brickInfo = BrickManager.Instance.BrickInfo;
        foreach (IBrick brick in brickInfo.Bricks)
        {
            brick.DecayDamageMultiple = 1.1f;
        }
        foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
        {
            brick.DecayDamageMultiple = 1.1f;
        }
        AugmentManager.instance.effectChance.DecayChance -= 0.1f;
    }
}

/*
 * 부식 스택이 최대인 상태로 블럭이 깨지면 폭발함
 */
public class DecayExplosion : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_20", "DecayExplosion", "DecayExplosion", "Effect", 2);
    }

    public override void Apply()
    {
        AugmentManager.instance.effectChance.DecayChance += 0.1f;
    }

    public override void Remove() 
    {
        AugmentManager.instance.effectChance.DecayChance += 0.1f;
    }
}

/*
 * 블럭과 블럭이 맞닿으면 서로 반대방향으로 힘을 받음. 추가로 이 증강이랑 바운스=파워 증강 같이 먹으면 벽이 아니라 공이랑 부딪혀도 바운스스택 올라가게 함
 */
public class ExplosionWithBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_21", "ExplosionWithBall", "ExplosionWithBall", "Bounce", 5);
    }

    public override void Apply() { }

    public override void Remove() { }
}