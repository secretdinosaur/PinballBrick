using UnityEngine;

public class FireBall : ISpecialAugment
{
    public string Name => "FireBall";
    public string Description => "Ball Become a Fireball";
    public string Type => "Effect";

    public void Apply()
    {
        StatManager.instance.Temporary_stat.EffectDamage += 0.1f;
        StatManager.instance.Temporary_stat.EffectDuration += 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }

    public void Remove()
    {
        StatManager.instance.Temporary_stat.EffectDamage -= 0.1f;
        StatManager.instance.Temporary_stat.EffectDuration -= 0.5f;
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}
