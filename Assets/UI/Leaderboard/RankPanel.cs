using TMPro;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    public int userRank;
    public string userName;
    public long userScore;

    private TMP_Text Rank;
    private TMP_Text Name;
    private TMP_Text Score;

    private void Awake()
    {
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Score = transform.Find("Score").GetComponent<TMP_Text>();
    }

    public void ChangeInformation(int Rank, string Name, long Score)
    {
        if (Rank == 1)
        {
            this.Rank.text = Rank.ToString("F0") + "st";
        }
        else if (Rank == 2)
        {
            this.Rank.text = Rank.ToString("F0") + "nd";
        }
        else if (Rank == 3)
        {
            this.Rank.text = Rank.ToString("F0") + "rd";
        }
        else
        {
            this.Rank.text = Rank.ToString("F0") + "th";
        }
        this.Name.text = Name;
        this.Score.text = Score.ToString("F0");

        userRank = Rank;
        userName = Name;
        userScore = Score;
    }
}
