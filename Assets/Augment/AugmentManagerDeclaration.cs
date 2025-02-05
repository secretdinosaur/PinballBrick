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
    
    void Awake()
    {
        instance = this;
        effectChance = new EffectChance(0.3f, 1f);
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
            ///////////////////////// Stat
            gameObject.AddComponent<AllDamage>(),
            gameObject.AddComponent<AngryBall>(),
            gameObject.AddComponent<PhoenixBall>(),
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
            gameObject.AddComponent<PoisonBall>(),
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            gameObject.GetComponent<PoisonBall>(),
        };

        foreach(IAugment augment in TestAugments)
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

    public EffectStatus(string name, bool isActive, int effectStack)
    {
        Name = name;
        IsActive = isActive;
        EffectStack = effectStack;
    }
}

public class EffectChance
{
    public float FireChance;
    public float PoisonChance;

    public EffectChance(float fireChance, float poisonChance)
    {
        FireChance = fireChance;
        PoisonChance = poisonChance;
    }
}