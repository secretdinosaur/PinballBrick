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
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<FireEffectStackBlast>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<TheFlamethrower>());
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<FireEffectStackBlast>());
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<TheFlamethrower>());
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.FireChance)
        {
            Brick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.FireDuration); 
        }
    }
}

public class Homing : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_2", "Magnetic Ball", "Ball chases bricks within range", "Special", 1);
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
        SetAugment("SPECIAL_3", "Thrust", "Ball gains +5% attack damage for each wall bounce up to a maximum of 25%", "Special", 3);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.RecentlyBall.BounceTime * StatManager.instance.statMultiplier.AttackDamageMultiplier * StatManager.instance.Persistent_stat.AttackDamageMultiplier;
        Brick.RecentlyBall.BounceTime = 1;
    }
}

public class AdditionalThreeAttack: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_4", "Afterimage", "Rapid three additional attacks follow after ball hits", "Special", 1);
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

public class Execution : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_6", "Execution", "THe ball executes bricks with less than 15% of max HP", "Special", 1);
    }

    public override void Apply(){ }

    public override void Remove(){ }
}

public class PercentDamage : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_7", "Rupture", "The ball deals 1% of brick's HP with each attack", "Special", 3);
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
        SetAugment("SPECIAL_8", "Fire Burst", "An explosion occurs when the brick reaches maximum fire stack", "Effect", 3);
    }

    public override void Apply()
    {
        
    }

    public override void Remove(){ }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        float BlastDamage = StatManager.instance.Temporary_stat.EffectDamage * 20;
        if (Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack == Brick.BrickEffect.EffectStatusSet["Fire"].EffectStackLimit)
        {
            Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack = (int)(Brick.BrickEffect.EffectStatusSet["Fire"].EffectStack * 0.3);
            foreach (IBrick nearbyBrick in Brick.NearbyBricks)
            {
                nearbyBrick.ArbitraryDamageAttack(BlastDamage
                    ,
                    false,
                    BlastDamage.ToString("F1"),
                    "Explosion");
                nearbyBrick.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.FireDuration);
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
        SetAugment("SPECIAL_9", "Chain Lightning", "Lightning chains to 3 nearby bricks", "Effect", 3);
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
                false, (1 * StatManager.instance.statMultiplier.EffectDamageMultiplier * StatManager.instance.Persistent_stat.EffectDamageMultiplier).ToString("F1"), "Lightning");
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
        SetAugment("SPECIAL_10", "Death EXP", "Gain EXP when the ball drops", "Special", 5);
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
        SetAugment("SPECIAL_11", "Death Gold", "Gain Gold when the ball drops", "Special", 5);
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
        SetAugment("SPECIAL_12", "Momentum", "Atack damage increases with ball speed", "Special", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.RecentlyBall.TransformationMultiple 
            * StatManager.instance.statMultiplier.AttackDamageMultiplier * StatManager.instance.Persistent_stat.AttackDamageMultiplier;
    }
}

/**
 * �ӵ��� ���� ���� ũ��Ƽ�� Ȯ�� ����
 */
public class SpeedCritical : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_13", "Quick and Accurate", "Critical chance increases with ball speed", "Special", 4);
    }

    public override void Apply(){ }

    public override void Remove(){ }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        Brick.TotalMultiple = Brick.RecentlyBall.TransformationMultiple 
            * StatManager.instance.statMultiplier.AttackDamageMultiplier * StatManager.instance.Persistent_stat.AttackDamageMultiplier;

    }
}

/**
 * Ÿ�ݽ� ���� ����
 */

public class Lightning : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_14", "Lightning Attack", "An additional lightning strike occurs on attack", "Effect", 5);
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
    private void OnEnable()
    {
        SetAugment("SPECIAL_15", "Poison Ball", "Chance to poison on hit", "Effect", 4);
    }

    public override void Apply() 
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<InfinitePoison>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<DeadlyPoison>());
        isGetAugment = false;
    }

    public override void Remove() 
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<InfinitePoison>());
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<DeadlyPoison>());
        isGetAugment = true;
    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if (Brick.gameObject.activeSelf && Brick.BrickType != "IgnoreBrick" && Brick.BrickType != "BlackBrick" &&
            Random.value < AugmentManager.instance.effectChance.PoisonChance)
        {
            Brick.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.PoisonDuration); 
        }
    }
    bool isGetAugment = false;
    private void Update()
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_18") && !isGetAugment)
        {
            AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<DecayingPoison>());
            isGetAugment = true;
        }
        else if (AugmentManager.instance.determineAugmentID("SPECIAL_1") && !isGetAugment)
        {
            AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<FlammablePoison>());
            isGetAugment = true;
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
                                Chosen.ShowText("Infection!", null, false);
                                Chosen.DamageByPoisonEffect(brick.BrickEffect.initialDuration);
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
        SetAugment("SPECIAL_17", "Deadly Poison", "Increase poison max stacks and chance", "Effect", 3);
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
public class Chaos : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "Chaos", "All augment probabilities are equal", "Special", 0);
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
public class DecayBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_18", "Decay Ball", "Chance to a decay effect that deals 10% additional damage per stack", "Effect", 4);
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
            Brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.DecayDuration);
        }
    }
}

