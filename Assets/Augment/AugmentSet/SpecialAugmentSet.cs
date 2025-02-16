using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using Unity.VisualScripting;
using System.Linq;

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
        FireDuration = 3f;
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.EffectDamage += FireDamage;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<FireEffectStackBlast>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<TheFlamethrower>());
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
            Brick.EffectStatusSet["Fire"].EffectStack += (Brick.EffectStatusSet["Fire"].EffectStack != Brick.EffectStatusSet["Fire"].EffectStackLimit) ? 1 : 0;
            Brick.Fire = StartCoroutine(Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + FireDuration));
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
            StartCoroutine(Brick.AdditionalAttack(0.2f, 0.1f, 0.2f));
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
            Brick.AdditionalAttack(0.5f, 0.5f, 1);
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
        float BlastDamage = StatManager.instance.Temporary_stat.EffectDamage * 20;
        if (Brick.EffectStatusSet["Fire"].EffectStack == Brick.EffectStatusSet["Fire"].EffectStackLimit)
        {
            Brick.EffectStatusSet["Fire"].EffectStack = (int)(Brick.EffectStatusSet["Fire"].EffectStack * 0.3);
            foreach (IBrick nearbyBrick in Brick.NearbyBricks)
            {
                nearbyBrick.EffectStatusSet["Fire"].EffectStack += 
                    (nearbyBrick.EffectStatusSet["Fire"].EffectStack != Brick.EffectStatusSet["Fire"].EffectStackLimit) ? 2 : 0;
                nearbyBrick.ArbitraryDamageAttack(BlastDamage
                    ,
                    false,
                    BlastDamage.ToString("F1"), 
                    "Fire");
                nearbyBrick.Fire = StartCoroutine(nearbyBrick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + FireDuration));
            }
            VisualEffectManager.Instance.SpawnParticle("FireBlast", Brick.transform.position, 30);
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

    public override void AfterDeathEffect(Ball ball)
    {
        LevelManager.Instance.GetExp(30f);
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

    public override void AfterDeathEffect(Ball ball)
    {
        StatManager.instance.GetGold(18);
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
        PoisonDuration = 1.4f;
    }

    public override void Apply() 
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<InfinitePoison>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<DeadlyPoison>());
    }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.PoisonChance)
        {
            Brick.EffectStatusSet["Poison"].EffectStack += (Brick.EffectStatusSet["Poison"].EffectStack < Brick.EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
            Brick.Poison = StartCoroutine(Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration + PoisonDuration));
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
        AugmentManager.instance.PoisonStackLimit = 5;
        AugmentManager.instance.effectChance.PoisonChance += 0.1f;
    }

    public override void Remove()
    {
        AugmentManager.instance.PoisonStackLimit = 3;
        AugmentManager.instance.effectChance.PoisonChance -= 0.1f;
    }
}

/*
 * 모든 증강 확률 동일하게 만듦(0티어는 대격변급이어서 걍 여전히 줄임)
 */
public class Chaos : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "Chaos", "Chaos", "Special", 0);
    }

    public override void Apply()
    {
        AugmentManager.instance.rarityWeights[0] = 0.1f;
        AugmentManager.instance.rarityWeights[1] = 19.9f;
        AugmentManager.instance.rarityWeights[2] = 20f;
        AugmentManager.instance.rarityWeights[3] = 20f;
        AugmentManager.instance.rarityWeights[4] = 20f;
        AugmentManager.instance.rarityWeights[5] = 20f;
    }

    public override void Remove()
    {
        AugmentManager.instance.rarityWeights[0] = 0.01f;
        AugmentManager.instance.rarityWeights[1] = 0.59f;
        AugmentManager.instance.rarityWeights[2] = 3.9f;
        AugmentManager.instance.rarityWeights[3] = 11.5f;
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

    public override void Apply() 
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<PowerfulDecay>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<DecayExplosion>());
    }

    public override void Remove() { }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.DecayChance)
        {
            print(Brick.EffectStatusSet["Decay"].EffectStack);
            Brick.EffectStatusSet["Decay"].EffectStack += (Brick.EffectStatusSet["Decay"].EffectStack != Brick.EffectStatusSet["Decay"].EffectStackLimit) ? 1 : 0;
            Brick.Decay = StartCoroutine(Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + DecayDuration));
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
        AugmentManager.instance.DecayDamageMultiple = 1.15f;
        AugmentManager.instance.effectChance.DecayChance += 0.1f;
    }

    public override void Remove()
    {
        AugmentManager.instance.DecayDamageMultiple = 1.1f;
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

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if (Brick.EffectStatusSet["Decay"].EffectStack == Brick.EffectStatusSet["Decay"].EffectStackLimit)
        {
            Brick.IsNeedDelayedDIeForEffect = true;
            VisualEffectManager.Instance.SpawnParticle("DecayExplosion", Brick.transform.position, 1);
            float explosionRadius = 3f;
            LayerMask explosionLayer = LayerMask.GetMask("Ball");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayer);

            foreach (Collider2D ball in colliders)
            {
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 explosionDirection = (rb.position - (Vector2)transform.position).normalized;
                    float distance = Vector2.Distance(rb.position, transform.position);
                    float forceMultiplier = Mathf.Clamp01(1 - (distance / explosionRadius));

                    rb.AddForce(explosionDirection * 5f * forceMultiplier, ForceMode2D.Impulse);
                }
            }
        }
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

