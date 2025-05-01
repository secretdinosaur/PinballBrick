using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ExpBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "ExpBrick";
    }

    public override void BrickDie()
    {
        LevelManager.Instance.GetExp(100f + GameManager.instance.Wave / 2);
        base.BrickDie();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
            LevelManager.Instance.GetExp(10f + GameManager.instance.Wave / 2);
        }
    }
}
