using UnityEngine;
using System;
using System.Collections.Generic;

public class BrickDetect : MonoBehaviour
{
    public List<IBrick> NearbyBricks;
    private Collider2D Collider;

    private void OnEnable()
    {
        Collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            if (!NearbyBricks.Contains(other.gameObject.GetComponent<IBrick>()))
                NearbyBricks.Add(other.gameObject.GetComponent<IBrick>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            if (NearbyBricks.Contains(other.gameObject.GetComponent<IBrick>()))
                NearbyBricks.Remove(other.gameObject.GetComponent<IBrick>());
        }
    }
}
