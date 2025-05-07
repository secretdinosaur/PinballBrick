using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GoldBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "GoldBrick";
        StartCoroutine(TimeSet());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
            StatManager.instance.GetGold(5 + GameManager.instance.Wave / 2);
        }
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(20f);
        BrickManager.Instance.TakeBrick(this);
        Debug.Log("MiniBoss has gone");
    }
}
