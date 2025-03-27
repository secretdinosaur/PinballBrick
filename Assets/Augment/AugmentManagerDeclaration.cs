using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public partial class AugmentManager : MonoBehaviour //Declaration
{
    public static AugmentManager instance;
    public List<IAugment> augmentsSet;

    public List<IAugment> RouletteAugmentsSet;
    //private List<int> usedIndexes;
    public List<StatAugment> activeStatAugments = new List<StatAugment>();
    public List<FieldAugment> activeFieldAugments = new List<FieldAugment>();
    public List<SpecialAugment> activeSpecialAugments = new List<SpecialAugment>();
    
    public EffectChance effectChance;
    public int FireStackLimit;
    public int PoisonStackLimit;
    public int DecayStackLimit;

    public List<IBrick> PoisonBrickSet = new List<IBrick>();

    public float DecayDamageMultiple;

    public float FieldGlobalCooldown = 10f;

    public bool isSacrificeQuest3Cleared;

    public int SpawnBallAmount;

    public int CurseActiveNumber;
    public int AfterDeathActiveNumber;

    public Dictionary<int, float> rarityWeights = new Dictionary<int, float>
    {
        { 0, 0.01f },
        { 1, 0.59f },
        { 2, 3.9f },
        { 3, 11.5f },
        { 4, 32f },
        { 5, 52f }
    };
    
    void Awake()
    {
        instance = this;
        effectChance = new EffectChance(1f, 1f, 1f, 1f);
        FireStackLimit = 5;
        PoisonStackLimit = 3;
        DecayStackLimit = 3;
        DecayDamageMultiple = 1.1f;
        isSacrificeQuest3Cleared = false;
        SpawnBallAmount = 1;
        CurseActiveNumber = 1;
        AfterDeathActiveNumber = 1;
        ClearAllAugments();
        augmentsSet = new List<IAugment>
        {
            gameObject.AddComponent<Atk10Up>(),
            gameObject.AddComponent<Atk30Up>(),
            gameObject.AddComponent<Atk50Up>(),
            gameObject.AddComponent<CritChance10Up>(),
            gameObject.AddComponent<CritChance20Up>(),
            gameObject.AddComponent<CritChance50Up>(),
            gameObject.AddComponent<CritDamage30Up>(),
            gameObject.AddComponent<CritDamage50Up>(),
            gameObject.AddComponent<GoldGain10Up>(),
            gameObject.AddComponent<GoldGain30Up>(),
            gameObject.AddComponent<GoldGain50Up>(),
            gameObject.AddComponent<BossDamageUp>(),
            gameObject.AddComponent<AtkUpWhenCritical>(),
            gameObject.AddComponent<EffectCritical>(),
            gameObject.AddComponent<PowerButSlow>(),
            gameObject.AddComponent<WeakButFast>(),
            gameObject.AddComponent<WeakButStrongEffect>(),
            gameObject.AddComponent<WeakButStrongAtk>(),
            gameObject.AddComponent<DecreaseFieldCooldown5>(),
            gameObject.AddComponent<DecreaseFieldCooldown30>(),
            gameObject.AddComponent<CritChanceToAttackDamage>(),
            gameObject.AddComponent<RandomStatUpOrDown>(),
            ///////////////////////// Stat
            gameObject.AddComponent<AllDamage>(),
            gameObject.AddComponent<AngryBall>(),
            gameObject.AddComponent<PhoenixBall>(),
            gameObject.AddComponent<TargetMark>(),
            gameObject.AddComponent<SatelliteBeam>(),
            gameObject.AddComponent<AutoGetExp>(),
            gameObject.AddComponent<AntiGravity>(),
            gameObject.AddComponent<BountyHunter>(),
            gameObject.AddComponent<LightningRod>(),
            gameObject.AddComponent<Laser>(),
            gameObject.AddComponent<AttackOrDie>(),
            ///////////////////////// Field
            gameObject.AddComponent<FireBall>(),
            gameObject.AddComponent<Homing>(),
            gameObject.AddComponent<BouncePower>(),
            gameObject.AddComponent<AdditionalThreeAttack>(),
            gameObject.AddComponent<CriticalAdditionalAttack>(),
            gameObject.AddComponent<Execution>(),
            gameObject.AddComponent<PercentDamage>(),
            gameObject.AddComponent<ChainLightning>(),
            gameObject.AddComponent<DeathExp>(),
            gameObject.AddComponent<DeathGold>(),
            gameObject.AddComponent<SpeedPower>(),
            gameObject.AddComponent<SpeedCritical>(),
            gameObject.AddComponent<Lightning>(),
            gameObject.AddComponent<PoisonBall>(),
            gameObject.AddComponent<Chaos>(),
            gameObject.AddComponent<DecayBall>(),
            gameObject.AddComponent<ExplosionWithBall>(),
            gameObject.AddComponent<LevelUpQuest>(),
            gameObject.AddComponent<GrowBallQuest>(),
            gameObject.AddComponent<InfiniteGrowBall>(),
            gameObject.AddComponent<SacrificeQuest1>(),
            gameObject.AddComponent<AttackAfterAdditionalBall>(),
            gameObject.AddComponent<ShareBallAtk>(),
            gameObject.AddComponent<StrongAdditionalAttack>(),
            gameObject.AddComponent<RandomEffect>(),
            gameObject.AddComponent<OneLineBall>(),
            gameObject.AddComponent<RebirthBall>(),
            gameObject.AddComponent<BossKillBall>(),
            gameObject.AddComponent<SpawnBallAmountMultiple1>(),
            gameObject.AddComponent<SpawnBallAmountMultiple2>(),
            gameObject.AddComponent<BallSwarm>(),
            gameObject.AddComponent<LonelyBall>(),
            gameObject.AddComponent<SuicideBoomBall>(),
            gameObject.AddComponent<SplitBall>(),
            gameObject.AddComponent<CurseBall>(),
            gameObject.AddComponent<DeathEffectTwice>(),
            gameObject.AddComponent<SpawnBombBall>(),
            gameObject.AddComponent<ChangeNormalToGlass>(),
            gameObject.AddComponent<OneMoreChance>(),
            gameObject.AddComponent<Concentrate>(),
            gameObject.AddComponent<GroupViolence>(),
            gameObject.AddComponent<Oneness>(),
            gameObject.AddComponent<HappyBirthday>(),
            ////////////////////////// Special
            gameObject.AddComponent<FireEffectStackBlast>(),
            gameObject.AddComponent<TheFlamethrower>(),

            gameObject.AddComponent<DeadlyPoison>(),
            gameObject.AddComponent<InfinitePoison>(),

            gameObject.AddComponent<PowerfulDecay>(),
            gameObject.AddComponent<DecayExplosion>(),

            gameObject.AddComponent<SacrificeQuest2>(),
            gameObject.AddComponent<SacrificeQuest3>(),

            gameObject.AddComponent<ShareBallAtk>(),

            gameObject.AddComponent<ThreeBrickCurseAttack>(),
            gameObject.AddComponent<CurseTwice>(),

            gameObject.AddComponent<RebornBombBall>(),
            ////////////////////////// Delete These Before Build
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            gameObject.GetComponent<RandomStatUpOrDown>(),
        };

        foreach (IAugment augment in TestAugments)
        {
            AddAugment(augment);
        }
    }
}

public class EffectStatus
{
    public string Name;
    public bool IsActive;
    public int EffectStack;
    public int EffectStackLimit;

    public EffectStatus(string name, bool isActive, int effectStack, int effectStackLimit)
    {
        Name = name;
        IsActive = isActive;
        EffectStack = effectStack;
        EffectStackLimit = effectStackLimit;
    }
}

public class EffectChance
{
    public float FireChance;
    public float PoisonChance;
    public float DecayChance;
    public float CurseChance;

    public EffectChance(float fireChance, float poisonChance, float decayChance, float curseChance)
    {
        FireChance = fireChance;
        PoisonChance = poisonChance;
        DecayChance = decayChance;
        CurseChance = curseChance;
    }
}