using System.IO;
using UnityEngine;
using System.Collections;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public partial class StatManager : MonoBehaviour
{
    public static StatManager instance;
    
    public Persistent_Stat Persistent_stat; // If you want to use it elsewhere, use this variable
    public Temporary_Stat Temporary_stat; //
    public StatMultiplier statMultiplier; //
    public static float AttackDamage;
    public static float EffectDamage;

    private string persistent_stat_filePath;
    private string temporary_stat_filePath;
    private string stat_Multiplier_filePath;
    void Awake()
    {
        instance = this;
        persistent_stat_filePath = Application.persistentDataPath + "/PinballBrick_persistentData.json";
        temporary_stat_filePath = Application.persistentDataPath + "/PinballBrick_temporaryData.json";
        stat_Multiplier_filePath = Application.persistentDataPath + "/PinballBrick_multiplierData.json";

        if (!File.Exists(persistent_stat_filePath))
        {
            InitializePersistentStats();
        }

        if (!File.Exists(temporary_stat_filePath))
        {
            InitializeTemporaryStats();
        }
        else
        {
            File.Delete(temporary_stat_filePath);
            InitializeTemporaryStats();
        }

        if (!File.Exists(stat_Multiplier_filePath))
        {
            InitializeStatMultiplier();
        }
        else
        {
            File.Delete(stat_Multiplier_filePath);
            InitializeStatMultiplier();
        }

        Persistent_stat = LoadPersistentStats();
        Temporary_stat = LoadTemporaryStat();
        statMultiplier = LoadStatMultiplier();

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
        isChangedStat = false;

        AttackDamage = (Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) * statMultiplier.AttackDamageMultiplier;
        EffectDamage = (Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) * statMultiplier.EffectDamageMultiplier;
    }
    void InitializePersistentStats() // persistent stats
    {
        Persistent_Stat initialStats = new Persistent_Stat
        {
            AttackDamage = 5f,
            EffectDamage = 5f,
            StartingBall = 5,
            BonusExp = 1f,
            CriticalChance = 0.0f,
            CriticalDamage = 1.1f,
            BonusGold = 1f,
            Gold = 100000f,

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

        string json = JsonUtility.ToJson(initialStats, true);
        File.WriteAllText(persistent_stat_filePath, json);
    }

    public void InitializeTemporaryStats() // temporary stats
    {
        Temporary_Stat initialStats = new Temporary_Stat
        {
            StatLevel = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            AttackDamage = 0f,
            CriticalDamage = 0.0f,
            CriticalChance = 0.0f,
            EffectDamage = 0.5f,
            EffectDuration = 1f,
            GoldGain = 0.0f,
            ExpGain = 0.0f,
            SummonBall = 5
        };
        
        string json = JsonUtility.ToJson(initialStats, true);
        File.WriteAllText(temporary_stat_filePath, json);
    }
    
    public void InitializeStatMultiplier() // stat multiplier
    {
        StatMultiplier initialMultiplier = new StatMultiplier
        {
            AttackDamageMultiplier = 1.0f,
            EffectDamageMultiplier = 1.0f
        };
        
        string json = JsonUtility.ToJson(initialMultiplier, true);
        File.WriteAllText(stat_Multiplier_filePath, json);
    }
    public Persistent_Stat LoadPersistentStats()
    {
        string json = File.ReadAllText(persistent_stat_filePath);
        return JsonUtility.FromJson<Persistent_Stat>(json);
    }
    public void SavePersistentStats(Persistent_Stat stats)
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(persistent_stat_filePath, json);

        json = File.ReadAllText(persistent_stat_filePath);
        Persistent_stat = JsonUtility.FromJson<Persistent_Stat>(json);
    }

    public Temporary_Stat LoadTemporaryStat() // Just Load
    {
        string json = File.ReadAllText(temporary_stat_filePath);
        return JsonUtility.FromJson<Temporary_Stat>(json);
    }
    
    public void SaveTemporaryStat(Temporary_Stat stats) //Save and Synchronization
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(temporary_stat_filePath, json);
        
        json = File.ReadAllText(temporary_stat_filePath);
        Temporary_stat = JsonUtility.FromJson<Temporary_Stat>(json);

        AttackDamage = (Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) * statMultiplier.AttackDamageMultiplier;
        EffectDamage = (Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) * statMultiplier.EffectDamageMultiplier;
    }
    
    public StatMultiplier LoadStatMultiplier() // Just Load
    {
        string json = File.ReadAllText(stat_Multiplier_filePath);
        return JsonUtility.FromJson<StatMultiplier>(json);
    }
    
    public void SaveStatMultiplier(StatMultiplier stats) //Save and Synchronization
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(stat_Multiplier_filePath, json);
        
        json = File.ReadAllText(stat_Multiplier_filePath);
        statMultiplier = JsonUtility.FromJson<StatMultiplier>(json);

        AttackDamage = (Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) * statMultiplier.AttackDamageMultiplier;
        EffectDamage = (Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) * statMultiplier.EffectDamageMultiplier;
    }

    public void RunAttackDamageUpFewSec(float setTime, float AttackUpValue)
    {
        StartCoroutine(AttackDamageUpFewSec(setTime, AttackUpValue));
    }

    IEnumerator AttackDamageUpFewSec(float setTime, float AttackUpValue)
    {
        statMultiplier.AttackDamageMultiplier += AttackUpValue;
        SaveStatMultiplier(statMultiplier);
        yield return new WaitForSeconds(setTime);
        statMultiplier.AttackDamageMultiplier -= AttackUpValue;
        SaveStatMultiplier(statMultiplier);
    }

    public void GetGold(int Value)
    {
        Persistent_stat.Gold += Value * (Persistent_stat.BonusGold + Temporary_stat.GoldGain);
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

    public bool isChangedStat;

    public IEnumerator ShowChangedStat()
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

        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + 1f)
        {
            float t = (Time.realtimeSinceStartup - startTime) / 1f;
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-165f, 129f), new Vector2(165f, 129f), t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(165f, 129f);
        canvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(1.5f);

        startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + 1f)
        {
            float t = (Time.realtimeSinceStartup - startTime) / 1f;

            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(165f, 129f), new Vector2(-165f, 129f), t);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(-165f, 129f);
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
        if (AugmentManager.instance.determineAugmentID("STAT_28"))
        {
            Temporary_stat.EffectDamage += value * 0.2f;
            TotalChangedStatValue[3] += value * 0.2f;
        }
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeTemporaryStats_CriticalDamage(float value)
    {
        Temporary_stat.CriticalDamage += value;
        TotalChangedStatValue[1] += value;
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeTemporaryStats_CriticalChance(float value)
    {
        float MaxCriChance = 1f;
        if (AugmentManager.instance.determineAugmentID("STAT_25"))
        {
            MaxCriChance = 0.1f;
        }
        if (AugmentManager.instance.determineAugmentID("STAT_21"))
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
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeTemporaryStats_EffectDamage(float value)
    {
        Temporary_stat.EffectDamage += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[3] += (float)System.Math.Round(value, 2);
        if (AugmentManager.instance.determineAugmentID("STAT_28"))
        {
            Temporary_stat.AttackDamage += value * 0.2f;
            TotalChangedStatValue[0] += value * 0.2f;
        }
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeTemporaryStats_EffectDuration(float value)
    {
        Temporary_stat.EffectDuration += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[4] += (float)System.Math.Round(value, 2);
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeTemporaryStats_GoldGain(float value)
    {
        Temporary_stat.GoldGain += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[5] += (float)System.Math.Round(value, 2);
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeTemporaryStats_ExpGain(float value)
    {
        Temporary_stat.ExpGain += (float)System.Math.Round(value, 2);
        TotalChangedStatValue[6] += (float)System.Math.Round(value, 2);
        SaveTemporaryStat(Temporary_stat);
        isChangedStat = true;
    }
    public void ChangeStatMultiplier_AttackDamageMultiplier(float value)
    {
        if(value >= 0)
        {
            statMultiplier.AttackDamageMultiplier *= (float)System.Math.Round(value, 3);
            TotalChangedStatValue[7] *= (float)System.Math.Round(value, 2);
            SaveStatMultiplier(statMultiplier);
        }
        else if(value < 0)
        {
            statMultiplier.AttackDamageMultiplier /= (float)System.Math.Round(-value, 3);
            TotalChangedStatValue[7] /= (float)System.Math.Round(-value, 2);
            SaveStatMultiplier(statMultiplier);
        }
        isChangedStat = true;
    }
    public void ChangeStatMultiplier_EffectDamageMultiplier(float value)
    {
        if(value >= 0)
        {
            statMultiplier.EffectDamageMultiplier *= (float)System.Math.Round(value, 3);
            TotalChangedStatValue[8] *= (float)System.Math.Round(value, 2);
            SaveStatMultiplier(statMultiplier);
        }
        else if(value < 0)
        {
            statMultiplier.EffectDamageMultiplier /= (float)System.Math.Round(-value, 3);
            TotalChangedStatValue[8] /= (float)System.Math.Round(-value, 2);
            SaveStatMultiplier(statMultiplier);
        }
        isChangedStat = true;
    }
}