public class PowerfulDecay : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_19", "Powerful Decay", "Increase decay damage increase rate and chance", "Effect", 3);
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

public class DecayExplosion : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_20", "Decay Explosion", "Brick explodes when die with max decay stack", "Effect", 2);
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
        if (Brick.BrickEffect.EffectStatusSet["Decay"].EffectStack + 1 == Brick.BrickEffect.EffectStatusSet["Decay"].EffectStackLimit)
        {
            Brick.IsNeedDelayedDIeForEffect = true;
            VisualEffectManager.Instance.DecayExplosionPS.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Brick.GetComponent<Renderer>().material.color);
            VisualEffectManager.Instance.SpawnParticle("DecayExplosion", Brick.transform.position, 1);
            float explosionRadius = 3f;
            LayerMask explosionLayer = LayerMask.GetMask("Ball");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Brick.transform.position, explosionRadius, explosionLayer);

            foreach (IBrick brick in Brick.NearbyBricks)
            {
                brick.ArbitraryDamageAttack(Brick.HpTemp / 20, false, (Brick.HpTemp / 20).ToString("F1"), "Explosion");
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
    }

    public override void Apply() { }

    public override void Remove() { }
}

public class TheFlamethrower : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_22", "Fire Aura", "Deals fire damage around the ball", "Effect", 2);
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
        foreach (IBall ball in BallManager.Instance.InGameBall)
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
    private int ClearRequiredCount = 10;
    private void OnEnable()
    {
        SetAugment("SPECIAL_23", "Slayer", "Gain 50% of EXP and accept a quest.\nComplete it to level up", "Quest", 5);
    }

    public override void Apply()
    {
        LevelManager.Instance.Exp += LevelManager.Instance.LevelBar.maxValue * 0.5f;

        QuestAlarm.Instance.SetQuest("Slayer", "Killed", 0, ClearRequiredCount);
    }

    public override void Remove(){ }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        KilledEnemy++;
        if(KilledEnemy <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Slayer", "Killed", KilledEnemy, ClearRequiredCount);

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
    int ClearRequiredCount = 20;
    private void OnEnable()
    {
        SetAugment("SPECIAL_24", "Grow Ball", "Accept a grow ball quest.\nComplete it to attack damage up", "Quest", 5);
    }

    public override void Apply()
    {
        QuestAlarm.Instance.SetQuest("Grow Ball", "Killed", KilledEnemy, ClearRequiredCount);
    }

    public override void Remove() { }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        KilledEnemy++;
        if (KilledEnemy <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Grow Ball", "Killed", KilledEnemy, ClearRequiredCount);
        if (Brick.RecentlyBall != null && !isCleared)
        {
            QuestProgressing(Brick);
            if(KilledEnemy >= ClearRequiredCount)
            {
                StatManager.instance.Temporary_stat.AttackDamage += (KeepAtk + 2);
                StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
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
        SetAugment("SPECIAL_25", "Infinite Grow Ball", "The ball's attack power increases after destroying bricks", "Special", 3);
    }

    public override void Apply() 
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<ShareBallAtk>());
    }

    public override void Remove() 
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<ShareBallAtk>());
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
    int ClearRequiredCount = 20;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_26", "Sacrifice I", "Accept a sacrifice I.\nComplete it to Receive a weak reward", "Quest", 5);
    }

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sacrifice I", "Sacrificed", SacrificeNumber, ClearRequiredCount); }

    public override void Remove() 
    {
        if (isCleared) 
        {
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-3f);
            StatManager.instance.ChangeTemporaryStats_EffectDamage(-1.5f);
        }
    }

    public override void AfterDeathEffect(IBall ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Sacrifice I", "Sacrificed", SacrificeNumber, ClearRequiredCount);
        if (SacrificeNumber >= ClearRequiredCount && !isCleared)
        {
            StatManager.instance.ChangeTemporaryStats_AttackDamage(3f);
            StatManager.instance.ChangeTemporaryStats_EffectDamage(1.5f);

            AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<SacrificeQuest2>());
            isCleared = true;
        }
    }
}

