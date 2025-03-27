using UnityEngine;

[System.Serializable]
public class Persistent_Stat  //Out Game Stats(Upgradeable Stats Using Gold)
{
    public float AttackDamage;
    public float EffectDamage;
    public float AttackDamageMultiplier;
    public float EffectDamageMultiplier;
    public int StartingBall;
    public float BonusExp;
    public float CriticalChance;
    public float CriticalDamage;
    public float Gold;

    public int AttackDamageLevel;
    public int EffectDamageLevel;
    public int BonusEXPLevel;
    public int CriChanceLevel;
    public int CriDamageLevel;
    public int StartingBallLevel;
}

[System.Serializable]
public class Temporary_Stat //In Game Stats(Upgradeable Stats Using Skill Points)
{
    public int[] StatLevel;
    public float AttackDamage;
    public float CriticalDamage;
    public float CriticalChance;
    public float BallSpeed;
    public float EffectDamage;
    public float EffectDuration;
    public float EffectCriticalDamage;
    public float GoldGain;
}

[System.Serializable]
public class StatMultiplier
{
    public float AttackDamageMultiplier;
    public float EffectDamageMultiplier;
}
