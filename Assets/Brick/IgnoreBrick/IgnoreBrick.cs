using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IgnoreBrick : IBrick
{
    private bool Ignoring = false;
    public float IgnoreTime;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "IgnoreBrick";
        Ignoring = false;
        StopCoroutine(IgnoreDamage());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && !Ignoring)
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
            StartCoroutine(IgnoreDamage());
        }
    }

    private IEnumerator IgnoreDamage()
    {
        Ignoring = true;
        yield return new WaitForSeconds(IgnoreTime);
        Ignoring = false;
    }
}
