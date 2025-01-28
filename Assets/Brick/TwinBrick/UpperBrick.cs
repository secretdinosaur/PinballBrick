using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpperBrick : IBrick
{
    private void OnEnable()
    {
        HP = BasicHP * BrickManager.Instance.HPPool[0];
        BrickType = "TwinBrick";
        GetComponents();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && HP > 0)
        {
            TakeDamage(collision.gameObject.GetComponent<Ball>());
        }
    }
}
