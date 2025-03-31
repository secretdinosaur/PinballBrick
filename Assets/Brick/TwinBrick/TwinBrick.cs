using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TwinBrick : IBrick
{
    bool IsWaitingHeal = false;
    public LowerBrick LowerBrick;
    private void OnEnable()
    {
        base.OnEnable();
        HP = BasicHP * BrickManager.Instance.HPPool[0];
        BrickType = "TwinBrick";
        GetComponents();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && HP > 0)
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    public void CheckBothBrickDead()
    {
        if (HP <= 0 && LowerBrick.HP <= 0)
        {
            IsWaitingHeal = false;
            BrickDie();
        }
        else if (HP <= 0 || LowerBrick.HP <= 0)
        {
            if (!IsWaitingHeal)
            {
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(HealAfter10s());

                }
            }
        }
    }

    public IEnumerator HealAfter10s()
    {
        IsWaitingHeal = true;
        yield return new WaitForSeconds(10);

        if (HP < LowerBrick.HP) HP = LowerBrick.HP;
        else LowerBrick.HP = HP;
        IsWaitingHeal = false;
    }
}
