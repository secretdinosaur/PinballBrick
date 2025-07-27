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
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            TakeDamage(other.gameObject.GetComponent<IBall>());
        }
    }
}
