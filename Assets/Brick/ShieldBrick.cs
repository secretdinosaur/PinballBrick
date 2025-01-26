using UnityEngine;

public class ShieldBrick : IBrick
{
    public float BasicShieldCount;
    public float ShieldCount;
    private bool IsShieldDestroyed;
    public float HPMultiplier;
    private void OnEnable()
    {
        IsShieldDestroyed = false;
        ShieldCount = BasicShieldCount * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)] / 5;
        HPMultiplier = ShieldCount / BasicShieldCount;
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
                    HP = (IsBoss) ? BasicHP * HPMultiplier : BasicHP * HPMultiplier * 10;
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
