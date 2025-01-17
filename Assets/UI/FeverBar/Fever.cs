using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour
{
    public static bool StartFever;
    [SerializeField]
    private TMP_Text MultipleText;
    [SerializeField]
    private Slider FeverBar;
    [SerializeField]
    private GameObject FeverObj;

    private static float feverGauge;
    private static int feverCount;

    public static float FeverGauge
    {
        get
        {
            return feverGauge;
        }
        set
        {
            feverGauge = Mathf.Max(0, value);
        }
    }
    public static int FeverCount
    {
        get
        {
            return feverCount;
        }
        set
        {
            feverCount = Mathf.Clamp(value, 1, 1024);
        }
    }
    void Start()
    {
        FeverObj = GameObject.Find("Canvas").transform.Find("Fever").gameObject;
        FeverGauge = 0;
        FeverCount = 1;
        FeverBar = FeverObj.GetComponent<Slider>();
        MultipleText = FeverObj.transform.Find("MultipleText").GetComponent<TMP_Text>();
        StartFever = false;
    }

    void Update()
    {
        ActiveFever();
        MultipleText.text = "x" + feverCount.ToString();

        if (FeverObj.activeSelf)
        {
            if (FeverGauge > 0)
            {
                FeverBar.value = FeverGauge;
                FeverGauge -= 0.1f;
            }
            else if ((int)FeverGauge == 0 && ScoreManager.Instance.ContinuousScore != 0)
            {
                ScoreManager.Instance.TotalScore += ScoreManager.Instance.ContinuousScore * FeverCount;
                ScoreManager.Instance.ContinuousScore = 0;
                FeverCount = 1;
                StartFever = false;
            }
        }

    }
    public void ActiveFever()
    {
        if (StartFever)
        {
            FeverObj.SetActive(true);
        }
        else
        {
            FeverObj.SetActive(false);
        }
    }
}
