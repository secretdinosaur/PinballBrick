using UnityEngine;

public interface IAchievement
{
    string AchievementID { get; }
    public abstract void Progress(int Progress);
}

public abstract class GoogleAchievement : IAchievement
{
    public abstract string AchievementID { get; }
    public abstract void Progress(int Progress);
}
