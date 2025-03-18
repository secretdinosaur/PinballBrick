using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using Unity.VisualScripting;
using System.Linq;
using Random = UnityEngine.Random;

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
 * Ȯ���� ���� �� ����Ʈ ȿ�� ����, �ش� ���� �� ����Ʈ ������ �ִ�ġ�� �ƴ϶�� +1
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
            if(Brick.Fire == null)
            {
                Brick.Fire = StartCoroutine(Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + FireDuration)); 
            }
        }
    }
}

/**
 * ���� �ֺ� ���� ã�Ƽ� ����
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

    public override void Remove()
    {

    }
}

/**
 * ���� ƨ�涧���� �ٿ Ƚ�� ����(������ ����)
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
 * ���� ���Ŀ� 3ȸ �߰�Ÿ�߻�
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
            StartCoroutine(Brick.AdditionalAttack(0.25f, 0.1f, 0.2f));
        }
    }
}


/**
 * ũ��Ƽ�� ���� ���Ŀ� �߰�Ÿ 1ȸ �߻�
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
 * ���� ü�� ���� �� ó��
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
 * �� �ִ�ü�� �ۼ�Ʈ ������
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
 * ���� �� ����Ʈ ������ �ִ��� �����ϸ� ���� �ֺ� ���� (�� �ִ뽺�� * 3) ��ŭ�� ������ ���̽�Ű�� �� ����Ʈ ȿ�� �ߵ�
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
 * ���ݹ��� ���� ������ �ֺ� 4���� ������ ���̵Ǵ� ����
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
            
            BrickToBrick[RandomNumber].ArbitraryDamageAttack(1 + (BrickToBrick[RandomNumber].HpTemp * Random.Range(0.01f, 0.02f)), 
                false, (1 * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"), "Lightning");
            BrickToBrick[RandomNumber].NearbyBricks.Remove(targetBrick);

            targetBrick = BrickToBrick[RandomNumber];

        }
        VisualEffectManager.Instance.SpawnChainLightning(OrderedTargetBricks);
    }
}

/**
 * ���� ������ ����ġ ȹ��
 */
public class DeathExp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_10", "Death = Exp", "Death = Exp", "Death", 5);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect(IBall ball)
    {
        LevelManager.Instance.GetExp(30f);
    }
}

/**
 * ���� ������ ��� ȹ��
 */
public class DeathGold : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_11", "Death = Gold", "Death = Gold", "Death", 5);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void AfterDeathEffect(IBall ball)
    {
        StatManager.instance.GetGold(18);
    }
}

/**
 * �ӵ��� ���� ���� ���ݷ� ����
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
 * �ӵ��� ���� ���� ũ��Ƽ�� Ȯ�� ����
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
 * Ÿ�ݽ� ���� ����
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
        brick.DamageByLightningEffect(1f, 1);
    }
}

/*
 * N�ʵ��� M�� �ֱ�� �� ����ü�� ��� ������
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
            if(Brick.Poison == null)
            {
                Brick.Poison = StartCoroutine(Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration + PoisonDuration)); 
            }
        }
    }
}

/*
 * When poisoned, there is a certain chance to increase the Poison Effect stack by 1 and refresh its duration.
 * If the stack reaches 3, the poison spreads to nearby blocks.
 */
public class InfinitePoison : SpecialAugment
{
    bool isGetAugment = false;
    Coroutine Routine = null;
    private void OnEnable()
    {
        SetAugment("SPECIAL_16", "InfinitePoison", "InfinitePoison", "Effect", 2);
    }

    public override void Apply()
    {
        isGetAugment = true;
        AugmentManager.instance.effectChance.PoisonChance += 0.1f;
        Routine = StartCoroutine(GetInfinitePoison());
    }

