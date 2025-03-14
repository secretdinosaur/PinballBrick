using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeManager : MonoBehaviour
{
    public TMP_Text CurrentStat;
    public GameObject AttDamage;
    public GameObject EffDamage;
    public GameObject BonusEXP;
    public GameObject CriDamage;
    public GameObject CriChance;
    public GameObject StartingBall;
    public UpgradeInfo UpgradeInfo;
    private float Gold;

    private Persistent_Stat PersistentStat;

    void Start()
    {
        PersistentStat = StatManager.instance.LoadPersistentStats();

        SetUpgradePanel();
    }

    void SetUpgradePanel()
    {
        CurrentStat.text = 
            "ATTACK DAMAGE\n" +
            "(" + PersistentStat.AttackDamage + " X " + PersistentStat.AttackDamageMultiflier + ") = " + PersistentStat.AttackDamage * PersistentStat.AttackDamageMultiflier + "\n\n" +
            "EFFECT DAMAGE\n" +
            "(" + PersistentStat.EffectDamage + " X " + PersistentStat.EffectDamageMultiflier + ") = " + PersistentStat.EffectDamage * PersistentStat.EffectDamageMultiflier + "\n\n" +
            "BONUS EXP: +" + PersistentStat.BonusExp + "%" + "\n\n" +
            "CRITICAL CHANCE: " + PersistentStat.CriticalChance + "%" + "\n\n" +
            "CRITICAL DAMAGE: X" + PersistentStat.CriticalDamage + "\n\n" +
            "STARTING BALL: +" + PersistentStat.StartingBall;
    }

    void Purchase(GameObject UpgradePanel)
    {

    }

    int GetUpgradeCost(string stat)
    {
        int Cost = 0;
        int level;

        switch (stat)
        {
            case "AttDamage":
                break;
            case "EffDamage":
                break;
            case "BonusEXP":
                break;
            case "CriChance":
                break;
            case "CriDamage":
                break;
            case "StartingBall":
                break;

        }

        return Cost;
    }
}
