using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public partial class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    public List<IAchievement> AchievementList;

    public List<List<object>> InGameAchievementProgress;

    enum InGameAchievementList
    {
        LevelUp,
        DestroyedBricks,
        SpawnedBalls,
        PassedWaves,
        DestroyedBossBricks
    }

    enum InGameAchievementIndex
    {
        Name,
        Level,
        Goal,
        Value,
        Reward
    }

    private void Awake()
    {
        instance = this;

        InGameAchievementProgress = new List<List<object>>();

        AchievementList = new List<IAchievement>
        {
            new ACHIEVEMENT_LEVEL_UP(),
            new ACHIEVEMENT_AUGMENTATION(),
            new ACHIEVEMENT_EUREKA(),
            new ACHIEVEMENT_GIANT_SLAYER(),
            
            new ACHIEVEMENT_PINBALLBRICKS_NEWBIE(),
            new ACHIEVEMENT_PINBALLBRICKS_NOVICE(),
            new ACHIEVEMENT_PINBALLBRICKS_VETERAN(),
            new ACHIEVEMENT_PINBALLBRICKS_MASTER(),
            
            new ACHIEVEMENT_BRICK_BREAKER(),
            new ACHIEVEMENT_BRICK_DESTROYER(),
            new ACHIEVEMENT_BRICK_ANNIHILATOR(),
            new ACHIEVEMENT_BRICK_OBLITERATOR(),
            
            new ACHIEVEMENT_WAVE_CLIMBER_1(),
            new ACHIEVEMENT_WAVE_CLIMBER_2(),
            new ACHIEVEMENT_WAVE_CLIMBER_3(),
            new ACHIEVEMENT_WAVE_CLIMBER_4(),
            new ACHIEVEMENT_WAVE_CLIMBER_5(),

            new ACHIEVEMENT_BALL_SPAWNER(),
            new ACHIEVEMENT_BALL_MACHINE(),
            new ACHIEVEMENT_BALL_PRODUCTION_LINE(),
            new ACHIEVEMENT_BALL_FACTORY()
        };

        LoadDailyQuests();
        GetComponent();

        if (!PlayerPrefs.HasKey("5_Reward"))
        {
            PlayerPrefs.SetInt("1_Level", 1);
            PlayerPrefs.SetInt("1_Goal", 5);
            PlayerPrefs.SetInt("1_Value", 0);
            PlayerPrefs.SetInt("1_Reward", 20);

            PlayerPrefs.SetInt("2_Level", 1);
            PlayerPrefs.SetInt("2_Goal", 20);
            PlayerPrefs.SetInt("2_Value", 0);
            PlayerPrefs.SetInt("2_Reward", 20);

            PlayerPrefs.SetInt("3_Level", 1);
            PlayerPrefs.SetInt("3_Goal", 20);
            PlayerPrefs.SetInt("3_Value", 0);
            PlayerPrefs.SetInt("3_Reward", 20);

            PlayerPrefs.SetInt("4_Level", 1);
            PlayerPrefs.SetInt("4_Goal", 10);
            PlayerPrefs.SetInt("4_Value", 0);
            PlayerPrefs.SetInt("4_Reward", 30);

            PlayerPrefs.SetInt("5_Level", 1);
            PlayerPrefs.SetInt("5_Goal", 5);
            PlayerPrefs.SetInt("5_Value", 0);
            PlayerPrefs.SetInt("5_Reward", 200);
        }

        InGameAchievementProgress = new List<List<object>>
        {
            new List<object>{ "Level Up", PlayerPrefs.GetInt("1_Level"), PlayerPrefs.GetInt("1_Goal"), PlayerPrefs.GetInt("1_Value"), PlayerPrefs.GetInt("1_Reward") },
            new List<object>{ "Destroyed Bricks", PlayerPrefs.GetInt("2_Level"), PlayerPrefs.GetInt("2_Goal"), PlayerPrefs.GetInt("2_Value"), PlayerPrefs.GetInt("2_Reward") },
            new List<object>{ "Spawned Balls", PlayerPrefs.GetInt("3_Level"), PlayerPrefs.GetInt("3_Goal"), PlayerPrefs.GetInt("3_Value"), PlayerPrefs.GetInt("3_Reward") },
            new List<object>{ "Passed Waves", PlayerPrefs.GetInt("4_Level"), PlayerPrefs.GetInt("4_Goal"), PlayerPrefs.GetInt("4_Value"), PlayerPrefs.GetInt("4_Reward") },
            new List<object>{ "Destroyed Boss Bricks", PlayerPrefs.GetInt("5_Level"), PlayerPrefs.GetInt("5_Goal"), PlayerPrefs.GetInt("5_Value"), PlayerPrefs.GetInt("5_Reward") },
        };
    }

    public void AchievementProgressing(IAchievement achievement, int progress)
    {
        achievement.Progress(progress);
    }

    public void AchievementCheck(StatisticsSet statistics)
    {
        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case (int)InGameAchievementList.LevelUp:
                    PlayerPrefs.SetInt("1_Value", PlayerPrefs.GetInt("1_Value") + statistics.LevelUpCount);
                    break;
                case (int)InGameAchievementList.DestroyedBricks:
                    PlayerPrefs.SetInt("2_Value", PlayerPrefs.GetInt("2_Value") + statistics.DestroyedBricks);
                    break;
                case (int)InGameAchievementList.SpawnedBalls:
                    PlayerPrefs.SetInt("3_Value", PlayerPrefs.GetInt("3_Value") + statistics.SummonedBalls);
                    break;
                case (int)InGameAchievementList.PassedWaves:
                    PlayerPrefs.SetInt("4_Value", PlayerPrefs.GetInt("4_Value") + statistics.PassedWave);
                    break;
                case (int)InGameAchievementList.DestroyedBossBricks:
                    PlayerPrefs.SetInt("5_Value", PlayerPrefs.GetInt("5_Value") + statistics.DestroyedBossBricks);
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (!DailyQuestSet[i].isCleared)
            {
                if (DailyQuestSet[i].QuestId == "PassedWave")
                    PlayerPrefs.SetInt("PassedWave", PlayerPrefs.GetInt("PassedWave") + statistics.PassedWave);

                else if (DailyQuestSet[i].QuestId == "ReachedWave")
                    PlayerPrefs.SetInt("ReachedWave", PlayerPrefs.GetInt("ReachedWave") + statistics.ReachedWave);

                else if (DailyQuestSet[i].QuestId == "DestroyBrick")
                    PlayerPrefs.SetInt("DestroyBrick", PlayerPrefs.GetInt("DestroyBrick") + statistics.DestroyedBricks);

                else if (DailyQuestSet[i].QuestId == "DestroyBoss")
                    PlayerPrefs.SetInt("DestroyBoss", PlayerPrefs.GetInt("DestroyBoss") + statistics.DestroyedBossBricks);

                else if (DailyQuestSet[i].QuestId == "DestroySpecialBrick")
                    PlayerPrefs.SetInt("DestroySpecialBrick", PlayerPrefs.GetInt("DestroySpecialBrick") + statistics.DestroyedSpecialBricks);

                else if (DailyQuestSet[i].QuestId == "SpawnBall")
                    PlayerPrefs.SetInt("SpawnBall", PlayerPrefs.GetInt("SpawnBall") + statistics.SummonedBalls);

                else if (DailyQuestSet[i].QuestId == "Play5Minutes")
                    PlayerPrefs.SetInt("Play5Minutes", PlayerPrefs.GetInt("Play5Minutes") + statistics.Playtime);
            }
        }

        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_LEVEL_UP>().FirstOrDefault(), statistics.LevelUpCount);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_AUGMENTATION>().FirstOrDefault(), statistics.AchievedAugmentCount);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_EUREKA>().FirstOrDefault(), statistics.AchievedRareAugmentCount);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_GIANT_SLAYER>().FirstOrDefault(), statistics.DestroyedBossBricks);

            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_PINBALLBRICKS_NEWBIE>().FirstOrDefault(), statistics.PassedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_PINBALLBRICKS_NOVICE>().FirstOrDefault(), statistics.PassedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_PINBALLBRICKS_VETERAN>().FirstOrDefault(), statistics.PassedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_PINBALLBRICKS_MASTER>().FirstOrDefault(), statistics.PassedWave);

            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BRICK_BREAKER>().FirstOrDefault(), statistics.DestroyedBricks);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BRICK_DESTROYER>().FirstOrDefault(), statistics.DestroyedBricks);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BRICK_ANNIHILATOR>().FirstOrDefault(), statistics.DestroyedBricks);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BRICK_OBLITERATOR>().FirstOrDefault(), statistics.DestroyedBricks);

            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_WAVE_CLIMBER_1>().FirstOrDefault(), statistics.ReachedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_WAVE_CLIMBER_2>().FirstOrDefault(), statistics.ReachedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_WAVE_CLIMBER_3>().FirstOrDefault(), statistics.ReachedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_WAVE_CLIMBER_4>().FirstOrDefault(), statistics.ReachedWave);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_WAVE_CLIMBER_5>().FirstOrDefault(), statistics.ReachedWave);

            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BALL_SPAWNER>().FirstOrDefault(), statistics.SummonedBalls);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BALL_MACHINE>().FirstOrDefault(), statistics.SummonedBalls);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BALL_PRODUCTION_LINE>().FirstOrDefault(), statistics.SummonedBalls);
            AchievementProgressing(AchievementList.OfType<ACHIEVEMENT_BALL_FACTORY>().FirstOrDefault(), statistics.SummonedBalls);
        }
    }

    public void UpdateInGameAchievement()
    {
        AchievementName_1.text = "Level Up";
        AchievementName_2.text = "Destroyed Brick";
        AchievementName_3.text = "Spawned Balls";
        AchievementName_4.text = "Passed Waves";
        AchievementName_5.text = "Destroyed Boss";

        AchievementLevel_1.text = "Lv. " + PlayerPrefs.GetInt("1_Level").ToString("F0");
        AchievementLevel_2.text = "Lv. " + PlayerPrefs.GetInt("2_Level").ToString("F0");
        AchievementLevel_3.text = "Lv. " + PlayerPrefs.GetInt("3_Level").ToString("F0");
        AchievementLevel_4.text = "Lv. " + PlayerPrefs.GetInt("4_Level").ToString("F0");
        AchievementLevel_5.text = "Lv. " + PlayerPrefs.GetInt("5_Level").ToString("F0");

        AchievementProgress_1.text = PlayerPrefs.GetInt("1_Value").ToString("F0") + " / " + PlayerPrefs.GetInt("1_Goal").ToString("F0");
        AchievementProgress_2.text = PlayerPrefs.GetInt("2_Value").ToString("F0") + " / " + PlayerPrefs.GetInt("2_Goal").ToString("F0");
        AchievementProgress_3.text = PlayerPrefs.GetInt("3_Value").ToString("F0") + " / " + PlayerPrefs.GetInt("3_Goal").ToString("F0");
        AchievementProgress_4.text = PlayerPrefs.GetInt("4_Value").ToString("F0") + " / " + PlayerPrefs.GetInt("4_Goal").ToString("F0");
        AchievementProgress_5.text = PlayerPrefs.GetInt("5_Value").ToString("F0") + " / " + PlayerPrefs.GetInt("5_Goal").ToString("F0");

        AchievementReward_1.text = PlayerPrefs.GetInt("1_Reward").ToString("F0");
        AchievementReward_2.text = PlayerPrefs.GetInt("2_Reward").ToString("F0");
        AchievementReward_3.text = PlayerPrefs.GetInt("3_Reward").ToString("F0");
        AchievementReward_4.text = PlayerPrefs.GetInt("4_Reward").ToString("F0");
        AchievementReward_5.text = PlayerPrefs.GetInt("5_Reward").ToString("F0");

        AchievementSlider_1.maxValue = PlayerPrefs.GetInt("1_Goal");
        AchievementSlider_1.value = PlayerPrefs.GetInt("1_Value");
        
        AchievementSlider_2.maxValue = PlayerPrefs.GetInt("2_Goal");
        AchievementSlider_2.value = PlayerPrefs.GetInt("2_Value");

        AchievementSlider_3.maxValue = PlayerPrefs.GetInt("3_Goal");
        AchievementSlider_3.value = PlayerPrefs.GetInt("3_Value");

        AchievementSlider_4.maxValue = PlayerPrefs.GetInt("4_Goal");
        AchievementSlider_4.value = PlayerPrefs.GetInt("4_Value");

        AchievementSlider_5.maxValue = PlayerPrefs.GetInt("5_Goal");
        AchievementSlider_5.value = PlayerPrefs.GetInt("5_Value");
    }

    public void OnAchievementButton_1()
    {
        if (PlayerPrefs.GetInt("1_Value") >= PlayerPrefs.GetInt("1_Goal"))
        {
            PlayerPrefs.SetInt("1_Level", PlayerPrefs.GetInt("1_Level") + 1);
            StatManager.instance.GetGold(PlayerPrefs.GetInt("1_Reward"));
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("1_Goal", PlayerPrefs.GetInt("1_Goal") + (5 * PlayerPrefs.GetInt("1_Level")));
            PlayerPrefs.SetInt("1_Reward", PlayerPrefs.GetInt("1_Reward") + 39);
            UpdateInGameAchievement();
        }
    }

    public void OnAchievementButton_2()
    {
        if (PlayerPrefs.GetInt("2_Value") >= PlayerPrefs.GetInt("2_Goal"))
        {
            PlayerPrefs.SetInt("2_Level", PlayerPrefs.GetInt("2_Level") + 1);
            StatManager.instance.GetGold(PlayerPrefs.GetInt("2_Reward"));
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("2_Goal", PlayerPrefs.GetInt("2_Goal") + (5 * PlayerPrefs.GetInt("2_Level")));
            PlayerPrefs.SetInt("2_Reward", PlayerPrefs.GetInt("2_Reward") + 45);
            UpdateInGameAchievement();
        }
    }

    public void OnAchievementButton_3()
    {
        if (PlayerPrefs.GetInt("3_Value") >= PlayerPrefs.GetInt("3_Goal"))
        {
            PlayerPrefs.SetInt("3_Level", PlayerPrefs.GetInt("3_Level") + 1);
            StatManager.instance.GetGold(PlayerPrefs.GetInt("3_Reward"));
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("3_Goal", PlayerPrefs.GetInt("3_Goal") + (5 * PlayerPrefs.GetInt("3_Level")));
            PlayerPrefs.SetInt("3_Reward", PlayerPrefs.GetInt("3_Reward") + 45);
            UpdateInGameAchievement();
        }
    }

    public void OnAchievementButton_4()
    {
        if (PlayerPrefs.GetInt("4_Value") >= PlayerPrefs.GetInt("4_Goal"))
        {
            PlayerPrefs.SetInt("4_Level", PlayerPrefs.GetInt("4_Level") + 1);
            StatManager.instance.GetGold(PlayerPrefs.GetInt("4_Reward"));
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("4_Goal", PlayerPrefs.GetInt("4_Goal") + (5 * PlayerPrefs.GetInt("4_Level")));
            PlayerPrefs.SetInt("4_Reward", PlayerPrefs.GetInt("4_Reward") + 53);
            UpdateInGameAchievement();
        }
    }

    public void OnAchievementButton_5()
    {
        if (PlayerPrefs.GetInt("5_Value") >= PlayerPrefs.GetInt("5_Goal"))
        {
            PlayerPrefs.SetInt("5_Level", PlayerPrefs.GetInt("5_Level") + 1);
            StatManager.instance.GetGold(PlayerPrefs.GetInt("5_Reward"));
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("5_Goal", PlayerPrefs.GetInt("5_Goal") + 2);
            PlayerPrefs.SetInt("5_Reward", PlayerPrefs.GetInt("5_Reward") + 67);
            UpdateInGameAchievement();
        }
    }
}

