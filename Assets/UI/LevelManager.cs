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
using Color = UnityEngine.Color;
using UnityEngine.InputSystem;
using System.Collections;
using System.Drawing;
using static UnityEngine.Analytics.IAnalytic;
using System.Xml;
using DG.Tweening;
using Unity.Android.Gradle.Manifest;
using System.Linq;
using UnityEditor.VisionOS;


public partial class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Slider LevelBar;
    public GameObject LevelUpPanel;
    public GameObject CanvasForLevelUpPanel;
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
            if (exp >= LevelBar.maxValue && !GameManager.instance.isGameEnded)
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
        GoldGain,
        ExpGain,
    }

    public List<List<float>> StatUpValue = new List<List<float>>
    {
        new List<float> { 0.5f, 1f }, //AttackDamage
        new List<float> { 0.05f, 0.1f }, //CriticalDamage
        new List<float> { 0.02f, 0.05f }, //CriticalChance
        new List<float> { 0.5f, 1f }, //EffectDamage
        new List<float> { 0.05f, 0.1f }, //EffectDuration
        new List<float> { 0.1f, 0.2f }, //GoldGain
        new List<float> { 0.05f, 0.1f }, //ExpGain
    };

    public float BaseStatUpValue;
    public float BaseMaxExpValue;

    void Awake()
    {
        Instance = this;
        LevelBar = GameObject.Find("Canvas").transform.Find("LevelBar").GetComponent<Slider>();
        LevelUpPanel = GameObject.Find("Canvas").transform.Find("LevelUpPanel").gameObject;
        CanvasForLevelUpPanel = GameObject.Find("Canvas").transform.Find("CanvasForLevelUpPanel").gameObject;
        LevelUpPanel.SetActive(false);
        CanvasForLevelUpPanel.SetActive(false);
        LevelUpQueue = new Queue<int>();

        NoticeLevelText = CanvasForLevelUpPanel.transform.Find("NoticeLevelText").GetComponent<TMP_Text>();

        OptionButtons = new[]
        {
            CanvasForLevelUpPanel.transform.Find("SelectButton_1").GetComponent<Button>(),
            CanvasForLevelUpPanel.transform.Find("SelectButton_2").GetComponent<Button>(),
            CanvasForLevelUpPanel.transform.Find("SelectButton_3").GetComponent<Button>()
        };

        OptionButtonShaders = new[]
        {
            CanvasForLevelUpPanel.transform.Find("RarityShader1").gameObject,
            CanvasForLevelUpPanel.transform.Find("RarityShader2").gameObject,
            CanvasForLevelUpPanel.transform.Find("RarityShader3").gameObject,
        };

        Option_1_Name = CanvasForLevelUpPanel.transform.Find("SelectButton_1").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_1_Description = CanvasForLevelUpPanel.transform.Find("SelectButton_1").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_1_Aura = Instantiate(CanvasForLevelUpPanel.transform.Find("RarityShader1").gameObject.GetComponent<Image>().material);
        CanvasForLevelUpPanel.transform.Find("RarityShader1").gameObject.GetComponent<Image>().material = Option_1_Aura;
        Option_1_Panel = Instantiate(CanvasForLevelUpPanel.transform.Find("SelectButton_1").gameObject.GetComponent<Image>().material);
        CanvasForLevelUpPanel.transform.Find("SelectButton_1").gameObject.GetComponent<Image>().material = Option_1_Panel;
        Option_1_Type = CanvasForLevelUpPanel.transform.Find("SelectButton_1/Type").GetComponent<TMP_Text>();
        Option_1_CoolTime = CanvasForLevelUpPanel.transform.Find("SelectButton_1/CoolTime").GetComponent<TMP_Text>();

        Option_2_Name = CanvasForLevelUpPanel.transform.Find("SelectButton_2").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_2_Description = CanvasForLevelUpPanel.transform.Find("SelectButton_2").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_2_Aura = Instantiate(CanvasForLevelUpPanel.transform.Find("RarityShader2").gameObject.GetComponent<Image>().material);
        CanvasForLevelUpPanel.transform.Find("RarityShader2").gameObject.GetComponent<Image>().material = Option_2_Aura;
        Option_2_Panel = Instantiate(CanvasForLevelUpPanel.transform.Find("SelectButton_2").gameObject.GetComponent<Image>().material);
        CanvasForLevelUpPanel.transform.Find("SelectButton_2").gameObject.GetComponent<Image>().material = Option_2_Panel;
        Option_2_Type = CanvasForLevelUpPanel.transform.Find("SelectButton_2/Type").GetComponent<TMP_Text>();
        Option_2_CoolTime = CanvasForLevelUpPanel.transform.Find("SelectButton_2/CoolTime").GetComponent<TMP_Text>();

        Option_3_Name = CanvasForLevelUpPanel.transform.Find("SelectButton_3").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_3_Description = CanvasForLevelUpPanel.transform.Find("SelectButton_3").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_3_Aura = Instantiate(CanvasForLevelUpPanel.transform.Find("RarityShader3").gameObject.GetComponent<Image>().material);
        CanvasForLevelUpPanel.transform.Find("RarityShader3").gameObject.GetComponent<Image>().material = Option_3_Aura;
        Option_3_Panel = Instantiate(CanvasForLevelUpPanel.transform.Find("SelectButton_3").gameObject.GetComponent<Image>().material);
        CanvasForLevelUpPanel.transform.Find("SelectButton_3").gameObject.GetComponent<Image>().material = Option_3_Panel;
        Option_3_Type = CanvasForLevelUpPanel.transform.Find("SelectButton_3/Type").GetComponent<TMP_Text>();
        Option_3_CoolTime = CanvasForLevelUpPanel.transform.Find("SelectButton_3/CoolTime").GetComponent<TMP_Text>();

        AdditionalDescriptionPanel = CanvasForLevelUpPanel.transform.Find("AdditionalDescription").gameObject;
        AdditionalDescriptionPanel.SetActive(false);
        AdditionalDescriptionText = AdditionalDescriptionPanel.transform.Find("Text").GetComponent<TMP_Text>();

        isAlreadyClickedButton = new bool[OptionButtons.Length];

        BaseStatUpValue = 1f;
        BaseMaxExpValue = 1f;

        CustomTime_1 = 0f;
        CustomTime_2 = 1f;
        CustomTime_3 = 2f;
    }
    void Start()
    {
        Exp = 0;
        Level = 1;

    }

    private void FixedUpdate()
    {
        if (LevelUpQueue.Count > 0 && !LevelUpPanel.activeSelf && !GameManager.instance.isGameEnded)
        {
            ShowLevelUpPanel();
        }
    }

    private void Update()
    {
        if (LevelUpPanel.activeSelf)
        {
            CustomTime_1 += Time.unscaledDeltaTime * Random.Range(0.1f, 1f);
            CustomTime_2 += Time.unscaledDeltaTime * Random.Range(0.1f, 1f);
            CustomTime_3 += Time.unscaledDeltaTime * Random.Range(0.1f, 1f);

            Option_1_Aura.SetFloat("_CustomTime", CustomTime_1);
            Option_2_Aura.SetFloat("_CustomTime", CustomTime_2);
            Option_3_Aura.SetFloat("_CustomTime", CustomTime_3);

            Option_1_Panel.SetFloat("_CustomTime", CustomTime_1);
            Option_2_Panel.SetFloat("_CustomTime", CustomTime_2);
            Option_3_Panel.SetFloat("_CustomTime", CustomTime_3);

            Option_1_Aura.SetColor("_Color", RarityColorList[0]);
            Option_2_Aura.SetColor("_Color", RarityColorList[0]);
            Option_3_Aura.SetColor("_Color", RarityColorList[0]);

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
        LevelBar.maxValue = BaseMaxExpValue * 
            (level > 90 ?
                110 + 98 * (level - 1) :
            level > 60 ?
                100 + 82 * (level - 1) :
            level > 30 ?
                80 + 62 * (level - 1) :
            //level <= 30
                50 + 27 * (level - 1));
    }

    public void GetExp(float value)
    {
        Exp += value * (StatManager.instance.Persistent_stat.BonusExp + StatManager.instance.Temporary_stat.ExpGain);
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
            { "Gold\nGain", 5 },
            { "Exp\nGain", 6 }
        };
        
        List<string> keys = new List<string>(statTable.Keys);

        if (AugmentManager.instance.determineAugmentID("SPECIAL_59"))
        {
            keys.RemoveAt(5);
        }

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
        float chanceSum = StatManager.instance.Persistent_stat.CriticalChance + StatManager.instance.Temporary_stat.CriticalChance;

        switch (i)
        {
            case (int)StatName.AttackDamage:
                StatManager.instance.ChangeTemporaryStats_AttackDamage
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.AttackDamage] % 5 == 0
                        ? StatUpValue[(int)StatName.AttackDamage][1] : StatUpValue[(int)StatName.AttackDamage][0])
                    );
                break;
            case (int)StatName.CriticalDamage:
                StatManager.instance.ChangeTemporaryStats_CriticalDamage
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.CriticalDamage] % 5 == 0
                        ? StatUpValue[(int)StatName.CriticalDamage][1] : StatUpValue[(int)StatName.CriticalDamage][0])
                    );
                break;
            case (int)StatName.CriticalChance:
                StatManager.instance.ChangeTemporaryStats_CriticalChance
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.CriticalChance] % 5 == 0
                        ? StatUpValue[(int)StatName.CriticalChance][1]
                        : StatUpValue[(int)StatName.CriticalChance][0])
                    );
                break;
            case (int)StatName.EffectDamage:
                StatManager.instance.ChangeTemporaryStats_EffectDamage
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.EffectDamage] % 5 == 0
                        ? StatUpValue[(int)StatName.EffectDamage][1] : StatUpValue[(int)StatName.EffectDamage][0])
                    );
                break;
            case (int)StatName.EffectDuration:
                StatManager.instance.ChangeTemporaryStats_EffectDuration
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.EffectDuration] % 5 == 0
                        ? StatUpValue[(int)StatName.EffectDuration][1] : StatUpValue[(int)StatName.EffectDuration][0])
                    );
                break;
            case (int)StatName.GoldGain:
                StatManager.instance.ChangeTemporaryStats_GoldGain
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.GoldGain] % 5 == 0
                        ? StatUpValue[(int)StatName.GoldGain][1] : StatUpValue[(int)StatName.GoldGain][0])
                    );
                break;
            case (int)StatName.ExpGain:
                StatManager.instance.ChangeTemporaryStats_ExpGain
                    (
                        BaseStatUpValue * (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.ExpGain] % 5 == 0
                        ? StatUpValue[(int)StatName.ExpGain][1] : StatUpValue[(int)StatName.ExpGain][0])
                    );
                break;
            default:
                break;
        }
    }
}

