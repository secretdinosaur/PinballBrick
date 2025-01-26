using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "BrickInfo", menuName = "Scriptable Objects/BrickInfo")]
public class BrickInfo : ScriptableObject
{
    public IBrick[] Bricks;

    public float[] BrickSpawnChance;

    public string[] BrickTypes;
}
