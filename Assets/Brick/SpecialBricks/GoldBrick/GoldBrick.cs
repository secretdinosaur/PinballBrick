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
            int i = Random.Range(0, 3);
            switch(i)
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
        }
    }

    IEnumerator TimeSet()
    {
        yield return new WaitForSeconds(20f);
        BrickManager.Instance.TakeBrick(this);
        Debug.Log("MiniBoss has gone");
    }
}
