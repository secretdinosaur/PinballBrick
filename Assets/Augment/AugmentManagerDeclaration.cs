using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.HableCurve;
using System.Collections;
using System;
using System.Linq;
using Unity.Mathematics;

public partial class AugmentManager : MonoBehaviour //Declaration
{
    public static AugmentManager instance;
    public List<IAugment> augmentsSet;
    public int ExceptedAugmentNumber;
    public List<IAugment> exceptionAugments;

    public List<IAugment> RouletteAugmentsSet;
    //private List<int> usedIndexes;
    public int ActivatedAugmentNumber;
    public List<IAugment> activeAugments = new List<IAugment>();
    
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

    public AugmentRoutine AugmentRoutines;
    [SerializeField]
    public static IAugment RandomAugment;


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
        effectChance = new EffectChance(0.3f, 0.3f, 0.3f, 1f, 1f);
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
        ExceptedAugmentNumber = 0;
        ClearAllAugments();
        AugmentRoutines = new AugmentRoutine(this);

        exceptionAugments = new List<IAugment>
        {

        };

        augmentsSet = new List<IAugment>
        {
            new AtkUp1(),
            new AtkUp2(),
            new AtkUp3(),
            new CritChance10Up(),
            new CritChance20Up(),
            new CritChance35Up(),
            new CritDamage15Up(),
            new CritDamage30Up(),
            new GoldGain10Up(),
            new GoldGain30Up(),
            new GoldGain50Up(),
            new CritChanceToAttackDamage(),
            new EffectDamageUp1(),
            new EffectDamageUp2(),
            new EffectDamageUp3(),
            new WeakButStrongEffect(),
            new WeakButStrongAtk(),
            new DecreaseFieldCooldown5(),
            new DecreaseFieldCooldown10(),
            new DecreaseFieldCooldown20(),
            new RandomStatUpOrDown(),
            new Trinity(),
            new Perfectionism(),
            new MassivelyCriticalDamageUp(),
            new ExpGain10Up(),
            new ExpGain30Up(),
            new ExpGain50Up(),
            new AttackEffectBothUp(),
            new LevelUpMania(),
            new EffectDuration10(),
            new EffectDuration15(),
            new EffectDuration30(),
            new LateBloomer(),
            ///////////////////////// Stat
            new AllDamage(),
            new AngryBall(),
            new PhoenixBall(),
            new TargetMark(),
            new SatelliteBeam(),
            new AutoGetExp(),
            new AntiGravity(),
            new BountyHunter(),
            new AttackOrDie(),
            new WeatherChange(),
            new DiceGamble(),
            new Trampoline(),
            ///////////////////////// Field
            new FireBall(),
            new Homing(),
            new BouncePower(),
            new AdditionalThreeAttack(),
            new CriticalAdditionalAttack(),
            new Execution(),
            new EffectChanceUp(),
            new DeathExp(),
            new DeathGold(),
            new BigBoss(),
            new ChangeBallSpawnPosition(),
            new Lightning(),
            new PoisonBall(),
            new Chaos(),
            new DecayBall(),
            new ExplosionWithBall(),
            new LevelUpQuest(),
            new GrowBallQuest(),
            new InfiniteGrowBall(),
            new SacrificeQuest1(),
            new AttackAfterAdditionalBall(),
            new StrongAdditionalAttack(),
            new RandomEffect(),
            new RebirthBall(),
            new BossKillBall(),
            new SpawnBallAmountMultiple1(),
            new SpawnBallAmountMultiple2(),
            new BallSwarm(),
            new LonelyBall(),
            new SplitBall(),
            new CurseBall(),
            new DeathEffectTwice(),
            new SpawnBombBall(),
            new ChangeNormalToGlass(),
            new OneMoreChance(),
            new Concentrate(),
            new GroupViolence(),
            new Oneness(),
            new HappyBirthday(),
            new HalfBallSacrifice(),
            new Chronobreak(),
            new CritAfterThreeAttack(),
            new DamageInfection(),
            new RollDiceBall(),
            new PortalBall(),
            new InfiniteRapid(),
            new MoreAndWeakBricks(),
            new LessAndStrongBricks(),
            new DoubleBoss(),
            new FusionBricks(),
            new GainBallAfterLevelUp(),
            new SpecialBrickChanceUp(),
            new IncreseStatUpValue(),
            new GoalWaveBonus(),
            new ReducesExp10(),
            new ReducesExp15(),
            new WaveFourBoss(),
            new RerollAugments(),
            new BossDamageUp(),
            new LevelUpAgain(),
            new AtkUpWhenCritical(),
            new AdditionalEffectDamage(),
            new PenetratingBall(),
            ////////////////////////// Special
            new FireEffectStackBlast(),
            new TheFlamethrower(),
            new Pyromaniac(),

            new DeadlyPoison(),
            new InfinitePoison(),
            new DecayingPoison(),
            new FlammablePoison(),

            new PowerfulDecay(),
            new DecayExplosion(),
            new DecayZone(),

            new ThunderStrike(),
            new ChainLightning(),
            new LightningRod(),

            new SacrificeQuest2(),
            new SacrificeQuest3(),

            new ShareBallAtk(),

            new SummonGhostBall(),
            new EnhanceCurseAttack(),
            new ContinuingCurseAttack(),
            new Cursebolt(),
            new Duality(),

            new RebornBombBall(),
            new SuicideBoomBall(),
            new Bombardment(),
            new InstantBombBall(),

        };

        foreach (IAugment augment in augmentsSet)
        {
            if (augment.isFollowUpAugment)
            {
                LockAugment(augment);
            }
        }

        if (RandomAugment != null)
        {
            Type type = RandomAugment.GetType();
            AddAugment(augmentsSet.FirstOrDefault(x => x.GetType() == type));
            RandomAugment = null;
        }

        List<IAugment> TestAugments = new List<IAugment>
        {
            new AntiGravity(),
            new ChangeNormalToGlass()
        };

        foreach (IAugment augment in TestAugments)
        {
            Type type = augment.GetType();
            AddAugment(augmentsSet.FirstOrDefault(x => x.GetType() == type));
        }
    }

    public void AddAugmentButton()
    {
        AddAugment(augmentsSet.OfType<Perfectionism>().FirstOrDefault());
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

public class AugmentRoutine
{
    private readonly MonoBehaviour runner;
    private readonly Dictionary<string, Coroutine> routines = new();

    public AugmentRoutine(MonoBehaviour coroutineRunner)
    {
        runner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
    }

    public Coroutine Add(string key, IEnumerator routine)
    {
        if (routines.ContainsKey(key))
        {
            Stop(key);
        }

        Coroutine coroutine = runner.StartCoroutine(routine);
        routines[key] = coroutine;
        return coroutine;
    }

    public void Stop(string key)
    {
        if (routines.TryGetValue(key, out var coroutine))
        {
            runner.StopCoroutine(coroutine);
            routines.Remove(key);
        }
    }

    public void StopAll()
    {
        foreach (var kvp in routines)
        {
            runner.StopCoroutine(kvp.Value);
        }
        routines.Clear();
    }

    public bool hasKey(string key)
    {
        if (routines.ContainsKey(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}