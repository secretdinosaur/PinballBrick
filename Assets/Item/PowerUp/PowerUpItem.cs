using UnityEngine;
using System.Collections;
public class PowerUpItem : Item
{
    public override void UseItem()
    {
        StatManager.instance.statMultiplier.AttackDamageMultiplier *= 2;
        StatManager.instance.statMultiplier.EffectDamageMultiplier *= 2;


        StartCoroutine(Hidefor10sAndReturn());
    }
    private IEnumerator Hidefor10sAndReturn()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(10f);

        GetComponent<SpriteRenderer>().enabled = true;
        StatManager.instance.statMultiplier.AttackDamageMultiplier /= 2;
        StatManager.instance.statMultiplier.EffectDamageMultiplier /= 2;
        ItemManager.Instance.TakePowerUpItem(this);
    }
}