public class TheFlamethrower : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_22", "TheFlamethrower", "TheFlamethrower", "Effect", 2);
    }

    public override void Apply() 
    {
        foreach (Ball ball in BallManager.Instance.InGameBall)
        {
            ball.flamethrower.FlamethrowerOn();
        }
        BallManager.Instance.isOnFlamerower = true;
    }

    public override void Remove() 
    {
        BallManager.Instance.isOnFlamerower = false;
    }
}
/*
 * 즉시 경험치 30% 획득, 블럭 20개 격파하면 레벨업 (1회)
 */
public class LevelUpQuest : SpecialAugment
{
    private int KilledEnemy;
    private bool isCleared = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_23", "LevelUpQuest", "LevelUpQuest", "Quest", 5);
    }

    public override void Apply()
    {
        Debug.Log(LevelManager.Instance.Exp);
        LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.5f;
        Debug.Log(LevelManager.Instance.Exp);
    }

    public override void Remove(){ }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        KilledEnemy++;
        Debug.Log(KilledEnemy);
        if(KilledEnemy >= 20 && !isCleared)
        {
            isCleared = true;
            LevelManager.Instance.LevelUp();
        }
    }
}

/*
 * 블럭 20개 격파할때까지 죽인 적 타입에 따라 float 누적, 클리어하면 누적치 +2 해서 공격력업
 */
public class GrowBallQuest : SpecialAugment
{
    private int KilledEnemy;
    private float KeepAtk;
    private bool isCleared = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_24", "GrowBallQuest", "GrowBallQuest", "Quest", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        KilledEnemy++;
        if (Brick.RecentlyBall != null && !isCleared)
        {
            QuestProgressing(Brick);
            if(KilledEnemy >= 20)
            {
                StatManager.instance.Temporary_stat.AttackDamage += (KeepAtk + 2);
                StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
                isCleared = true;
            }
        }
    }

    public void QuestProgressing(IBrick brick)
    {
        if (brick.IsBoss)
        {
            switch (brick.BrickType)
            {
                case "HardBrick":
                    KeepAtk += 0.4f;
                    break;
                case "IgnoreBrick":
                    KeepAtk += 0.45f;
                    break;
                case "Twinbrick":
                    KeepAtk += 0.45f;
                    break;
                case "BlackBrick":
                    KeepAtk += 0.125f;
                    break;
                default:
                    KeepAtk += 0.1f;
                    break;
            }
        }
        else
        {
            switch (brick.BrickType)
            {
                case "HardBrick":
                    KeepAtk += 0.05f;
                    break;
                case "IgnoreBrick":
                    KeepAtk += 0.06f;
                    break;
                case "Twinbrick":
                    KeepAtk += 0.06f;
                    break;
                case "BlackBrick":
                    KeepAtk += 0.02f;
                    break;
                default:
                    KeepAtk += 0.04f;
                    break;
            }
        }
    }
}

public class InfiniteGrowBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_25", "InfiniteGrowBall", "InfiniteGrowBall", "Stat", 3);
    }

    public override void Apply() 
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<ShareBallAtk>());
    }

    public override void Remove() { }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        Brick.RecentlyBall.Grow(Brick);
    }
}