public partial class LevelManager : MonoBehaviour
{
    public Sprite[] OptionButtonSprites;
    private TMP_Text NoticeLevelText;

    public Button[] OptionButtons;
    private GameObject[] OptionButtonShaders;

    private TMP_Text Option_1_Name;
    private TMP_Text Option_1_Description;
    private TMP_Text Option_1_Type;
    private TMP_Text Option_1_CoolTime;
    private Material Option_1_Aura;
    private Material Option_1_Panel;
    private TMP_Text Option_2_Name;
    private TMP_Text Option_2_Description;
    private TMP_Text Option_2_Type;
    private TMP_Text Option_2_CoolTime;
    private Material Option_2_Aura;
    private Material Option_2_Panel;
    private TMP_Text Option_3_Name;
    private TMP_Text Option_3_Description;
    private TMP_Text Option_3_Type;
    private TMP_Text Option_3_CoolTime;
    private Material Option_3_Aura;
    private Material Option_3_Panel;

    private List<KeyValuePair<string, int>> randomStats;
    private List<IAugment> augmentsSet;
    private bool CheckStatOrAugment;

    private GameObject AdditionalDescriptionPanel;
    private TMP_Text AdditionalDescriptionText;

    private float CustomTime_1;
    private float CustomTime_2;
    private float CustomTime_3;

