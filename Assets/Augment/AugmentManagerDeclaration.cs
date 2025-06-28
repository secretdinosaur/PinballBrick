using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.HableCurve;

public partial class AugmentManager : MonoBehaviour //Declaration
{
    public static AugmentManager instance;
    public List<IAugment> augmentsSet;
    public List<IAugment> exceptionAugments;

    public List<IAugment> RouletteAugmentsSet;
    //private List<int> usedIndexes;
    public int ActivatedAugmentNumber;
    public List<IAugment> activeAugments = new List<IAugment>();
    public static GameObject AugmentSet;
    
    public EffectChance effectChance;
    public int FireStackLimit;
    public int PoisonStackLimit;
    public int DecayStackLimit;
    public int HolyStackLimit;

    public EffectDuration effectDuration;

    public List<IBrick> PoisonBrickSet = new List<IBrick>();

    public float DecayDamageMultiple;

    public float FieldGlobalCooldown = 10f;

    public bool isSacrificeQuest3Cleared = false;

    public int SpawnBallAmount;

    public float CurseDamageValue;
    public int AfterDeathActiveNumber;

    public bool isLevelUpOnceAgain;

    public Dictionary<int, float> rarityWeights = new Dictionary<int, float>
    {
        { 0, 0.005f },
        { 1, 0.045f },
        { 2, 0.1f },
        { 3, 0.25f },
        { 4, 0.4f },
        { 5, 0.2f }
    };
    
