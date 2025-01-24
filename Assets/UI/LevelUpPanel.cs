using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
   private TMP_Text NoticeLevelText;

   private Button[] OptionButtons;
   
   private TMP_Text Option_1_Name;
   private TMP_Text Option_1_Description;
   private TMP_Text Option_2_Name;
   private TMP_Text Option_2_Description;
   private TMP_Text Option_3_Name;
   private TMP_Text Option_3_Description;

   private List<KeyValuePair<string, int>> randomStats;
    void Awake()
    {
        NoticeLevelText = LevelManager.Instance.LevelUpPanel.transform.Find("NoticeLevelText").GetComponent<TMP_Text>();
        
        OptionButtons = new []
        {
            LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_1").GetComponent<Button>(),
            LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_2").GetComponent<Button>(),
            LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_3").GetComponent<Button>()
        };
        Option_1_Name = LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_1").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_1_Description = LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_1").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_2_Name = LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_2").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_2_Description = LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_2").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
        Option_3_Name = LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_3").gameObject
            .transform.Find("Name").GetComponent<TMP_Text>();
        Option_3_Description = LevelManager.Instance.LevelUpPanel.transform.Find("SelectButton_3").gameObject
            .transform.Find("Description").GetComponent<TMP_Text>();
    }
    
    private bool isActive;
    void Update()
    {
        if (LevelManager.Instance.LevelUpPanel.activeSelf && !isActive)
        {
            isActive = true;
            NoticeLevelText.text = "Now Your Level: " + LevelManager.Instance.Level;
            RollRandomStats();
            Option_1_Name.text = randomStats[0].Key;
            Option_1_Description.text = 
                "Lv. " + StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 
                " -> " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 1);
            Option_2_Name.text = randomStats[1].Key;
            Option_2_Description.text = 
                "Lv. " + StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 
                " -> " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 1);
            Option_3_Name.text = randomStats[2].Key;
            Option_3_Description.text = 
                "Lv. " + StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 
                " -> " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 1);
        }
        else if (!LevelManager.Instance.LevelUpPanel.activeSelf && isActive)
        {
            isActive = false;
        }
    }

    private void RollRandomStats()
    {
        randomStats = LevelManager.Instance.GetRandomStats();
    }
    
    private int optionIndex;
    public void OptionButtonClick(Button clickedButton)
    {
        foreach (Button button in OptionButtons)
        {
            button.transform.localScale = button == clickedButton ? Vector3.one : new Vector3(0.93f, 0.93f, 0.93f);
        }
        optionIndex = Array.IndexOf(OptionButtons, clickedButton);
    }
    
    public void LevelUpEnd()
    {
        LevelManager.Instance.Temporary_Stat.StatLevel[randomStats[optionIndex].Value]++;
        Time.timeScale = 1f;
        StatManager.instance.SaveTemporaryStat(LevelManager.Instance.CalculateTemporaryStatWithLevel());
        LevelManager.Instance.LevelUpPanel.SetActive(false);
    }
}
