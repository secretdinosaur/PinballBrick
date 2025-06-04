using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using static UnityEngine.Rendering.HableCurve;

public partial class AugmentManager : MonoBehaviour //Declaration
{
    public static AugmentManager instance;
    public List<IAugment> augmentsSet;

    public List<IAugment> RouletteAugmentsSet;
    //private List<int> usedIndexes;
    public int ActivatedAugmentNumber;
    public List<IAugment> activeAugments = new List<IAugment>();
    
    public EffectChance effectChance;
    public int FireStackLimit;
    public int PoisonStackLimit;
    public int DecayStackLimit;

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
        { 0, 0.1f },
        { 1, 0.18f },
        { 2, 0.18f },
        { 3, 0.18f },
        { 4, 0.16f },
        { 5, 0.2f }
    };
    
    void Awake()
    {
        instance = this;
        effectChance = new EffectChance(0.3f, 0.3f, 0.3f, 1f);
        effectDuration = new EffectDuration(1.1f, 4.5f, 2.5f);
        FireStackLimit = 5;
        PoisonStackLimit = 3;
        DecayStackLimit = 3;
        DecayDamageMultiple = 1.1f;
        isSacrificeQuest3Cleared = false;
        SpawnBallAmount = 1;
        CurseDamageValue = 1f;
        AfterDeathActiveNumber = 1;
        isLevelUpOnceAgain = false;
        ActivatedAugmentNumber = 0;
        ClearAllAugments();

        augmentsSet = new List<IAugment>
        {
            
            gameObject.AddComponent<AtkUp1>(),
            gameObject.AddComponent<AtkUp2>(),
            gameObject.AddComponent<AtkUp3>(),
            gameObject.AddComponent<CritChance10Up>(),
            gameObject.AddComponent<CritChance20Up>(),
            gameObject.AddComponent<CritChance35Up>(),
            gameObject.AddComponent<CritDamage15Up>(),
            gameObject.AddComponent<CritDamage30Up>(),
            gameObject.AddComponent<GoldGain10Up>(),
            gameObject.AddComponent<GoldGain30Up>(),
            gameObject.AddComponent<GoldGain50Up>(),
            gameObject.AddComponent<CritChanceToAttackDamage>(),
            gameObject.AddComponent<EffectDamageUp1>(),
            gameObject.AddComponent<EffectDamageUp2>(),
            gameObject.AddComponent<EffectDamageUp3>(),
            gameObject.AddComponent<WeakButStrongEffect>(),
            gameObject.AddComponent<WeakButStrongAtk>(),
            gameObject.AddComponent<DecreaseFieldCooldown5>(),
            gameObject.AddComponent<DecreaseFieldCooldown10>(),
            gameObject.AddComponent<DecreaseFieldCooldown20>(),
            gameObject.AddComponent<RandomStatUpOrDown>(),
            gameObject.AddComponent<Trinity>(),
            gameObject.AddComponent<Perfectionism>(),
            gameObject.AddComponent<MassivelyCriticalDamageUp>(),
            gameObject.AddComponent<ExpGain10Up>(),
            gameObject.AddComponent<ExpGain30Up>(),
            gameObject.AddComponent<ExpGain50Up>(),
            gameObject.AddComponent<AttackEffectBothUp>(),
            gameObject.AddComponent<LevelUpMania>(),
            gameObject.AddComponent<EffectDuration10>(),
            gameObject.AddComponent<EffectDuration15>(),
            gameObject.AddComponent<EffectDuration30>(),
            gameObject.AddComponent<LateBloomer>(),
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
            gameObject.AddComponent<AttackOrDie>(),
            gameObject.AddComponent<WeatherChange>(),
            gameObject.AddComponent<DiceGamble>(),
            gameObject.AddComponent<Trampoline>(),
            gameObject.AddComponent<Bombardment>(),
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
            gameObject.AddComponent<BigBoss>(),
            gameObject.AddComponent<ChangeBallSpawnPosition>(),
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
            gameObject.AddComponent<StrongAdditionalAttack>(),
            gameObject.AddComponent<RandomEffect>(),
            gameObject.AddComponent<ThunderStrike>(),
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
            gameObject.AddComponent<MidasHand>(),
            gameObject.AddComponent<HalfBallSacrifice>(),
            gameObject.AddComponent<Chronobreak>(),
            gameObject.AddComponent<CritAfterThreeAttack>(),
            gameObject.AddComponent<DamageInfection>(),
            gameObject.AddComponent<RollDiceBall>(),
            gameObject.AddComponent<PortalBall>(),
            gameObject.AddComponent<InfiniteRapid>(),
            gameObject.AddComponent<MoreAndWeakBricks>(),
            gameObject.AddComponent<LessAndStrongBricks>(),
            gameObject.AddComponent<DoubleBoss>(),
            gameObject.AddComponent<FusionBricks>(),
            gameObject.AddComponent<GainBallAfterLevelUp>(),
            gameObject.AddComponent<SpecialBrickChanceUp>(),
            gameObject.AddComponent<IncreseStatUpValue>(),
            gameObject.AddComponent<GoalWaveBonus>(),
            gameObject.AddComponent<ReducesExp10>(),
            gameObject.AddComponent<ReducesExp15>(),
            gameObject.AddComponent<WaveFourBoss>(),
            gameObject.AddComponent<RerollAugments>(),
            gameObject.AddComponent<BossDamageUp>(),
            gameObject.AddComponent<LevelUpAgain>(),
            gameObject.AddComponent<AtkUpWhenCritical>(),
            ////////////////////////// Special
            gameObject.AddComponent<FireEffectStackBlast>(),
            gameObject.AddComponent<TheFlamethrower>(),

            gameObject.AddComponent<DeadlyPoison>(),
            gameObject.AddComponent<InfinitePoison>(),
            gameObject.AddComponent<DecayingPoison>(),
            gameObject.AddComponent<FlammablePoison>(),

            gameObject.AddComponent<PowerfulDecay>(),
            gameObject.AddComponent<DecayExplosion>(),
            gameObject.AddComponent<DecayZone>(),

            gameObject.AddComponent<SacrificeQuest2>(),
            gameObject.AddComponent<SacrificeQuest3>(),

            gameObject.AddComponent<ShareBallAtk>(),

            gameObject.AddComponent<SummonGhostBall>(),
            gameObject.AddComponent<EnhanceCurseAttack>(),
            gameObject.AddComponent<ContinuingCurseAttack>(),

            gameObject.AddComponent<RebornBombBall>(),
            ////////////////////////// Delete These Before Build
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            gameObject.GetComponent<CurseBall>(),
        };

        foreach (IAugment augment in TestAugments)
        {
            AddAugment(augment);
        }
    }

    public void AddAugmentButton()
    {
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