    List<Color> RarityColorList = new List<Color>()
    {
        Color.cyan * 2,
        new Color(1.0f, 0f, 129 / 255f) * 2,
        Color.yellow * 2,
        new Color(1.0f, 0.41f, 0.71f) * 2,
        new Color(51 / 255f, 255 / 255f, 4 / 255f) * 2,
        Color.white * 2
    };

    enum AugmentRarity
    {
        Myth,
        Epic,
        Legendary,
        Unique,
        Rare,
        Common
    }
    IEnumerator AugAniCoroutine(int[] rarities, Button[] SelectButtons, List<IAugment> augments)
    {
        Option_1_Aura.SetFloat("_OuterFade", 0f);
        Option_2_Aura.SetFloat("_OuterFade", 0f);
        Option_3_Aura.SetFloat("_OuterFade", 0f);

        for (int i = 0; i < 3; i++)
        {
            switch (rarities[i])
            {
                case 5:
                    yield return StartCoroutine(MovePanel(i));
                    yield return new WaitForSecondsRealtime(0.5f);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    break;

                case 4:
                    yield return StartCoroutine(MovePanel(i));
                    yield return new WaitForSecondsRealtime(0.5f);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    break;

                case 3:
                    yield return StartCoroutine(MovePanel(i));
                    yield return StartCoroutine(SetParticle(3, i));
                    yield return new WaitForSecondsRealtime(0.5f);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    break;

                case 2:
                    yield return StartCoroutine(MovePanel(i));
                    yield return StartCoroutine(SetParticle(2, i));
                    yield return new WaitForSecondsRealtime(0.5f);
                    yield return StartCoroutine(FlashPanel(2, i));
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    ParticleBurst(2, i, 50);
                    break;

                case 1:
                    int j = Random.Range(0, 2);
                    if(j == 0)
                    {
                        yield return StartCoroutine(MovePanel(i));
                        yield return StartCoroutine(SetParticle(1, i));

                    }
                    else
                    {
                        yield return StartCoroutine(MovePanel(i));
                        yield return StartCoroutine(SetParticle(2, i));
                        yield return StartCoroutine(FlashPanel(2, i));
                        yield return StartCoroutine(ShakePanel(i));
                    }
                    yield return StartCoroutine(FlashPanel(1, i));
                    yield return new WaitForSecondsRealtime(0.5f);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    ParticleBurst(1, i, 100);
                    break;


                case 0:

                    int k = Random.Range(0, 3);
                    if (k == 0)
                    {
                        yield return StartCoroutine(MovePanel(i));
                        yield return StartCoroutine(SetParticle(0, i));
                    }
                    else if(k == 1)
                    {
                        yield return StartCoroutine(MovePanel(i));
                        yield return StartCoroutine(SetParticle(2, i));
                        yield return StartCoroutine(FlashPanel(2, i));
                        yield return StartCoroutine(ShakePanel(i));
                        yield return StartCoroutine(FlashPanel(1, i));
                        yield return StartCoroutine(ShakePanel(i));
                    }
                    else
                    {
                        yield return StartCoroutine(MovePanel(i));
                        yield return StartCoroutine(SetParticle(1, i));
                        yield return StartCoroutine(FlashPanel(1, i));
                        yield return StartCoroutine(ShakePanel(i));
                    }

                    yield return StartCoroutine(FlashPanel(0, i));
                    yield return new WaitForSecondsRealtime(0.5f);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    ParticleBurst(0, i, 150);
                    SetBackLight(i);
                    break;
            }
        }

        IEnumerator MovePanel(int num)
        {
            Vector3 FinalPosition = new Vector3(260 * (num - 1), 0, 0);
            bool isFinished = false;
            SelectButtons[num].transform.DOLocalMove(FinalPosition, 0.25f)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true)
                        .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
        }