public class SacrificeQuest2 : SpecialAugment
{
    private int SacrificeNumber = 0;
    private bool isCleared;
    int ClearRequiredCount = 50;
    private void OnEnable()
    {
        SetAugment("SPECIAL_27", "Sacrifice II", "Accept a sacrifice II.\nComplete it to Receive a moderate reward", "Quest", 3);
    }

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sacrifice II", "Sacrificed", SacrificeNumber, ClearRequiredCount); }

    public override void Remove() 
    {
        if (isCleared)
        {
            StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.4f);
            StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.2f);
        }
    }

    public override void AfterDeathEffect(IBall ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Sacrifice II", "Sacrificed", SacrificeNumber, ClearRequiredCount);
        if (SacrificeNumber >= 50 && !isCleared)
        {
            StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.4f);
            StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.2f);
            StatManager.instance.GetGold(50);

            AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<SacrificeQuest3>());
            isCleared = true;
        }
    }
}

public class SacrificeQuest3 : SpecialAugment
{
    private int SacrificeNumber = 0;
    int ClearRequiredCount = 70;
    private bool isCleared;
    private void OnEnable()
    {
        SetAugment("SPECIAL_28", "Sacrifice III", "Accept a sacrifice III.\nComplete it to Receive a powerful reward", "Quest", 2);
    }

    public override void Apply() { QuestAlarm.Instance.SetQuest("Sacrifice III", "Sacrificed", SacrificeNumber, ClearRequiredCount); }

    public override void Remove() { }

    public override void AfterDeathEffect(IBall ball)
    {
        SacrificeNumber++;
        if (SacrificeNumber <= ClearRequiredCount)
            QuestAlarm.Instance.SetQuest("Sacrifice III", "Sacrificed", SacrificeNumber, ClearRequiredCount);
        if (SacrificeNumber >= 70 && !isCleared)
        {
            Debug.Log("SacrificeQuest3 Clear");
            AugmentManager.instance.isSacrificeQuest3Cleared = true;
            isCleared = true;
        }
    }
}
public class AttackAfterAdditionalBall : SpecialAugment
{
    private int attackNumber = 0;
    private int RequiredCount = 50;
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
        SetAugment("SPECIAL_29", "Sincerity", "A ball is created every 50 hits", "Special", 5);
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
    float StrongAttackChance = 0.1f;
    float SuperStrongAttackChance = 0.01f;
    float SuperUltraStrongAttackChance = 0.001f;
    private void OnEnable()
    {
        SetAugment("SPECIAL_31", "One in a thousand", "A powerful additional attack triggers at low chance", "Special", 2);
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
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 100f, true));
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
        { "BossSpawn", 0.01f },
        { "Execution", 0.01f },
    };
    private void OnEnable()
    {
        SetAugment("SPECIAL_32", "Random Effect Attack", "A random effect triggers on attack\nIt may be positive or negative", "Special", 3);
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
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.5f));
                return;
            }

            accumulatedProbability += ProbabilitySet["AdditionalDamage"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("AdditionalDamage");
                StartCoroutine(DamageManager.AdditionalAttack(Brick, 0.1f, 0.1f, 0.8f));
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
                    Brick.BrickEffect.DamageByFireEffect(StatManager.instance.Temporary_stat.EffectDuration + 1f);
                }
                else if (Number == 2)
                {
                    Brick.BrickEffect.DamageByPoisonEffect(StatManager.instance.Temporary_stat.EffectDuration + 1f);
                }
                else if (Number == 3)
                {
                    Brick.BrickEffect.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + 1f);
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
                BrickManager.Instance.BrickQueue.Enqueue(BrickManager.Instance.SettingBrick());
                return;
            }

            accumulatedProbability += ProbabilitySet["BossSpawn"];
            if (RandomValue < accumulatedProbability)
            {
                Debug.Log("BossSpawn");
                BrickManager.Instance.BrickQueue.Enqueue(BrickManager.Instance.SettingBoss());
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

public class ThunderStrike : SpecialAugment
{
    Coroutine routine = null;
    private void OnEnable()
    {
        SetAugment("SPECIAL_33", "Thunder Strike", "ThunderStrike", "Special", 2);
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
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            List<IBall> Balls = BallManager.Instance.InGameBall;
            if (Balls.Count >= 2)
            {
                VisualEffectManager.Instance.SpawnThunderStrike(Balls);

                for (int i = 0; i < Balls.Count - 1; i++)
                {
                    Vector2 start = Balls[i].transform.position;
                    Vector2 end = Balls[i + 1].transform.position;

                    RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
                    for(int j = 0; j < hits.Length; j++)
                        if (hits[j].collider.CompareTag("Brick"))
                        {
                            IBrick brick = hits[j].collider.GetComponent<IBrick>();

                            brick.ArbitraryDamageAttack(1 + 0.03f * brick.HpTemp, false, (1 + 0.03f * brick.HpTemp).ToString("F1"), "Lightning");
                        }
                }
            }
        }
    }
}

