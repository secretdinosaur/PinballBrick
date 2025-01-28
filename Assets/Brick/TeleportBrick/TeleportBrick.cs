using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TeleportBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "TeleportBrick";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}