/*
 * 공 일정 희생시키면 미약한 버프, 증강 풀에 SacrificeQuest2 추가
 */
public class SacrificeQuest1 : SpecialAugment
{
    private int SacrificeNumber;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_26", "SacrificeQuest1", "SacrificeQuest1", "Quest", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(Ball ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber >= 20 && !isCleared)
        {
            StatManager.instance.statMultiplier.AttackDamageMultiplier *= 1.1f;
            StatManager.instance.statMultiplier.EffectDamageMultiplier *= 1.1f;
            StatManager.instance.SaveStatMultiplier(StatManager.instance.statMultiplier);

            AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<SacrificeQuest2>());
            Debug.Log("SacrificeQuest1 Clear");
            isCleared = true;
        }
    }
}

/*
 * 공 일정 희생시키면 중간 버프, 증강 풀에 SacrificeQuest3 추가
 */
public class SacrificeQuest2 : SpecialAugment
{
    private int SacrificeNumber;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_27", "SacrificeQuest2", "SacrificeQuest2", "Quest", 3);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(Ball ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber >= 50 && !isCleared)
        {
            StatManager.instance.Temporary_stat.AttackDamage += 10;
            StatManager.instance.Temporary_stat.EffectDamage += 2;
            StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
            StatManager.instance.Persistent_stat.Gold += 50 * StatManager.instance.Temporary_stat.GoldGain;

            Debug.Log("SacrificeQuest2 Clear");
            AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<SacrificeQuest3>());
            isCleared = true;
        }
    }
}

/*
 * 공 일정 희생시키면 이후 증강 먹는 타이밍에 1회 확정으로 1티어 증강 3개 등장
 */
public class SacrificeQuest3 : SpecialAugment
{
    private int SacrificeNumber;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_28", "SacrificeQuest", "SacrificeQuest", "Quest", 2);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(Ball ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber >= 70 && !isCleared)
        {
            Debug.Log("SacrificeQuest3 Clear");
            AugmentManager.instance.isSacrificeQuest3Cleared = true;
            isCleared = true;
        }
    }
}
/*
 * 50회 타격시 추가 공 하나
 */
public class AttackAfterAdditionalBall : SpecialAugment
{
    private int attackNumber;
    private int AttackNumber
    {
        get
        {
            return attackNumber;
        }
        set
        {
            attackNumber = value;
            if(attackNumber >= 50)
            {
                BallManager.Instance.CreateBall(1);
                attackNumber = 0;
            }
        }
    }
    private void OnEnable()
    {
        SetAugment("SPECIAL_29", "AttackAfterAdditionalBall", "AttackAfterAdditionalBall", "Ball", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterAttackEffect(IBrick Brick)
    {
        AttackNumber++;
    }
}
public class ShareBallAtk : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_30", "ShareBallAtk", "ShareBallAtk", "Ball", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(Ball ball)
    {
        float ShareAtk = ball.GrowthAttackDamage *= 0.7f;
        foreach (Ball inGameBall in BallManager.Instance.InGameBall)
        {
            inGameBall.GrowthAttackDamage += ShareAtk / BallManager.Instance.InGameBall.Count;
        }
    }
}
// 확률에 따른 강한 추가타(어택데미지)
public class StrongAdditionalAttack : SpecialAugment
{
    float StrongAttackChance = 0.1f;
    float SuperStrongAttackChance = 0.01f;
    float SuperUltraStrongAttackChance = 0.001f;
    private void OnEnable()
    {
        SetAugment("SPECIAL_31", "Strong Add Atack", "Strong Add Atack", "AdditionalAttack", 2);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float RandomValue = Random.value;
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick")
        {
            if (RandomValue < SuperUltraStrongAttackChance)
                StartCoroutine(Brick.AdditionalAttack(0.1f, 0.1f, 500f));
            else if (RandomValue < SuperStrongAttackChance)
                StartCoroutine(Brick.AdditionalAttack(0.1f, 0.1f, 100f));
            else if (RandomValue < StrongAttackChance)
                StartCoroutine(Brick.AdditionalAttack(0.1f, 0.1f, 10f));
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
        { "BossSpawn", 0.01f },
        { "Execution", 0.01f },
    };
    private void OnEnable()
    {
        SetAugment("SPECIAL_32", "RandomEffect", "RandomEffect", "AdditionalAttack", 3);
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
                Debug.Log("WeakAdditionalDamage");
                StartCoroutine(Brick.AdditionalAttack(0.1f, 0.1f, 0.5f));
                return;
            }

            accumulatedProbability += ProbabilitySet["AdditionalDamage"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("AdditionalDamage");
                StartCoroutine(Brick.AdditionalAttack(0.1f, 0.1f, 0.8f));
                return;
            }

            accumulatedProbability += ProbabilitySet["Nothing"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("Nothing");
                return;
            }

            accumulatedProbability += ProbabilitySet["RandomEffect"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("RandomEffect");
                int Number = Random.Range(1, 4);
                if (Number == 1)
                {
                    Brick.EffectStatusSet["Fire"].EffectStack += (Brick.EffectStatusSet["Fire"].EffectStack != Brick.EffectStatusSet["Fire"].EffectStackLimit) ? 1 : 0;
                    Brick.Fire = StartCoroutine(Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + 1f));
                }
                else if (Number == 2)
                {
                    Brick.EffectStatusSet["Poison"].EffectStack += (Brick.EffectStatusSet["Poison"].EffectStack < Brick.EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
                    Brick.Poison = StartCoroutine(Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration + 1f));
                }
                else if (Number == 3)
                {
                    Brick.EffectStatusSet["Decay"].EffectStack += (Brick.EffectStatusSet["Decay"].EffectStack != Brick.EffectStatusSet["Decay"].EffectStackLimit) ? 1 : 0;
                    Brick.Decay = StartCoroutine(Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + 1f));
                }
                return;
            }

