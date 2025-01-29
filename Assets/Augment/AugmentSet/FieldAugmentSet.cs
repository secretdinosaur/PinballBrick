using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
/*
public class Example : FieldAugment
{
     public Example() : base( "FIELD_n", "Example" , "Just Example", 1 ) { }

    public override void Apply(){ Augment Effect (Plus) }

    public override void Remove(){ Augment Effect (Minus) }
}
 */

public class AllDamage : FieldAugment
{
    Coroutine Routine = null;

    private void OnEnable()
    {
        SetAugment("FIELD_1", "All Damage", "All Damage", 3);
    }

    public override void Apply()
    {
        Routine = StartCoroutine(StartAllDamage(5f, 10f));
    }

    public override void Remove()
    {
        if (Routine != null)
        {
            StopCoroutine(Routine);
            Routine = null;
        }
    }

    private IEnumerator StartAllDamage(float period, float Damage)
    {
        while(true)
        {
            yield return new WaitForSeconds(period);

            foreach (IBrick brick in BrickManager.Instance.CurrentBricks)
            {
                brick.HP -= 10 * StatManager.instance.statMultiplier.AttackDamageMultiplier;
            }
        }
    }
}