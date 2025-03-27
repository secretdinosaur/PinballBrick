using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeManager : MonoBehaviour
{
    public TMP_Text CurrentStat;
    public GameObject AttDamage;
    public GameObject EffDamage;
    public GameObject BonusEXP;
    public GameObject CriChance;
    public GameObject CriDamage;
    public GameObject StartingBall;
    public GameObject AttDamage_;
    public GameObject EffDamage_;
    public GameObject BonusEXP_;
    public UpgradeInfo UpgradeInfo;

    public TMP_Text GoldText;
    string[] Descriptions =
        {
            "ATT DAMAGE +5",
            "EFF DAMAGE +5",
            "BONUS EXP +5%",
            "CRI CHANCE +1%",
            "CRI DAMAGE +0.05",
            "STARTING BALL +1",
            "ATT DAMAGE +5",
            "EFF DAMAGE +5",
            "BONUS EXP +5%",
            "ATT DAMAGE +5%",
            "EFF DAMAGE +5%",
            "BONUS EXP +20%",
            "CRI CHANCE +11%",
            "CRI DAMAGE +0.25",
            "STARTING BALL +2",
            "ATT DAMAGE +5%",
            "EFF DAMAGE +5%",
            "BONUS EXP +20%",
        };

    float[] UpgradeCostCoefficient =
    {
        1.01f,
        1.015f,
        1.02f,
        1.03f,
        1.03f,
        1.03f,
        1.01f,
        1.015f,
        1.02f,
    };
    private Persistent_Stat PersistentStat;
    int[] levels;
    GameObject[] Panels;

    void Start()
    {
        PersistentStat = StatManager.instance.LoadPersistentStats();
        levels = new int[] { PersistentStat.AttackDamageLevel, PersistentStat.EffectDamageLevel, PersistentStat.BonusEXPLevel, PersistentStat.CriChanceLevel, PersistentStat.CriDamageLevel, PersistentStat.StartingBallLevel,PersistentStat.AttackDamageLevel, PersistentStat.EffectDamageLevel, PersistentStat.BonusEXPLevel,  };
        Panels = new GameObject[] { AttDamage, EffDamage, BonusEXP, CriChance, CriDamage, StartingBall, AttDamage_, EffDamage_, BonusEXP_ };
        SetUpgradePanel();
    }

    void SetUpgradePanel()
    {
        TMP_Text[] texts;
        for(int i = 0; i < 9; i++)
        {
            GameObject panel = Panels[i];
            texts = panel.GetComponentsInChildren<TMP_Text>();
            texts[0].text = "LV." + levels[i] + " -) LV." + (levels[i] + 1);
            if((levels[i] + 1) % 10 == 0)
                texts[1].text = Descriptions[i + 9];
            else
                texts[1].text = Descriptions[i];
            texts[2].text = GetUpgradeCost(i).ToString();
        }
        CurrentStat.text = 
            "ATTACK DAMAGE\n" +
            "(" + PersistentStat.AttackDamage + " X " + PersistentStat.AttackDamageMultiplier + ") = " + PersistentStat.AttackDamage * PersistentStat.AttackDamageMultiplier + "\n\n" +
            "EFFECT DAMAGE\n" +
            "(" + PersistentStat.EffectDamage + " X " + PersistentStat.EffectDamageMultiplier + ") = " + PersistentStat.EffectDamage * PersistentStat.EffectDamageMultiplier + "\n\n" +
            "BONUS EXP: +" + PersistentStat.BonusExp + "%" + "\n\n" +
            "CRITICAL CHANCE: " + PersistentStat.CriticalChance + "%" + "\n\n" +
            "CRITICAL DAMAGE: X" + PersistentStat.CriticalDamage + "\n\n" +
            "STARTING BALL: +" + PersistentStat.StartingBall;
        GoldText.text = "GOLD: " + PersistentStat.Gold.ToString();
    }

    public void Purchase(int statNum)
    {
        int Cost = GetUpgradeCost(statNum);
        if (PersistentStat.Gold > Cost)
        {
            levels[statNum] += 1;
            if (statNum < 3)
                levels[statNum + 6] += 1;
            PersistentStat.Gold -= Cost;
            SetPersistentStat();
            SetUpgradePanel();
        }
        else
        {
            print("lack of gold");
        }
    }

    void SetPersistentStat()
    {
        PersistentStat.AttackDamageLevel = levels[0];
        PersistentStat.EffectDamageLevel = levels[1];
        PersistentStat.BonusEXPLevel = levels[2];
        PersistentStat.CriChanceLevel = levels[3];
        PersistentStat.CriDamageLevel = levels[4];
        PersistentStat.StartingBallLevel = levels[5];

        PersistentStat.AttackDamage = (levels[0] - levels[0] / 10) * 5;
        PersistentStat.EffectDamage = (levels[1] - levels[1] / 10) * 5;
        PersistentStat.BonusExp = (levels[2] - levels[2] / 10 - 1) * 5 + levels[2] / 10 * 20;
        PersistentStat.CriticalChance = (levels[3] - levels[3] / 10 - 1) * 1 + levels[3] / 10 * 11;
        PersistentStat.CriticalDamage = (levels[4] - levels[4] / 10 + 1) * 0.05f + levels[4] / 10 * 0.25f + 1;
        PersistentStat.StartingBall = (levels[5] - levels[5] / 10) + levels[5] / 10 * 2;

        PersistentStat.AttackDamageMultiplier = levels[0] / 10 * 0.05f + 1;
        PersistentStat.EffectDamageMultiplier = levels[1] / 10 * 0.05f + 1;

        StatManager.instance.SavePersistentStats(PersistentStat);
    }

    int GetUpgradeCost(int statNum)
    {
        int Cost = 0;
        if(levels[statNum] % 10 != 0)
            Cost = (int)(UpgradeInfo.UpgradeBasicCosts[statNum] * Mathf.Pow(UpgradeCostCoefficient[statNum], levels[statNum]));
        else
            Cost = (int)(UpgradeInfo.UpgradeBasicCosts[statNum] * Mathf.Pow(UpgradeCostCoefficient[statNum], levels[statNum])) * 2;
        return Cost;
    }
}
