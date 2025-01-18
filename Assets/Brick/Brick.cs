using UnityEngine;

public class Brick : MonoBehaviour
{
    private float hp = 5;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;

            if(hp <= 0)
            {
                //destroy brick
                BrickManager.Instance.TakeBrick(this);
            }
        }
    }

    private void OnEnable()
    {
        HP = 5;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HP -= collision.gameObject.GetComponent<Ball>().Power;
            Debug.Log("Activate");
            Fever.StartFever = true;
            Fever.Instance.FeverCount *= 2;
            Fever.Instance.FeverGauge = 100;
            ScoreManager.Instance.ContinuousScore++;
        }
    }
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
