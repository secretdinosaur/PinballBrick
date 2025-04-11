using System;
using System.Collections.Generic;
using UnityEngine;

public class BallDetect : MonoBehaviour
{
    public List<IBrick> NearbyBricks;
    public List<IBrick> BoomArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            if(!NearbyBricks.Contains(collision.gameObject.GetComponent<IBrick>()))
                NearbyBricks.Add(collision.gameObject.GetComponent<IBrick>());

            if (!BoomArea.Contains(collision.gameObject.GetComponent<IBrick>()))
                BoomArea.Add(collision.gameObject.GetComponent<IBrick>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            if(NearbyBricks.Contains(collision.gameObject.GetComponent<IBrick>()))
                NearbyBricks.Remove(collision.gameObject.GetComponent<IBrick>());

            if (BoomArea.Contains(collision.gameObject.GetComponent<IBrick>()))
                BoomArea.Remove(collision.gameObject.GetComponent<IBrick>());
        }
    }
}
