using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Objects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public float[] ItemSpawnChance;

    public string[] ItemName;
}
