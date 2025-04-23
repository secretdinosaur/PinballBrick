using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class QuestAlarm : MonoBehaviour
{
    public static QuestAlarm Instance;
    public TextMeshProUGUI[] Quests;

    private void Awake()
    {
        Instance = this;
        Quests = GetComponentsInChildren<TextMeshProUGUI>();

    }

    public void ClearQuest()
    {
        Quests[0].GetComponent<QuestText>().QuestName = null;
        Quests[1].GetComponent<QuestText>().QuestName = null;
        Quests[2].GetComponent<QuestText>().QuestName = null;
        Quests[3].GetComponent<QuestText>().QuestName = null;
        Quests[4].GetComponent<QuestText>().QuestName = null;
        Quests[5].GetComponent<QuestText>().QuestName = null;

        Quests[0].text = "";
        Quests[1].text = "";
        Quests[2].text = "";
        Quests[3].text = "";
        Quests[4].text = "";
        Quests[5].text = "";

    }
    public void SetQuest(string QuestName, string Text, int CurrentCount, int MaxCount)
    {

        if (CurrentCount == MaxCount)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Quests[i].GetComponent<QuestText>().QuestName == QuestName)
                {
                    Quests[i].text = "";
                    Quests[i].GetComponent<QuestText>().QuestName = null;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (Quests[i].text == "")
                {
                    Quests[i].text = Quests[i + 1].text;
                    Quests[i + 1].text = "";
                    Quests[i].GetComponent<QuestText>().QuestName = Quests[i+1].GetComponent<QuestText>().QuestName;
                }
            }
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            if (Quests[i].GetComponent<QuestText>().QuestName != null && Quests[i].GetComponent<QuestText>().QuestName == QuestName)    
            {
                Quests[i].text = MaxCount == 0 ? QuestName + ": " + Text + " " + CurrentCount : QuestName + ": " + Text + " " + CurrentCount + " / " + MaxCount;
                return;
            }
            
        }
        for(int i = 0; i < 6; i++)
        {
            if (Quests[i].text == "")
            {
                Quests[i].text = MaxCount == 0 ? QuestName + ": " + Text + " " + CurrentCount : QuestName + ": " + Text + " " + CurrentCount + " / " + MaxCount;
                Quests[i].GetComponent<QuestText>().QuestName = QuestName;
                return;
            }
        }
    }
}
