using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BallBrick : IBrick
{
    CircleCollider2D[] Balls;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "BallBrick";
        StartCoroutine(TimeSet());

        Balls = GetComponentsInChildren<CircleCollider2D>(true);
        for(int i = 0; i < HP; i++)
        {
            Balls[i].gameObject.SetActive(true);
            Balls[i].transform.localPosition = new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(-0.125f, 0.125f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
            var activeBalls = System.Array.FindAll(Balls, b => b.gameObject.activeSelf);
            int index = Random.Range(0, activeBalls.Length);
            activeBalls[index].gameObject.SetActive(false);
            for (int i = 0; i < Balls.Length; i++)
            {
                Balls[i].GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            }
            BallManager.Instance.CreateBall(1);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].gameObject.SetActive(false);
        }
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(20f);
        BrickManager.Instance.TakeBrick(this);
    }
}
