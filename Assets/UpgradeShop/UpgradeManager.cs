using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;
public partial class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public Canvas canvas;

    void Awake()
    {
        Instance = this;
        Panels = new GameObject[] { AttDamage, EffDamage, BonusEXP, CriChance, CriDamage, StartingBall};

        Color color = new Color(1f, 1, 1) * 3f;
        RandomAugment.GetComponent<Image>().material.SetColor("_Color", color);
        RandomAugment.GetComponent<Image>().material.SetFloat("_Alpha", 1f);
        RandomAugment.GetComponentInChildren<TextMeshProUGUI>().text = "Get Random Augmentation";
        RandomAugment.GetComponent<Button>().interactable = true;
    }
}

public partial class UpgradeManager
{
    public GameObject AttDamage;
    public GameObject EffDamage;
    public GameObject BonusEXP;
    public GameObject CriChance;
    public GameObject CriDamage;
    public GameObject StartingBall;
    public UpgradeInfo UpgradeInfo;

    public GameObject[] UpgradeButtons;

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
        1.035f,
        1.035f,
        1.05f,
        1.1f,
        1.05f,
        1.18f,
    };
    public int[] levels;
    GameObject[] Panels;
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
        while (true)
        {
            Purchase(pressedNum);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Purchase(int statNum)
    {
        int Cost = GetUpgradeCost(statNum);
        if (StatManager.instance.Persistent_stat.Gold >= Cost)
        {
            SoundManager.instance.UpgradeSound();
            levels[statNum] += 1;
            StatManager.instance.Persistent_stat.Gold -= Cost;
            SetPersistentStat();
            SetUpgradePanel();
        }
        else
        {
            //
        }
    }

    public void SetPersistentStat()
    {
        StatManager.instance.Persistent_stat.AttackDamageLevel = levels[0];
        StatManager.instance.Persistent_stat.EffectDamageLevel = levels[1];
        StatManager.instance.Persistent_stat.BonusEXPLevel = levels[2];
        StatManager.instance.Persistent_stat.CriChanceLevel = levels[3];
        StatManager.instance.Persistent_stat.CriDamageLevel = levels[4];
        StatManager.instance.Persistent_stat.StartingBallLevel = levels[5];

        StatManager.instance.Persistent_stat.AttackDamage = 5 + (levels[0] - levels[0] / 10 - 1) * 0.1f + levels[0] / 10 * 0.3f;
        StatManager.instance.Persistent_stat.EffectDamage = 5 + (levels[1] - levels[1] / 10 - 1) * 0.1f + levels[1] / 10 * 0.3f;
        StatManager.instance.Persistent_stat.BonusExp = 1 + (levels[2] - levels[2] / 10 - 1) * 0.03f + levels[2] / 10 * 0.1f;
        StatManager.instance.Persistent_stat.CriticalChance = (levels[3] - levels[3] / 10 - 1) * 0.005f + levels[3] / 10 * 0.02f;
        StatManager.instance.Persistent_stat.CriticalDamage = 1.1f + (levels[4] - levels[4] / 10 - 1) * 0.005f + levels[4] / 10 * 0.02f;
        StatManager.instance.Persistent_stat.StartingBall = 5 + (levels[5] - levels[5] / 10 - 1) + levels[5] / 10 * 3;

        StatManager.instance.SavePersistentStats();
    }

    public void SetUpgradePanel()
    {
        List<float> statList = new List<float>
        {
            StatManager.instance.Persistent_stat.AttackDamage,
            StatManager.instance.Persistent_stat.EffectDamage,
            StatManager.instance.Persistent_stat.BonusExp,
            StatManager.instance.Persistent_stat.CriticalChance,
            StatManager.instance.Persistent_stat.CriticalDamage,
            StatManager.instance.Persistent_stat.StartingBall
        };

        List<int> LevelList = new List<int>
        {
            StatManager.instance.Persistent_stat.AttackDamageLevel,
            StatManager.instance.Persistent_stat.EffectDamageLevel,
            StatManager.instance.Persistent_stat.BonusEXPLevel,
            StatManager.instance.Persistent_stat.CriChanceLevel,
            StatManager.instance.Persistent_stat.CriDamageLevel,
            StatManager.instance.Persistent_stat.StartingBallLevel
        };


        TMP_Text[] texts;
        for (int i = 0; i < 6; i++)
        {
            int Cost = GetUpgradeCost(i);
            if (StatManager.instance.Persistent_stat.Gold >= Cost)
            {
                UpgradeButtons[i].GetComponent<Image>().material.SetFloat("_HasEnoughMoney", 1);
                UpgradeButtons[i].GetComponent<Image>().material.SetColor("_Color", new Color(12 / 255f, 191 / 255f, 58 / 255f));
                UpgradeButtons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                UpgradeButtons[i].GetComponent<Image>().material.SetFloat("_HasEnoughMoney", 5);
                UpgradeButtons[i].GetComponent<Image>().material.SetColor("_Color", new Color(191 / 255f, 12 / 255f, 15 / 255f));
                UpgradeButtons[i].GetComponent<Button>().interactable = false;
            }
            switch (i)
            {
                case 0: //Attack Damage
                    if ((levels[i] + 1) % 10 == 0)
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F1")
                            + " -> " +
                            (statList[i] + Descriptions[i + 6]).ToString("F1");
                    else
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F1")
                            + " -> " +
                            (statList[i] + Descriptions[i]).ToString("F1");
                    break;
                case 1: //Effect Damage
                    if ((levels[i] + 1) % 10 == 0)
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F1")
                            + " -> " +
                            (statList[i] + Descriptions[i + 6]).ToString("F1");
                    else
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F1")
                            + " -> " +
                            (statList[i] + Descriptions[i]).ToString("F1");
                    break;
                case 2:// bonus exp
                    if ((levels[i] + 1) % 10 == 0)
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F0")
                            + "%" + " -> " +
                            ((statList[i] + Descriptions[i + 6] - 1) * 100).ToString("F0") + "%";
                    else
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F0")
                            + "%" + " -> " +
                            ((statList[i] + Descriptions[i] - 1) * 100).ToString("F0") + "%";
                    break;
                case 3: //Critical Chance
                    if ((levels[i] + 1) % 10 == 0)
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i]) * 100).ToString("F1")
                            + "%" + " -> " +
                            ((statList[i] + Descriptions[i + 6]) * 100).ToString("F1") + "%";
                    else
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i]) * 100).ToString("F1")
                            + "%" + " -> " +
                            ((statList[i] + Descriptions[i]) * 100).ToString("F1") + "%";
                    break;
                case 4: //Critical Damage
                    if ((levels[i] + 1) % 10 == 0)
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F1")
                            + "%" + " -> " +
                            ((statList[i] + Descriptions[i + 6] - 1) * 100).ToString("F1") + "%";
                    else
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = ((statList[i] - 1) * 100).ToString("F1")
                            + "%" + " -> " +
                            ((statList[i] + Descriptions[i] - 1) * 100).ToString("F1") + "%";
                    break;
                case 5: //Starting Ball
                    if ((levels[i] + 1) % 10 == 0)
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F0")
                            + " -> " +
                            (statList[i] + Descriptions[i + 6]).ToString("F0");
                    else
                        Panels[i].GetComponentsInChildren<TMP_Text>()[1].text = statList[i].ToString("F0")
                            + " -> " +
                            (statList[i] + Descriptions[i]).ToString("F0");
                    break;
                default:
                    break;

            }
            Panels[i].GetComponentsInChildren<TMP_Text>()[2].text = GetUpgradeCost(i).ToString();
            Panels[i].GetComponentsInChildren<TMP_Text>()[3].text = LevelList[i].ToString();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());
        canvas.GetComponent<RectTransform>().ForceUpdateRectTransforms();
        UpdateGoldUI();
        canvas.gameObject.SetActive(false);
        canvas.gameObject.SetActive(true);
        UpgradeButtons[0].GetComponent<Image>().SetLayoutDirty();
        UpgradeButtons[1].GetComponent<Image>().SetLayoutDirty();
        UpgradeButtons[2].GetComponent<Image>().SetLayoutDirty();
        UpgradeButtons[3].GetComponent<Image>().SetLayoutDirty();
        UpgradeButtons[4].GetComponent<Image>().SetLayoutDirty();
        UpgradeButtons[5].GetComponent<Image>().SetLayoutDirty();
        UpgradeButtons[0].GetComponent<Image>().SetAllDirty();
        UpgradeButtons[1].GetComponent<Image>().SetAllDirty();
        UpgradeButtons[2].GetComponent<Image>().SetAllDirty();
        UpgradeButtons[3].GetComponent<Image>().SetAllDirty();
        UpgradeButtons[4].GetComponent<Image>().SetAllDirty();
        UpgradeButtons[5].GetComponent<Image>().SetAllDirty();
    }

    public void UpdateGoldUI()
    {
        GoldText.text = ChangeNumber(StatManager.instance.Persistent_stat.Gold.ToString("F0"));
    }

    int GetUpgradeCost(int statNum)
    {
        int Cost = 0;
        if (levels[statNum] % 10 != 9)
            Cost = (int)(UpgradeInfo.UpgradeBasicCosts[statNum] * Mathf.Pow(UpgradeCostCoefficient[statNum], levels[statNum]));
        else
            Cost = (int)(UpgradeInfo.UpgradeBasicCosts[statNum] * Mathf.Pow(UpgradeCostCoefficient[statNum], levels[statNum])) * 2;
        return Cost;
    }

    public void InitializeStat()
    {
        int totalGold = 0;
        for (int statNum = 0; statNum < levels.Length; statNum++)
        {
            float baseCost = UpgradeInfo.UpgradeBasicCosts[statNum];
            float coeff = UpgradeCostCoefficient[statNum];

            for (int lvl = 0; lvl < levels[statNum]; lvl++)
            {
                int cost = (int)(baseCost * Mathf.Pow(coeff, lvl));
                if (lvl % 10 == 9)
                    cost *= 2; // 매 10레벨마다 비용 2배

                totalGold += cost;
            }
        }
        StatManager.instance.InitializePersistentStats();
        StatManager.instance.GetGold(totalGold);
        SetUpgradePanel();
        SetPersistentStat();


    }
} //Stat Upgrade

public partial class UpgradeManager //Powers
{
    public GameObject RandomAugment;
    private int Count;

    public void SettingRandomAugment()
    {
        Color color = new Color(191 / 255f, 0f, 0f) * 3f;
        RandomAugment.GetComponent<Image>().material.SetColor("_Color", color);
        RandomAugment.GetComponent<Image>().material.SetFloat("_Alpha", 3f);
        RandomAugment.GetComponentInChildren<TextMeshProUGUI>().text = "Augment is ready";
        RandomAugment.GetComponent<Button>().interactable = false;
    }

    public void GetRandomAugment()
    {
        float Cost = 1000;
        if (StatManager.instance.Persistent_stat.Gold >= Cost)
        {
            StatManager.instance.Persistent_stat.Gold -= Cost;
            AugmentManager.RandomAugment =
                Random.value < 0.05 ? AugmentManager.instance.GetRandomAugmentByRarity(1) :
                Random.value < 0.15 ? AugmentManager.instance.GetRandomAugmentByRarity(2) :
                Random.value < 0.4 ? AugmentManager.instance.GetRandomAugmentByRarity(3) :
                AugmentManager.instance.GetRandomAugmentByRarity(4);
            SettingRandomAugment();
            UpdateGoldUI();
        }
    }
}