public partial class AchievementManager //daily quest
{
    public QuestSet questSet;
    public List<Quest> DailyQuestSet = new List<Quest>();

    private const string LastResetKey = "LastQuestReset";
    private const string DailyQuestKeyPrefix = "DailyQuest_";

    IEnumerator ResetTimer()
    {
        while (true)
        {
            TimeSpan Time = DateTime.UtcNow - DateTime.UtcNow.Date;
            TimeSpan remaining = TimeSpan.FromHours(24) - Time;
            DailyQuestResetTimer.text = remaining.ToString(@"hh\:mm\:ss");
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    void LoadDailyQuests()
    {
        string lastDate = PlayerPrefs.GetString(LastResetKey, "");
        string today = DateTime.UtcNow.ToString("yyyyMMdd");

        if (lastDate != today)
        {
            GenerateNewDailyQuests();
            PlayerPrefs.SetString(LastResetKey, today);
        }
        else
        {
            DailyQuestSet.Clear();
            for (int i = 0; i < 3; i++)
            {
                string id = PlayerPrefs.GetString(DailyQuestKeyPrefix + i);
                Quest quest = questSet.allQuests.Find(q => q.QuestId == id);
                quest.isCleared = false;
                if (quest != null)
                    DailyQuestSet.Add(quest);
            }
        }
    }

    void GenerateNewDailyQuests()
    {
        DailyQuestSet.Clear();

        List<Quest> shuffled = questSet.allQuests.OrderBy(q => UnityEngine.Random.value).ToList();
        for (int i = 0; i < 3; i++)
        {
            Quest quest = shuffled[i];
            DailyQuestSet.Add(quest);
            PlayerPrefs.SetString(DailyQuestKeyPrefix + i, quest.QuestId);
            PlayerPrefs.SetInt("isGetDailyQuest" + (i + 1) + "Reward", 0);
            PlayerPrefs.SetInt(quest.QuestId, 0);
        }

        PlayerPrefs.Save();
    }

    public void DailyQuestUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            if (DailyQuestSet[i].QuestId == "PassedWave")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("PassedWave");

            else if (DailyQuestSet[i].QuestId == "ReachedWave")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("ReachedWave");

            else if (DailyQuestSet[i].QuestId == "DestroyBrick")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("DestroyBrick");

            else if (DailyQuestSet[i].QuestId == "DestroyBoss")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("DestroyBoss");

            else if (DailyQuestSet[i].QuestId == "DestroySpecialBrick")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("DestroySpecialBrick");

            else if (DailyQuestSet[i].QuestId == "SpawnBall")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("SpawnBall");

            else if (DailyQuestSet[i].QuestId == "Play5Minutes")
                DailyQuestSet[i].QuestProgressValue = PlayerPrefs.GetInt("Play5Minutes");
            DailyQuestSet[i].isCleared = PlayerPrefs.GetInt("isGetDailyQuest" + (i + 1) + "Reward") == 1 ? true : false;
        }

        DailyQuest_1_Name.text = DailyQuestSet[0].QuestName;
        DailyQuest_2_Name.text = DailyQuestSet[1].QuestName;
        DailyQuest_3_Name.text = DailyQuestSet[2].QuestName;

        DailyQuest_1_Progress.text = DailyQuestSet[0].isCleared ?
            "Clear" :
            DailyQuestSet[0].QuestId == "Play5Minutes" ? 
            (DailyQuestSet[0].QuestProgressValue / 60).ToString("F1") + " / " + (DailyQuestSet[0].QuestGoal / 60).ToString("F1") :
            DailyQuestSet[0].QuestProgressValue.ToString("F0") + " / " + DailyQuestSet[0].QuestGoal.ToString("F0");

        DailyQuest_2_Progress.text = DailyQuestSet[1].isCleared ? 
            "Clear" :
            DailyQuestSet[1].QuestId == "Play5Minutes" ?
            (DailyQuestSet[1].QuestProgressValue / 60).ToString("F1") + " / " + (DailyQuestSet[1].QuestGoal / 60).ToString("F1") :
            DailyQuestSet[1].QuestProgressValue.ToString("F0") + " / " + DailyQuestSet[1].QuestGoal.ToString("F0");

        DailyQuest_3_Progress.text = DailyQuestSet[2].isCleared ?
            "Clear" :
            DailyQuestSet[2].QuestId == "Play5Minutes" ?
            (DailyQuestSet[2].QuestProgressValue / 60).ToString("F1") + " / " + (DailyQuestSet[2].QuestGoal / 60).ToString("F1") :
            DailyQuestSet[2].QuestProgressValue.ToString("F0") + " / " + DailyQuestSet[2].QuestGoal.ToString("F0");

        DailyQuest_1_Reward.text = DailyQuestSet[0].QuestRewards.ToString("F0");
        DailyQuest_2_Reward.text = DailyQuestSet[1].QuestRewards.ToString("F0");
        DailyQuest_3_Reward.text = DailyQuestSet[2].QuestRewards.ToString("F0");

        DailyQuest_1_Slider.maxValue = DailyQuestSet[0].QuestGoal;
        DailyQuest_1_Slider.value = DailyQuestSet[0].QuestProgressValue;

        DailyQuest_2_Slider.maxValue = DailyQuestSet[1].QuestGoal;
        DailyQuest_2_Slider.value = DailyQuestSet[1].QuestProgressValue;

        DailyQuest_3_Slider.maxValue = DailyQuestSet[2].QuestGoal;
        DailyQuest_3_Slider.value = DailyQuestSet[2].QuestProgressValue;
    }

