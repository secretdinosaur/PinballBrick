using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GoldBrick : IBrick
{
    public CircleCollider2D[] Balls;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "GoldBrick";
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
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    ItemManager.Instance.SpawnSpecificItem("Gold1", transform.position, 1);
                    break;
                case 1:
                    ItemManager.Instance.SpawnSpecificItem("Gold2", transform.position, 1);
                    break;
                case 2:
                    ItemManager.Instance.SpawnSpecificItem("Gold3", transform.position, 1);
                    break;
            }

            var activeBalls = System.Array.FindAll(Balls, b => b.gameObject.activeSelf);
            int index = Random.Range(0, activeBalls.Length);
            activeBalls[index].gameObject.SetActive(false);
            for (int j = 0; j < Balls.Length; j++)
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
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    ItemManager.Instance.SpawnSpecificItem("Gold1", transform.position, 1);
                    break;
                case 1:
                    ItemManager.Instance.SpawnSpecificItem("Gold2", transform.position, 1);
                    break;
                case 2:
                    ItemManager.Instance.SpawnSpecificItem("Gold3", transform.position, 1);
                    break;
            }

            var activeBalls = System.Array.FindAll(Balls, b => b.gameObject.activeSelf);
            int index = Random.Range(0, activeBalls.Length);
            activeBalls[index].gameObject.SetActive(false);
            for (int j = 0; j < Balls.Length; j++)
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
