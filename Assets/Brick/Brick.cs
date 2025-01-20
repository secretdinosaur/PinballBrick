using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Brick : MonoBehaviour
{
    public int Point;
    public float BasicHP;
    private float hp;
    [SerializeField]
    private Stat stat;

    public TextMeshPro HPText;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (HPText != null)
            {
                HPText.text = hp.ToString();
            }
            else
            {
                HPText = GetComponentInChildren<TextMeshPro>();
                HPText.text = hp.ToString();
            }

            if (hp <= 0)
            {
                BrickManager.Instance.TakeBrick(this);
                Fever.StartFever = true;
                Fever.Instance.FeverCount *= 2;
                Fever.Instance.FeverGauge = 100;
                ScoreManager.Instance.ContinuousScore += Point;
                Level.Instance.Exp += 10 * stat.BonusExp;
            }
        }
    }

    private void OnEnable()
    {
        HP = BasicHP * BrickManager.Instance.BrickStage;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Random.value < stat.CriticalChance)
            {
                HP -= (stat.AttackDamage * stat.CriticalMultiplier);
            }
            else
            {
                HP -= stat.AttackDamage;
            }
        }
    }
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        HPText = GetComponentInChildren<TextMeshPro>();
        stat = StatManager.instance.LoadStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
