using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Slider LevelBar;
    public GameObject LevelUpPanel;
    public int StatPoints;
    public int StatPointsTemp;
    public Temporary_Stat Temporary_Stat;
    public int[] StatLevel;
    public int[] StatLevelTemp;
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
        StatPoints = 0;
        StatLevel = new[] { 1, 1, 1, 1, 1, 1, 1 };
        Temporary_Stat = StatManager.instance.LoadTemporaryStat();
    }
    void Update()
    {
        LevelBar.value = Exp;
    }

    private void LevelUp()
    {
        Exp -= LevelBar.maxValue;
        Level++;
        LevelBar.maxValue *= 1.2f;
        StatPoints += 2;
        StatPointsTemp = StatPoints;
        StatLevelTemp = (int[])StatLevel.Clone();
        Time.timeScale = 0f;
        LevelUpPanel.SetActive(true);
    }

    public Temporary_Stat CalculateTemporaryStatWithLevel()
    {
        Temporary_Stat stat = new Temporary_Stat();
        stat.AttackDamage = 1 + (StatLevel[0] - 1) * 2;
        stat.CriticalDamage = 0.01f * StatLevel[1];
        stat.CriticalChance = 0.02f * StatLevel[2];
        stat.BallSpeed = 0.01f * StatLevel[3];
        stat.EffectDamage = 0.01f * StatLevel[4];
        stat.EffectDuration = 0.5f * StatLevel[5];
        stat.GoldGain = 0.05f * StatLevel[6];
        return stat;
    }
}