public class RebirthBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_34", "Immortality", "Adds a low chance to revive after the ball falls", "Special", 5);
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
        SetAugment("SPECIAL_35", "Uprising", "Summons an Three ball when the boss is defeated", "Special", 5);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterBrickDieEffect(IBrick Brick)
    {
        if (Brick.IsBoss)
        {
            BallManager.Instance.CreateBall(3);
        }
    }
}

public class SpawnBallAmountMultiple1 : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_36", "2x!", "All summoned balls are doubled", "Special", 2);
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
        SetAugment("SPECIAL_37", "3x!!", "All summoned balls are tripled", "Special", 1);
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

    int RequiredBall = 30;
    private void OnEnable()
    {
        SetAugment("SPECIAL_38", "Ball Swarm", "Attack power increases \nwhen 30 or more balls are summoned", "Special", 4);
    }

    public override void Apply()
    {
        isGetAugment = true;
    }

    public override void Remove()
    {
        isGetAugment = false;
        if (Upgrade)
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }

    private void Update()
    {
        if (isGetAugment)
        { 
            if (BallManager.Instance.InGameBall.Count >= RequiredBall && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(2f);
            }
            else if(BallManager.Instance.InGameBall.Count < 30 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
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
        SetAugment("SPECIAL_39", "Lonely Ball", "Attack power increases \nwhen 5 or less balls are summoned", "Special", 4);
    }

    public override void Apply()
    {
        isGetAugment = true;
    }

    public override void Remove()
    {
        isGetAugment = false;
        if (Upgrade)
            StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }

    private void Update()
    {
        if (isGetAugment)
        {
            if (BallManager.Instance.InGameBall.Count <= 5 && !Upgrade)
            {
                Upgrade = true;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(2f);
                BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(2, 2, 2);
            }
            else if (BallManager.Instance.InGameBall.Count > 5 && Upgrade)
            {
                Upgrade = false;
                StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
                BallManager.Instance.InGameBall[0].Renderer.sharedMaterial.color = new Color(1, 1, 1);
            }
        }
    }
}

public class SuicideBoomBall : SpecialAugment
{
    float damage = 50;
    private void OnEnable()
    {
        SetAugment("SPECIAL_40", "Explosive Ball", "A low chance for the ball to explode after an attack", "Special", 1);
    }

    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        IBall ball = Brick.RecentlyBall;
        if(Random.value < 0.1f)
        {
            VisualEffectManager.Instance.SpawnParticle("SuicideBoomBall", Brick.transform.position, 1);
            Brick.ArbitraryDamageAttack(
                    (damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier * StatManager.instance.Persistent_stat.EffectDamageMultiplier,
                    false,
                    ((damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier * StatManager.instance.Persistent_stat.EffectDamageMultiplier).ToString("F1"),
                    "Explosion");
            foreach (IBrick brick in Brick.NearbyBricks)
            {
                brick.ArbitraryDamageAttack(
                    (damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier * StatManager.instance.Persistent_stat.EffectDamageMultiplier,
                    false,
                    ((damage + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier * StatManager.instance.Persistent_stat.EffectDamageMultiplier).ToString("F1"),
                    "Explosion");
            }
            ball.BallDied();
            if(Random.value < 0.4f)
            {
                BallManager.Instance.CreateSpecificBall(1, ball.BallType);
            }
        }
    }
}

public class SplitBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_41", "Diffraction", "A low chance for the ball to split after an attack", "Special", 3);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void BeforeAttackEffect(IBrick Brick)
    {
        if(Random.value < 0.11f)
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
            StartCoroutine(SplitBallEffect(ball));
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
        SetAugment("SPECIAL_42", "Curse Ball", "When the ball falls, \nit deals curse damage to a random block", "Effect", 4);
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<ThreeBrickCurseAttack>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<CurseTwice>());
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<DeathEffectTwice>());
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<ThreeBrickCurseAttack>());
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<CurseTwice>());
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<DeathEffectTwice>());
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
                try
                {
                    BrickManager.Instance.CurrentBricks[index].DamageByCurse();
                }
                catch(ArgumentOutOfRangeException)
                {
                    Debug.Log("There are no Bricks");
                }
            }
        }
    }
}

