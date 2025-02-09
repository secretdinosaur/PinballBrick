using System.IO;
using UnityEngine;
using System.Collections;
public class StatManager : MonoBehaviour
{
    public static StatManager instance;
    
    public Persistent_Stat Persistent_stat; // If you want to use it elsewhere, use this variable
    public Temporary_Stat Temporary_stat; //
    public StatMultiplier statMultiplier; //
    
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
    }
    void InitializePersistentStats() // persistent stats
    {
        Persistent_Stat initialStats = new Persistent_Stat
        {
            AttackDamage = 5,
            StartingBall = 1,
            BonusExp = 1f,
            ItemDropChance = 0.2f,
            CriticalChance = 0.0f,
            CriticalDamage = 1.1f,
            Gold = 0.0f
        };

        string json = JsonUtility.ToJson(initialStats, true);
        File.WriteAllText(persistent_stat_filePath, json);
    }

    public void InitializeTemporaryStats() // temporary stats
    {
        Temporary_Stat initialStats = new Temporary_Stat
        {
            StatLevel = new []{1, 1, 1, 1, 1, 1, 1},
            AttackDamage = 0,
            CriticalDamage = 0.0f,
            CriticalChance = 0.0f,
            BallSpeed = 0.0f,
            EffectDamage = 0.5f,
            EffectDuration = 0.5f,
            EffectCriticalDamage = 1.0f,
            GoldGain = 0.0f
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

}
