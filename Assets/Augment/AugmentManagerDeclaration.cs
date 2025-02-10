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
        effectChance = new EffectChance(1.3f, 0.3f, 0.3f);
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
            gameObject.AddComponent<GrowBall>(),
            ///////////////////////// Stat
            gameObject.AddComponent<AllDamage>(),
            gameObject.AddComponent<AngryBall>(),
            gameObject.AddComponent<PhoenixBall>(),
            gameObject.AddComponent<TargetMark>(),
            ///////////////////////// Field
            gameObject.AddComponent<FireBall>(),
            gameObject.AddComponent<Homing>(),
            gameObject.AddComponent<BouncePower>(),
            gameObject.AddComponent<AdditionalThreeAttack>(),
            gameObject.AddComponent<CriticalAdditionalAttack>(),
            gameObject.AddComponent<Execution>(),
            gameObject.AddComponent<PercentDamage>(),
            gameObject.AddComponent<FireEffectStackBlast>(),
            gameObject.AddComponent<ChainLightning>(),
            gameObject.AddComponent<DeathExp>(),
            gameObject.AddComponent<DeathGold>(),
            gameObject.AddComponent<SpeedPower>(),
            gameObject.AddComponent<SpeedCritical>(),
            gameObject.AddComponent<Lightning>(),
            gameObject.AddComponent<PoisonBall>(),
            gameObject.AddComponent<InfinitePoison>(),
            gameObject.AddComponent<DeadlyPoison>(),
            gameObject.AddComponent<Chaos>(),
            gameObject.AddComponent<DecayBall>(),
            gameObject.AddComponent<PowerfulDecay>(),
            gameObject.AddComponent<DecayExplosion>(),
            gameObject.AddComponent<ExplosionWithBall>(),

        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            gameObject.AddComponent<FireEffectStackBlast>(),
            gameObject.GetComponent<PoisonBall>(),
            gameObject.AddComponent<AdditionalThreeAttack>(),
            gameObject.GetComponent<InfinitePoison>(),
            gameObject.AddComponent<FireBall>(),
            gameObject.GetComponent<DeadlyPoison>(),
            gameObject.AddComponent<Lightning>(),
            gameObject.AddComponent<ChainLightning>(),
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

    public EffectChance(float fireChance, float poisonChance, float decayChance)
    {
        FireChance = fireChance;
        PoisonChance = poisonChance;
        DecayChance = decayChance;
    }
}