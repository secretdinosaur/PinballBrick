using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;
using Button = UnityEngine.UI.Button;


public partial class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Slider LevelBar;
    public GameObject LevelUpPanel;
    public Temporary_Stat Temporary_Stat;
    public Queue<int> LevelUpQueue;
    public int Level { get; set; }
    private float exp;
    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            LevelBar.value = exp;
            if (exp < 0)
            {
                exp = 0;
            }
            if (exp >= LevelBar.maxValue)
            {
                LevelUp();
            }
        }
    }

    enum StatName
    {
        AttackDamage,
        CriticalDamage,
        CriticalChance,
        EffectDamage,
        EffectDuration,
        EffectCriticalDamage,
        GoldGain,
    }

    List<List<float>> StatUpValue = new List<List<float>>
    {
        new List<float> { 0.6f, 1.7f },
        new List<float> { 0.03f, 0.08f },
        new List<float> { 0.02f, 0.05f },
        new List<float> { 0.04f, 1.1f },
        new List<float> { 0.02f, 0.05f },
        new List<float> { 0.03f, 0.08f },
        new List<float> { 0.012f, 0.034f },
    };

    public float BaseStatUpValue;

    void Awake()
    {
        Instance = this;
        LevelBar = GameObject.Find("Canvas").transform.Find("LevelBar").GetComponent<Slider>();
        LevelUpPanel = GameObject.Find("Canvas").transform.Find("LevelUpPanel").gameObject;
        LevelUpQueue = new Queue<int>();

        NoticeLevelText = LevelUpPanel.transform.Find("NoticeLevelText").GetComponent<TMP_Text>();

        OptionButtons = new[]
        {
            LevelUpPanel.transform.Find("SelectButton_1").GetComponent<Button>(),
            LevelUpPanel.transform.Find("SelectButton_2").GetComponent<Button>(),
            LevelUpPanel.transform.Find("SelectButton_3").GetComponent<Button>()
        };
        Option_1_Name = LevelUpPanel.transform.Find("SelectButton_1").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_1_Description = LevelUpPanel.transform.Find("SelectButton_1").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_2_Name = LevelUpPanel.transform.Find("SelectButton_2").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_2_Description = LevelUpPanel.transform.Find("SelectButton_2").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_3_Name = LevelUpPanel.transform.Find("SelectButton_3").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_3_Description = LevelUpPanel.transform.Find("SelectButton_3").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();

        isAlreadyClickedButton = new bool[OptionButtons.Length];

        BaseStatUpValue = 1f;
    }
    void Start()
    {
        Exp = 0;
        Level = 1;
        Temporary_Stat = StatManager.instance.Temporary_stat;
    }

    private void FixedUpdate()
    {
        if(LevelUpQueue.Count > 0 && !LevelUpPanel.activeSelf)
        {
            ShowLevelUpPanel();
        }
    }

    public void LevelUp()
    {
        if (AugmentManager.instance.determineAugmentID("SPECIAL_66"))
        {
            DiceManager.Instance.StartCoroutine(DiceManager.Instance.RollDices(AugmentManager.instance.SpawnBallAmount, BallManager.Instance.OnCreateBall));
        }
        Exp -= LevelBar.maxValue;
        Level++;
        LevelBarMaxValueSet(Level);
        LevelUpQueue.Enqueue(Level);
    }

    public void LevelSet(int value)
    {
        Exp = 0;
        Level = value;
        LevelBarMaxValueSet(Level);
    }

    public void LevelBarMaxValueSet(int level)
    {
        LevelBar.maxValue = Mathf.Round(100 * Mathf.Pow(1.2f, level - 1));
    }

    public void GetExp(float value)
    {
        Exp += value * StatManager.instance.Persistent_stat.BonusExp;
    }

    public List<KeyValuePair<string, int>> GetRandomStats()
    {
        Dictionary<string, int> statTable = new Dictionary<string, int>
        {
            { "Attack\nDamage", 0},
            { "Critical\nDamage", 1 },
            { "Critical\nChance", 2 },
            { "Effect\nDamage", 3 },
            { "Effect\nDuration", 4 },
            { "Effect\nCritical\nDamage", 5 },
            { "Gold\nGain", 6 }
        };
        
        List<string> keys = new List<string>(statTable.Keys);
        
        List<KeyValuePair<string, int>> selectedStats = new List<KeyValuePair<string, int>>();
        while (selectedStats.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, keys.Count);
            string randomKey = keys[randomIndex];
            
            if (!selectedStats.Exists(stat => stat.Key == randomKey))
            {
                selectedStats.Add(new KeyValuePair<string, int>(randomKey, statTable[randomKey]));
            }
        }
        return selectedStats;
    }

    public void CalculateTemporaryStatWithLevel(int i) // stat formula
    {
        switch (i)
        {
            case (int)StatName.AttackDamage:
                StatManager.instance.Temporary_stat.AttackDamage += 
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.AttackDamage] % 5 == 0 
                    ? StatUpValue[(int)StatName.AttackDamage][1] : StatUpValue[(int)StatName.AttackDamage][0]);
                break;
            case (int)StatName.CriticalDamage:
                StatManager.instance.Temporary_stat.CriticalDamage += 
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.CriticalDamage] % 5 == 0 
                    ? StatUpValue[(int)StatName.CriticalDamage][1] : StatUpValue[(int)StatName.CriticalDamage][0]);
                break;
            case (int)StatName.CriticalChance:
                StatManager.instance.Temporary_stat.CriticalChance +=
                    StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance >= 1f ?
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.CriticalDamage] % 5 == 0 
                    ? StatUpValue[(int)StatName.CriticalDamage][1] : StatUpValue[(int)StatName.CriticalDamage][0]) :
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.CriticalChance] % 5 == 0 
                    ? StatUpValue[(int)StatName.CriticalChance][1] : StatUpValue[(int)StatName.CriticalChance][0]);
                break;
            case (int)StatName.EffectDamage:
                StatManager.instance.Temporary_stat.EffectDamage +=
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.EffectDamage] % 5 == 0 
                    ? StatUpValue[(int)StatName.EffectDamage][1] : StatUpValue[(int)StatName.EffectDamage][0]);
                break;
            case (int)StatName.EffectDuration:
                StatManager.instance.Temporary_stat.EffectDuration +=
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.EffectDuration] % 5 == 0 
                    ? StatUpValue[(int)StatName.EffectDuration][1] : StatUpValue[(int)StatName.EffectDuration][0]);
                break;
            case (int)StatName.EffectCriticalDamage:
                StatManager.instance.Temporary_stat.EffectCriticalDamage +=
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.EffectCriticalDamage] % 5 == 0 
                    ? StatUpValue[(int)StatName.EffectCriticalDamage][1] : StatUpValue[(int)StatName.EffectCriticalDamage][0]);
                break;
            case (int)StatName.GoldGain:
                StatManager.instance.Temporary_stat.GoldGain +=
                    BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.GoldGain] % 5 == 0 
                    ? StatUpValue[(int)StatName.GoldGain][1] : StatUpValue[(int)StatName.GoldGain][0]);
                break;
            default:
                break;
        }
        StatManager.instance.SaveTemporaryStat(StatManager.instance.Temporary_stat);
    }
}