    public void OnDailyQuestClear_1()
    {
        if (DailyQuestSet[0].QuestProgressValue >= DailyQuestSet[0].QuestGoal && PlayerPrefs.GetInt("isGetDailyQuest1Reward") == 0)
        {
            DailyQuestSet[0].isCleared = true;
            StatManager.instance.GetGold((int)DailyQuestSet[0].QuestRewards);
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("isGetDailyQuest1Reward", 1);
            DailyQuestUpdate();
        }
    }

    public void OnDailyQuestClear_2()
    {
        if (DailyQuestSet[1].QuestProgressValue >= DailyQuestSet[1].QuestGoal && PlayerPrefs.GetInt("isGetDailyQuest2Reward") == 0)
        {
            DailyQuestSet[1].isCleared = true;
            StatManager.instance.GetGold((int)DailyQuestSet[1].QuestRewards);
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("isGetDailyQuest2Reward", 1);
            DailyQuestUpdate();
        }
    }

    public void OnDailyQuestClear_3()
    {
        if (DailyQuestSet[2].QuestProgressValue >= DailyQuestSet[2].QuestGoal && PlayerPrefs.GetInt("isGetDailyQuest3Reward") == 0)
        {
            DailyQuestSet[2].isCleared = true;
            StatManager.instance.GetGold((int)DailyQuestSet[2].QuestRewards);
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("isGetDailyQuest3Reward", 1);
            DailyQuestUpdate();
        }
    }

}

