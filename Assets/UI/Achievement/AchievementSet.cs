using GooglePlayGames;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ACHIEVEMENT_LEVEL_UP : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_level_up;

    public override void Progress(int Progress)
    {
        if(Progress >= 1)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_AUGMENTATION: GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_augmentation;

    public override void Progress(int Progress)
    {
        if (Progress >= 1)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_EUREKA : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_eureka;

    public override void Progress(int Progress)
    {
        if (Progress >= 1)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_GIANT_SLAYER: GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_giant_slayer;

    public override void Progress(int Progress)
    {
        if (Progress >= 1)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_NEWBIE : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_pinballbricks_newbie;

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, null);
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_NOVICE : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_pinballbricks_novice;

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, null);
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_VETERAN : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_pinballbricks_veteran;
    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, null);
    }
}

public class ACHIEVEMENT_PINBALLBRICKS_MASTER : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_pinballbricks_master;

    public override void Progress(int Progress)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, Progress, null);
    }
}

public class ACHIEVEMENT_BRICK_BREAKER: GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_brick_breaker;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 4;
        if(progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_BRICK_DESTROYER : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_brick_destroyer;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 4;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_BRICK_ANNIHILATOR : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_brick_annihilator;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 4;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_BRICK_OBLITERATOR : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_brick_obliterator;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 4;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_1 : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_wave_climber_1;
    public override void Progress(int Progress)
    {
        if(Progress >= 20)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_2 : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_wave_climber_2;
    public override void Progress(int Progress)
    {
        if (Progress >= 40)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_3 : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_wave_climber_3;
    public override void Progress(int Progress)
    {
        if (Progress >= 60)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_4 : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_wave_climber_4;
    public override void Progress(int Progress)
    {
        if (Progress >= 80)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_WAVE_CLIMBER_5 : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_wave_climber_5;
    public override void Progress(int Progress)
    {
        if (Progress >= 100)
        {
            PlayGamesPlatform.Instance.ReportProgress(AchievementID, 100.0f, null);
        }
    }
}

public class ACHIEVEMENT_BALL_SPAWNER : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_ball_spawner;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 10;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_BALL_MACHINE : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_ball_machine;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 10;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_BALL_PRODUCTION_LINE : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_ball_production_line;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 10;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}

public class ACHIEVEMENT_BALL_FACTORY : GoogleAchievement
{
    public override string AchievementID => GPGSIds.achievement_ball_factory;
    private float progressBuffer = 0f;
    public override void Progress(int Progress)
    {
        int x;
        progressBuffer += Progress / 10;
        if (progressBuffer >= 1f)
        {
            x = (int)progressBuffer;
            progressBuffer -= x;
            PlayGamesPlatform.Instance.IncrementAchievement(AchievementID, x, null);
        }
    }
}