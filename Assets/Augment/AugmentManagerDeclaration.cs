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
            gameObject.AddComponent<Atk30Up>(),
            gameObject.AddComponent<Atk70Up>(),
            gameObject.AddComponent<CritChanceUp>(),
            gameObject.AddComponent<CritDamage30Up>(),
            gameObject.AddComponent<CritDamage70Up>(),
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
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            
        };

        foreach(IAugment augment in TestAugments)
        {
            AddAugment(augment);
        }
    }
}