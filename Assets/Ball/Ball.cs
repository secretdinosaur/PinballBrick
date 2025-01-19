using UnityEngine;
using System.Collections;
public class Ball : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GetComponent<Collider2D>().enabled = false;
        transform.position = new Vector2(0, -5);
        gameObject.GetComponent<Rigidbody2D>().linearVelocityY = 10;
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = Random.Range(-5f, 5f);
        StartCoroutine(EnableCollider());
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -10)
        {
            BallManager.Instance.TakeBall(this);
        }
    }
}
