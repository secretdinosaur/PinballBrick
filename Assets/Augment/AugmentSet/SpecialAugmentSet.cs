using UnityEngine;

public class FireBall : SpecialAugment
{
    private float FireDamage; //EffectDamage
    private float FireDuration; //EffectDuration
    public FireBall() : base("FireBall", "Ball becomes a fireball", "Effect")
    {
        FireDamage += 0.1f;
        FireDuration += 0.5f;
    }

    public void Apply()
    {
        StatManager.instance.Temporary_stat.EffectDamage += FireDamage;
        StatManager.instance.Temporary_stat.EffectDuration += FireDuration;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public void Remove()
    {
        StatManager.instance.Temporary_stat.EffectDamage -= FireDamage;
        StatManager.instance.Temporary_stat.EffectDuration -= FireDuration;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}
