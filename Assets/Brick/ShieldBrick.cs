using UnityEngine;

public class ShieldBrick : IBrick
{
    public int ShieldCount;
    private bool IsShieldDestroyed = false;
    private void OnEnable()
    {
        HP = ShieldCount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            if (!IsShieldDestroyed)
            {
                ShieldCount -= 1;
                if (ShieldCount == 0)
                {
                    HP = BasicHP * BrickManager.Instance.BrickStage;
                    IsShieldDestroyed = true;
                }
                else
                {
                    HP = ShieldCount;
                }
            }
            else
            {
                TakeDamage(collision.gameObject.GetComponent<Ball>());
            }
            
        }
    }
}
