using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ExpBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "ExpBrick";
        StartCoroutine(TimeSet());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
            LevelManager.Instance.GetExp(10f + GameManager.instance.Wave / 2);
        }
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(10f);
        BrickManager.Instance.TakeBrick(this);
        Debug.Log("MiniBoss has gone");
    }
}
