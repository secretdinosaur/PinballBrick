using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class HealBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "HealBrick";
        BrickHealCoroutine = StartCoroutine(BrickHeal());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    public IEnumerator BrickHeal()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            if(HP + HpTemp / 10 > HpTemp && HP != HpTemp)
            {
                HP = HpTemp;
                ShowText("+" + (HpTemp - HP).ToString(), "Heal", false);
            }
            else if (HP != HpTemp)
            {
                HP += (int)(HpTemp / 10);
                ShowText("+" + ((int)(HpTemp / 10)).ToString(), "Heal", false);
            }
        }
    }
}
