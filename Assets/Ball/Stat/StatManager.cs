using System.IO;
using UnityEngine;
using System.Collections;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Text;
public partial class StatManager : MonoBehaviour
{
    public static StatManager instance;
    
    public Persistent_Stat Persistent_stat; // If you want to use it elsewhere, use this variable
    public Temporary_Stat Temporary_stat; //
    public StatMultiplier statMultiplier; //
    public static float AttackDamage;
    public static float EffectDamage;

    void Awake()
    {
        instance = this;

        LoadPersistentStats();
        InitializeTemporaryStats();
        InitializeStatMultiplier();
        UpdateUniversalStat();
        SettingStatisticsSet();

        string json = JsonUtility.ToJson(Persistent_stat, true);
        Debug.LogWarning("Stat Manager : \n" + json);


        NoticeChangedStatPanel = GameObject.Find("NoticeChangedStatPanel");

        string currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName != "Main")
        {
            AttackDamageStatValueText = NoticeChangedStatPanel.transform.Find("AttackDamageIcon/AttackDamageStatValueText").GetComponent<TMP_Text>();
            AttackDamageStatValueText.text = "";
            AttackDamageChangedStatValueText = NoticeChangedStatPanel.transform.Find("AttackDamageIcon/AttackDamageChangedStatValueText").GetComponent<TMP_Text>();
            AttackDamageChangedStatValueText.text = "";

            CriticalDamageStatValueText = NoticeChangedStatPanel.transform.Find("CriticalDamageIcon/CriticalDamageStatValueText").GetComponent<TMP_Text>();
            CriticalDamageStatValueText.text = "";
            CriticalDamageChangedStatValueText = NoticeChangedStatPanel.transform.Find("CriticalDamageIcon/CriticalDamageChangedStatValueText").GetComponent<TMP_Text>();
            CriticalDamageChangedStatValueText.text = "";

            CriticalChanceStatValueText = NoticeChangedStatPanel.transform.Find("CriticalChanceIcon/CriticalChanceStatValueText").GetComponent<TMP_Text>();
            CriticalChanceStatValueText.text = "";
            CriticalChanceChangedStatValueText = NoticeChangedStatPanel.transform.Find("CriticalChanceIcon/CriticalChanceChangedStatValueText").GetComponent<TMP_Text>();
            CriticalChanceChangedStatValueText.text = "";

            EffectDamageStatValueText = NoticeChangedStatPanel.transform.Find("EffectDamageIcon/EffectDamageStatValueText").GetComponent<TMP_Text>();
            EffectDamageStatValueText.text = "";
            EffectDamageChangedStatValueText = NoticeChangedStatPanel.transform.Find("EffectDamageIcon/EffectDamageChangedStatValueText").GetComponent<TMP_Text>();
            EffectDamageChangedStatValueText.text = "";

            EffectDurationStatValueText = NoticeChangedStatPanel.transform.Find("EffectDurationIcon/EffectDurationStatValueText").GetComponent<TMP_Text>();
            EffectDurationStatValueText.text = "";
            EffectDurationChangedStatValueText = NoticeChangedStatPanel.transform.Find("EffectDurationIcon/EffectDurationChangedStatValueText").GetComponent<TMP_Text>();
            EffectDurationChangedStatValueText.text = "";

            GoldGainStatValueText = NoticeChangedStatPanel.transform.Find("GoldGainIcon/GoldGainStatValueText").GetComponent<TMP_Text>();
            GoldGainStatValueText.text = "";
            GoldGainChangedStatValueText = NoticeChangedStatPanel.transform.Find("GoldGainIcon/GoldGainChangedStatValueText").GetComponent<TMP_Text>();
            GoldGainChangedStatValueText.text = "";

            ExpGainStatValueText = NoticeChangedStatPanel.transform.Find("ExpGainIcon/ExpGainStatValueText").GetComponent<TMP_Text>();
            ExpGainStatValueText.text = "";
            ExpGainChangedStatValueText = NoticeChangedStatPanel.transform.Find("ExpGainIcon/ExpGainChangedStatValueText").GetComponent<TMP_Text>();
            ExpGainChangedStatValueText.text = "";
        }
        

