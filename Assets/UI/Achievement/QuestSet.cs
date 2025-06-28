using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSet", menuName = "Quests/QuestSet")]
public class QuestSet: ScriptableObject
{
    public List<Quest> allQuests;
}