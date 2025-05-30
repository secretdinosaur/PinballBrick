using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteBeamObj : SpecialObj
{
    private float TouchDamage;
    private float EffectDamage;

    public Coroutine Routine = null;

    private List<IBrick> TouchedBricks = new List<IBrick>(); 

    public Vector2 StartCoordinate;
    public Vector2 FinishCoordinate;

    void OnEnable()
    {
        base.OnEnable();
        TouchedBricks.Clear();
        SpecialObjType = "SatelliteBeamObj";
    }

    public void StartAttack(float Duration)
    {
        StartCoordinate = StartCoordinateSetting();
        transform.position = StartCoordinate;
        FinishCoordinate = FinishCoordinateSetting(StartCoordinate);
        Routine = StartCoroutine(SatelliteBeam(Duration));
    }

    IEnumerator SatelliteBeam(float Duration)
    {
        StartCoroutine(MoveSatelliteBeam(StartCoordinate, FinishCoordinate, Duration));
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            for(int i = 0; i < detectArea.NearbyBricks.Count; i++)
            {
                IBrick brick = detectArea.NearbyBricks[i];
                if (brick != null)
                {
                    if (!TouchedBricks.Contains(brick))
                    {
                        TouchDamage = (200f + StatManager.instance.Temporary_stat.AttackDamage) * StatManager.instance.statMultiplier.AttackDamageMultiplier;
                        brick.ArbitraryDamageAttack(TouchDamage, false, TouchDamage.ToString("F1"), "Normal");
                        TouchedBricks.Add(brick);
                    }
                    EffectDamage = (2f + StatManager.instance.Temporary_stat.EffectDamage) * StatManager.instance.statMultiplier.EffectDamageMultiplier;
                    brick.ArbitraryDamageAttack(EffectDamage, false, EffectDamage.ToString("F1"), "Fire");
                }
            }
        }
    }

    private IEnumerator MoveSatelliteBeam(Vector2 StartLocation, Vector2 FinishLocation, float Duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 12.5f;
            t = Mathf.SmoothStep(0, 1, t);
            transform.position = Vector2.Lerp(StartLocation, FinishLocation, t);
            yield return null;
        }

        SpecialObjManager.Instance.TakeSpecialObj(this);
    }

    private Vector2 StartCoordinateSetting()
    {
        float X, Y;

        X = Random.value < 0.5f ? 5f : -5f;
        Y = Random.Range(-1.5f, 3.5f);

        return new Vector2(X, Y);
    }

    private Vector2 FinishCoordinateSetting(Vector2 StartCoordinate)
    {
        float X, Y;

        X = StartCoordinate.x == 5f ? -5f : 5f;
        Y = Random.Range(-1f, 4f);

        return new Vector2(X, Y);
    }
}