public class ThreeBrickCurseAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_43", "Deep Grudge", "Deals 3 additional curse attacks", "Effect", 3);
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
            for (int i = 0; i < AugmentManager.instance.CurseActiveNumber * 2; i++)
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
        SetAugment("SPECIAL_44", "Persistent Grudge", "All curse attack are doubled", "Effect", 2);
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
        SetAugment("SPECIAL_45", "Die again?", "All actions performed after the ball's death are doubled", "Special", 1);
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
        SetAugment("SPECIAL_47", "Glassification", "The basic ball becomes a glass ball with higher attack", "Special", 3);
    }

    public override void Apply()
    {
        BallManager.Instance.NormalBall = "GlassBall";
        StatManager.instance.ChangeTemporaryStats_AttackDamage(2f);
    }

    public override void Remove()
    {
        BallManager.Instance.NormalBall = "Ball";
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-2f);
    }
}

public class RebornBombBall : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_48", "Bomb Modification", "Bomb ball revive a low chance after explode", "Special", 1);
    }

    public override void Apply() { }

    public override void Remove() { }
}

public class OneMoreChance : SpecialAugment
{
    bool isRevived = true;
    private void OnEnable()
    {
        SetAugment("SPECIAL_49", "One More Chance", "If no balls exist,\nsummon 5 balls at once", "Special", 2);
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
            BallManager.Instance.CreateSpecificBall(5, BallManager.Instance.NormalBall, true);
            isRevived = true;
        }
    }
}

public class Concentrate : SpecialAugment
{
    int MaxCount = 3;
    int Count = 3;
    private void OnEnable()
    {
        SetAugment("SPECIAL_50", "Concentrate", "Stats increase massively, \nbut 3 ball fall, it's game over", "Special", 0);
    }

    public override void Apply()
    {
        QuestAlarm.Instance.SetQuest("Concentrate", "GameOver In",  Count, 0);
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(2f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.7f);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(StatManager.instance.Temporary_stat.EffectDuration * 0.3f);
    }

    public override void Remove() 
    {
        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-2f);
        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.7f);
        StatManager.instance.ChangeTemporaryStats_EffectDuration(-(StatManager.instance.Temporary_stat.EffectDuration * 0.3f));
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<SpawnBombBall>());
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
}

public class GroupViolence: SpecialAugment
{
    bool isGetAugment = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_51", "Group Violence", "Only attacks the brick with the highest health", "Special", 3);
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
            BrickManager.Instance.MainBrick = BrickManager.Instance.CurrentBricks.FirstOrDefault(b => b.HP == maxHp);
        }
    }
}

public class Oneness: SpecialAugment
{
    IBall MainBall = null;
    bool isGetAugment = false;
    private void OnEnable()
    {
        SetAugment("SPECIAL_52", "Oneness", "All balls merge into one\nThe ball becomes stronger as it merges", "Special", 2);
    }

    public override void Apply() 
    {
        isGetAugment = true;
    }

