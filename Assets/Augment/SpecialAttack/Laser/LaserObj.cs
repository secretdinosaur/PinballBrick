using UnityEngine;

public class LaserObj : SpecialObj
{
    private Rigidbody2D rb;
    public float Speed;
    private int Count;
    private void OnEnable()
    {
        Count = 2;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.GetComponent<IBrick>().ArbitraryDamageAttack(1, false, "1", null);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if(Count <= 0)
            {
                //todo
            }
            Count--;
        }
    }
}