public partial class AchievementManager
{
    private TMP_Text AchievementName_1;
    private TMP_Text AchievementName_2;
    private TMP_Text AchievementName_3;
    private TMP_Text AchievementName_4;
    private TMP_Text AchievementName_5;

    private TMP_Text AchievementLevel_1;
    private TMP_Text AchievementLevel_2;
    private TMP_Text AchievementLevel_3;
    private TMP_Text AchievementLevel_4;
    private TMP_Text AchievementLevel_5;

    private TMP_Text AchievementProgress_1;
    private TMP_Text AchievementProgress_2;
    private TMP_Text AchievementProgress_3;
    private TMP_Text AchievementProgress_4;
    private TMP_Text AchievementProgress_5;

    private TMP_Text AchievementReward_1;
    private TMP_Text AchievementReward_2;
    private TMP_Text AchievementReward_3;
    private TMP_Text AchievementReward_4;
    private TMP_Text AchievementReward_5;

    private Slider AchievementSlider_1;
    private Slider AchievementSlider_2;
    private Slider AchievementSlider_3;
    private Slider AchievementSlider_4;
    private Slider AchievementSlider_5;

    private TMP_Text DailyQuest_1_Name;
    private TMP_Text DailyQuest_2_Name;
    private TMP_Text DailyQuest_3_Name;

