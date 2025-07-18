using UnityEngine;

public class RebornBrick : IBrick
{
    int RebornCount = 2;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "RebornBrick";
        RebornCount = 2;
    }
    public void RebornProcess(System.Action<int> callback)
    {
        if(RebornCount >= 1)
        {
            Fever.StartFever = true;
            Fever.Instance.CalFeverCount();
            Fever.Instance.FeverGauge = 100;
            ScoreManager.Instance.ContinuousScore += Point;
            transform.localScale = new Vector3(transform.localScale.x * ((RebornCount + 0.6f) / 3f), transform.localScale.y * ((RebornCount + 0.6f) / 3f));
            BrickManager.Instance.TeleportBrickPosition(this);
            RebornCount--;
            if (callback != null)
            {
                callback.Invoke(RebornCount);
            }
        }
        else
        {
            BrickDie();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            TakeDamage(other.gameObject.GetComponent<IBall>());
        }
    }
}
