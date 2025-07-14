using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RankPanel : MonoBehaviour
{
    public int userRank = 1;
    public string userName = "user";
    public long userScore = 0;

    private TMP_Text Rank;
    private TMP_Text Name;
    private TMP_Text Score;

    private void Awake()
    {
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Rank = transform.Find("Rank").GetComponent<TMP_Text>();
        Score = transform.Find("Score").GetComponent<TMP_Text>();
    }

    public void ChangeInformation(int Rank, string Name, long Score)
    {
        if (Rank == 1)
        {
            this.Rank.text = Rank.ToString("F0") + "st";
            GetComponent<Image>().color = new Color(253 / 255, 30 / 255, 0);
        }
        else if (Rank == 2)
        {
            this.Rank.text = Rank.ToString("F0") + "nd";
            GetComponent<Image>().color = new Color(1, 144 / 255f, 0);
        }
        else if (Rank == 3)
        {
            this.Rank.text = Rank.ToString("F0") + "rd";
            GetComponent<Image>().color = new Color(77 / 255f, 1, 0);
        }
        else
        {
            this.Rank.text = Rank.ToString("F0") + "th";
            GetComponent<Image>().color = new Color(136 / 255, 153 / 255f, 212 / 255f);
        }
        this.Name.text = Name;
        this.Score.text = Score.ToString("F0");

        userRank = Rank;
        userName = Name;
        userScore = Score;
    }
}