        IEnumerator SetParticle(int rarity, int num)
        {
            bool isFinished = false;
            VisualEffectManager.Instance.SetActiveAugmentPanelPs(num, RarityColorList[rarity], rarity);
            VisualEffectManager.Instance.AugmentPanelPS[num].transform.DOMove(new Vector3(VisualEffectManager.Instance.AugmentPanelPS[num].transform.position.x, 0, 0), 0.25f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
        }

        IEnumerator FlashPanel(int rarity, int num)
        {
            bool isFinished = false;
            SelectButtons[num].GetComponent<Image>().material.DOColor(RarityColorList[rarity] * 2, 0.1f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
            SelectButtons[num].GetComponent<Image>().material.DOColor(RarityColorList[rarity], 0.1f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
        }

        void ParticleBurst(int rarity, int num, int Emit)
        {
            bool isFinished = false;
            VisualEffectManager.Instance.AugmentPanelBurstPS[num].GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", RarityColorList[rarity]);
            VisualEffectManager.Instance.AugmentPanelPS[num].GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", RarityColorList[rarity]);
            VisualEffectManager.Instance.AugmentPanelBurstPS[num].Emit(Emit);
            SelectButtons[num].GetComponent<Image>().material.SetFloat("_IsCornerEffect", 1);
        }

        IEnumerator ShakePanel(int num)
        {
            bool isFinished = false;
            SelectButtons[num].transform.transform.DOShakePosition(2, new Vector3(3, 0, 0), 10, 1, false, false)
                 .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
        }

        void SetBackLight(int i)
        {
            if (i == 0)
                Option_1_Aura.SetFloat("_OuterFade", 50f);
            else if (i == 1)
                Option_2_Aura.SetFloat("_OuterFade", 50f);
            else if (i == 2)
                Option_3_Aura.SetFloat("_OuterFade", 50f);
        }
    }

    IEnumerator AugAniCoroutineForStat(int[] rarities, Button[] SelectButtons, List<KeyValuePair<string, int>> stats)
    {

        Option_1_Aura.SetFloat("_OuterFade", 0f);
        Option_2_Aura.SetFloat("_OuterFade", 0f);
        Option_3_Aura.SetFloat("_OuterFade", 0f);

        for (int i = 0; i < 3; i++)
        {
            SetAugmentPanel(i, stats[i].Key, LevelUpTextWhenStatUp(i), "", 0);
            rarities[i] = 5 - (rarities[i] / 5);
            if (rarities[i] < 0)
                rarities[i] = 0;
            bool isFinished = false;
            Vector3 FinalPosition = new Vector3(260 * (i - 1), 0, 0);

            SelectButtons[i].transform.DOLocalMove(FinalPosition, 0.25f)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true)
                        .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            switch (rarities[i])
            {
                case 3:
                    yield return StartCoroutine(SetParticle(3, i));
                    break;

                case 2:
                    yield return StartCoroutine(SetParticle(2, i));
                    yield return StartCoroutine(FlashPanel(2, i));
                    ParticleBurst(2, i, 50);
                    break;

                case 1:
                    yield return StartCoroutine(SetParticle(1, i));
                    yield return StartCoroutine(FlashPanel(1, i));
                    ParticleBurst(1, i, 100);
                    break;

                case 0:
                    yield return StartCoroutine(SetParticle(0, i));
                    yield return StartCoroutine(FlashPanel(0, i));
                    ParticleBurst(0, i, 150);
                    SetBackLight(i);
                    break;
            }
        }

        IEnumerator SetParticle(int rarity, int num)
        {
            bool isFinished = false;
            VisualEffectManager.Instance.SetActiveAugmentPanelPs(num, RarityColorList[rarity], rarity);
            VisualEffectManager.Instance.AugmentPanelPS[num].transform.DOMove(new Vector3(VisualEffectManager.Instance.AugmentPanelPS[num].transform.position.x, 0, 0), 0.25f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
        }

        IEnumerator FlashPanel(int rarity, int num)
        {
            bool isFinished = false;
            SelectButtons[num].GetComponent<Image>().material.DOColor(RarityColorList[rarity] * 2, 0.1f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;
            SelectButtons[num].GetComponent<Image>().material.DOColor(RarityColorList[rarity], 0.1f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
        }

        void ParticleBurst(int rarity, int num, int Emit)
        {
            bool isFinished = false;
            VisualEffectManager.Instance.AugmentPanelBurstPS[num].GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", RarityColorList[rarity]);
            VisualEffectManager.Instance.AugmentPanelPS[num].GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", RarityColorList[rarity]);
            VisualEffectManager.Instance.AugmentPanelBurstPS[num].Emit(Emit);
            SelectButtons[num].GetComponent<Image>().material.SetFloat("_IsCornerEffect", 1);
        }


        void SetBackLight(int i)
        {
            if (i == 0)
                Option_1_Aura.SetFloat("_OuterFade", 50f);
            else if (i == 1)
                Option_2_Aura.SetFloat("_OuterFade", 50f);
            else if (i == 2)
                Option_3_Aura.SetFloat("_OuterFade", 50f);
        }
    }

    void ShowLevelUpPanel()
    {
        Time.timeScale = 0f;
        LevelUpPanel.SetActive(true);
        CanvasForLevelUpPanel.SetActive(true);
        AdditionalDescriptionPanel.SetActive(false);
        AdditionalDescriptionText.text = "";
        SoundManager.instance.LevelUpSound();
        Level = LevelUpQueue.Dequeue();

        NoticeLevelText.text = "Now Your Level: " + Level;
        ShowStatOrAugment();
    }
    public bool isThereRerolled = false;
    public IAugment RerollAugment = null;

    void SetAugmentPanel(int i, string name, string desc, string type, float cooltime)
    {
        TMP_Text[] Names = { Option_1_Name, Option_2_Name, Option_3_Name };
        TMP_Text[] Descs = { Option_1_Description, Option_2_Description, Option_3_Description };
        TMP_Text[] types = { Option_1_Type, Option_2_Type, Option_3_Type };
        TMP_Text[] cools = { Option_1_CoolTime, Option_2_CoolTime, Option_3_CoolTime };

        Names[i].text = name;
        Descs[i].text = desc;
        types[i].text = type;
        cools[i].text = cooltime == 0 ? "" : (cooltime / (10 / AugmentManager.instance.FieldGlobalCooldown)).ToString() + "s";
    }

    void ClearAugmentPanel()
    {
        TMP_Text[] Names = { Option_1_Name, Option_2_Name, Option_3_Name };
        TMP_Text[] Descs = { Option_1_Description, Option_2_Description, Option_3_Description };
        TMP_Text[] types = { Option_1_Type, Option_2_Type, Option_3_Type };
        TMP_Text[] cools = { Option_1_CoolTime, Option_2_CoolTime, Option_3_CoolTime };

        for(int i = 0; i < 3; i++)
        {
            Names[i].text = "";
            Descs[i].text = "";
            types[i].text = "";
            cools[i].text = "";

        }
    }
    public void ShowStatOrAugment()
    {
        ClearAugmentPanel();
        optionIndex = -1;
        if (IsStatOrAugment())
        {
            randomStats = GetRandomStats();
            
            SetMaterialColor(StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 1, Option_1_Panel, true);
            SetMaterialColor(StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 1, Option_2_Panel, true);
            SetMaterialColor(StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 1, Option_3_Panel, true);

            CheckStatOrAugment = true;
            int[] levels = new int[] {
                StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 1,
                StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 1,
                StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 1
            };

            StartCoroutine(AugAniCoroutineForStat(levels, OptionButtons, randomStats));
        }
        else
        {
            augmentsSet = new List<IAugment>();
            augmentsSet.Clear();
            if (AugmentManager.instance.isSacrificeQuest3Cleared)
            {
                for (int i = 0; i < 3; i++)
                {
                    float chance = Random.value;
                    int rarity;
                    if (chance < AugmentManager.instance.rarityWeights[0])
                        rarity = 0;
                    else
                        rarity = 1;
                    augmentsSet.Add(AugmentManager.instance.GetRandomAugmentByRarity(rarity));
                }
                AugmentManager.instance.isSacrificeQuest3Cleared = false;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    augmentsSet.Add(AugmentManager.instance.GetRandomAugment());
                }
            }
            SetMaterialColor(augmentsSet[0].Rarity, Option_1_Panel, false);
            SetMaterialColor(augmentsSet[1].Rarity, Option_2_Panel, false);
            SetMaterialColor(augmentsSet[2].Rarity, Option_3_Panel, false);

            CheckStatOrAugment = false;

            int[] Rarities = new int[] {
                augmentsSet[0].Rarity,
                augmentsSet[1].Rarity,
                augmentsSet[2].Rarity
            };

            StartCoroutine(AugAniCoroutine(Rarities, OptionButtons, augmentsSet));
        }
    }

    private string LevelUpTextWhenStatUp(int i)
    {
        string percent = "%";
        bool percentBool = (randomStats[i].Value == 1 || randomStats[i].Value == 2 || randomStats[i].Value == 4 || randomStats[i].Value == 5 || randomStats[i].Value == 6);


        return " +" + 
                (
                    (StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1) % 5 == 0 ?
                    (BaseStatUpValue * StatUpValue[randomStats[i].Value][1] * (percentBool ? 100 : 1)) : 
                    (BaseStatUpValue * StatUpValue[randomStats[i].Value][0] * (percentBool ? 100 : 1))
                )
                + 
                (percentBool ? percent
                : "")
                + "\n\nLv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1);
    }

    private void SetMaterialColor(int i, Material material, bool isStat)
    {
        if(isStat)
        {
            i = 5 - (i / 5);
            if (i < 0)
                i = 0;
        }
        switch (i)
        {
            case 0:
                material.SetColor("_Color", RarityColorList[2]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
            case 1:
                material.SetColor("_Color", RarityColorList[2]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
            case 2:
                material.SetColor("_Color", RarityColorList[2]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
            case 3:
                material.SetColor("_Color", RarityColorList[3]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
            case 4:
                material.SetColor("_Color", RarityColorList[4]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
            case 5:
                material.SetColor("_Color", RarityColorList[5]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
            default:
                material.SetColor("_Color", RarityColorList[5]);
                material.SetFloat("_OuterFade", 0f);
                material.SetFloat("_IsCornerEffect", 0);
                break;
        }
    }

    private void ShowAdditionalDescription(int i)
    {
        RectTransform rectTransform = AdditionalDescriptionPanel.GetComponent<RectTransform>();

        if (augmentsSet[i].AdditionalDescription != null)
        {
            AdditionalDescriptionPanel.SetActive(true);
            AdditionalDescriptionText.text = augmentsSet[i].AdditionalDescription;
            AdditionalDescriptionText.ForceMeshUpdate();
            int lineCount = AdditionalDescriptionText.textInfo.lineCount;
            rectTransform.sizeDelta = new Vector2(765, 50 + (lineCount * 50));
            rectTransform.anchoredPosition = new Vector2(0, -285 - (lineCount * 25f));
        }
        else
        {
            AdditionalDescriptionPanel.SetActive(false);
            AdditionalDescriptionText.text = null;
        }
    }

    private bool IsStatOrAugment()
    {
        return Level % 3 != 0;
    }

    private int optionIndex = -1;
    private bool[] isAlreadyClickedButton;
    public void OptionButtonClick(Button clickedButton)
    {
        if (!isAlreadyClickedButton[Array.IndexOf(OptionButtons, clickedButton)])
        {
            for (int i = 0; i < OptionButtons.Length; i++)
            {
                OptionButtons[i].transform.localScale = i == Array.IndexOf(OptionButtons, clickedButton) ? new Vector3(0.93f, 0.93f, 0.93f) : new Vector3(0.9f, 0.9f, 0.9f);
                OptionButtonShaders[i].transform.localScale = i == Array.IndexOf(OptionButtons, clickedButton) ? new Vector3(0.93f, 0.93f, 0.93f) : new Vector3(0.9f, 0.9f, 0.9f);
                isAlreadyClickedButton[i] = i == Array.IndexOf(OptionButtons, clickedButton);
            }
            if (!CheckStatOrAugment)
                ShowAdditionalDescription(Array.IndexOf(OptionButtons, clickedButton));
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
            StatManager.instance.Temporary_stat.StatLevel[randomStats[optionIndex].Value]++;
            CalculateTemporaryStatWithLevel(randomStats[optionIndex].Value);
        }
        else
        {
            Debug.Log(optionIndex);
            for (int i = 0; i < augmentsSet.Count; i++)
            {
                Debug.Log(augmentsSet[i]);
            }
            AugmentManager.instance.AddAugment(augmentsSet[optionIndex]);
            AugmentManager.instance.RemoveAugment(augmentsSet[optionIndex]);
        }
        isAlreadyClickedButton = new bool[OptionButtons.Length];
        optionIndex = -1;
        if (isThereRerolled)
        {
            isThereRerolled = false;
            ShowStatOrAugment();
            AugmentManager.instance.RemoveOnExceptAugmentsList(RerollAugment);
            return;
        }
        for (int i = 0; i < OptionButtons.Length; i++)
        {
            OptionButtons[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            OptionButtonShaders[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        if (LevelUpQueue.Count > 0)
        {
            ShowLevelUpPanel();
        }
        else
        {
            CustomTime_1 = 0;
            CustomTime_2 = 0;
            CustomTime_3 = 0;
            Time.timeScale = 1f;
            OptionButtons[0].gameObject.transform.localPosition = new Vector3(-260, 1144, 0);
            OptionButtons[1].gameObject.transform.localPosition = new Vector3(0, 1144, 0);
            OptionButtons[2].gameObject.transform.localPosition = new Vector3(260, 1144, 0);
            AdditionalDescriptionPanel.SetActive(false);
            LevelUpPanel.SetActive(false);
            CanvasForLevelUpPanel.SetActive(false);
            VisualEffectManager.Instance.UnsetAugmentPanelPs();
            if (StatManager.instance.isChangedStat)
            {
                StatManager.instance.ShowChangedStatRoutine = StartCoroutine(StatManager.instance.ShowChangedStat());
                StatManager.instance.isChangedStat = false;
            }
        }
    }
}
