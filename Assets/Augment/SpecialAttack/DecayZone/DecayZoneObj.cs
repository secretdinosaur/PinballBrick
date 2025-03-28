using System.Collections;
using UnityEngine;

public class DecayZoneObj : SpecialObj
{
    public Coroutine Routine = null;
    void OnEnable()
    {
        base.OnEnable();
        SpecialObjType = "DecayZoneObj";
        transform.position = new Vector2(Random.Range(0f, 2.5f), Random.Range(0f, 4.75f));
        Routine = StartCoroutine(DecayZone());
    }
    IEnumerator DecayZone()
    {
        float time = 0f;
        while (time < 5f)
        {
            if (detectArea.NearbyBricks.Count > 0)
            {
                foreach (IBrick brick in detectArea.NearbyBricks)
                {
                    if(brick.gameObject.activeInHierarchy && brick.BrickEffect.EffectStatusSet["Decay"].EffectStack < 1)
                    {
                        brick.DecayEffect(StatManager.instance.Temporary_stat.EffectDuration + AugmentManager.instance.effectDuration.DecayDuration);
                    }
                }
            }
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        SpecialObjManager.Instance.TakeSpecialObj(this);
    }
}
