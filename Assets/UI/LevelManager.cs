using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Slider LevelBar;
    public GameObject LevelUpPanel;
    public Temporary_Stat Temporary_Stat;
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
    void Awake()
    {
        Instance = this;
        LevelBar = GameObject.Find("Canvas").transform.Find("LevelBar").GetComponent<Slider>();
        LevelUpPanel = GameObject.Find("Canvas").transform.Find("LevelUpPanel").gameObject;
    }
    void Start()
    {
        Exp = 0;
        Level = 1;
        Temporary_Stat = StatManager.instance.Temporary_stat;
    }

    public void LevelUp()
    {
        Exp -= LevelBar.maxValue;
        Level++;
        LevelBarMaxValueSet(Level);
        Time.timeScale = 0f;
        LevelUpPanel.SetActive(true);
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

    public List<KeyValuePair<string, int>> GetRandomStats()
    {
        Dictionary<string, int> statTable = new Dictionary<string, int>
        {
            { "Attack\nDamage", 0},
            { "Critical\nDamage", 1 },
            { "Critical\nChance", 2 },
            { "Ball\nSpeed", 3 },
            { "Effect\nDamage", 4 },
            { "Effect\nDuration", 5 },
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

    public void GetExp(float value)
    {
        Exp += value * StatManager.instance.Persistent_stat.BonusExp;
    }

    public Temporary_Stat CalculateTemporaryStatWithLevel() // stat formula
    {
        Temporary_Stat stat = new Temporary_Stat();
        stat.StatLevel = Temporary_Stat.StatLevel;
        stat.AttackDamage = Temporary_Stat.AttackDamage + Temporary_Stat.StatLevel[0] - 1;
        stat.EffectDamage = Temporary_Stat.EffectDamage + Temporary_Stat.StatLevel[4] - 1;
        return stat;
    }
}
