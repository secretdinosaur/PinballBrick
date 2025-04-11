using UnityEngine;

public class SpecialObj : MonoBehaviour
{
    public string SpecialObjType;
    public SpecialDetect detectArea;
    public void OnEnable()
    {
        detectArea = GetComponentInChildren<SpecialDetect>();
        detectArea.NearbyBricks.Clear();
    }
}