    public override void Remove() 
    {
        isGetAugment = false;
        AugmentManager.instance.effectChance.PoisonChance -= 0.1f;
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
                    if (brick.NearbyBricks.Count > 0 && Random.value < 0.25f)
                    {
                        List<IBrick> ChosenBricks = new List<IBrick>(brick.NearbyBricks);
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

                                Chosen.EffectStatusSet["Poison"].EffectStack +=
                                    (Chosen.EffectStatusSet["Poison"].EffectStack != Chosen.EffectStatusSet["Poison"].EffectStackLimit) ? 1 : 0;
                                Chosen.Poison = Chosen.StartCoroutine(Chosen.DamageByPoisonEffect(brick.initialDuration));
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
 * ��� ���� Ȯ�� �����ϰ� ����(0Ƽ��� ��ݺ����̾ �� ������ ����)
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
 * ���� �޴� ������ (1.1^�ν� ����) ��ŭ ����
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
            Brick.EffectStatusSet["Decay"].EffectStack += (Brick.EffectStatusSet["Decay"].EffectStack != Brick.EffectStatusSet["Decay"].EffectStackLimit) ? 1 : 0;
            if(Brick.Decay == null)
            { 
                Brick.Decay = StartCoroutine(Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + DecayDuration));
            }
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
 * �ν� ������ �ִ��� ���·� ���� ������ ������
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
 * ���� ���� �´����� ���� �ݴ�������� ���� ����. �߰��� �� �����̶� �ٿ=�Ŀ� ���� ���� ������ ���� �ƴ϶� ���̶� �ε����� �ٿ���� �ö󰡰� ��
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
        foreach (IBall ball in BallManager.Instance.InGameBall)
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
 * ��� ����ġ 30% ȹ��, �� 20�� �����ϸ� ������ (1ȸ)
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
 * �� 20�� �����Ҷ����� ���� �� Ÿ�Կ� ���� float ����, Ŭ�����ϸ� ����ġ +2 �ؼ� ���ݷ¾�
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
 * �� ���� �����Ű�� �̾��� ����, ���� Ǯ�� SacrificeQuest2 �߰�
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

    public override void AfterDeathEffect(IBall ball)
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
 * �� ���� �����Ű�� �߰� ����, ���� Ǯ�� SacrificeQuest3 �߰�
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

    public override void AfterDeathEffect(IBall ball)
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
 * �� ���� �����Ű�� ���� ���� �Դ� Ÿ�ֿ̹� 1ȸ Ȯ������ 1Ƽ�� ���� 3�� ����
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

    public override void AfterDeathEffect(IBall ball)
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
 * 50ȸ Ÿ�ݽ� �߰� �� �ϳ�
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

    public override void AfterDeathEffect(IBall ball)
    {
        float ShareAtk = ball.GrowthAttackDamage *= 0.7f;
        foreach (IBall inGameBall in BallManager.Instance.InGameBall)
        {
            inGameBall.GrowthAttackDamage += ShareAtk / BallManager.Instance.InGameBall.Count;
        }
    }
}
// Ȯ���� ���� ���� �߰�Ÿ(���õ�����)
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

public class RebirthBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_34", "RebirthBall", "RebirthBall", "Ball", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterDeathEffect(IBall ball)
    {
        if (Random.value < 0.05f)
        {
            BallManager.Instance.CreateSpecificBall(AugmentManager.instance.SpawnBallAmount, ball.BallType);
        }
    }
}

public class BossKillBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_35", "BossKillBall", "BossKillBall", "Ball", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            BallManager.Instance.CreateBall(AugmentManager.instance.SpawnBallAmount);
        }
    }
}

public class SpawnBallAmountMultiple1 : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_36", "SpawnBallAmountMultiple1", "SpawnBallAmountMultiple1", "Ball", 2);
    }

    public override void Apply() 
    {
        AugmentManager.instance.SpawnBallAmount *= 2;
    }

    public override void Remove()
    {
        AugmentManager.instance.SpawnBallAmount /= 2;
    }
}

public class SpawnBallAmountMultiple2 : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_37", "SpawnBallAmountMultiple2", "SpawnBallAmountMultiple2", "Ball", 1);
    }

    public override void Apply()
    {
        AugmentManager.instance.SpawnBallAmount *= 3;
    }

    public override void Remove()
    {
        AugmentManager.instance.SpawnBallAmount /= 3;
    }
}

public class BallSwarm : SpecialAugment
{
    bool isGetAugment = false;
    bool Upgrade = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_38", "BallSwarm", "BallSwarm", "Ball", 4);
    }

    public override void Apply()
    {
        isGetAugment = true;
    }

    public override void Remove()
    {
        isGetAugment = false;
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if(BallManager.Instance.InGameBall.Count >= 30 && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.Temporary_stat.AttackDamage += 2;
                StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
            }
            else if(BallManager.Instance.InGameBall.Count < 30 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.Temporary_stat.AttackDamage -= 2;
                StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
            }
        }
    }
}

public class LonelyBall : SpecialAugment
{
    bool isGetAugment = false;
    bool Upgrade = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_39", "LonelyBall", "LonelyBall", "Ball", 4);
    }

    public override void Apply()
    {
        isGetAugment = true;
    }

    public override void Remove()
    {
        isGetAugment = false;
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if (BallManager.Instance.InGameBall.Count <= 5 && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.Temporary_stat.AttackDamage += 2;
                StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
            }
            else if (BallManager.Instance.InGameBall.Count > 5 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.Temporary_stat.AttackDamage -= 2;
                StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
            }
        }
    }
}

