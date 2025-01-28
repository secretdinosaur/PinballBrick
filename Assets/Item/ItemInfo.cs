using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Objects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public Item[] Items;

    public float[] ItemSpawnChance;

    public string[] ItemName;
}
