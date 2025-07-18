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
            feverCount = Mathf.Clamp(value, 0, 16);
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
        FeverCount = 0;
        FeverGaugeLossValue = 0.1f;
        StartFever = false;
    }

    void Update()
    {
        ActiveFever();
        MultipleText.text = "x" + feverCount.ToString();
        MultipleText.fontSize = Mathf.Clamp(MultipleText.fontSize, 70f, 90f);

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
                FeverCount = 0;
                FeverGaugeLossValue = 0.1f;
                MultipleText.fontSize = 70f;
                StartFever = false;
            }
        }
    }

    public void CalFeverCount()
    {
        FeverCount +=
            FeverCount < 4 ? 1 :
            FeverCount < 8 ? 2 :
            FeverCount < 16 ? 4 :
            1;
        MultipleText.fontSize += 2.5f;
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

    public void FeverOff()
    {
        ScoreManager.Instance.TotalScore += ScoreManager.Instance.ContinuousScore * FeverCount;
        ScoreManager.Instance.ContinuousScore = 0;
        FeverGaugeLossValue = 0.1f;
        MultipleText.fontSize = 70f;
        FeverGauge = 0;
        FeverCount = 0;
        StartFever = false;
    }
}