    private TMP_Text DailyQuest_1_Progress;
    private TMP_Text DailyQuest_2_Progress;
    private TMP_Text DailyQuest_3_Progress;

    private TMP_Text DailyQuest_1_Reward;
    private TMP_Text DailyQuest_2_Reward;
    private TMP_Text DailyQuest_3_Reward;

    private Slider DailyQuest_1_Slider;
    private Slider DailyQuest_2_Slider;
    private Slider DailyQuest_3_Slider;

    private TMP_Text DailyQuestResetTimer;

    private void GetComponent()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            AchievementName_1 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Level Up/Name").GetComponent<TMP_Text>();
            AchievementName_2 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Bricks/Name").GetComponent<TMP_Text>();
            AchievementName_3 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Spawned Balls/Name").GetComponent<TMP_Text>();
            AchievementName_4 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Passed Waves/Name").GetComponent<TMP_Text>();
            AchievementName_5 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Boss Bricks/Name").GetComponent<TMP_Text>();

            AchievementProgress_1 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Level Up/Progress").GetComponent<TMP_Text>();
            AchievementProgress_2 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Bricks/Progress").GetComponent<TMP_Text>();
            AchievementProgress_3 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Spawned Balls/Progress").GetComponent<TMP_Text>();
            AchievementProgress_4 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Passed Waves/Progress").GetComponent<TMP_Text>();
            AchievementProgress_5 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Boss Bricks/Progress").GetComponent<TMP_Text>();

