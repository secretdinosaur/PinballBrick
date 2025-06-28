using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Button = UnityEngine.UI.Button;
using Color = UnityEngine.Color;
using System.Collections;
using DG.Tweening;


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
        SummonBalls
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
        new List<float> { 2f } //SummonBalls
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
        Level = 0;

    }

    private void FixedUpdate()
    {
        if (LevelUpQueue.Count > 0 && !LevelUpPanel.activeSelf && !GameManager.instance.isGameEnded && !GameManager.instance.OnChaos)
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
        Exp -= LevelBar.maxValue;
        Level++;
        LevelBarMaxValueSet(Level);
        LevelUpQueue.Enqueue(Level);
        GameManager.instance.RecordStatistics.LevelUpCount++;
        foreach (IAugment augment in AugmentManager.instance.activeAugments)
            augment.AfterLevelUpEffect();
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
                50 + 60 * (level - 1) :
            level > 60 ?
                50 + 50 * (level - 1) :
            level > 30 ?
                50 + 40 * (level - 1) :
            //level <= 30
                50 + 30 * (level - 1));
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
            { "Exp\nGain", 6 },
            { "Summon\nBalls", 7 }
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
            case (int)StatName.SummonBalls:
                BallManager.Instance.CreateBall
                    (
                        (StatManager.instance.Temporary_stat.StatLevel[(int)StatName.SummonBalls] / 5)
                        + (int)StatUpValue[(int)StatName.SummonBalls][0]
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
        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < 3; i++)
        {

            switch (rarities[i])
            {
                case 5:
                    SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
                    yield return StartCoroutine(MovePanel(i));
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    break;

                case 4:
                    SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
                    yield return StartCoroutine(MovePanel(i));
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    break;

                case 3:
                    SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
                    yield return StartCoroutine(MovePanel(i));
                    SetParticle(3, i);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    break;

                case 2:
                    SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
                    yield return StartCoroutine(MovePanel(i));
                    SetParticle(2, i);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    ParticleBurst(2, i, 50);
                    yield return StartCoroutine(FlashPanel(2, i));
                    break;

                case 1:
                    int j = Random.Range(0, 2);
                    if(j == 0)
                    {
                        OptionButtons[i].GetComponent<Image>().material.SetColor("_Color", RarityColorList[1]);
                        SoundManager.instance.HighRarityAugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());

                        yield return StartCoroutine(MovePanel(i));
                    }
                    else
                    {
                    SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
                        yield return StartCoroutine(MovePanel(i));
                        yield return StartCoroutine(FlashPanel(2, i));
                        yield return StartCoroutine(ShakePanel(i));
                    }
                    ParticleBurst(1, i, 100);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    yield return StartCoroutine(FlashPanel(1, i));
                    break;


                case 0:

                    int k = Random.Range(0, 3);
                    if (k == 0)
                    {
                        OptionButtons[i].GetComponent<Image>().material.SetColor("_Color", RarityColorList[0]);
                        SoundManager.instance.HighRarityAugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());

                        yield return StartCoroutine(MovePanel(i));
                        SetParticle(0, i);
                    }
                    else if(k == 1)
                    {
                        SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
                        yield return StartCoroutine(MovePanel(i));
                        SetParticle(2, i);
                        yield return StartCoroutine(FlashPanel(2, i));
                        yield return StartCoroutine(ShakePanel(i));
                        SetParticle(1, i);
                        yield return StartCoroutine(FlashPanel(1, i));
                        yield return StartCoroutine(ShakePanel(i));
                    }
                    else
                    {
                        OptionButtons[i].GetComponent<Image>().material.SetColor("_Color", RarityColorList[1]);
                        SoundManager.instance.HighRarityAugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());

                        yield return StartCoroutine(MovePanel(i));
                        SetParticle(1, i);
                        yield return StartCoroutine(FlashPanel(1, i));
                        yield return StartCoroutine(ShakePanel(i));
                    }
                    ParticleBurst(0, i, 150);
                    SetAugmentPanel(i, augments[i].Name, augments[i].Description, augments[i].Type, augments[i].CoolTime);
                    yield return StartCoroutine(FlashPanel(0, i));
                    SetBackLight(i);
                    break;
            }
        }

        IEnumerator MovePanel(int num)
        {
            Vector3 FinalPosition = new Vector3(470 * (num - 1), 0, 0);
            bool isFinished = false;
            SelectButtons[num].transform.DOLocalMove(FinalPosition, 0.25f)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true)
                        .OnComplete(() => { isFinished = true; });
            yield return new WaitUntil(() => isFinished);
        }

        void SetParticle(int rarity, int num)
        {
            VisualEffectManager.Instance.SetActiveAugmentPanelPs(num, RarityColorList[rarity], rarity);
            VisualEffectManager.Instance.AugmentPanelPS[num].transform.position = (new Vector3(VisualEffectManager.Instance.AugmentPanelPS[num].transform.position.x, 0, 0));
        }

        IEnumerator FlashPanel(int rarity, int num)
        {
            SetParticle(rarity, num);
            if(rarity < 2)
            {
                SoundManager.instance.AugmentFlashSound2(SelectButtons[num].GetComponent<AudioSource>());
            }
            else
                SoundManager.instance.AugmentFlashSound(SelectButtons[num].GetComponent<AudioSource>());

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
            SoundManager.instance.AugmentShakeSound(SelectButtons[num].GetComponent<AudioSource>());
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
        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < 3; i++)
        {
            SetAugmentPanel(i, stats[i].Key, LevelUpTextWhenStatUp(i), "", 0);
            rarities[i] = 5 - (rarities[i] / 5);
            if (rarities[i] < 0)
                rarities[i] = 0;
            bool isFinished = false;
            Vector3 FinalPosition = new Vector3(470 * (i - 1), 0, 0);

            if(rarities[i] > 1)
            {
                SoundManager.instance.AugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
            }
            else
            {
                SoundManager.instance.HighRarityAugmentDrawSound(SelectButtons[i].GetComponent<AudioSource>());
            }

            SelectButtons[i].transform.DOLocalMove(FinalPosition, 0.25f)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true)
                        .OnComplete(() => isFinished = true);
            yield return new WaitUntil(() => isFinished);
            isFinished = false;

            switch (rarities[i])
            {
                case 5:
                    break;

                case 4:
                    break;

                case 3:
                    SetParticle(3, i);
                    break;

                case 2:
                    SetParticle(2, i);
                    yield return StartCoroutine(FlashPanel(2, i));
                    ParticleBurst(2, i, 50);
                    break;

                case 1:
                    SetParticle(1, i);
                    yield return StartCoroutine(FlashPanel(1, i));
                    ParticleBurst(1, i, 100);
                    break;

                case 0:
                    SetParticle(0, i);
                    yield return StartCoroutine(FlashPanel(0, i));
                    ParticleBurst(0, i, 150);
                    SetBackLight(i);
                    break;
            }
        }

        void SetParticle(int rarity, int num)
        {
            VisualEffectManager.Instance.SetActiveAugmentPanelPs(num, RarityColorList[rarity], rarity);
            VisualEffectManager.Instance.AugmentPanelPS[num].transform.position = (new Vector3(VisualEffectManager.Instance.AugmentPanelPS[num].transform.position.x, 0, 0));
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
            SoundManager.instance.AugmentFlashSound(SelectButtons[num].GetComponent<AudioSource>());

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
        GameManager.instance.WaveNoticeText.enabled = false;
        GameManager.instance.WaveExtraNoticeText.enabled = false;
        Time.timeScale = 0f;
        LevelUpPanel.SetActive(true);
        SoundManager.instance.LevelUpSound(LevelUpPanel.GetComponent<AudioSource>());
        SoundManager.instance.PausePausableSounds();

        CanvasForLevelUpPanel.SetActive(true);
        AdditionalDescriptionPanel.SetActive(false);
        AdditionalDescriptionText.text = "";
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

        Names[i].ForceMeshUpdate();

        if (Option_1_Name.textInfo.lineCount == 1)
        {
            Option_1_Name.rectTransform.anchoredPosition = new Vector2(Option_1_Name.rectTransform.anchoredPosition.x, -75);
            Option_1_Description.rectTransform.anchoredPosition = new Vector2(Option_1_Description.rectTransform.anchoredPosition.x, -350);
        }
        else
        {
            Option_1_Name.rectTransform.anchoredPosition = new Vector2(Option_1_Name.rectTransform.anchoredPosition.x, -50);
            Option_1_Description.rectTransform.anchoredPosition = new Vector2(Option_1_Description.rectTransform.anchoredPosition.x, -400);
        }
        if (Option_2_Name.textInfo.lineCount == 1)
        {
            Option_2_Name.rectTransform.anchoredPosition = new Vector2(Option_2_Name.rectTransform.anchoredPosition.x, -75);
            Option_2_Description.rectTransform.anchoredPosition = new Vector2(Option_2_Description.rectTransform.anchoredPosition.x, -350);
        }
        else
        {
            Option_2_Name.rectTransform.anchoredPosition = new Vector2(Option_2_Name.rectTransform.anchoredPosition.x, -50);
            Option_2_Description.rectTransform.anchoredPosition = new Vector2(Option_2_Description.rectTransform.anchoredPosition.x, -400);
        }
        if (Option_3_Name.textInfo.lineCount == 1)
        {
            Option_3_Name.rectTransform.anchoredPosition = new Vector2(Option_3_Name.rectTransform.anchoredPosition.x, -75);
            Option_3_Description.rectTransform.anchoredPosition = new Vector2(Option_3_Description.rectTransform.anchoredPosition.x, -350);
        }
        else
        {
            Option_3_Name.rectTransform.anchoredPosition = new Vector2(Option_3_Name.rectTransform.anchoredPosition.x, -50);
            Option_3_Description.rectTransform.anchoredPosition = new Vector2(Option_3_Description.rectTransform.anchoredPosition.x, -400);
        }
    }

    void ClearAugmentPanel()
    {
        TMP_Text[] Names = { Option_1_Name, Option_2_Name, Option_3_Name };
        TMP_Text[] Descs = { Option_1_Description, Option_2_Description, Option_3_Description };
        TMP_Text[] types = { Option_1_Type, Option_2_Type, Option_3_Type };
        TMP_Text[] cools = { Option_1_CoolTime, Option_2_CoolTime, Option_3_CoolTime };
        OptionButtons[0].gameObject.transform.localPosition = new Vector3(-470, 2000, 0);
        OptionButtons[1].gameObject.transform.localPosition = new Vector3(0, 2000, 0);
        OptionButtons[2].gameObject.transform.localPosition = new Vector3(470, 2000, 0);
        VisualEffectManager.Instance.UnsetAugmentPanelPs();
        for (int i = 0; i < 3; i++)
        {
            Names[i].text = "";
            Descs[i].text = "";
            types[i].text = "";
            cools[i].text = "";

        }
    }

    Coroutine AniRoutine;

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

            AniRoutine = StartCoroutine(AugAniCoroutineForStat(levels, OptionButtons, randomStats));
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
                    if (chance < 0.2f)
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

            AniRoutine = StartCoroutine(AugAniCoroutine(Rarities, OptionButtons, augmentsSet));

            
        }
    }

    private string LevelUpTextWhenStatUp(int i)
    {
        string percent = "%";
        bool percentBool = (randomStats[i].Value == 1 || randomStats[i].Value == 2 || randomStats[i].Value == 4 || randomStats[i].Value == 5 || randomStats[i].Value == 6);

        if(randomStats[i].Value == 7)
        {
            return ((StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] / 5) + StatUpValue[randomStats[i].Value][0]) + " Balls "
                + "\n\nLv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[i].Value] + 1); ;
                
        }
        else
        {
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
            rectTransform.sizeDelta = new Vector2(1350, 50 + (lineCount * 100));
            rectTransform.anchoredPosition = new Vector2(0, -450);
        }
        else
        {
            AdditionalDescriptionPanel.SetActive(false);
            AdditionalDescriptionText.text = null;
        }
    }

    private bool IsStatOrAugment()
    {
        if (Level == 1)
            return false;
        return Level % 5 != 0;
    }

    private int optionIndex = -1;
    private bool[] isAlreadyClickedButton;
    public void OptionButtonClick(Button clickedButton)
    {
        if (!isAlreadyClickedButton[Array.IndexOf(OptionButtons, clickedButton)])
        {
            SoundManager.instance.AugmentClickSound();
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
            SoundManager.instance.AugmentSelectSound();
            LevelUpEnd();
        }
    }

    public void LevelUpEnd()
    {
        StopCoroutine(AniRoutine);
        if (CheckStatOrAugment)
        {
            StatManager.instance.Temporary_stat.StatLevel[randomStats[optionIndex].Value]++;
            CalculateTemporaryStatWithLevel(randomStats[optionIndex].Value);
        }
        else
        {
            AugmentManager.instance.AddAugment(augmentsSet[optionIndex]);
            AugmentManager.instance.RemoveOnAugmentsSet(augmentsSet[optionIndex]);
        }
        isAlreadyClickedButton = new bool[OptionButtons.Length];
        optionIndex = -1;
        if (isThereRerolled)
        {
            isThereRerolled = false;
            ShowStatOrAugment();
            AugmentManager.instance.UnlockAugment(RerollAugment);
            return;
        }
        for (int i = 0; i < OptionButtons.Length; i++)
        {
            OptionButtons[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            OptionButtonShaders[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        CustomTime_1 = 0;
        CustomTime_2 = 0;
        CustomTime_3 = 0;
        Time.timeScale = 1f;
        OptionButtons[0].gameObject.transform.localPosition = new Vector3(-470, 2000, 0);
        OptionButtons[1].gameObject.transform.localPosition = new Vector3(0, 2000, 0);
        OptionButtons[2].gameObject.transform.localPosition = new Vector3(470, 2000, 0);
        AdditionalDescriptionPanel.SetActive(false);
        LevelUpPanel.SetActive(false);
        CanvasForLevelUpPanel.SetActive(false);
        VisualEffectManager.Instance.UnsetAugmentPanelPs();

        if (LevelUpQueue.Count > 0)
        {
            ShowLevelUpPanel();
        }
        else
        {
            if (AugmentManager.instance.isLevelUpOnceAgain)
                AugmentManager.instance.isLevelUpOnceAgain = false;
            if (StatManager.instance.isChangedStat)
            {
                StatManager.instance.ShowChangedStatRoutine = StartCoroutine(StatManager.instance.ShowChangedStat());
                StatManager.instance.isChangedStat = false;
            }
            SoundManager.instance.UnPausePausableSounds();

            GameManager.instance.WaveNoticeText.enabled = true;
            GameManager.instance.WaveExtraNoticeText.enabled = true;
        }
        
    }
}
