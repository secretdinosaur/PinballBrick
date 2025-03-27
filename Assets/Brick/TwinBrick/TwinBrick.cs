using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class TwinBrick : IBrick
{
    public LowerBrick LowerBrick;
    public UpperBrick UpperBrick;

    bool IsWaitingHeal = false;

    private void OnEnable()
    {
        BrickType = "TwinBrick";

        base.OnEnable();
    }
    public void CheckBothBrickDead()
    {
        if(UpperBrick.HP <= 0 && LowerBrick.HP <= 0)
        {
            IsWaitingHeal = false;
            BrickDie();
        }
        else if (UpperBrick.HP <= 0 || LowerBrick.HP <= 0)
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

        if (UpperBrick.HP < LowerBrick.HP) UpperBrick.HP = LowerBrick.HP;
        else LowerBrick.HP = UpperBrick.HP;
        IsWaitingHeal = false;
    }
}