        TotalChangedStatValue = new List<float>
        {
            0, 0, 0, 0, 0, 0, 0, 1, 1
        };

        ShowChangedStatRoutine = null;
    }

    public void InitializeTemporaryStats() // temporary stats
    {
        Temporary_stat = new Temporary_Stat
        {
            StatLevel = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            AttackDamage = 0f,
            CriticalDamage = 0.0f,
            CriticalChance = 0.0f,
            EffectDamage = 0.0f,
            EffectDuration = 1f,
            GoldGain = 0.0f,
            ExpGain = 0.0f,
            SummonBall = 5
        };
    }
    
    public void InitializeStatMultiplier() // stat multiplier
    {
        statMultiplier = new StatMultiplier
        {
            AttackDamageMultiplier = 1.0f,
            EffectDamageMultiplier = 1.0f
        };
    }
    
    public void UpdateUniversalStat()
    {
        AttackDamage = (Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) * statMultiplier.AttackDamageMultiplier;
        EffectDamage = (Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) * statMultiplier.EffectDamageMultiplier;
    }

    #region Save and Load Persistent Stat

    private string cloudSaveFileName = "PersistentStat_SaveData.json";

    Persistent_Stat initialPersistentStats = new Persistent_Stat
    {
        AttackDamage = 5f,
        EffectDamage = 5f,
        StartingBall = 5,
        BonusExp = 1f,
        CriticalChance = 0.0f,
        CriticalDamage = 1.1f,
        BonusGold = 1f,
        Gold = 0f,

        AttackDamageMultiplier = 1f,
        EffectDamageMultiplier = 1f,

        AttackDamageLevel = 1,
        EffectDamageLevel = 1,
        BonusEXPLevel = 1,
        BonusGoldLevel = 1,
        CriChanceLevel = 1,
        CriDamageLevel = 1,
        StartingBallLevel = 1
    };

    public void SavePersistentStats()
    {
        OpenSavePersistentStats();
    }

    private void OpenSavePersistentStats()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(cloudSaveFileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLastKnownGood,
                OnSavedPersistentStatsOpend);
        }
        else
        {
            Debug.Log("Fail to connect online");

            SavePersistentStatOnLocal();
        }
    }

    private void OnSavedPersistentStatsOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success to connect online");

            var update = new SavedGameMetadataUpdate.Builder().Build();

            var json = JsonUtility.ToJson(Persistent_stat);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            Debug.Log("Save Data : " + bytes);

            savedGameClient.CommitUpdate(game, update, bytes, OnSavedPersistentStatsWritten);
        }
        else
        {
            Debug.Log("Fail to connect online");

            SavePersistentStatOnLocal();
        }
    }

    private void OnSavedPersistentStatsWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    public void LoadPersistentStats()
    {
        OpenLoadPersistentStats();
    }

    private void OpenLoadPersistentStats()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(cloudSaveFileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLastKnownGood,
                LoadPersistentStatsData);
        }
        else
        {
            LoadPersistentStatOnLocal();
        }
    }

    private void LoadPersistentStatsData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success to connect online");

            savedGameClient.ReadBinaryData(data, OnLoadedPersistentStatsData);
        }
        else
        {
            Debug.Log("Fail to connect online");

            LoadPersistentStatOnLocal();
        }
    }

    private void OnLoadedPersistentStatsData(SavedGameRequestStatus status, byte[] loadedData)
    {
        if (loadedData == null || loadedData.Length == 0)
        {
            Debug.Log("No cloud data found. Saving default data to cloud.");
            SaveDefaultPersistentStats();
            return;
        }

        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        if (data == "")
        {
            Debug.Log("There's no data, try to save default data");
            SaveDefaultPersistentStats();
        }
        else
        {
            Debug.Log("Loaded data : " + data);

            Persistent_stat = JsonUtility.FromJson<Persistent_Stat>(data);
            synchronizeToUpgradeManagerWithStatManager();
        }
    }

    public void SaveDefaultPersistentStats()
    {
        OpenSaveDefaultPersistentStats();
    }

    private void OpenSaveDefaultPersistentStats()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(cloudSaveFileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLastKnownGood,
                OnSavedDefaultPersistentStatsOpend);
        }
        else
        {
            Debug.Log("Fail to connect online");

            InitializePersistentStats();
        }
    }

    private void OnSavedDefaultPersistentStatsOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success to connect online");

            var update = new SavedGameMetadataUpdate.Builder().Build();

            var json = JsonUtility.ToJson(initialPersistentStats);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            Persistent_stat = initialPersistentStats;
            synchronizeToUpgradeManagerWithStatManager();

            Debug.Log("Save Data : " + bytes);
            savedGameClient.CommitUpdate(game, update, bytes, OnSavedDefaultPersistentStatsWritten);
        }
        else
        {
            Debug.Log("Fail to connect online");

            InitializePersistentStats();
        }
    }

    private void OnSavedDefaultPersistentStatsWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    private void SavePersistentStatOnLocal() // Save Persistent Stat on local session
    {
        string json = JsonUtility.ToJson(Persistent_stat, true);
        File.WriteAllText(Application.persistentDataPath + "/PinballBrick_persistentData.json", json);

        Debug.Log("Save Successfully on local session");
    }
    private void LoadPersistentStatOnLocal() // Load Persistent Stat on local session
    {
        string json;
        if (!File.Exists(Application.persistentDataPath + "/PinballBrick_persistentData.json"))
        {
            Debug.Log("There's no data, try to save default data to the local session");

            InitializePersistentStats();
        }
        else
        {
            json = File.ReadAllText(Application.persistentDataPath + "/PinballBrick_persistentData.json");
            Persistent_stat = JsonUtility.FromJson<Persistent_Stat>(json);
            synchronizeToUpgradeManagerWithStatManager();
        }
        Debug.Log("Load Successfully on local session");

    }
    private void InitializePersistentStats() // Initialize Persistent Stat on local session
    {
        string json = JsonUtility.ToJson(initialPersistentStats, true);
        File.WriteAllText(Application.persistentDataPath + "/PinballBrick_persistentData.json", json);
        Debug.Log("Initialize Successfully on local session");

        Persistent_stat = JsonUtility.FromJson<Persistent_Stat>(json);
        synchronizeToUpgradeManagerWithStatManager();
    }

    private void synchronizeToUpgradeManagerWithStatManager()
    {
        if(UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.levels = new int[]
            {
                Persistent_stat.AttackDamageLevel,
                Persistent_stat.EffectDamageLevel,
                Persistent_stat.BonusEXPLevel,
                Persistent_stat.CriChanceLevel,
                Persistent_stat.CriDamageLevel,
                Persistent_stat.StartingBallLevel
            };
            UpgradeManager.Instance.SetUpgradePanel();
        }
    }

    #endregion

    public void GetGold(int Value)
    {
        Persistent_stat.Gold += Value * (Persistent_stat.BonusGold + Temporary_stat.GoldGain);
        if (SceneManager.GetActiveScene().name == "Dolsot")
        {
            GameManager.instance.RecordStatistics.GainedGold += Value * (Persistent_stat.BonusGold + Temporary_stat.GoldGain);
        }
        else
        {
            Statistics.GainedGold += Value * (Persistent_stat.BonusGold + Temporary_stat.GoldGain);
            SaveStatisticsSet();
        }
    }
}


