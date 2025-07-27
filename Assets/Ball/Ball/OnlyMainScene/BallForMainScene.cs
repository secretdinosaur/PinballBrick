using UnityEngine;

public class BallForMainScene : MonoBehaviour
{
    private void Update()
    {
        if(transform.position.y < -10)
        {
            Background.Instance.TakeBall(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
