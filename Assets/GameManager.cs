using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; //DebugCanvas Off
    }
}
