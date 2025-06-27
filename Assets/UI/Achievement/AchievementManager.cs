using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;

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
            gameObject.AddComponent<ACHIEVEMENT_LEVEL_UP>(),
            gameObject.AddComponent<ACHIEVEMENT_AUGMENTATION>(),
            gameObject.AddComponent<ACHIEVEMENT_EUREKA>(),
            gameObject.AddComponent<ACHIEVEMENT_GIANT_SLAYER>(),
            gameObject.AddComponent<ACHIEVEMENT_PINBALLBRICKS_NEWBIE>(),
            gameObject.AddComponent<ACHIEVEMENT_PINBALLBRICKS_NOVICE>(),
            gameObject.AddComponent<ACHIEVEMENT_PINBALLBRICKS_VETERAN>(),
            gameObject.AddComponent<ACHIEVEMENT_PINBALLBRICKS_MASTER>(),
            gameObject.AddComponent<ACHIEVEMENT_BRICK_BREAKER>(),
            gameObject.AddComponent<ACHIEVEMENT_BRICK_DESTROYER>(),
            gameObject.AddComponent<ACHIEVEMENT_BRICK_ANNIHILATOR>(),
            gameObject.AddComponent<ACHIEVEMENT_BRICK_OBLITERATOR>(),
            gameObject.AddComponent<ACHIEVEMENT_WAVE_CLIMBER_1>(),
            gameObject.AddComponent<ACHIEVEMENT_WAVE_CLIMBER_2>(),
            gameObject.AddComponent<ACHIEVEMENT_WAVE_CLIMBER_3>(),
            gameObject.AddComponent<ACHIEVEMENT_WAVE_CLIMBER_4>(),
            gameObject.AddComponent<ACHIEVEMENT_WAVE_CLIMBER_5>(),
            gameObject.AddComponent<ACHIEVEMENT_BALL_SPAWNER>(),
            gameObject.AddComponent<ACHIEVEMENT_BALL_MACHINE>(),
            gameObject.AddComponent<ACHIEVEMENT_BALL_PRODUCTION_LINE>(),
            gameObject.AddComponent<ACHIEVEMENT_BALL_FACTORY>(),
        };

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

            UpdateInGameAchievement();
        }

        if (!PlayerPrefs.HasKey("1_Level"))
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

    public void AchievementCheck(int DestroyedBricks, int PassedWave, int ReachedWave, int SpawnedBalls, int LevelUpCount, int DestroyedBossBricks)
    {
        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case (int)InGameAchievementList.LevelUp:
                    PlayerPrefs.SetInt("1_Value", PlayerPrefs.GetInt("1_Value") + LevelUpCount);
                    break;
                case (int)InGameAchievementList.DestroyedBricks:
                    PlayerPrefs.SetInt("2_Value", PlayerPrefs.GetInt("2_Value") + DestroyedBricks);
                    break;
                case (int)InGameAchievementList.SpawnedBalls:
                    PlayerPrefs.SetInt("3_Value", PlayerPrefs.GetInt("3_Value") + SpawnedBalls);
                    break;
                case (int)InGameAchievementList.PassedWaves:
                    PlayerPrefs.SetInt("4_Value", PlayerPrefs.GetInt("4_Value") + PassedWave);
                    break;
                case (int)InGameAchievementList.DestroyedBossBricks:
                    PlayerPrefs.SetInt("5_Value", PlayerPrefs.GetInt("5_Value") + DestroyedBossBricks);
                    break;
                default:
                    break;
            }
        }
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_PINBALLBRICKS_NEWBIE>(), PassedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_PINBALLBRICKS_NOVICE>(), PassedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_PINBALLBRICKS_VETERAN>(), PassedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_PINBALLBRICKS_MASTER>(), PassedWave);

            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BRICK_BREAKER>(), DestroyedBricks);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BRICK_DESTROYER>(), DestroyedBricks);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BRICK_ANNIHILATOR>(), DestroyedBricks);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BRICK_OBLITERATOR>(), DestroyedBricks);

            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_WAVE_CLIMBER_1>(), ReachedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_WAVE_CLIMBER_2>(), ReachedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_WAVE_CLIMBER_3>(), ReachedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_WAVE_CLIMBER_4>(), ReachedWave);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_WAVE_CLIMBER_5>(), ReachedWave);

            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BALL_SPAWNER>(), SpawnedBalls);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BALL_MACHINE>(), SpawnedBalls);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BALL_PRODUCTION_LINE>(), SpawnedBalls);
            AchievementProgressing(gameObject.GetComponent<ACHIEVEMENT_BALL_FACTORY>(), SpawnedBalls);
        }
    }

    public void UpdateInGameAchievement()
    {
        AchievementName_1.text = "Level Up";
        AchievementName_2.text = "Destroyed Bricks";
        AchievementName_3.text = "Spawned Balls";
        AchievementName_4.text = "Passed Waves";
        AchievementName_5.text = "Destroyed Bricks";

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
}

public partial class AchievementManager
{
    public TMP_Text AchievementName_1;
    public TMP_Text AchievementName_2;
    public TMP_Text AchievementName_3;
    public TMP_Text AchievementName_4;
    public TMP_Text AchievementName_5;

    public TMP_Text AchievementLevel_1;
    public TMP_Text AchievementLevel_2;
    public TMP_Text AchievementLevel_3;
    public TMP_Text AchievementLevel_4;
    public TMP_Text AchievementLevel_5;

    public TMP_Text AchievementProgress_1;
    public TMP_Text AchievementProgress_2;
    public TMP_Text AchievementProgress_3;
    public TMP_Text AchievementProgress_4;
    public TMP_Text AchievementProgress_5;

    public TMP_Text AchievementReward_1;
    public TMP_Text AchievementReward_2;
    public TMP_Text AchievementReward_3;
    public TMP_Text AchievementReward_4;
    public TMP_Text AchievementReward_5;

    public Slider AchievementSlider_1;
    public Slider AchievementSlider_2;
    public Slider AchievementSlider_3;
    public Slider AchievementSlider_4;
    public Slider AchievementSlider_5;

    public void OnAchievementButton_1()
    {
        if (PlayerPrefs.GetInt("1_Value") >= PlayerPrefs.GetInt("1_Goal"))
        {
            PlayerPrefs.SetInt("1_Level", PlayerPrefs.GetInt("1_Level") + 1);
            StatManager.instance.GetGold(PlayerPrefs.GetInt("1_Reward"));
            UpgradeManager.Instance.UpdateGoldUI();
            PlayerPrefs.SetInt("1_Goal", PlayerPrefs.GetInt("1_Goal") + 3 + PlayerPrefs.GetInt("2_Level"));
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
            PlayerPrefs.SetInt("2_Goal", PlayerPrefs.GetInt("2_Goal") + 4 + PlayerPrefs.GetInt("2_Level"));
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
            PlayerPrefs.SetInt("3_Goal", PlayerPrefs.GetInt("3_Goal") + 5 + PlayerPrefs.GetInt("3_Level"));
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
            PlayerPrefs.SetInt("4_Goal", PlayerPrefs.GetInt("4_Goal") + 4 + PlayerPrefs.GetInt("4_Level"));
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
            PlayerPrefs.SetInt("5_Goal", PlayerPrefs.GetInt("5_Goal") + 3 + PlayerPrefs.GetInt("5_Level"));
            PlayerPrefs.SetInt("5_Reward", PlayerPrefs.GetInt("5_Reward") + 67);
            UpdateInGameAchievement();
        }
    }

}
