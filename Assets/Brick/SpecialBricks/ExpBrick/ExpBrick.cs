using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ExpBrick : IBrick
{
    CircleCollider2D[] Balls;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "ExpBrick";
        StartCoroutine(TimeSet());

        Balls = GetComponentsInChildren<CircleCollider2D>(true);
        for (int i = 0; i < HP; i++)
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
            ItemManager.Instance.SpawnSpecificItem("ExpBead", transform.position, 1);
            var activeBalls = System.Array.FindAll(Balls, b => b.gameObject.activeSelf);
            int index = Random.Range(0, activeBalls.Length);
            activeBalls[index].gameObject.SetActive(false);
            for (int i = 0; i < Balls.Length; i++)
            {
                Balls[i].GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            TakeDamage(other.gameObject.GetComponent<IBall>());
            ItemManager.Instance.SpawnSpecificItem("ExpBead", transform.position, 1);
            var activeBalls = System.Array.FindAll(Balls, b => b.gameObject.activeSelf);
            int index = Random.Range(0, activeBalls.Length);
            activeBalls[index].gameObject.SetActive(false);
            for (int i = 0; i < Balls.Length; i++)
            {
                Balls[i].GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            }
        }
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(20f);
        BrickManager.Instance.TakeBrick(this);
    }
}
