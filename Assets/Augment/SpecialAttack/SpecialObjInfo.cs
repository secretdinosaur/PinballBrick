using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialObjInfo", menuName = "Scriptable Objects/SpecialObjInfo")]
public class SpecialObjInfo : ScriptableObject
{
    public SpecialObj[] SpecialObjs;

    public string[] SpecialObjTypes;
}