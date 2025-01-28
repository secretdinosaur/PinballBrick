using UnityEngine;

public class BlackBrick : IBrick
{
    public int RebirthCount = 3;
    public BlackAndWhiteBrick BlackAndWhiteBrick;
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
            Debug.Log("Active 2");
            TakeDamage(collision.gameObject.GetComponent<Ball>());
            BlackAndWhiteBrick.TouchedBlackHole(collision);
        }
    }
}