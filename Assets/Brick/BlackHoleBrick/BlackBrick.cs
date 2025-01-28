using UnityEngine;

public class BlackBrick : IBrick
{
    private void OnEnable()
    {
        BrickType = "BlackBrick";
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