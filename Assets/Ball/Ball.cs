using UnityEngine;
using System.Collections;
public class Ball : MonoBehaviour
{
    private float power = 5;
    public float Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GetComponent<Collider2D>().enabled = false;
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
