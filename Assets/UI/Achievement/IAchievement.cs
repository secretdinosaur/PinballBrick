using UnityEngine;

public abstract class IAchievement : MonoBehaviour
{
    public string AchievementID;
    public string AchievementName;
    public string AchievementDescription;
    protected void SetAchievement(string AchievementID, string AchievementName, string AchievementDescription)
    {
        this.AchievementID = AchievementID;
        this.AchievementName = AchievementName;
        this.AchievementDescription = AchievementDescription;
    }
    public virtual void Rewards() { }
    public virtual void Progress(int Progress) { }
}