            AchievementLevel_1 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Level Up/Level").GetComponent<TMP_Text>();
            AchievementLevel_2 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Bricks/Level").GetComponent<TMP_Text>();
            AchievementLevel_3 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Spawned Balls/Level").GetComponent<TMP_Text>();
            AchievementLevel_4 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Passed Waves/Level").GetComponent<TMP_Text>();
            AchievementLevel_5 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Boss Bricks/Level").GetComponent<TMP_Text>();

            AchievementReward_1 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Level Up/CompleteButton/Reward").GetComponent<TMP_Text>();
            AchievementReward_2 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Bricks/CompleteButton/Reward").GetComponent<TMP_Text>();
            AchievementReward_3 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Spawned Balls/CompleteButton/Reward").GetComponent<TMP_Text>();
            AchievementReward_4 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Passed Waves/CompleteButton/Reward").GetComponent<TMP_Text>();
            AchievementReward_5 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Boss Bricks/CompleteButton/Reward").GetComponent<TMP_Text>();

            AchievementSlider_1 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Level Up/Slider").GetComponent<Slider>();
            AchievementSlider_2 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Bricks/Slider").GetComponent<Slider>();
            AchievementSlider_3 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Spawned Balls/Slider").GetComponent<Slider>();
            AchievementSlider_4 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Passed Waves/Slider").GetComponent<Slider>();
            AchievementSlider_5 = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Achievement/Panels/Scroll View/Viewport/Content/Destroyed Boss Bricks/Slider").GetComponent<Slider>();

