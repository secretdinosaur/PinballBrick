using UnityEngine;

[System.Serializable]
public class Quest
{
    public string QuestId;
    public string QuestName;
    public float QuestGoal;
    public float QuestProgressValue;
    public float QuestRewards;
    public bool isCleared;
}
