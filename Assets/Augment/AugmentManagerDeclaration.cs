using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public partial class AugmentManager : MonoBehaviour //Declaration
{
    public static AugmentManager instance;
    public List<IAugment> augmentsSet;
    //private List<int> usedIndexes;
    public List<StatAugment> activeStatAugments = new List<StatAugment>();
    public List<FieldAugment> activeFieldAugments = new List<FieldAugment>();
    public List<SpecialAugment> activeSpecialAugments = new List<SpecialAugment>();
    
    void Awake()
    {
        instance = this;
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
            ///////////////////////// Stat
            gameObject.AddComponent<AllDamage>(),
            ///////////////////////// Field
            gameObject.AddComponent<FireBall>(),
            gameObject.AddComponent<Homing>(),
            gameObject.AddComponent<BouncePower>(),
            gameObject.AddComponent<AdditionalThreeAttack>(),
            gameObject.AddComponent<CriticalAdditionalAttack>(),
            gameObject.AddComponent<Execution>(),
            gameObject.AddComponent<PercentDamage>(),
            gameObject.AddComponent<EffectStackBlast>(),
            gameObject.AddComponent<ChainLightning>(),
            gameObject.AddComponent<DeathExp>(),
            gameObject.AddComponent<DeathGold>(),
            gameObject.AddComponent<Lightning>(),
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            gameObject.GetComponent<Lightning>(),

            gameObject.AddComponent<ChainLightning>(),
        };

        foreach(IAugment augment in TestAugments)
        {
            AddAugment(augment);
        }
    }
}