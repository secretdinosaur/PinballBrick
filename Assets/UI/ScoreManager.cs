using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int TotalScore {  get; set; }
    public int ContinuousScore { get; set; }

    public int KilledEnemy {  get; set; }

    public TMP_Text TotalText;
    public TMP_Text ContinuousText;

    void Start()
    {
        Instance = this;
        TotalScore = 0;
        ContinuousScore = 0;
    }
    void Update()
    {
        TotalText.text = TotalScore.ToString();
        ContinuousText.text = ContinuousScore.ToString();
    }
}
