using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AbsorbBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "AbsorbBrick";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    public void absorbHP(IBrick Brick)
    {
        HP += Brick.HpTemp / 10;
    }
}
