using UnityEngine;

public class HardBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "HardBrick";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}
