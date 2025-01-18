using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Brick : MonoBehaviour
{
    public float BasicHP;
    private float hp;

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
            HP -= collision.gameObject.GetComponent<Ball>().Power;
            
            Fever.StartFever = true;
            Fever.Instance.FeverCount *= 2;
            Fever.Instance.FeverGauge = 100;
            ScoreManager.Instance.ContinuousScore++;

            Level.Instance.Exp += 10f;
        }
    }
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPText = GetComponentInChildren<TextMeshPro>();
        HPText.text = HP.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
