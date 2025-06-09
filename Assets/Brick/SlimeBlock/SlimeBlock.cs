using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SlimeBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "SlimeBrick";
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
