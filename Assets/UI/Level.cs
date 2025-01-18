using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public static Level Instance;
    [SerializeField]
    private Slider LevelBar;

    public int Lv {  get; set; }
    private float exp;
    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            if (exp >= LevelBar.maxValue)
            {
                LevelUp();
            }
        }
    }
    void Awake()
    {
        LevelBar = GameObject.Find("Canvas").transform.Find("Level").GetComponent<Slider>();
    }
    void Start()
    {
        Instance = this;
        Exp = 0;
        Lv = 1;
    }
    void Update()
    {
       LevelBar.value = Exp;
    }

    private void LevelUp()
    {
        Exp -= LevelBar.maxValue;
        Lv++;
        LevelBar.maxValue *= 1.5f;

        Debug.Log("Now Level : " + Lv);
    }
}
