using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpperBrick : IBrick
{
    private void OnEnable()
    {
        HP = BasicHP * BrickManager.Instance.HPPool[0];
        BrickType = "TwinBrick";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}