    public override void Remove()
    {
        isGetAugment = false;
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

    public void Update()
    {
        if (BallManager.Instance.InGameBall.Count > 0 && isGetAugment)
        {
            if (BallManager.Instance.InGameBall.Count == 1)
            {
                MainBall = BallManager.Instance.InGameBall[0];
            }
            else if (BallManager.Instance.InGameBall.Count >= 2)
            {
                if (MainBall == null)
                    MainBall = BallManager.Instance.InGameBall[0];
                for (int i = 1; i < BallManager.Instance.InGameBall.Count; i++)
                {
                    IBall ball = BallManager.Instance.InGameBall[i];
                    if (ball.FusionMoveRoutine == null && MainBall != null)
                    {
                        ball.FusionMoveRoutine = StartCoroutine(MoveToPosition(ball, MainBall, 5f, 5f));
                    }
                }
            }
            else if(BallManager.Instance.InGameBall.Count <= 0)
            {
                MainBall = null;
            }
        }
    }

    IEnumerator MoveToPosition(IBall obj, IBall target, float duration, float smoothTime)
    {
        obj.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(obj.transform.position, target.transform.position) > 0.28f)
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
        MainBall.GrowthAttackDamage += 0.25f;
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
    private void OnEnable()
    {
        SetAugment("SPECIAL_53", "Happy Birth Day", "After the ball is summoned,\ngain a temporary boost in attack damage", "Special", 4);
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
        ball.BallBaseDamage += 30;
        ball.Renderer.material.color = ball.Renderer.material.color * 2;
        yield return new WaitForSeconds(1f);
        ball.Renderer.material.color = ball.Renderer.material.color / 2;
        ball.BallBaseDamage -= 30;
    }
}

public class MidasHand : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_54", "Midas Hand", "Attacked bricks have a low chance to\ntransform into golden blocks", "Special", 4);
    }
    Coroutine Routine = null;
    public override void Apply() { }

    public override void Remove() { }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if(Random.value < 0.1f 
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
    }

    public override void Apply() 
    {
        if(BallManager.Instance.InGameBall.Count > 1)
        {
            for(int i = 0; i < BallManager.Instance.InGameBall.Count / 2; i++)
            {
                StartCoroutine(BallRemoveEffect(BallManager.Instance.InGameBall[i]));
                AtkUpValue += 0.1f;
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

public class FreshNewStart : SpecialAugment
{
    int AugmentNum = 0;
    private void OnEnable()
    {
        SetAugment("SPECIAL_54", "Fresh New Start", "Receive a massive reward\nand return to the beginning state", "Special", 0);
    }

    public override void Apply()
    {
        AugmentNum = AugmentManager.instance.activeStatAugments.Count
                   + AugmentManager.instance.activeFieldAugments.Count
                   + AugmentManager.instance.activeSpecialAugments.Count;
        AugmentManager.instance.ClearAllAugments();
        StatManager.instance.ChangeTemporaryStats_AttackDamage(AugmentNum * 2);
        LevelManager.Instance.LevelSet(1);
        foreach (IBall ball in BallManager.Instance.InGameBall)
        {
            StartCoroutine(BallRemoveEffect(ball));
            BallManager.Instance.TakeBall(ball);
        }
        BallManager.Instance.CreateBall(1);
        QuestAlarm.Instance.ClearQuest();
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager").GetComponent<FreshNewStart>());
    }

    private IEnumerator BallRemoveEffect(IBall ball)
    {
        VisualEffectManager.Instance.SpawnParticle("BallSacrifice", ball.transform.position, 30);
        yield return null;
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
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager")?.GetComponent<FlammablePoison>());
        AugmentManager.instance.effectDuration.FireDuration += 1f;
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<FlammablePoison>());
        AugmentManager.instance.effectDuration.FireDuration -= 1f;
    }
}

public class FlammablePoison: SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_56", "Flammable Poison", "When poisoned, fire is also applied", "Effect", 3);
    }

    public override void Apply()
    {
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager")?.GetComponent<DecayingPoison>());
        AugmentManager.instance.effectDuration.DecayDuration += 2f;
    }

    public override void Remove()
    {
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<DecayingPoison>());
        AugmentManager.instance.effectDuration.DecayDuration -= 2f;
    }
}

public class CritAfterThreeAttack : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_57", "Weakness Detection", "The third attack always triggers a critical hit", "Special", 3);
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
        SetAugment("SPECIAL_58", "Miraculous Technique", "Attacks nearby bricks,\nIt count as direct hits by the ball", "Special", 1);
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }

    public override void AfterMainAttackEffect(IBrick Brick)
    {
        if(Brick.NearbyBricks.Count > 0)
        {
            List<IBrick> ChosenBricks = new List<IBrick>(Brick.NearbyBricks);
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
    }

    public override void Apply()
    {

    }

    public override void Remove()
    {

    }
}

