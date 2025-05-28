using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TwinBrick : IBrick
{
    public TwinBrick Twin = null;
    public bool IsHpZero;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "TwinBrick";
        IsHpZero = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && !IsHpZero)
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    public IEnumerator StartRebirth()
    {
        GetComponent<Collider2D>().enabled = false;
        IsHpZero = true;
        float Timer = 10;
        HPText.color = Color.blue;

        while (Timer > 0)
        {
            if (Twin.IsHpZero || !Twin.enabled || Twin == null)
            {
                IsHpZero = false;
                Twin.IsHpZero = false;
                GetComponent<Collider2D>().enabled = true;

                if(Twin.enabled)
                {
                    Twin.IsHpZero = false;
                    Twin.Twin = null;
                    Twin.BrickDie();
                }
                Twin = null;
                IsHpZero = false;
                BrickDie();
            }
            HPText.text = Timer.ToString();
            HPText.enabled = true;
            yield return new WaitForSeconds(1);
            Timer -= 1;
        }
        HP = Twin.HP;
        GetComponent<Collider2D>().enabled = true;

        HPText.color = IsBoss ? Color.red : Color.white;
        IsHpZero = false;
    }
}