public partial class StatManager
{
    GameObject NoticeChangedStatPanel;

    TMP_Text AttackDamageStatValueText;
    TMP_Text AttackDamageChangedStatValueText;

    TMP_Text CriticalDamageStatValueText;
    TMP_Text CriticalDamageChangedStatValueText;

    TMP_Text CriticalChanceStatValueText;
    TMP_Text CriticalChanceChangedStatValueText;

    TMP_Text EffectDamageStatValueText;
    TMP_Text EffectDamageChangedStatValueText;

    TMP_Text EffectDurationStatValueText;
    TMP_Text EffectDurationChangedStatValueText;

    TMP_Text GoldGainStatValueText;
    TMP_Text GoldGainChangedStatValueText;

    TMP_Text ExpGainStatValueText;
    TMP_Text ExpGainChangedStatValueText;

    TMP_Text StartingBallStatValueText;

    List<float> TotalChangedStatValue;

    public Coroutine ShowChangedStatRoutine;

    float startTime;

    private void GetShowChangedStat()
    {
        if(ShowChangedStatRoutine != null)
        {
            StopCoroutine(ShowChangedStatRoutine);
        }
        ShowChangedStatRoutine = StartCoroutine(ShowChangedStat());
    }

    private IEnumerator ShowChangedStat()
    {
        CanvasGroup canvasGroup = NoticeChangedStatPanel.GetComponent<CanvasGroup>();
        RectTransform rectTransform = NoticeChangedStatPanel.GetComponent<RectTransform>();

        AttackDamageStatValueText.text = ((Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) * statMultiplier.AttackDamageMultiplier).ToString("F1");
        AttackDamageChangedStatValueText.text = "";

        CriticalDamageStatValueText.text = ((Persistent_stat.CriticalDamage + Temporary_stat.CriticalDamage) * 100).ToString("F1") + "%";
        CriticalDamageChangedStatValueText.text = "";

        CriticalChanceStatValueText.text = ((Persistent_stat.CriticalChance + Temporary_stat.CriticalChance) * 100).ToString("F1") + "%";
        CriticalChanceChangedStatValueText.text = "";

        EffectDamageStatValueText.text = ((Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) * statMultiplier.EffectDamageMultiplier).ToString("F1");
        EffectDamageChangedStatValueText.text = "";

        EffectDurationStatValueText.text = ((Temporary_stat.EffectDuration - 1) * 100).ToString("F1") + "%";
        EffectDurationChangedStatValueText.text = "";

        GoldGainStatValueText.text = ((Persistent_stat.BonusGold + Temporary_stat.GoldGain) * 100).ToString("F1") + "%";
        GoldGainChangedStatValueText.text = "";

        ExpGainStatValueText.text = ((Persistent_stat.BonusExp + Temporary_stat.ExpGain) * 100).ToString("F1") + "%";
        ExpGainChangedStatValueText.text = "";

        UpdateChangedStatValueText();

        startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + 1f)
        {
            float t = (Time.realtimeSinceStartup - startTime) / 1f;
            rectTransform.anchoredPosition = 
                Vector2.Lerp(new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y), new Vector2(410f, 129f), t); //new Vector2(-320f, 129f)
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(410f, 129f);
        canvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(1.5f);

        startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + 1f)
        {
            float t = (Time.realtimeSinceStartup - startTime) / 1f;

            rectTransform.anchoredPosition =
                Vector2.Lerp(new Vector2(165f, 129f), new Vector2(-165f, 129f), t);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(-320f, 129f);
        canvasGroup.alpha = 0f;

        TotalChangedStatValue = new List<float>
        {
            0, 0, 0, 0, 0, 0, 0, 1, 1
        };
    }



    private void UpdateChangedStatValueText()
    {
        for (int i = 0; i < TotalChangedStatValue.Count; i++)
        {
            switch (i)
            {
                case 0:
                    if(TotalChangedStatValue[0] != 0)
                    {
                        if (TotalChangedStatValue[0] < 0)
                        {
                            AttackDamageChangedStatValueText.text = TotalChangedStatValue[0].ToString("0.##");
                            AttackDamageChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }
                        else if(TotalChangedStatValue[0] > 0)
                        {
                            AttackDamageChangedStatValueText.text = "+" + TotalChangedStatValue[0].ToString("0.##");
                            AttackDamageChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }
                    }
                    break;
                case 1:
                    if (TotalChangedStatValue[1] != 0)
                    {
                        if (TotalChangedStatValue[1] < 0)
                        {
                            CriticalDamageChangedStatValueText.text = (TotalChangedStatValue[1] * 100).ToString("0.##") + "%";
                            CriticalDamageChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }
                        else if (TotalChangedStatValue[1] > 0)
                        {
                            CriticalDamageChangedStatValueText.text = "+" + (TotalChangedStatValue[1] * 100).ToString("0.##") + "%";
                            CriticalDamageChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }
                    }
                    break;
                case 2:
                    if (TotalChangedStatValue[2] != 0)
                    {
                        if (TotalChangedStatValue[2] < 0)
                        {
                            CriticalChanceChangedStatValueText.text = (TotalChangedStatValue[2] * 100).ToString("0.##") + "%";
                            CriticalChanceChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }
                        else if (TotalChangedStatValue[2] > 0)
                        {
                            CriticalChanceChangedStatValueText.text = "+" + (TotalChangedStatValue[2] * 100).ToString("0.##") + "%";
                            CriticalChanceChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        } 
                    }
                    break;
                case 3:
                    if (TotalChangedStatValue[3] != 0)
                    {
                        if (TotalChangedStatValue[3] < 0)
                        {
                            EffectDamageChangedStatValueText.text = TotalChangedStatValue[3].ToString("0.##");
                            EffectDamageChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }                           
                        else if (TotalChangedStatValue[3] > 0)
                        {
                            EffectDamageChangedStatValueText.text = "+" + TotalChangedStatValue[3].ToString("0.##");
                            EffectDamageChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }   
                    }
                    break;
                case 4:
                    if (TotalChangedStatValue[4] != 0)
                    {
                        if (TotalChangedStatValue[4] < 0)
                        {
                            EffectDurationChangedStatValueText.text = (TotalChangedStatValue[4] * 100).ToString("0.##") + "%";
                            EffectDurationChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }                        
                        else if (TotalChangedStatValue[4] > 0)
                        {
                            EffectDurationChangedStatValueText.text = "+" + (TotalChangedStatValue[4] * 100).ToString("0.##") + "%";
                            EffectDurationChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }
                    }
                    break;
                case 5:
                    if (TotalChangedStatValue[5] != 0)
                    {
                        if (TotalChangedStatValue[5] < 0)
                        {
                            GoldGainChangedStatValueText.text = (TotalChangedStatValue[5] * 100).ToString("0.##") + "%";
                            GoldGainChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }       
                        else if (TotalChangedStatValue[5] > 0)
                        {
                            GoldGainChangedStatValueText.text = "+" + (TotalChangedStatValue[5] * 100).ToString("0.##") + "%";
                            GoldGainChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }  
                    }
                    break;
                case 6:
                    if (TotalChangedStatValue[6] != 0)
                    {
                        if (TotalChangedStatValue[6] < 0)
                        {
                            ExpGainChangedStatValueText.text = (TotalChangedStatValue[6] * 100).ToString("0.##") + "%";
                            ExpGainChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }                       
                        else if (TotalChangedStatValue[6] > 0)
                        {
                            ExpGainChangedStatValueText.text = "+" + (TotalChangedStatValue[6] * 100).ToString("0.##") + "%";
                            ExpGainChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }        
                    }
                    break;
                case 7:
                    if (TotalChangedStatValue[7] != 1)
                    {
                        float beforeAttackDamage = float.Parse(AttackDamageStatValueText.text) - ((float.Parse(AttackDamageStatValueText.text) / TotalChangedStatValue[7]));
                        Debug.Log(float.Parse(AttackDamageStatValueText.text));
                        for(int y = 0; y < TotalChangedStatValue.Count; y++)
                        {
                            Debug.Log(TotalChangedStatValue[i]);
                        }
                        if (beforeAttackDamage > 0)
                        {
                            AttackDamageChangedStatValueText.text = "+" + beforeAttackDamage.ToString("0.##");
                            AttackDamageChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }
                        else
                        {
                            AttackDamageChangedStatValueText.text = beforeAttackDamage.ToString("0.##");
                            AttackDamageChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }             
                    }
                    break;
                case 8:
                    if (TotalChangedStatValue[8] != 1)
                    {
                        float beforeEffectDamage = float.Parse(EffectDamageStatValueText.text) - ((float.Parse(EffectDamageStatValueText.text) / TotalChangedStatValue[8]));
                        if( beforeEffectDamage > 0)
                        {
                            EffectDamageChangedStatValueText.text = "+" + beforeEffectDamage.ToString("0.##");
                            EffectDamageChangedStatValueText.color = new Color(0.588f, 1f, 0.588f);
                        }        
                        else
                        {
                            EffectDamageChangedStatValueText.text = beforeEffectDamage.ToString("0.##");
                            EffectDamageChangedStatValueText.color = new Color(1f, 0.588f, 0.588f);
                        }   
                    }
                    break;
                default: 
                    break;
            }
        }
    }

    public void ChangeTemporaryStats_AttackDamage(float value)
    {
        Temporary_stat.AttackDamage += value;
        TotalChangedStatValue[0] += value;
        if (AugmentManager.instance.determineAugment(typeof(AttackEffectBothUp)))
        {
            Temporary_stat.EffectDamage += value * 0.2f;
            TotalChangedStatValue[3] += value * 0.2f;
        }
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeTemporaryStats_CriticalDamage(float value)
    {
        Temporary_stat.CriticalDamage += value;
        TotalChangedStatValue[1] += value;
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeTemporaryStats_CriticalChance(float value)
    {
        float MaxCriChance = 1f;
        if (AugmentManager.instance.determineAugment(typeof(MassivelyCriticalDamageUp)))
        {
            MaxCriChance = 0.1f;
        }
        if (AugmentManager.instance.determineAugment(typeof(CritChanceToAttackDamage)))
        {
            if(Persistent_stat.CriticalChance + Temporary_stat.CriticalChance + value != 0)
            {
                float x = Persistent_stat.CriticalChance;
                float y = (Persistent_stat.CriticalChance + Temporary_stat.CriticalChance + value) * 100;
                Temporary_stat.CriticalChance = (float)System.Math.Round(-x, 2);
                TotalChangedStatValue[2] -= (float)System.Math.Round(x, 2);

                Temporary_stat.AttackDamage += (float)System.Math.Round(y * 0.04f, 2);
                TotalChangedStatValue[0] += (float)System.Math.Round(y * 0.04f, 2);
            }
            else
            {
                Temporary_stat.AttackDamage += (float)System.Math.Round((value * 100) * 0.04f, 2);
                TotalChangedStatValue[0] += (float)System.Math.Round((value * 100) * 0.04f, 2);
            }
        }
        else
        {
            if (Persistent_stat.CriticalChance + Temporary_stat.CriticalChance + value >= MaxCriChance)
            {
                float x = MaxCriChance - (Persistent_stat.CriticalChance + Temporary_stat.CriticalChance);
                float y = Mathf.Floor(((float)System.Math.Round(value, 2) - (float)System.Math.Round(x, 2)) * 100f) / 100f;
                Temporary_stat.CriticalChance += (float)System.Math.Round(x, 2);
                TotalChangedStatValue[2] += (float)System.Math.Round(x, 2);
                if (y > 0)
                {
                    Temporary_stat.CriticalDamage += (float)System.Math.Round(y, 2);
                    TotalChangedStatValue[1] += (float)System.Math.Round(y, 2);
                }
            }
            else
            {
                Temporary_stat.CriticalChance += (float)System.Math.Round(value, 2);
                TotalChangedStatValue[2] += (float)System.Math.Round(value, 2);
            }
        }
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeTemporaryStats_EffectDamage(float value)
    {
        Temporary_stat.EffectDamage += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[3] += (float)System.Math.Round(value, 2);
        if (AugmentManager.instance.determineAugment(typeof(AttackEffectBothUp)))
        {
            Temporary_stat.AttackDamage += value * 0.2f;
            TotalChangedStatValue[0] += value * 0.2f;
        }
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeTemporaryStats_EffectDuration(float value)
    {
        Temporary_stat.EffectDuration += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[4] += (float)System.Math.Round(value, 2);
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeTemporaryStats_GoldGain(float value)
    {
        Temporary_stat.GoldGain += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[5] += (float)System.Math.Round(value, 2);
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeTemporaryStats_ExpGain(float value)
    {
        Temporary_stat.ExpGain += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[6] += (float)System.Math.Round(value, 2);
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeStatMultiplier_AttackDamageMultiplier(float value)
    {
        if(value >= 0)
        {
            statMultiplier.AttackDamageMultiplier *= (float)System.Math.Round(value, 3);
            TotalChangedStatValue[7] *= (float)System.Math.Round(value, 2);
        }
        else if(value < 0)
        {
            statMultiplier.AttackDamageMultiplier /= (float)System.Math.Round(-value, 3);
            TotalChangedStatValue[7] /= (float)System.Math.Round(-value, 2);
        }
        UpdateUniversalStat();
        GetShowChangedStat();
    }
    public void ChangeStatMultiplier_EffectDamageMultiplier(float value)
    {
        if(value >= 0)
        {
            statMultiplier.EffectDamageMultiplier *= (float)System.Math.Round(value, 3);
            TotalChangedStatValue[8] *= (float)System.Math.Round(value, 2);
        }
        else if(value < 0)
        {
            statMultiplier.EffectDamageMultiplier /= (float)System.Math.Round(-value, 3);
            TotalChangedStatValue[8] /= (float)System.Math.Round(-value, 2);
        }
        UpdateUniversalStat();
        GetShowChangedStat();
    }
}

public partial class StatManager //for other stats
{
    public StatisticsSet Statistics;
    private void SettingStatisticsSet()
    {
        LoadStatisticsSet();
    }

    #region Save and Load Statistics
    private string cloudSaveFileName_StatisticsSet = "StatisticsSet_SaveData.json";

    StatisticsSet initialStatistics = new StatisticsSet
    {
        LevelUpCount = 0,
        AchievedAugmentCount = 0,
        AchievedRareAugmentCount = 0,
        GainedGold = 0,
        ReachedWave = 0,
        PassedWave = 0,
        DestroyedBricks = 0,
        DestroyedBossBricks = 0,
        SummonedBalls = 0,
        DiedBalls = 0,
        Playtime = 0
    };


    public void SaveStatisticsSet()
    {
        OpenSaveStatisticsSet();
    }

    private void OpenSaveStatisticsSet()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(cloudSaveFileName_StatisticsSet,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLastKnownGood,
                OnSavedStatisticsSetOpend);
        }
        else
        {
            Debug.Log("Fail to connect online");

            SaveStatisticsSetOnLocal();
        }
    }

    private void OnSavedStatisticsSetOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success to connect online");

            var update = new SavedGameMetadataUpdate.Builder().Build();

            var json = JsonUtility.ToJson(Statistics);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            Debug.Log("Save Data : " + bytes);

            savedGameClient.CommitUpdate(game, update, bytes, OnSavedStatisticsSetWritten);
        }
        else
        {
            Debug.Log("Fail to connect online");

            SaveStatisticsSetOnLocal();
        }
    }

    private void OnSavedStatisticsSetWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    public void LoadStatisticsSet()
    {
        OpenLoadStatisticsSet();
    }

    private void OpenLoadStatisticsSet()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(cloudSaveFileName_StatisticsSet,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLastKnownGood,
                LoadStatisticsSetData);
        }
        else
        {
            LoadStatisticsSetOnLocal();
        }
    }

    private void LoadStatisticsSetData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success to connect online");

            savedGameClient.ReadBinaryData(data, OnLoadedStatisticsSetData);
        }
        else
        {
            Debug.Log("Fail to connect online");

            LoadStatisticsSetOnLocal();
        }
    }

    private void OnLoadedStatisticsSetData(SavedGameRequestStatus status, byte[] loadedData)
    {
        if (loadedData == null || loadedData.Length == 0)
        {
            Debug.Log("No cloud data found. Saving default data to cloud.");
            SaveDefaultStatisticsSet();
            return;
        }

        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        if (data == "")
        {
            Debug.Log("There's no data, try to save default data");
            SaveDefaultStatisticsSet();
        }
        else
        {
            Debug.Log("Loaded data : " + data);

            Statistics = JsonUtility.FromJson<StatisticsSet>(data);
        }
    }

    public void SaveDefaultStatisticsSet()
    {
        OpenSaveDefaultStatisticsSet();
    }

    private void OpenSaveDefaultStatisticsSet()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(cloudSaveFileName_StatisticsSet,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLastKnownGood,
                OnSavedDefaultStatisticsSetOpend);
        }
        else
        {
            Debug.Log("Fail to connect online");

            InitializeStatisticsSet();
        }
    }

    private void OnSavedDefaultStatisticsSetOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success to connect online");

            var update = new SavedGameMetadataUpdate.Builder().Build();

            var json = JsonUtility.ToJson(initialStatistics);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            Statistics = initialStatistics;

            Debug.Log("Save Data : " + bytes);
            savedGameClient.CommitUpdate(game, update, bytes, OnSavedDefaultStatisticsSetWritten);
        }
        else
        {
            Debug.Log("Fail to connect online");

            InitializeStatisticsSet();
        }
    }

    private void OnSavedDefaultStatisticsSetWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.Log("Fail");
        }
    }

    private void SaveStatisticsSetOnLocal() // Save Statistics on local session
    {
        string json = JsonUtility.ToJson(Statistics, true);
        File.WriteAllText(Application.persistentDataPath + "/PinballBrick_StatisticsData.json", json);

        Debug.Log("Save Successfully on local session");
    }
    private void LoadStatisticsSetOnLocal() // Load Statistics on local session
    {
        string json;
        if (!File.Exists(Application.persistentDataPath + "/PinballBrick_StatisticsData.json"))
        {
            Debug.Log("There's no data, try to save default data to the local session");

            InitializeStatisticsSet();
        }
        else
        {
            json = File.ReadAllText(Application.persistentDataPath + "/PinballBrick_StatisticsData.json");
            Statistics = JsonUtility.FromJson<StatisticsSet>(json);
        }
        Debug.Log("Load Successfully on local session");

    }
    private void InitializeStatisticsSet() // Initialize Statistics on local session
    {
        string json = JsonUtility.ToJson(initialStatistics, true);
        File.WriteAllText(Application.persistentDataPath + "/PinballBrick_StatisticsData.json", json);
        Debug.Log("Initialize Successfully on local session");

        Statistics = JsonUtility.FromJson<StatisticsSet>(json);
    }
    #endregion

    public void UpdateStatistics(StatisticsSet statisticsSet)
    {
        Statistics.LevelUpCount += statisticsSet.LevelUpCount;
        Statistics.AchievedAugmentCount += statisticsSet.AchievedAugmentCount;
        Statistics.AchievedRareAugmentCount += statisticsSet.AchievedRareAugmentCount;

        Statistics.GainedGold += statisticsSet.GainedGold;

        Statistics.ReachedWave = 
            Statistics.ReachedWave < statisticsSet.ReachedWave ? 
            statisticsSet.ReachedWave : Statistics.ReachedWave;
        Statistics.PassedWave += statisticsSet.PassedWave;

        Statistics.DestroyedBricks += statisticsSet.DestroyedBricks;
        Statistics.DestroyedBossBricks += statisticsSet.DestroyedBossBricks;
        Statistics.DestroyedSpecialBricks += statisticsSet.DestroyedSpecialBricks;

        Statistics.SummonedBalls += statisticsSet.SummonedBalls;
        Statistics.DiedBalls += statisticsSet.DiedBalls;

        Statistics.Playtime += statisticsSet.Playtime;

        SaveStatisticsSet();
    }
}