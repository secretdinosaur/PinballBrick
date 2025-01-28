using UnityEngine;

public class ShieldBrick : IBrick
{
    public float BasicShieldCount;
    public float ShieldCount;
    public bool IsShieldDestroyed;
    public float HPMultiplier;
    public bool FirstShieldSpawn = true;
    private void OnEnable()
    {
        BrickType = "ShieldBrick";

        IsShieldDestroyed = false;
        ShieldCount = BasicShieldCount * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)] / 5;
        HPMultiplier = ShieldCount / BasicShieldCount;
        HP = ShieldCount;

        GetComponents();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }

    public void DestroyShield()
    {
        IsShieldDestroyed = true;
        HP = (IsBoss) ? BasicHP * HPMultiplier : BasicHP * HPMultiplier * 10;
    }
}
