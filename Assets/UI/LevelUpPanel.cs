using UnityEngine;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
   private TMP_Text NoticeLevelText;
   private TMP_Text SPText;

   private TMP_Text AtkStatLevel;
   private TMP_Text CritDamageStatLevel;
   private TMP_Text CritChanceStatLevel;
   private TMP_Text BallSpeedStatLevel;
   private TMP_Text EffectDamageStatLevel;
   private TMP_Text EffectDurationStatLevel;
   private TMP_Text GoldGainStatLevel;

   private int maxLevel = 20;
    void Start()
    {
        NoticeLevelText = LevelManager.Instance.LevelUpPanel.transform.Find("NoticeLevelText").GetComponent<TMP_Text>();
        SPText = LevelManager.Instance.LevelUpPanel.transform.Find("SkillPointText").GetComponent<TMP_Text>();
        
        AtkStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("AttackDamageUp").gameObject
            .transform.Find("AtkStatLevel").GetComponent<TMP_Text>();
        CritDamageStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("CriticalDamageUp").gameObject
            .transform.Find("CritDamageStatLevel").GetComponent<TMP_Text>();
        CritChanceStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("CriticalChanceUp").gameObject
            .transform.Find("CritChanceStatLevel").GetComponent<TMP_Text>();
        BallSpeedStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("BallSpeedUp").gameObject
            .transform.Find("BallSpeedStatLevel").GetComponent<TMP_Text>();
        EffectDamageStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("EffectDamageUp").gameObject
            .transform.Find("EffectDamageStatLevel").GetComponent<TMP_Text>();
        EffectDurationStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("EffectDurationUp").gameObject
            .transform.Find("EffectDurationStatLevel").GetComponent<TMP_Text>();
        GoldGainStatLevel = LevelManager.Instance.LevelUpPanel.transform.Find("GoldGainUp").gameObject
            .transform.Find("GoldGainStatLevel").GetComponent<TMP_Text>();
    }

    void Update()
    {
        NoticeLevelText.text = "Now Your Level: " + LevelManager.Instance.Level;
        SPText.text = "SP: " + LevelManager.Instance.StatPoints;

        AtkStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[0] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[0] : "Max");
        CritDamageStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[1] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[1] : "Max");
        CritChanceStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[2] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[2] : "Max");
        BallSpeedStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[3] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[3] : "Max");
        EffectDamageStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[4] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[4] : "Max");
        EffectDurationStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[5] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[5] : "Max");
        GoldGainStatLevel.text = 
            "Lv. " + (LevelManager.Instance.Temporary_Stat.StatLevel[6] != maxLevel 
                ? LevelManager.Instance.Temporary_Stat.StatLevel[6] : "Max");
    }

    public void AtkLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[0] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[0]++;
            LevelManager.Instance.StatPoints--;
        }
    }

    public void CritDamageLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[1] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[1]++;
            LevelManager.Instance.StatPoints--;
        }
    }
    
    public void CritChanceLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[2] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[2]++;
            LevelManager.Instance.StatPoints--;
        }
    }
    
    public void BallSpeedLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[3] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[3]++;
            LevelManager.Instance.StatPoints--;
        }
    }

    public void EffectDamageLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[4] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[4]++;
            LevelManager.Instance.StatPoints--;
        }
    }

    public void EffectDurationLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[5] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[5]++;
            LevelManager.Instance.StatPoints--;
        }
    }

    public void GoldGainLevelUpButton()
    {
        if (LevelManager.Instance.Temporary_Stat.StatLevel[6] < maxLevel && LevelManager.Instance.StatPoints != 0)
        {
            LevelManager.Instance.Temporary_Stat.StatLevel[6]++;
            LevelManager.Instance.StatPoints--;
        }
    }

    public void ResetButton()
    {
        LevelManager.Instance.StatPoints = LevelManager.Instance.StatPointsTemp;
        LevelManager.Instance.Temporary_Stat.StatLevel = (int[])LevelManager.Instance.StatLevelTemp.Clone();
    }
    
    public void LevelUpEnd()
    {
        Time.timeScale = 1f;
        StatManager.instance.SaveTemporaryStat(LevelManager.Instance.CalculateTemporaryStatWithLevel());
        LevelManager.Instance.LevelUpPanel.SetActive(false);
    }

    public void TestAugmentButton()
    {
        IAugment TestAtkUp1 = AugmentManager.instance.TestAtkUp1();
        AugmentManager.instance.AddAugment(TestAtkUp1);
    }
}
