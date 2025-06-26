using GooglePlayGames;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ACHIEVEMENT_LEVEL_UP : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_level_up, "Level Up!", "Level up for the first time");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_up, 1, (achieve) =>
            {
                PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                    foreach (var ach in achievements)
                    {
                        if (ach.id == AchievementID && ach.completed)
                        {
                            Rewards();
                        }
                    }
                });
            });
        }
        else
        {

        }
    }
}

public class ACHIEVEMENT_AUGMENTATION: IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_augmentation, "Augmentation!", "Choose augmentation for the first time");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_augmentation, 1, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_EUREKA : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_eureka, "Eureka!", "Obtain the rarest augmentation");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(1000);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_eureka, 1, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_GIANT_SLAYER: IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_giant_slayer, "Giant Slayer", "Defeat the boss for the first time");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_giant_slayer, 1, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_NEWBIE : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_pinballbricks_newbie, "Pinballbricks Newbie", "Passed 10 Waves");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_NOVICE : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_pinballbricks_novice, "Pinballbricks Novice", "Passed 100 Waves");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(500);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_VETERAN : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_pinballbricks_veteran, "Pinballbricks Veteran", "Passed 1,000 Waves");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(1000);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_MASTER : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_pinballbricks_master, "Pinballbricks Master", "Passed 5,000 Waves");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(3000);
    }

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BRICK_BREAKER: IAchievement
{
    private int Count;
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_brick_breaker, "Brick Breaker", "Destroy 500 bricks");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 4 <= 0 ? Progress : Progress / 4;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 4, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BRICK_DESTROYER : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_brick_destroyer, "Brick Destroyer", "Destroy 2,000 bricks");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(500);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 4 <= 0 ? Progress : Progress / 4;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 4, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BRICK_ANNIHILATOR : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_brick_annihilator, "Brick Annihilator", "Destroy 10,000 bricks");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(2000);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 4 <= 0 ? Progress : Progress / 4;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 4, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BRICK_OBLITERATOR : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_brick_obliterator, "Brick Obliterator", "Destroy 40,000 bricks");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(6000);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 4 <= 0 ? Progress : Progress / 4;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 4, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_1 : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_wave_climber_1, "Wave Climber 1", "Reaches Wave 20");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        float x = (Progress / 20) * 100;
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_wave_climber_1, x, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_2 : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_wave_climber_2, "Wave Climber 2", "Reaches Wave 40");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(400);
    }

    public override void Progress(int Progress)
    {
        float x = (Progress / 40) * 100;
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_wave_climber_2, x, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_3 : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_wave_climber_3, "Wave Climber 3", "Reaches Wave 60");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(600);
    }

    public override void Progress(int Progress)
    {
        float x = (Progress / 60) * 100;
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_wave_climber_3, x, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_4 : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_wave_climber_4, "Wave Climber 4", "Reaches Wave 80");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(800);
    }

    public override void Progress(int Progress)
    {
        float x = (Progress / 80) * 100;
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_wave_climber_4, x, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_5 : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_wave_climber_5, "Wave Climber 5", "Reaches Wave 100");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(1000);
    }

    public override void Progress(int Progress)
    {
        float x = (Progress / 100) * 100;
        PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_wave_climber_5, x, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BALL_SPAWNER : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_ball_spawner, "Ball Spawner", "Spawn 100 Balls");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(200);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 10 <= 0 ? Progress : Progress / 10;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 10, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BALL_MACHINE : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_ball_machine, "Ball Machine", "Spawn 1,000 Balls");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(500);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 10 <= 0 ? Progress : Progress / 10;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 10, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BALL_PRODUCTION_LINE : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_ball_production_line, "Ball Production Line", "Spawn 10,000 Balls");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(2000);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 10 <= 0 ? Progress : Progress / 10;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 10, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}

public class ACHIEVEMENT_BALL_FACTORY : IAchievement
{
    public void OnEnable()
    {
        SetAchievement(GPGSIds.achievement_ball_factory, "Ball Factory", "Spawn 100,000 Balls");
    }

    public override void Rewards()
    {
        StatManager.instance.GetGold(6000);
    }

    public override void Progress(int Progress)
    {
        Progress = Progress / 10 <= 0 ? Progress : Progress / 10;
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress / 10, (achieve) =>
        {
            PlayGamesPlatform.Instance.LoadAchievements((achievements) => {
                foreach (var ach in achievements)
                {
                    if (ach.id == AchievementID && ach.completed)
                    {
                        Rewards();
                    }
                }
            });
        });
    }
}