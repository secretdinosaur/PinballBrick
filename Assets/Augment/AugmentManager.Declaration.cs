using UnityEngine;

public partial class AugmentManager
{
    public IStatAugment Atk30Up() { return new Atk30Up(); }
    public IStatAugment Atk70Up() { return new Atk70Up(); }
    public IStatAugment CritChanceUp() { return new CritChanceUp(); }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public ISpecialAugment FireBall() { return new FireBall(); }
    
}
