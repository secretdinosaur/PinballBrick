using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string ItemName;
    public abstract void MoveItem();
    public abstract void MoveItem_Main();
}