            DailyQuest_1_Name = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_1/Name").GetComponent<TMP_Text>();
            DailyQuest_2_Name = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_2/Name").GetComponent<TMP_Text>();
            DailyQuest_3_Name = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_3/Name").GetComponent<TMP_Text>();

            DailyQuest_1_Progress = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_1/Progress").GetComponent<TMP_Text>();
            DailyQuest_2_Progress = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_2/Progress").GetComponent<TMP_Text>();
            DailyQuest_3_Progress = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_3/Progress").GetComponent<TMP_Text>();

            DailyQuest_1_Reward = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_1/CompleteButton/Reward").GetComponent<TMP_Text>();
            DailyQuest_2_Reward = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_2/CompleteButton/Reward").GetComponent<TMP_Text>();
            DailyQuest_3_Reward = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_3/CompleteButton/Reward").GetComponent<TMP_Text>();

            DailyQuest_1_Slider = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_1/Slider").GetComponent<Slider>();
            DailyQuest_2_Slider = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_2/Slider").GetComponent<Slider>();
            DailyQuest_3_Slider = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/Panels/DailyQuest_3/Slider").GetComponent<Slider>();

            DailyQuestResetTimer = GameObject.Find("Canvas").
                transform.Find("Scroll View/Viewport/Content/Mission/Daily Mission/ResetTimer").GetComponent<TMP_Text>();

            UpdateInGameAchievement();
            DailyQuestUpdate();
            StartCoroutine(ResetTimer());
        }
    }
}