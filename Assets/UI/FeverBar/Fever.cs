using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour
{
    public static Fever Instance;
    public static bool StartFever;

    private TMP_Text MultipleText;
    private Slider FeverBar;
    private GameObject FeverObj;
    private static float feverGauge;
    private static int feverCount;
    private float FeverGaugeLossValue;

    public float FeverGauge
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
    public int FeverCount
    {
        get
        {
            return feverCount;
        }
        set
        {
            FeverGaugeLossValue += 0.1f;
            feverCount = Mathf.Clamp(value, 1, 1024);
        }
    }
    void Awake()
    {
        FeverObj = GameObject.Find("Canvas").transform.Find("Fever").gameObject;
        FeverBar = FeverObj.GetComponent<Slider>();
        MultipleText = FeverObj.transform.Find("MultipleText").GetComponent<TMP_Text>();
    }
    void Start()
    {
        Instance = this;
        FeverGauge = 0;
        FeverCount = 1;
        FeverGaugeLossValue = 0.1f;
        StartFever = false;
    }

    void Update()
    {
        ActiveFever();
        MultipleText.text = "x" + feverCount.ToString();

        if (FeverObj.activeSelf && Time.timeScale != 0)
        {
            if (FeverGauge > 0)
            {
                FeverBar.value = FeverGauge;
                FeverGauge -= FeverGaugeLossValue;
            }
            else if ((int)FeverGauge == 0 && ScoreManager.Instance.ContinuousScore != 0)
            {
                ScoreManager.Instance.TotalScore += ScoreManager.Instance.ContinuousScore * FeverCount;
                ScoreManager.Instance.ContinuousScore = 0;
                FeverCount = 1;
                FeverGaugeLossValue = 0.1f;
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
