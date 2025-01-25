using UnityEngine;

/*
public class Example : SpecialAugment
{
    public Example() : base( "SPECIAL_N", "Example", "Just Example", "Preset" ) { } 

    public override void Apply(){ Augment Effect (Plus) }

    public override void Remove(){ Augment Effect (Minus) }
}
 */

public class FireBall : SpecialAugment
{
    private float FireDamage; //EffectDamage
    private float FireDuration; //EffectDuration
    public FireBall() : base("SPECIAL_1","FireBall", "And FireBall.", "Effect")
    {
        FireDamage += 0.1f;
        FireDuration += 0.5f;
    }

    public override void Apply()
    {
        StatManager.instance.Temporary_stat.EffectDamage += FireDamage;
        StatManager.instance.Temporary_stat.EffectDuration += FireDuration;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public override void Remove()
    {
        StatManager.instance.Temporary_stat.EffectDamage -= FireDamage;
        StatManager.instance.Temporary_stat.EffectDuration -= FireDuration;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public class Homing : SpecialAugment
{
    public Homing() : base( "SPECIAL_2","Homing Ball" , "Lock On!", "Homing" ) { }

    public override void Apply(){ }

    public override void Remove(){  }
}