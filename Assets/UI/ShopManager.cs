using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
        private float Gold;

        void Start()
        { 
                Gold = StatManager.instance.LoadPersistentStats().Gold;
        }

        void Purchase()
        {
                StatManager.instance.SavePersistentStats(StatManager.instance.Persistent_stat);
        }
}