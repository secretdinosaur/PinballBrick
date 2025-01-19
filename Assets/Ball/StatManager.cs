using System.IO;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager instance;

    private string filePath;
    void Awake()
    {
        filePath = Application.persistentDataPath + "/PinballBrickPlayer.json";

        if (!File.Exists(filePath))
        {
            InitializeStats();
        }
    }
    void Start()
    {
        instance = this;
    }
    void InitializeStats()
    {
        Stat initialStats = new Stat
        {
            AttackDamage = 5,
            StartingBall = 1,
            BonusExp = 1f,
            ItemDropChance = 0.2f,
            CriticalChance = 0.0f,
            CriticalMultiplier = 1.1f
        };

        string json = JsonUtility.ToJson(initialStats, true);
        File.WriteAllText(filePath, json);
    }
    public Stat LoadStats()
    {
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<Stat>(json);
    }
    public void SaveStats(Stat stats)
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(filePath, json);
    }
}
