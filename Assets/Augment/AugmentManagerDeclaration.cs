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
    private List<SpecialAugment> activeSpecialAugments = new List<SpecialAugment>();
    
    void Awake()
    {
        instance = this;
        ClearAllAugments();
        augmentsSet = new List<IAugment>
        {
            new Atk30Up(),
            new Atk70Up(),
            new CritChanceUp(),
            new CritDamage30Up(),
            new CritDamage70Up(),
            new BossDamageUp(),
            ///////////////////////// Stat
            
            ///////////////////////// Field
            new FireBall(),
            new Homing(),
            new BouncePower(),
            ///////////////////////// Special
        };
    }
}