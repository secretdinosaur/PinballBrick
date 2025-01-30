using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class LevelUpPanel : MonoBehaviour
{ 
   public Sprite[] OptionButtonSprites;
   private TMP_Text NoticeLevelText;

   private Button[] OptionButtons;
   
   private TMP_Text Option_1_Name;
   private TMP_Text Option_1_Description;
   private TMP_Text Option_2_Name;
   private TMP_Text Option_2_Description;
   private TMP_Text Option_3_Name;
   private TMP_Text Option_3_Description;
   
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
    private List<KeyValuePair<string, int>> randomStats;
    private List<IAugment> augmentsSet;
    private bool CheckStatOrAugment;
    void Update()
    {
        if (LevelManager.Instance.LevelUpPanel.activeSelf && !isActive)
        {
            isActive = true;
            NoticeLevelText.text = "Now Your Level: " + LevelManager.Instance.Level;
            optionIndex = -1;
            if (IsStatOrAugment())
            {
                randomStats = LevelManager.Instance.GetRandomStats();
                for (int i = 0; i < 3; i++)
                {
                    OptionButtons[i].gameObject.GetComponent<Image>().sprite = OptionButtonSprites[4];
                }
                Option_1_Name.text = randomStats[0].Key;
                Option_1_Description.text =
                    "Lv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[0].Value] + 1);
                Option_2_Name.text = randomStats[1].Key;
                Option_2_Description.text =
                    "Lv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[1].Value] + 1);
                Option_3_Name.text = randomStats[2].Key;
                Option_3_Description.text =
                    "Lv. " + (StatManager.instance.Temporary_stat.StatLevel[randomStats[2].Value] + 1);
                CheckStatOrAugment = true;
            }
            else
            {
                augmentsSet = new List<IAugment>();
                for (int i = 0; i < 3; i++)
                {
                    augmentsSet.Add(AugmentManager.instance.GetRandomAugment());
                    OptionButtons[i].gameObject.GetComponent<Image>().sprite = OptionButtonSprites[augmentsSet[i].Rarity - 1];
                }
                Option_1_Name.text = augmentsSet[0].Name;
                Option_1_Description.text = augmentsSet[0].Description;
                Option_2_Name.text = augmentsSet[1].Name;
                Option_2_Description.text = augmentsSet[1].Description;
                Option_3_Name.text = augmentsSet[2].Name;
                Option_3_Description.text = augmentsSet[2].Description;
                CheckStatOrAugment = false;
            }
        }
        else if (!LevelManager.Instance.LevelUpPanel.activeSelf && isActive)
        {
            isActive = false;
        }
    }

    private bool IsStatOrAugment()
    {
        return LevelManager.Instance.Level % 2 != 0;
    }
    
    private int optionIndex = -1;
    public void OptionButtonClick(Button clickedButton)
    {
        foreach (Button button in OptionButtons)
        {
            button.transform.localScale = button == clickedButton ? new Vector3(1.05f, 1.05f, 1.05f) : Vector3.one;
        }
        optionIndex = Array.IndexOf(OptionButtons, clickedButton);
    }
    
    public void LevelUpEnd()
    {
        if (CheckStatOrAugment)
        {
            if (optionIndex == -1)
            {
                LevelManager.Instance.Temporary_Stat.StatLevel[randomStats[Random.Range(0, 3)].Value]++;
                StatManager.instance.SaveTemporaryStat(LevelManager.Instance.CalculateTemporaryStatWithLevel());
            }
            else
            {
                LevelManager.Instance.Temporary_Stat.StatLevel[randomStats[optionIndex].Value]++;
                StatManager.instance.SaveTemporaryStat(LevelManager.Instance.CalculateTemporaryStatWithLevel());
            }
        }
        else
        {
            if (optionIndex == -1)
            {
                AugmentManager.instance.AddAugment(augmentsSet[Random.Range(0, 3)]);
            }
            else
            {
                AugmentManager.instance.AddAugment(augmentsSet[optionIndex]);
            }
        }
        Time.timeScale = 1f;
        LevelManager.Instance.LevelUpPanel.SetActive(false);
    }
}
