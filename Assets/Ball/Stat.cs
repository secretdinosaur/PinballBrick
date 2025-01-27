using UnityEngine;

[System.Serializable]
public class Persistent_Stat  //Out Game Stats(Upgradeable Stats Using Gold)
{
    public int AttackDamage;
    public int StartingBall;
    public float BonusExp;
    public float ItemDropChance;
    public float CriticalChance;
    public float CriticalDamage;
    public float Gold;
}

[System.Serializable]
public class Temporary_Stat //In Game Stats(Upgradeable Stats Using Skill Points)
{
    public int[] StatLevel;
    public int AttackDamage;
    public float CriticalDamage;
    public float CriticalChance;
    public float BallSpeed;
    public float EffectDamage;
    public float EffectDuration;
    public float GoldGain;
}

[System.Serializable]
public class StatMultiplier
{
    public float AttackDamageMultiplier;
    public float EffectDamageMultiplier;
}
