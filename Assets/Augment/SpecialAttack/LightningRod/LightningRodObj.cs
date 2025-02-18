using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class LightningRodObj : MonoBehaviour
{
    private List<IBrick> NearbyBricks;
    public Coroutine Routine = null;
    void OnEnable()
    {
        NearbyBricks = new List<IBrick>();
        Routine = StartCoroutine(LightningRod());
    }

    IEnumerator LightningRod()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (NearbyBricks.Count > 0)
            {
                int DamageBricksNumber = Random.Range(1, NearbyBricks.Count % 2 == 0 ? NearbyBricks.Count / 2 : 
                                                        NearbyBricks.Count != 1 ? (NearbyBricks.Count - 1) / 2 : 
                                                        NearbyBricks.Count);
                for (int i = 0; i < DamageBricksNumber; i++)
                {
                    int index = Random.Range(0, NearbyBricks.Count);
                    if (NearbyBricks[index] != null && NearbyBricks[index].gameObject.activeInHierarchy)
                    {
                        NearbyBricks[index].DamageByLightningEffect(5f, 1);
                    }
                }
            }
        }
    }

    void Update()
    {
        CollectNearbyBricks();
        RemoveInactiveBricks();
    }

    private float detectionRadius = 1.5f;
    void CollectNearbyBricks()
    {
        LayerMask detectionLayer = LayerMask.GetMask("Game");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);
        foreach (Collider2D collider in colliders)
        {
            IBrick obj = collider.gameObject.GetComponent<IBrick>();
            if (obj != null && collider.gameObject.activeInHierarchy && !NearbyBricks.Contains(obj) && obj != this)
            {
                NearbyBricks.Add(obj);
            }
        }
    }

    void RemoveInactiveBricks()
    {
        NearbyBricks.RemoveAll(brick => brick == null || !brick.gameObject.activeInHierarchy);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
