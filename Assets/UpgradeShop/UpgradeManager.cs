using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
public class UpgradeManager : MonoBehaviour
{
    public GameObject AttDamage;
    public GameObject EffDamage;
    public GameObject BonusEXP;
    public GameObject CriChance;
    public GameObject CriDamage;
    public GameObject StartingBall;
    public UpgradeInfo UpgradeInfo;

    public TMP_Text GoldText;
    float[] Descriptions =
        {
            0.1f,
            0.1f,
            0.03f,
            0.005f,
            0.005f,
            1,

            // per 10 level

            0.3f,
            0.3f,
            0.1f,
            0.02f,
            0.02f,
            3,
        };

    float[] UpgradeCostCoefficient =
    {
        1.025f,
        1.025f,
        1.05f,
        1.1f,
        1.05f,
        1.2f,
    };
    private Persistent_Stat PersistentStat;
    int[] levels;
    GameObject[] Panels;

    public static UpgradeManager Instance;

    void Start()
    {
        Instance = this;
        PersistentStat = StatManager.instance.Persistent_stat;
        levels = new int[] { PersistentStat.AttackDamageLevel, PersistentStat.EffectDamageLevel, PersistentStat.BonusEXPLevel, PersistentStat.CriChanceLevel, PersistentStat.CriDamageLevel, PersistentStat.StartingBallLevel};
        Panels = new GameObject[] { AttDamage, EffDamage, BonusEXP, CriChance, CriDamage, StartingBall};
        SetUpgradePanel();
    }

    void SetUpgradePanel()
    {
        List<float> statList = new List<float>
        {
            PersistentStat.AttackDamage,
            PersistentStat.EffectDamage,
            PersistentStat.BonusExp,
            PersistentStat.CriticalChance,
            PersistentStat.CriticalDamage,
            PersistentStat.StartingBall
        };

        List<int> LevelList = new List<int>
        {
            PersistentStat.AttackDamageLevel,
            PersistentStat.EffectDamageLevel,
            PersistentStat.BonusEXPLevel,
            PersistentStat.CriChanceLevel,
            PersistentStat.CriDamageLevel,
            PersistentStat.StartingBallLevel
        };

        
        TMP_Text[] texts;
        for(int i = 0; i < 6; i++)
        {
            if(i == 2)// bonus exp
            {
                if ((levels[i] + 1) % 10 == 0)
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F0") + "%" + " -> " + ((statList[i] + Descriptions[i + 6] - 1) * 100).ToString("F0") + "%";
                else
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F0") + "%" + " -> " + ((statList[i] + Descriptions[i] - 1) * 100).ToString("F0") + "%";
            }
            else if (i == 4) // cri damage
            {
                if ((levels[i] + 1) % 10 == 0)
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F1") + "%" + " -> " + ((statList[i] + Descriptions[i + 6] - 1) * 100).ToString("F1") + "%";
                else
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F1") + "%" + " -> " + ((statList[i] + Descriptions[i] - 1) * 100).ToString("F1") + "%";
            }
            else if (i == 3) // cri chance
            {
                if ((levels[i] + 1) % 10 == 0)
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i]) * 100).ToString("F1") + "%" + " -> " + ((statList[i] + Descriptions[i + 6]) * 100).ToString("F1") + "%";
                else
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i]) * 100).ToString("F1") + "%" + " -> " + ((statList[i] + Descriptions[i]) * 100).ToString("F1") + "%";
            }
            else if(i == 0 || i == 1) // damage
            {
                if ((levels[i] + 1) % 10 == 0)
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F1") + " -> " + (statList[i] + Descriptions[i + 6]).ToString("F1");
                else
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F1") + " -> " + (statList[i] + Descriptions[i]).ToString("F1");
            }
            else // starting ball
            {
                if ((levels[i] + 1) % 10 == 0)
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F0") + " -> " + (statList[i] + Descriptions[i + 6]).ToString("F0");
                else
                    Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F0") + " -> " + (statList[i] + Descriptions[i]).ToString("F0");
            }

            Panels[i].GetComponentsInChildren<TMP_Text>()[2].text = GetUpgradeCost(i).ToString();
            Panels[i].GetComponentsInChildren<TMP_Text>()[3].text = LevelList[i].ToString();
        }
        
        GoldText.text = ChangeNumber(PersistentStat.Gold.ToString());
    }

    public static string ChangeNumber(string number)
    {
        char[] unitAlphabet = new char[3] { 'K', 'M', 'B' };
        int unit = 0;
        while (number.Length > 6)
        {
            unit++;
            number = number.Substring(0, number.Length - 3);
        }
        if (number.Length > 3)
        {
            int newInt = int.Parse(number);
            if (number.Length > 4)
            {
                return (newInt / 1000).ToString() + unitAlphabet[unit];
            }
            else
            {
                return (newInt / 1000f).ToString("0.0") + unitAlphabet[unit];
            }
        }
        else
        {
            int newInt = int.Parse(number);
            return (newInt).ToString();
        }
    }

    public IEnumerator RepeatPurchace(int pressedNum)
    {
        while(true)
        {
            Purchase(pressedNum);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Purchase(int statNum)
    {
        int Cost = GetUpgradeCost(statNum);
        if (PersistentStat.Gold > Cost)
        {
            levels[statNum] += 1;
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

        PersistentStat.AttackDamage = 5 + (levels[0] - levels[0] / 10 - 1) * 0.1f + levels[0] / 10 * 0.3f;
        PersistentStat.EffectDamage = 5 + (levels[1] - levels[1] / 10 - 1) * 0.1f + levels[1] / 10 * 0.3f;
        PersistentStat.BonusExp = 1 + (levels[2] - levels[2] / 10 - 1) * 0.03f + levels[2] / 10 * 0.1f;
        PersistentStat.CriticalChance = (levels[3] - levels[3] / 10 - 1) * 0.005f + levels[3] / 10 * 0.02f;
        PersistentStat.CriticalDamage = 1 + (levels[4] - levels[4] / 10 - 1) * 0.005f + levels[4] / 10 * 0.02f;
        PersistentStat.StartingBall = 5 + (levels[5] - levels[5] / 10 - 1) + levels[5] / 10 * 3;

        StatManager.instance.Persistent_stat = PersistentStat;
        StatManager.instance.SavePersistentStats();
    }

    int GetUpgradeCost(int statNum)
    {
        int Cost = 0;
        if(levels[statNum] % 10 != 9)
            Cost = (int)(UpgradeInfo.UpgradeBasicCosts[statNum] * Mathf.Pow(UpgradeCostCoefficient[statNum], levels[statNum]));
        else
            Cost = (int)(UpgradeInfo.UpgradeBasicCosts[statNum] * Mathf.Pow(UpgradeCostCoefficient[statNum], levels[statNum])) * 2;
        return Cost;
    }
}
