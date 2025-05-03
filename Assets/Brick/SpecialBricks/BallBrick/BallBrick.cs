using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BallBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "BallBrick";
        StartCoroutine(TimeSet());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
            BallManager.Instance.CreateBall(1);
        }
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(20f);
        BrickManager.Instance.TakeBrick(this);
        Debug.Log("MiniBoss has gone");
    }
}
