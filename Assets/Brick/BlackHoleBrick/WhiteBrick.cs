using UnityEngine;

public class WhiteBrick : IBrick
{
    private void OnEnable()
    {
        BrickType = "WhiteBrick";
        GetComponents();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}