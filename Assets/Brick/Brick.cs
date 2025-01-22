using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Brick : MonoBehaviour
{
    public int Point;
    public float BasicHP;
    private float hp;
    [SerializeField]
    private Persistent_Stat Persistent_Stat;

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
                LevelManager.Instance.Exp += 10 * Persistent_Stat.BonusExp;
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
            if (Random.value < (Persistent_Stat.CriticalChance != 0.0f 
                    ? Persistent_Stat.CriticalChance : 1) * StatManager.instance.Temporary_stat.CriticalChance)
            {
                HP -= (Persistent_Stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage) 
                       * (Persistent_Stat.CriticalDamage * StatManager.instance.Temporary_stat.CriticalDamage);
            }
            else
            {
                HP -= Persistent_Stat.AttackDamage + StatManager.instance.Temporary_stat.AttackDamage;
            }
        }
    }
    
    public void Start()
    {
        HPText = GetComponentInChildren<TextMeshPro>();
        Persistent_Stat = StatManager.instance.LoadPersistentStats();
    }
}
