using System.IO;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager instance;
    
    private Temporary_Stat temporarystat;
    public Temporary_Stat TemporaryStat
    {
        get
        {
            temporarystat = LoadTemporaryStat();
            return temporarystat;
        }
        set
        {
            SaveTemporaryStat(value);
        }
    }
    
    private string persistent_stat_filePath;
    private string temporary_stat_filePath;
    void Awake()
    {
        instance = this;
        persistent_stat_filePath = Application.persistentDataPath + "/PinballBrick_persistentData.json";
        temporary_stat_filePath = Application.persistentDataPath + "/PinballBrick_temporaryData.json";

        if (!File.Exists(persistent_stat_filePath))
        {
            InitializePersistentStats();
        }

        if (!File.Exists(temporary_stat_filePath))
        {
            InitializeTemporaryStats();
        }
        else if (File.Exists(temporary_stat_filePath))
        {
            File.Delete(temporary_stat_filePath);
            InitializeTemporaryStats();
        }
    }
    void InitializePersistentStats()
    {
        Persistent_Stat initialStats = new Persistent_Stat
        {
            AttackDamage = 5,
            StartingBall = 1,
            BonusExp = 1f,
            ItemDropChance = 0.2f,
            CriticalChance = 0.0f,
            CriticalDamage = 1.1f
        };

        string json = JsonUtility.ToJson(initialStats, true);
        File.WriteAllText(persistent_stat_filePath, json);
    }

    public void InitializeTemporaryStats()
    {
        Temporary_Stat initialStats = new Temporary_Stat
        {
            AttackDamage = 0,
            CriticalDamage = 0.0f,
            CriticalChance = 0.0f,
            BallSpeed = 0.0f,
            EffectDamage = 0.0f,
            EffectDuration = 0.0f,
            GoldGain = 0.0f
        };
        
        string json = JsonUtility.ToJson(initialStats, true);
        File.WriteAllText(temporary_stat_filePath, json);
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

    public Temporary_Stat LoadTemporaryStat()
    {
        string json = File.ReadAllText(temporary_stat_filePath);
        return JsonUtility.FromJson<Temporary_Stat>(json);
    }
    
    public void SaveTemporaryStat(Temporary_Stat stats)
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(temporary_stat_filePath, json);
    }
    
}
