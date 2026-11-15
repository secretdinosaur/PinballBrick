using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "BrickInfo", menuName = "Scriptable Objects/BrickInfo")]
public class BrickInfo : ScriptableObject
{
    public IBrick[] Bricks;

    public string[] BrickTypes;

    public float[] BrickSpawnChance;

    public IBrick[] SpecialBricks;

    public string[] SpecialBrickTypes;
}
