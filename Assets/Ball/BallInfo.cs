using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "BallInfo", menuName = "Scriptable Objects/BallInfo")]
public class BallInfo : ScriptableObject
{
    public IBall[] Balls;

    public string[] BallTypes;
}