    void Awake()
    {
        instance = this;
        effectChance = new EffectChance(0.3f, 0.3f, 0.3f, 0.3f, 1f);
        effectDuration = new EffectDuration(1.1f, 4.5f, 2.5f);
        FireStackLimit = 3;
        PoisonStackLimit = 3;
        DecayStackLimit = 3;
        HolyStackLimit = 5;
        DecayDamageMultiple = 0.1f;
        isSacrificeQuest3Cleared = false;
        SpawnBallAmount = 1;
        CurseDamageValue = 1f;
        AfterDeathActiveNumber = 1;
        isLevelUpOnceAgain = false;
        ActivatedAugmentNumber = 0;
        ClearAllAugments();
        AugmentSet = GameObject.Find("AugmentSet").gameObject;

        exceptionAugments = new List<IAugment>
        {

        };

        augmentsSet = new List<IAugment>
        {
            AugmentSet.AddComponent<AtkUp1>(),
            AugmentSet.AddComponent<AtkUp2>(),
            AugmentSet.AddComponent<AtkUp3>(),
            AugmentSet.AddComponent<CritChance10Up>(),
            AugmentSet.AddComponent<CritChance20Up>(),
            AugmentSet.AddComponent<CritChance35Up>(),
            AugmentSet.AddComponent<CritDamage15Up>(),
            AugmentSet.AddComponent<CritDamage30Up>(),
            AugmentSet.AddComponent<GoldGain10Up>(),
            AugmentSet.AddComponent<GoldGain30Up>(),
            AugmentSet.AddComponent<GoldGain50Up>(),
            AugmentSet.AddComponent<CritChanceToAttackDamage>(),
            AugmentSet.AddComponent<EffectDamageUp1>(),
            AugmentSet.AddComponent<EffectDamageUp2>(),
            AugmentSet.AddComponent<EffectDamageUp3>(),
            AugmentSet.AddComponent<WeakButStrongEffect>(),
            AugmentSet.AddComponent<WeakButStrongAtk>(),
            AugmentSet.AddComponent<DecreaseFieldCooldown5>(),
            AugmentSet.AddComponent<DecreaseFieldCooldown10>(),
            AugmentSet.AddComponent<DecreaseFieldCooldown20>(),
            AugmentSet.AddComponent<RandomStatUpOrDown>(),
            AugmentSet.AddComponent<Trinity>(),
            AugmentSet.AddComponent<Perfectionism>(),
            AugmentSet.AddComponent<MassivelyCriticalDamageUp>(),
            AugmentSet.AddComponent<ExpGain10Up>(),
            AugmentSet.AddComponent<ExpGain30Up>(),
            AugmentSet.AddComponent<ExpGain50Up>(),
            AugmentSet.AddComponent<AttackEffectBothUp>(),
            AugmentSet.AddComponent<LevelUpMania>(),
            AugmentSet.AddComponent<EffectDuration10>(),
            AugmentSet.AddComponent<EffectDuration15>(),
            AugmentSet.AddComponent<EffectDuration30>(),
            AugmentSet.AddComponent<LateBloomer>(),
            ///////////////////////// Stat
            AugmentSet.AddComponent<AllDamage>(),
            AugmentSet.AddComponent<AngryBall>(),
            AugmentSet.AddComponent<PhoenixBall>(),
            AugmentSet.AddComponent<TargetMark>(),
            AugmentSet.AddComponent<SatelliteBeam>(),
            AugmentSet.AddComponent<AutoGetExp>(),
            AugmentSet.AddComponent<AntiGravity>(),
            AugmentSet.AddComponent<BountyHunter>(),
            AugmentSet.AddComponent<AttackOrDie>(),
            AugmentSet.AddComponent<WeatherChange>(),
            AugmentSet.AddComponent<DiceGamble>(),
            AugmentSet.AddComponent<Trampoline>(),
            ///////////////////////// Field
            AugmentSet.AddComponent<FireBall>(),
            AugmentSet.AddComponent<Homing>(),
            AugmentSet.AddComponent<BouncePower>(),
            AugmentSet.AddComponent<AdditionalThreeAttack>(),
            AugmentSet.AddComponent<CriticalAdditionalAttack>(),
            AugmentSet.AddComponent<Execution>(),
            AugmentSet.AddComponent<EffectChanceUp>(),
            AugmentSet.AddComponent<DeathExp>(),
            AugmentSet.AddComponent<DeathGold>(),
            AugmentSet.AddComponent<BigBoss>(),
            AugmentSet.AddComponent<ChangeBallSpawnPosition>(),
            AugmentSet.AddComponent<Lightning>(),
            AugmentSet.AddComponent<PoisonBall>(),
            AugmentSet.AddComponent<Chaos>(),
            AugmentSet.AddComponent<DecayBall>(),
            AugmentSet.AddComponent<ExplosionWithBall>(),
            AugmentSet.AddComponent<LevelUpQuest>(),
            AugmentSet.AddComponent<GrowBallQuest>(),
            AugmentSet.AddComponent<InfiniteGrowBall>(),
            AugmentSet.AddComponent<SacrificeQuest1>(),
            AugmentSet.AddComponent<AttackAfterAdditionalBall>(),
            AugmentSet.AddComponent<StrongAdditionalAttack>(),
            AugmentSet.AddComponent<RandomEffect>(),
            AugmentSet.AddComponent<RebirthBall>(),
            AugmentSet.AddComponent<BossKillBall>(),
            AugmentSet.AddComponent<SpawnBallAmountMultiple1>(),
            AugmentSet.AddComponent<SpawnBallAmountMultiple2>(),
            AugmentSet.AddComponent<BallSwarm>(),
            AugmentSet.AddComponent<LonelyBall>(),
            AugmentSet.AddComponent<SplitBall>(),
            AugmentSet.AddComponent<CurseBall>(),
            AugmentSet.AddComponent<DeathEffectTwice>(),
            AugmentSet.AddComponent<SpawnBombBall>(),
            AugmentSet.AddComponent<ChangeNormalToGlass>(),
            AugmentSet.AddComponent<OneMoreChance>(),
            AugmentSet.AddComponent<Concentrate>(),
            AugmentSet.AddComponent<GroupViolence>(),
            AugmentSet.AddComponent<Oneness>(),
            AugmentSet.AddComponent<HappyBirthday>(),
            AugmentSet.AddComponent<MidasHand>(),
            AugmentSet.AddComponent<HalfBallSacrifice>(),
            AugmentSet.AddComponent<Chronobreak>(),
            AugmentSet.AddComponent<CritAfterThreeAttack>(),
            AugmentSet.AddComponent<DamageInfection>(),
            AugmentSet.AddComponent<RollDiceBall>(),
            AugmentSet.AddComponent<PortalBall>(),
            AugmentSet.AddComponent<InfiniteRapid>(),
            AugmentSet.AddComponent<MoreAndWeakBricks>(),
            AugmentSet.AddComponent<LessAndStrongBricks>(),
            AugmentSet.AddComponent<DoubleBoss>(),
            AugmentSet.AddComponent<FusionBricks>(),
            AugmentSet.AddComponent<GainBallAfterLevelUp>(),
            AugmentSet.AddComponent<SpecialBrickChanceUp>(),
            AugmentSet.AddComponent<IncreseStatUpValue>(),
            AugmentSet.AddComponent<GoalWaveBonus>(),
            AugmentSet.AddComponent<ReducesExp10>(),
            AugmentSet.AddComponent<ReducesExp15>(),
            AugmentSet.AddComponent<WaveFourBoss>(),
            AugmentSet.AddComponent<RerollAugments>(),
            AugmentSet.AddComponent<BossDamageUp>(),
            AugmentSet.AddComponent<LevelUpAgain>(),
            AugmentSet.AddComponent<AtkUpWhenCritical>(),
            AugmentSet.AddComponent<AdditionalEffectDamage>(),
            AugmentSet.AddComponent<PenetratingBall>(),
            ////////////////////////// Special
            AugmentSet.AddComponent<FireEffectStackBlast>(),
            AugmentSet.AddComponent<TheFlamethrower>(),
            AugmentSet.AddComponent<Pyromaniac>(),

            AugmentSet.AddComponent<DeadlyPoison>(),
            AugmentSet.AddComponent<InfinitePoison>(),
            AugmentSet.AddComponent<DecayingPoison>(),
            AugmentSet.AddComponent<FlammablePoison>(),

            AugmentSet.AddComponent<PowerfulDecay>(),
            AugmentSet.AddComponent<DecayExplosion>(),
            AugmentSet.AddComponent<DecayZone>(),

            AugmentSet.AddComponent<ThunderStrike>(),
            AugmentSet.AddComponent<ChainLightning>(),
            AugmentSet.AddComponent<LightningRod>(),

            AugmentSet.AddComponent<SacrificeQuest2>(),
            AugmentSet.AddComponent<SacrificeQuest3>(),

            AugmentSet.AddComponent<ShareBallAtk>(),

            AugmentSet.AddComponent<SummonGhostBall>(),
            AugmentSet.AddComponent<EnhanceCurseAttack>(),
            AugmentSet.AddComponent<ContinuingCurseAttack>(),

            AugmentSet.AddComponent<RebornBombBall>(),
            AugmentSet.AddComponent<SuicideBoomBall>(),
            AugmentSet.AddComponent<Bombardment>(),
            AugmentSet.AddComponent<InstantBombBall>()
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            AugmentSet.GetComponent<Trinity>()
        };

        foreach (IAugment augment in TestAugments)
        {
            AddAugment(augment);
        }
    }

    public void AddAugmentButton()
    {
        AddAugment(AugmentSet.GetComponent<Perfectionism>());
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
    public float LightningChance;

    public EffectChance(float fireChance, float poisonChance, float decayChance, float curseChance, float lightningChance)
    {
        FireChance = fireChance;
        PoisonChance = poisonChance;
        DecayChance = decayChance;
        CurseChance = curseChance;
        LightningChance = lightningChance;
    }
}

public class EffectDuration
{
    public float FireDuration;
    public float PoisonDuration;
    public float DecayDuration;

    public EffectDuration(float fireDuration, float poisonDuration, float decayDuration)
    {
        FireDuration = fireDuration;
        PoisonDuration = poisonDuration;
        DecayDuration = decayDuration;
    }
}
