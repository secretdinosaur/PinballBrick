using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Brick : IBrick
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}