            accumulatedProbability += ProbabilitySet["FullHealth"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("FullHealth");
                Brick.HP = Brick.HpTemp;
                return;
            }

            accumulatedProbability += ProbabilitySet["BrickSpawn"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("BrickSpawn");
                BrickManager.Instance.SpawnBrick();
                return;
            }

            accumulatedProbability += ProbabilitySet["BossSpawn"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("BossSpawn");
                BrickManager.Instance.SpawnBoss();
                return;
            }

            accumulatedProbability += ProbabilitySet["Execution"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("Execution");
                Brick.HP -= 999999 + Brick.HpTemp;
                Brick.ShowText("EXECUTION", null, true);
            }
        }
    }
}

public class OneLineBall : SpecialAugment
{
    private List<List<Vector3>> PositionSet = new List<List<Vector3>>();
    bool isGetThisAugment = false;
    bool turnOn = false;

    private void OnEnable()
    {
        SetAugment("SPECIAL_33", "OneLineBall", "OneLineBall", "Special", 2);
    }

    public override void Apply() 
    {
        int BallLayer = LayerMask.NameToLayer("Ball");
        Physics2D.IgnoreLayerCollision(BallLayer, BallLayer, true);
        isGetThisAugment = true;
    }

    public override void Remove() 
    {
        int BallLayer = LayerMask.NameToLayer("Ball");
        Physics2D.IgnoreLayerCollision(BallLayer, BallLayer, false);
    }

    private void Update()
    {
        if (isGetThisAugment)
        {
            while (PositionSet.Count < BallManager.Instance.inGameBallPosition.Count)
            {
                PositionSet.Add(new List<Vector3>());
            }
            if (turnOn && BallManager.Instance.inGameBallPosition.Count >= 1)
            {
                BallManager.Instance.inGameBallPosition[0].GetComponent<SpriteRenderer>().color = Color.blue;
                turnOn = false;
            }
            if (BallManager.Instance.inGameBallPosition.Count >= 2)
            {
                for (int i = 0; i < BallManager.Instance.inGameBallPosition.Count; i++)
                {
                    PositionSet[i].Add(BallManager.Instance.inGameBallPosition[i].position);
                    if (PositionSet[i].Count > Mathf.RoundToInt(0.02f / Time.deltaTime))
                    {
                        PositionSet[i].RemoveAt(0);
                    }
                }

                for (int i = 1; i < BallManager.Instance.inGameBallPosition.Count; i++)
                {
                    int previousIndex = i - 1;

                    if (PositionSet[previousIndex].Count > 0)
                    {
                        Vector3 delayedPosition = PositionSet[previousIndex][0];
                        BallManager.Instance.inGameBallPosition[i].position = delayedPosition;
                    }
                }
            }
            else
            {
                turnOn = true;
            }
        }
    }
}