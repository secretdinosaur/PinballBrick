using UnityEngine;

public class BlackBrick : IBrick
{
    public WhiteBrick WhiteBrick;
    private void OnEnable()
    {
        BrickType = "BlackBrick";
        GetComponents();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            WhiteBrick.TouchedBlackHole(collision.gameObject.GetComponent<IBall>());
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }
}