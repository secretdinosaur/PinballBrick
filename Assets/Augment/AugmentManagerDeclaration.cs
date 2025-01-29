using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public partial class AugmentManager : MonoBehaviour //Declaration
{
    public static AugmentManager instance;
    private List<IAugment> augmentsSet;
    //private List<int> usedIndexes;
    private List<StatAugment> activeStatAugments = new List<StatAugment>();
    private List<FieldAugment> activeFieldAugments = new List<FieldAugment>();
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

            ///////////////////////// Stat
            gameObject.AddComponent<AllDamage>(),
            ///////////////////////// Field
            gameObject.AddComponent<FireBall>(),
            gameObject.AddComponent<Homing>(),
            gameObject.AddComponent<BouncePower>(),
            gameObject.AddComponent<AdditionalThreeAttack>(),
            gameObject.AddComponent<CriticalAdditionalAttack>(),
            gameObject.AddComponent<Execution>(),
            gameObject.AddComponent<EffectCritical>(),
        };

        List<IAugment> TestAugments = new List<IAugment>
        {
            GetComponent<EffectCritical>(),
            GetComponent<CritChanceUp>(),
            GetComponent<FireBall>(),
        };

        foreach(IAugment augment in TestAugments)
        {
            AddAugment(augment);
        }
    }
}