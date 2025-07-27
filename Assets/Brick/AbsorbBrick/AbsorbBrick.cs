using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
public class AbsorbBrick : IBrick
{
    public ParticleSystem AbsorbParticleSys;
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "AbsorbBrick";
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

    public void absorbHP(IBrick Brick)
    {
        HP += Brick.HpTemp / 10;
        if (HP + Brick.HpTemp / 10 >= HpTemp && HP != HpTemp)
        {
            AbsorbParticleSys.Emit((int)((HpTemp - HP) * 5));
            ShowText("+" + (HpTemp - HP).ToString("F1"), "Heal", false);
        }
        else if (HP + Brick.HpTemp / 10 < HpTemp)
        {
            AbsorbParticleSys.Emit((int)(Brick.HpTemp / 2));
            ShowText("+" + (Brick.HpTemp / 10).ToString("F1"), "Heal", false);
        }
    }
}
