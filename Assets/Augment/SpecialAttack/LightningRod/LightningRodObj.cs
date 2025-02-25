using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class LightningRodObj : SpecialObj
{
    public Coroutine Routine = null;
    void OnEnable()
    {
        base.OnEnable();
        SpecialObjType = "LightningRodObj";
        transform.position = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 4f));
        Routine = StartCoroutine(LightningRod());
    }

    IEnumerator LightningRod()
    {
        int time = 0;
        while (time < 10)
        {
            if (detectArea.NearbyBricks.Count > 0)
            {
                int DamageBricksNumber = Random.Range(1, detectArea.NearbyBricks.Count % 2 == 0 ? detectArea.NearbyBricks.Count / 2 :
                                                        detectArea.NearbyBricks.Count != 1 ? (detectArea.NearbyBricks.Count - 1) / 2 :
                                                        detectArea.NearbyBricks.Count);
                for (int i = 0; i < DamageBricksNumber; i++)
                {
                    int index = Random.Range(0, detectArea.NearbyBricks.Count);
                    if (detectArea.NearbyBricks[index] != null && detectArea.NearbyBricks[index].gameObject.activeInHierarchy)
                    {
                        detectArea.NearbyBricks[index].DamageByLightningEffect(5f, 1);
                    }
                }
            }
            time += 1;
            yield return new WaitForSeconds(1f);
        }
        SpecialObjManager.Instance.TakeSpecialObj(this);
    }
}
