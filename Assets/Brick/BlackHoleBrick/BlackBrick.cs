using UnityEngine;

public class BlackBrick : IBrick
{
    public WhiteBrick WhiteBrick;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "BlackBrick";
        GetComponents();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            WhiteBrick.TouchedBlackHole(collision.gameObject.GetComponent<Ball>());
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}