public partial class LevelManager : MonoBehaviour
{
    public Sprite[] OptionButtonSprites;
    private TMP_Text NoticeLevelText;

    private Button[] OptionButtons;

    private TMP_Text Option_1_Name;
    private TMP_Text Option_1_Description;
    private TMP_Text Option_2_Name;
    private TMP_Text Option_2_Description;
    private TMP_Text Option_3_Name;
    private TMP_Text Option_3_Description;

    private List<KeyValuePair<string, int>> randomStats;
    private List<IAugment> augmentsSet;
    private bool CheckStatOrAugment;

    enum AugmentRarity
    {
        Myth,
        Epic,
        Legendary,
        Unique,
        Rare,
        Common
    }

    void ShowLevelUpPanel()
    {
        Time.timeScale = 0f;
        LevelUpPanel.SetActive(true);
        SoundManager.instance.LevelUpSound();
        Level = LevelUpQueue.Dequeue();

        NoticeLevelText.text = "Now Your Level: " + Level;
        optionIndex = -1;
        if (IsStatOrAugment())
        {
            randomStats = GetRandomStats();
            for (int i = 0; i < 3; i++)
            {
                OptionButtons[i].gameObject.GetComponent<Image>().sprite =
                    OptionButtonSprites
                    [
                        StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1 >= 25 ? (int)AugmentRarity.Myth :
                        StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1 >= 20 ? (int)AugmentRarity.Epic :
                        StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1 >= 15 ? (int)AugmentRarity.Legendary :
                        StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1 >= 10 ? (int)AugmentRarity.Unique :
                        StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1 >= 5 ? (int)AugmentRarity.Rare :
                        (int)AugmentRarity.Common
                    ];
            }
            Option_1_Name.text = randomStats[0].Key;
            Option_1_Description.text =
                " +"  + ((StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 1) % 5 == 0 ? 
                StatUpValue[randomStats[0].Value][1] : StatUpValue[randomStats[0].Value][0])
                + "\n\nLv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 1);

            Option_2_Name.text = randomStats[1].Key;
            Option_2_Description.text =
                " +" + ((StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 1) % 5 == 0 ? 
                StatUpValue[randomStats[1].Value][1] : StatUpValue[randomStats[1].Value][0])
                + "\n\nLv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 1);

            Option_3_Name.text = randomStats[2].Key;
            Option_3_Description.text =
                " +" + ((StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 1) % 5 == 0 ? 
                StatUpValue[randomStats[2].Value][1] : StatUpValue[randomStats[2].Value][0])
                + "\n\nLv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 1);

            CheckStatOrAugment = true;
        }
        else
        {
            augmentsSet = new List<IAugment>();
            if (AugmentManager.instance.isSacrificeQuest3Cleared)
            {
                for (int i = 0; i < 3; i++)
                {
                    augmentsSet.Add(AugmentManager.instance.GetRandomAugmentByRarity(1));
                    OptionButtons[i].gameObject.GetComponent<Image>().sprite = OptionButtonSprites[augmentsSet[i].Rarity];
                }
                AugmentManager.instance.isSacrificeQuest3Cleared = false;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    augmentsSet.Add(AugmentManager.instance.GetRandomAugment());
                    OptionButtons[i].gameObject.GetComponent<Image>().sprite = OptionButtonSprites[augmentsSet[i].Rarity];
                }
            }
            Option_1_Name.text = augmentsSet[0].Name;
            Option_1_Description.text = augmentsSet[0].Description;
            Option_2_Name.text = augmentsSet[1].Name;
            Option_2_Description.text = augmentsSet[1].Description;
            Option_3_Name.text = augmentsSet[2].Name;
            Option_3_Description.text = augmentsSet[2].Description;
            CheckStatOrAugment = false;
        }
    }

    private bool IsStatOrAugment()
    {
        return Level % 2 != 0;
    }

    private int optionIndex = -1;
    private bool[] isAlreadyClickedButton;
    public void OptionButtonClick(Button clickedButton)
    {
        if (!isAlreadyClickedButton[Array.IndexOf(OptionButtons, clickedButton)])
        {
            for (int i = 0; i < OptionButtons.Length; i++)
            {
                OptionButtons[i].transform.localScale = i == Array.IndexOf(OptionButtons, clickedButton) ? Vector3.one : new Vector3(0.93f, 0.93f, 0.93f);
                isAlreadyClickedButton[i] = i == Array.IndexOf(OptionButtons, clickedButton);
            }

            optionIndex = Array.IndexOf(OptionButtons, clickedButton);
        }
        else
        {
            LevelUpEnd();
        }
    }

    public void LevelUpEnd()
    {
        if (CheckStatOrAugment)
        {
            if (optionIndex == -1)
            {
                int RandomNumber = Random.Range(0, 3);
                StatManager.instance.Temporary_stat.StatLevel[randomStats[RandomNumber].Value]++;
                CalculateTemporaryStatWithLevel(randomStats[RandomNumber].Value);
            }
            else
            {
                StatManager.instance.Temporary_stat.StatLevel[randomStats[optionIndex].Value]++;
                CalculateTemporaryStatWithLevel(randomStats[optionIndex].Value);
            }
        }
        else
        {
            if (optionIndex == -1)
            {
                int RandomNumber = Random.Range(0, 3);
                AugmentManager.instance.AddAugment(augmentsSet[RandomNumber]);
                AugmentManager.instance.RemoveAugment(augmentsSet[RandomNumber]);
            }
            else
            {
                AugmentManager.instance.AddAugment(augmentsSet[optionIndex]);
                AugmentManager.instance.RemoveAugment(augmentsSet[optionIndex]);
            }
        }
        isAlreadyClickedButton = new bool[OptionButtons.Length];
        optionIndex = -1;
        for (int i = 0; i < OptionButtons.Length; i++)
        {
            OptionButtons[i].transform.localScale = new Vector3(0.93f, 0.93f, 0.93f);
        }
        if (LevelUpQueue.Count > 0)
        {
            ShowLevelUpPanel();
        }
        else
        {
            Time.timeScale = 1f;
            LevelUpPanel.SetActive(false);
        }
    }
}
