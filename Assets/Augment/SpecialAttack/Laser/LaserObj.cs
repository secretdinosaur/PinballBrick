using System.Collections;
using UnityEngine;

public class LaserObj : SpecialObj
{
    private Rigidbody2D rb;
    public float Speed;
    private int Count;
    private void OnEnable()
    {
        SpecialObjType = "LaserObj";
        transform.position = GetParentBall().transform.position;
        Count = 3;
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.AddForce(randomDirection * 9f, ForceMode2D.Impulse);
        StartCoroutine(TimeSetting());
    }

    IEnumerator TimeSetting()
    {
        yield return new WaitForSeconds(3f);
        if (gameObject.activeInHierarchy)
        {
            SpecialObjManager.Instance.TakeSpecialObj(this);
        }
    }

    private IBall GetParentBall()
    {
        int n = Random.Range(0, BallManager.Instance.InGameBall.Count);
        return BallManager.Instance.InGameBall[n];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.GetComponent<IBrick>().ArbitraryDamageAttack(1, false, "1", null);
            SpecialObjManager.Instance.TakeSpecialObj(this);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(rb.linearVelocity, normal);

            Vector2 randomDirection = new Vector2(
                Random.Range(-45f, 45f),
                Random.Range(-45f, 45f)
            );
            rb.linearVelocity = (reflectedVelocity + randomDirection).normalized * 9f;

            if (Count <= 0)
            {
                SpecialObjManager.Instance.TakeSpecialObj(this);
            }
            Count--;
        }
    }
}