public class SuicideBoomBall : SpecialAugment
{
    float damage = 50;
    private void OnEnable()
    {
        SetAugment("SPECIAL_40", "SuicideBoomBall", "SuicideBoomBall", "Ball", 1);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        IBall ball = Brick.RecentlyBall;
        if(Random.value < 0.1f)
        {
            Brick.ArbitraryDamageAttack(
                    (damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                    false,
                    ((damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"),
                    null);
            foreach (IBrick brick in Brick.NearbyBricks)
            {
                brick.ArbitraryDamageAttack(
                    (damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier,
                    false,
                    ((damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier).ToString("F1"),
                    null);
            }
            ball.BallDied();
            if(Random.value < 0.4f)
            {
                BallManager.Instance.CreateSpecificBall(AugmentManager.instance.SpawnBallAmount, ball.BallType);
            }
        }
    }
}

public class SplitBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_41", "SplitBall", "SplitBall", "Ball", 3);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if(Random.value < 0.01f)
        {
            IBall RecentlyBall = Brick.RecentlyBall;
            CreateBall(AugmentManager.instance.SpawnBallAmount, RecentlyBall);
        }
    }

    public void CreateBall(int Num, IBall RecentlyBall)
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
            if(ball.linearVelocityYValue >= 0)
            {
                ball.linearVelocityYValue += 3f;
            }
            else
            {
                ball.linearVelocityYValue -= 3f;
            }
            ball.gameObject.SetActive(true);
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
        SetAugment("SPECIAL_42", "CurseBall", "CurseBall", "Death", 4);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void AfterDeathEffect(IBall ball)
    {
        if (Random.value < AugmentManager.instance.effectChance.CurseChance)
        {
            for (int i = 0; i < AugmentManager.instance.CurseActiveNumber; i++)
            {
                int index = Random.Range(0, BrickManager.Instance.CurrentBricks.Count);
                if (BrickManager.Instance.CurrentBricks.Count != 0)
                    while (BrickManager.Instance.CurrentBricks[index].RecentlyBall == null)
                    {
                        index = Random.Range(0, BrickManager.Instance.CurrentBricks.Count);
                    }
                BrickManager.Instance.CurrentBricks[index].DamageByCurse();
            }
        }
    }
}

public class ThreeBrickCurseAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_43", "ThreeBrickCurseAttack", "ThreeBrickCurseAttack", "Death", 3);
    }

    public override void Apply() 
    {
        AugmentManager.instance.effectChance.CurseChance += 0.1f;
    }

    public override void Remove() 
    {
        AugmentManager.instance.effectChance.CurseChance -= 0.1f;
    }

    public override void AfterDeathEffect(IBall ball)
    {
        if(Random.value < AugmentManager.instance.effectChance.CurseChance)
        {
            for (int i = 0; i < AugmentManager.instance.CurseActiveNumber * 3; i++)
            {
                int index = Random.Range(0, BrickManager.Instance.CurrentBricks.Count);
                if (BrickManager.Instance.CurrentBricks.Count != 0)
                {
                    BrickManager.Instance.CurrentBricks[index].DamageByCurse();
                }
            }
        }     
    }
}

public class CurseTwice : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_44", "CurseTwice", "CurseTwice", "Death", 2);
    }

    public override void Apply() 
    {
        AugmentManager.instance.CurseActiveNumber = 2;
    }

    public override void Remove() 
    {
        AugmentManager.instance.CurseActiveNumber = 1;
    }
}

public class DeathEffectTwice : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_45", "DeathEffectTwice", "DeathEffectTwice", "Death", 1);
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
        SetAugment("SPECIAL_46", "SpawnBombBall", "SpawnBombBall", "Ball", 3);
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<RebornBombBall>());
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").AddComponent<RebornBombBall>());
    }
}

public class ChangeNormalToGlass : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_47", "ChangeNormalToGlass", "ChangeNormalToGlass", "Ball", 3);
    }

    public override void Apply()
    {
        BallManager.Instance.NormalBall = "GlassBall";
        StatManager.instance.Temporary_stat.AttackDamage += 2f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        BallManager.Instance.NormalBall = "Ball";
        StatManager.instance.Temporary_stat.AttackDamage -= 2f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class RebornBombBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_48", "RebornBombBall", "RebornBombBall", "Ball", 1);
    }

    public override void Apply() { }

    public override void Remove() { }
}