public class ItemEnhance : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_61", "Item Enhancement", "Increases item spawn rate and enhances them", "Special", 2);
    }

    public override void Apply()
    {
        ItemManager.Instance.BaseItemSpawnChance *= 2;
        //todo: think item inhancement
    }

    public override void Remove()
    {
        ItemManager.Instance.BaseItemSpawnChance /= 2;
    }
}

public class InfiniteRapid : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_62", "Rapidfire", "Atk Up,\nAll waves appear as transformed rapid waves", "Special", 3);
    }

    public override void Apply()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(3f);
    }

    public override void Remove()
    {
        StatManager.instance.ChangeTemporaryStats_AttackDamage(-3f);
    }
}

public class MoreAndWeakBricks : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_63", "Legion", "Every Bricks appear 50% more frequently, \nbut their health is reduced by 33%", "Special", 3);
    }

    public override void Apply()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1.5f;
        BrickManager.Instance.BaseBrickHealthValue = 0.66f;
        BrickManager.Instance.BaseMiniBossHealthValue = 0.66f;
        BrickManager.Instance.BaseBossHealthValue = 0.66f;
        BrickManager.Instance.BaseWaitingTimeValue = 6;
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager")?.GetComponent<LessAndStrongBricks>());
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1f;
        BrickManager.Instance.BaseBrickHealthValue = 1f;
        BrickManager.Instance.BaseMiniBossHealthValue = 1f;
        BrickManager.Instance.BaseBossHealthValue = 1f;
        BrickManager.Instance.BaseWaitingTimeValue = 5;
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<LessAndStrongBricks>());
    }
}

public class LessAndStrongBricks : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_63", "Elite Group", "Every Bricks appear 50% less frequently, \nbut their health is increased by 33%.", "Special", 3);
    }

    public override void Apply()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 0.5f;
        BrickManager.Instance.BaseBrickHealthValue = 1.33f;
        BrickManager.Instance.BaseMiniBossHealthValue = 1.33f;
        BrickManager.Instance.BaseBossHealthValue = 1.33f;
        BrickManager.Instance.BaseWaitingTimeValue = 4;
        AugmentManager.instance.augmentsSet.Remove(GameObject.Find("GameManager")?.GetComponent<MoreAndWeakBricks>());
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseBrickSpawnValue = 1f;
        BrickManager.Instance.BaseBrickHealthValue = 1f;
        BrickManager.Instance.BaseMiniBossHealthValue = 1f;
        BrickManager.Instance.BaseBossHealthValue = 1f;
        BrickManager.Instance.BaseWaitingTimeValue = 5;
        AugmentManager.instance.augmentsSet.Add(GameObject.Find("GameManager").AddComponent<MoreAndWeakBricks>());
    }
}

public class DoubleBoss : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_64", "Twins!", "The boss splits into two, \neach having 66% the original health.", "Special", 3);
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
        SetAugment("SPECIAL_65", "Only One", "Waves are replaced by one brick with combined health. \nIt is treated as a boss", "Special", 0);
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
        SetAugment("SPECIAL_66", "Promotion Gift", "After level up, roll a dice \nand gain that many balls", "Special", 4);
    }

    public override void Apply()
    {
        BrickManager.Instance.BaseSpawnTimeValue = 0.4f;
    }

    public override void Remove()
    {
        BrickManager.Instance.BaseSpawnTimeValue = 1f;
    }
}

public class SpecialBrickChanceUp : SpecialAugment
{
    private void OnEnable()
    {
        SetAugment("SPECIAL_67", "Lucky Day", "Mini bosses spawn more frequently.", "Special", 3);
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
        SetAugment("SPECIAL_68", "Stick to the Basics", "Gain 73% More When Selecting Stats After Level Up.", "Special", 2);
    }

    public override void Apply()
    {
        LevelManager.Instance.BaseStatUpValue = 1.73f;
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
                        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.1f);
                        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.1f);
                        break;
                    case 4:
                        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.1f);
                        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.1f);
                        break;
                    case 5:
                        StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(-1.2f);
                        StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(-1.2f);
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
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.1f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.1f);
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
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.1f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.1f);
            }
            else if (GameManager.instance.Wave == 100 && !Goals[5])
            {
                Goals[5] = true;
                for (int i = 0; i < 20; i++)
                {
                    LevelManager.Instance.GetExp(20);
                }
                StatManager.instance.ChangeStatMultiplier_AttackDamageMultiplier(1.2f);
                StatManager.instance.ChangeStatMultiplier_EffectDamageMultiplier(1.2f);
            }
        }
    }
}